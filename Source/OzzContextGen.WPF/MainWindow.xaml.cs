using OzzContextGen.WPF.Models;
using OzzContextGen.WPF.ViewModels;
using OzzMarkdown.Core.Models;
using System.Windows;

namespace OzzContextGen.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AppSettings _appSettings = AppSettings.GetAppSettings();
        private MainViewModel _viewModel;
        private readonly string? _filePathToOpen;
        private bool _isLlmAndPromptsFocused = false;
        public MainWindow() : this(null) { }

        public MainWindow(string? filePathToOpen)
        {
            InitializeComponent();
            _filePathToOpen = filePathToOpen;
            SourceInitialized += MainWindow_SourceInitialized;
            Closing += MainWindow_Closing;
        }

        private async void MainWindow_SourceInitialized(object? sender, EventArgs e)
        {
            SourceInitialized -= MainWindow_SourceInitialized;
            Title = $"OzzContextGen - LLM Context Packer - v{AppVersion.Version}";
            _viewModel = new MainViewModel();
            this.DataContext = _viewModel;
            _appSettings.MainWindowPosition.SetWindowPositions(this);
            _viewModel.PropertyChanged += OnPropertyChanged;

            if (!string.IsNullOrEmpty(_filePathToOpen))
            {
                _viewModel.ProfilePath = _filePathToOpen;
                _ = _viewModel.OpenProfile(showDialog: false);
            }
        }

        private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainViewModel.SelectedFile))
            {
                if (_viewModel.SelectedFile != null && !_isLlmAndPromptsFocused)
                {
                    SelectedFileTab.IsSelected = true;
                }
            }
        }

        private void RecentProjectsLabel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            cboRecentProjects.IsDropDownOpen = true;
            e.Handled = true;
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _viewModel.PropertyChanged -= OnPropertyChanged;
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.Shutdown();
            }
            _appSettings.MainWindowPosition.GetWindowPositions(this);
            _appSettings.Save();
        }

        private void LlmAndPrompts_GotFocus(object sender, RoutedEventArgs e)
        {
            _isLlmAndPromptsFocused = true;
        }

        private async void LlmAndPrompts_LostFocus(object sender, RoutedEventArgs e)
        {
            await Task.Delay(200);
            _isLlmAndPromptsFocused = false;
        }
    }
}