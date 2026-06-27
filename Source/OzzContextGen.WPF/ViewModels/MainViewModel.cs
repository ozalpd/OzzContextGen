using OzzContextGen.Core;
using OzzContextGen.Core.Models;
using OzzContextGen.i18n;
using OzzContextGen.WPF.Commands;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;

namespace OzzContextGen.WPF.ViewModels;

public class MainViewModel : AbstractViewModel
{
    private readonly PackerEngine _packerEngine;
    private readonly StateService _stateService;
    private ContextStateProfile _currentProfile = new();
    public MainViewModel()
    {
        _packerEngine = new PackerEngine();
        _stateService = new StateService();

        BrowseSourceCommand = new RelayCommand(BrowseSource);
        BrowseOutputCommand = new RelayCommand(BrowseOutput);
        OpenProfileCommand = new RelayCommand(async () => await OpenProfile());
        AnalyzeChangesCommand = new RelayCommand(async () => await AnalyzeChangesAsync());
        PackCommand = new RelayCommand(async () => await PackContextAsync(), CanPack);

        PropertyChanged += OnPropertyChanged;
        TrackedFiles.CollectionChanged += OnCollectionChanged;
    }


    public RelayCommand BrowseSourceCommand { get; }
    public RelayCommand BrowseOutputCommand { get; }
    public RelayCommand OpenProfileCommand { get; }
    public RelayCommand AnalyzeChangesCommand { get; }
    public RelayCommand PackCommand { get; }



    public string OutputPath
    {
        get => _outputPath;
        set
        {
            _outputPath = value;
            RaisePropertyChanged(nameof(OutputPath));
            PackCommand.RaiseCanExecuteChanged();
        }
    }
    private string _outputPath = string.Empty;

    public string ProfilePath
    {
        get => _profilePath;
        set
        {
            _profilePath = value;
            RaisePropertyChanged(nameof(ProfilePath));
        }
    }
    private string _profilePath = string.Empty;

    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            _statusMessage = value;
            RaisePropertyChanged(nameof(StatusMessage));
        }
    }
    private string _statusMessage = LocalizedStrings.Ready;

    public FileChangeViewModel? SelectedFile
    {
        get => _selectedFile;
        set
        {
            if (_selectedFile != null)
                _selectedFile.PropertyChanged -= OnSelectedFilePropertyChanged;

            _selectedFile = value;

            if (_selectedFile != null)
                _selectedFile.PropertyChanged += OnSelectedFilePropertyChanged;

            RaisePropertyChanged(nameof(SelectedFile));
        }
    }
    private FileChangeViewModel? _selectedFile;


    public ObservableCollection<FileChangeViewModel> TrackedFiles { get; } = new();

    public string SourcePath
    {
        get => _sourcePath;
        set
        {
            _sourcePath = value;
            RaisePropertyChanged(nameof(SourcePath));
        }
    }
    private string _sourcePath = string.Empty;

    private void BrowseSource()
    {
        var dialog = new FolderBrowserDialog();
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            SourcePath = dialog.SelectedPath;
        }
    }

    private void BrowseOutput()
    {
        var dialog = new Microsoft.Win32.SaveFileDialog { Filter = LocalizedStrings.MarkdownFileFilter };
        if (dialog.ShowDialog() == true)
        {
            OutputPath = dialog.FileName;
        }
    }

    private async Task OpenProfile()
    {
        var dialog = new Microsoft.Win32.OpenFileDialog { Filter = LocalizedStrings.ProfileFileFilter };
        if (dialog.ShowDialog() == true)
        {
            ProfilePath = dialog.FileName;
        }
        var profile = await _stateService.LoadProfileAsync(ProfilePath);
        _currentProfile = profile;
        SourcePath = profile.TargetSourcePath;
    }

    private async Task AnalyzeChangesAsync()
    {
        if (string.IsNullOrEmpty(SourcePath) || !Directory.Exists(SourcePath))
        {
            StatusMessage = $"{LocalizedStrings.InvalidSourceFolder}";
            return;
        }

        StatusMessage = $"{LocalizedStrings.ScanningAnalyzingChanges}";
        TrackedFiles.Clear();

        // 1. Mevcut profil varsa yükle, yoksa yeni oluştur
        // 1. Load existing profile if available, otherwise create a new one
        var profile = string.IsNullOrEmpty(ProfilePath)
            ? new ContextStateProfile()
            : await _stateService.LoadProfileAsync(ProfilePath);

        _currentProfile = profile;

        // 2. Çekirdek motordan dosya listesini al ve analiz et
        // PackerEngine'deki recursive tarama mantığıyla diskteki güncel listeyi simüle ediyoruz
        // 2. Get the file list from the core engine and analyze it
        // We simulate the current list on disk using the recursive scanning logic in PackerEngine

        var suffixes = profile.SelectedSuffixes.Count > 0
            ? profile.SelectedSuffixes.ToArray()
            : SourceLanguages.All.Keys.ToArray();
        var codeCrawler = new CodeCrawler(suffixes);
        var csFiles = codeCrawler.GetCodeFiles(SourcePath).ToList();

        var changes = _stateService.AnalyzeChanges(SourcePath, profile, csFiles);

        foreach (var change in changes)
        {
            TrackedFiles.Add(new FileChangeViewModel(change));
        }

        StatusMessage = string.Format(LocalizedStrings.ScanningFinished, TrackedFiles.Count);
    }

    private bool CanPack() => !string.IsNullOrEmpty(OutputPath) && TrackedFiles.Any(f => f.IsSelected);

    private async Task PackContextAsync()
    {
        if (string.IsNullOrEmpty(OutputPath))
        {
            StatusMessage = $"{LocalizedStrings.SpecifyOutputPath}.";
            return;
        }

        StatusMessage = $"{LocalizedStrings.StartingPackingProcess}";

        // Sadece GUI üzerinde kullanıcının check attığı (seçtiği) dosyaları filtreliyoruz
        // We are filtering only the files selected by the user on the GUI
        var selectedFiles = TrackedFiles.Where(f => f.IsSelected).Select(f => f.AbsolutePath).ToList();

        if (!selectedFiles.Any())
        {
            StatusMessage = $"{LocalizedStrings.NoFilesSelectedForPacking}.";
            return;
        }

        // Triggering PackerEngine (async to avoid UI freezing)
        string markdownResult = await PackerEngine.PackSourceCodeAsync(selectedFiles, SourcePath, message =>
        {
            StatusMessage = message; // Displaying the progress status in real-time
        });

        // Write the file
        await File.WriteAllTextAsync(OutputPath, markdownResult, System.Text.Encoding.UTF8);

        // Eğer profil yolu varsa .ctxgen dosyasını da güncelle
        // If a profile path exists, also update the .ctxgen file
        if (!string.IsNullOrEmpty(ProfilePath))
        {
            var updatedTrackedFiles = TrackedFiles.ToDictionary(
                f => f.RelativePath,
                f => new FileStateInfo
                {
                    RelativePath = f.RelativePath,
                    LastWriteTime = File.GetLastWriteTime(f.AbsolutePath),
                    FileSize = new FileInfo(f.AbsolutePath).Length,
                    IsSelectedForPacking = f.IsSelected
                });

            var newProfile = new ContextStateProfile
            {
                ProfileName = _currentProfile.ProfileName,
                TargetSourcePath = SourcePath,
                LastPackedAt = DateTime.Now,
                TrackedFiles = updatedTrackedFiles,
                SelectedSuffixes = _currentProfile.SelectedSuffixes
            };

            await _stateService.SaveProfileAsync(ProfilePath, newProfile);
        }

        StatusMessage = $"✓ {LocalizedStrings.CompletedProfilePacking}!";
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        PackCommand.RaiseCanExecuteChanged();
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(TrackedFiles):
                PackCommand.RaiseCanExecuteChanged();
                break;
        }
    }

    private void OnSelectedFilePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(FileChangeViewModel.IsSelected))
        {
            PackCommand.RaiseCanExecuteChanged();
        }
    }
}
