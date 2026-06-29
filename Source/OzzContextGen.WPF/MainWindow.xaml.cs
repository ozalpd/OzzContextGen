using OzzContextGen.WPF.Models;
using OzzContextGen.WPF.ViewModels;
using System.Windows;

namespace OzzContextGen.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Title = $"OzzContextGen - LLM Context Packer - v{AppVersion.Version}";
            this.DataContext = new MainViewModel();
        }
    }
}