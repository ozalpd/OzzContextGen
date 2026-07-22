using OzzContextGen.WPF.Models;
using OzzWpf.Core.Controls;
using System.Windows;

namespace OzzContextGen.WPF.Views
{
    /// <summary>
    /// Interaction logic for MarkdownView.xaml
    /// </summary>
    public partial class MarkdownView : Window
    {
        private readonly AppSettings _appSettings = AppSettings.GetAppSettings();
        private readonly MarkdownViewer _markdownViewer;
        private string? _markdownContent;
        private bool _isLoaded = false;

        public MarkdownView()
        {
            InitializeComponent();
            _markdownViewer = new MarkdownViewer(_appSettings);
            MarkdownViewerHost.Content = _markdownViewer;

            SourceInitialized += MarkdownView_SourceInitialized;
            Closing += MarkdownView_Closing;
        }

        private async void MarkdownView_SourceInitialized(object? sender, EventArgs e)
        {
            SourceInitialized -= MarkdownView_SourceInitialized;
            _appSettings.MarkdownWindowPosition.SetWindowPositions(this);
            if (!string.IsNullOrEmpty(_markdownContent))
            {
                _markdownViewer.LoadMarkdown(_markdownContent);
            }

            _isLoaded = true;
        }

        public bool GenerateToc
        {
            get => _markdownViewer.GenerateToc;
            set => _markdownViewer.GenerateToc = value;
        }


        public void LoadMarkdown(string markdown)
        {
            _markdownContent = markdown;
            if (_isLoaded)
                _markdownViewer.LoadMarkdown(markdown);
        }

        private void MarkdownView_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _appSettings.MarkdownWindowPosition.GetWindowPositions(this);
            _appSettings.Save();
        }
    }
}
