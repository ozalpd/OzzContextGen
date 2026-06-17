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
    public MainViewModel()
    {
        _packerEngine = new PackerEngine();
        _stateService = new StateService();

        BrowseSourceCommand = new RelayCommand(BrowseSource);
        BrowseOutputCommand = new RelayCommand(BrowseOutput);
        BrowseProfileCommand = new RelayCommand(BrowseProfile);
        AnalyzeChangesCommand = new RelayCommand(async () => await AnalyzeChangesAsync());
        PackCommand = new RelayCommand(async () => await PackContextAsync(), CanPack);

        PropertyChanged += OnPropertyChanged;
        TrackedFiles.CollectionChanged += OnCollectionChanged;
    }


    public RelayCommand BrowseSourceCommand { get; }
    public RelayCommand BrowseOutputCommand { get; }
    public RelayCommand BrowseProfileCommand { get; }
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
        // Klasör seçme diyalog kutusu (Ookii.Dialogs veya WinForms FolderBrowserDialog kullanılabilir)
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

    private void BrowseProfile()
    {
        var dialog = new Microsoft.Win32.OpenFileDialog { Filter = LocalizedStrings.ProfileFileFilter };
        if (dialog.ShowDialog() == true)
        {
            ProfilePath = dialog.FileName;
        }
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
        var profile = string.IsNullOrEmpty(ProfilePath)
            ? new ContextStateProfile()
            : await _stateService.LoadProfileAsync(ProfilePath);

        // 2. Çekirdek motordan dosya listesini al ve analiz et
        // PackerEngine'deki recursive tarama mantığıyla diskteki güncel listeyi simüle ediyoruz

        var codeCrawler = new CodeCrawler(".cs");
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
        var selectedFiles = TrackedFiles.Where(f => f.IsSelected).Select(f => f.AbsolutePath).ToList();

        if (!selectedFiles.Any())
        {
            StatusMessage = $"{LocalizedStrings.NoFilesSelectedForPacking}.";
            return;
        }

        // PackerEngine'i tetikliyoruz (UI kilitlenmesin diye async)
        string markdownResult = await PackerEngine.PackSourceCodeAsync(selectedFiles, SourcePath, message =>
        {
            StatusMessage = message; // İlerleme durumunu anlık olarak alt barda gösteriyoruz
        });

        // Dosyayı yaz
        await File.WriteAllTextAsync(OutputPath, markdownResult, System.Text.Encoding.UTF8);

        // Eğer profil yolu varsa .ctxgen dosyasını da güncelle
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
                TargetSourcePath = SourcePath,
                LastPackedAt = DateTime.Now,
                TrackedFiles = updatedTrackedFiles
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
