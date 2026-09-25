using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OzzContextGen.Core;
using OzzWpf.Core.Commands;
using OzzWpf.Core.ViewModels;

namespace OzzContextGen.WPF.ViewModels
{
    public class ExcludedFoldersEditVM : AbstractViewModel
    {
        private string _newFolderName = string.Empty;
        private string? _selectedFolder;

        public ObservableCollection<string> Folders { get; }

        public string NewFolderName
        {
            get => _newFolderName;
            set
            {
                _newFolderName = value;
                RaisePropertyChanged(nameof(NewFolderName));
                AddCommand.RaiseCanExecuteChanged();
            }
        }

        public string? SelectedFolder
        {
            get => _selectedFolder;
            set
            {
                _selectedFolder = value;
                RaisePropertyChanged(nameof(SelectedFolder));
                RemoveCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand RemoveCommand { get; }
        public RelayCommand ResetDefaultsCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        public event Action? RequestClose;
        public bool DialogResult { get; private set; }

        public ExcludedFoldersEditVM(IEnumerable<string>? excludedFolders = null)
        {
            var initial = (excludedFolders != null && excludedFolders.Any())
                ? excludedFolders
                : CtxDefaults.ExcludedFolders;

            Folders = new ObservableCollection<string>(initial);

            AddCommand = new RelayCommand(AddFolder, CanAddFolder);
            RemoveCommand = new RelayCommand(RemoveFolder, CanRemoveFolder);
            ResetDefaultsCommand = new RelayCommand(ResetDefaults);
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private bool CanAddFolder() =>
            !string.IsNullOrWhiteSpace(NewFolderName) &&
            !Folders.Any(f => string.Equals(f, NewFolderName.Trim(), StringComparison.OrdinalIgnoreCase));

        private void AddFolder()
        {
            var trimmed = NewFolderName.Trim();
            if (!string.IsNullOrEmpty(trimmed) && !Folders.Any(f => string.Equals(f, trimmed, StringComparison.OrdinalIgnoreCase)))
            {
                Folders.Add(trimmed);
                NewFolderName = string.Empty;
            }
        }

        private bool CanRemoveFolder() => !string.IsNullOrEmpty(SelectedFolder);

        private void RemoveFolder()
        {
            if (!string.IsNullOrEmpty(SelectedFolder))
            {
                Folders.Remove(SelectedFolder);
                SelectedFolder = null;
            }
        }

        private void ResetDefaults()
        {
            Folders.Clear();
            foreach (var folder in CtxDefaults.ExcludedFolders)
            {
                Folders.Add(folder);
            }
            SelectedFolder = null;
        }

        private void Save()
        {
            DialogResult = true;
            RequestClose?.Invoke();
        }

        private void Cancel()
        {
            DialogResult = false;
            RequestClose?.Invoke();
        }
    }
}
