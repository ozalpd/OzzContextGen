using OzzContextGen.WPF.ViewModels;
using System.Windows;

namespace OzzContextGen.WPF.Views
{
    /// <summary>
    /// Interaction logic for BulkPackingModeEdit.xaml
    /// </summary>
    public partial class BulkPackingModeEdit : Window
    {
        public BulkPackingModeEdit()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is BulkPackingModeEditVM oldVm)
                oldVm.RequestClose -= OnRequestClose;

            if (e.NewValue is BulkPackingModeEditVM newVm)
                newVm.RequestClose += OnRequestClose;
        }

        private void OnRequestClose()
        {
            if (DataContext is BulkPackingModeEditVM vm)
                DialogResult = vm.DialogResult;

            Close();
        }
    }
}
