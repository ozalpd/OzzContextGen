using OzzContextGen.Core.Models;

namespace OzzContextGen.WPF.ViewModels
{
    public class FileChangeViewModel : AbstractViewModel
    {
        public FileChangeViewModel(FileChangeSummary summary)
        {
            _summary = summary;
        }

        private readonly FileChangeSummary _summary;

        public string RelativePath => _summary.RelativePath;
        public string AbsolutePath => _summary.AbsolutePath;
        public string ChangeType => _summary.Change.ToString();

        public bool IsSelected
        {
            get => _summary.IsSelected;
            set
            {
                _summary.IsSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }
    }
}
