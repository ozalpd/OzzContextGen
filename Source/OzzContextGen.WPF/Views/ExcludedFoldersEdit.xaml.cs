using System.Windows;
using OzzContextGen.WPF.ViewModels;

namespace OzzContextGen.WPF.Views
{
    /// <summary>
    /// Interaction logic for ExcludedFoldersEdit.xaml
    /// </summary>
    public partial class ExcludedFoldersEdit : Window
    {
        public ExcludedFoldersEdit()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ExcludedFoldersEditVM oldVm)
            {
                oldVm.RequestClose -= OnRequestClose;
            }
            if (e.NewValue is ExcludedFoldersEditVM newVm)
            {
                newVm.RequestClose += OnRequestClose;
            }
        }

        private void OnRequestClose()
        {
            if (DataContext is ExcludedFoldersEditVM vm)
            {
                DialogResult = vm.DialogResult;
            }
            Close();
        }
    }
}
