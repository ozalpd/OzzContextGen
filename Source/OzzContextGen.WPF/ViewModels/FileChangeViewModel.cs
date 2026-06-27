using OzzContextGen.Core.Helpers;
using OzzContextGen.Core.Models;
using System.IO;

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

        public string FileSize => _summary.FileSize.ToFileSize();


        public string ContextNote
        {
            get => _summary.ContextNote;
            set
            {
                _summary.ContextNote = value;
                RaisePropertyChanged(nameof(ContextNote));
            }
        }

        public bool IsSelected
        {
            get => _summary.IsSelected;
            set
            {
                _summary.IsSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }

        public FileContextEntry ToFileContextEntry()
        {
            return new FileContextEntry
            {
                RelativePath = RelativePath,
                LastWriteTime = File.GetLastWriteTime(AbsolutePath),
                FileSize = _summary.Change != Core.Models.ChangeType.Deleted
                         ? new FileInfo(AbsolutePath).Length : 0,
                IsSelected = IsSelected,
                ContextNote = ContextNote
            };
        }
    }
}
