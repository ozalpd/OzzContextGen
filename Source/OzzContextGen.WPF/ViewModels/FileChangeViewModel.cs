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

        public bool IsDeleted => _summary.Change == Core.Models.ChangeType.Deleted;

        public string ContextNote
        {
            get => _summary.ContextNote;
            set
            {
                _summary.ContextNote = value;
                RaisePropertyChanged(nameof(ContextNote));
            }
        }

        public PackingMode PackingMode
        {
            get => _summary.PackingMode;
            set
            {
                _summary.PackingMode = value;
                RaisePropertyChanged(nameof(PackingMode));
            }
        }

        public bool IsSelected
        {
            get => _summary.PackingMode > PackingMode.Excluded;
            set
            {
                if (value && _summary.PackingMode == PackingMode.Excluded)
                {
                    _summary.PackingMode = _summary.GetDefaultPackingMode();
                }
                else if (!value)
                {
                    _summary.PackingMode = PackingMode.Excluded;
                }

                RaisePropertyChanged(nameof(IsSelected));
                RaisePropertyChanged(nameof(PackingMode));
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
                PackingMode = PackingMode,
                ContextNote = ContextNote
            };
        }
    }
}
