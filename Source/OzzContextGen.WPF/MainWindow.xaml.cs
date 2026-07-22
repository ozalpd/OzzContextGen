using OzzContextGen.WPF.Models;
using OzzContextGen.WPF.ViewModels;
using OzzWpf.Core.Models;
using System.Windows;

namespace OzzContextGen.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AppSettings _appSettings = AppSettings.GetAppSettings();

        public MainWindow()
        {
            InitializeComponent();
            SourceInitialized += MainWindow_SourceInitialized;
            Closing += MainWindow_Closing;
        }

        private async void MainWindow_SourceInitialized(object? sender, EventArgs e)
        {
            SourceInitialized -= MainWindow_SourceInitialized;
            Title = $"OzzContextGen - LLM Context Packer - v{AppVersion.Version}";
            this.DataContext = new MainViewModel();
            _appSettings.MainWindowPosition.SetWindowPositions(this);
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.Shutdown();
            }
            _appSettings.MainWindowPosition.GetWindowPositions(this);
            _appSettings.Save();
        }
    }
}