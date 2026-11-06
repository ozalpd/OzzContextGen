using OzzContextGen.Core.Models;
using OzzContextGen.Core.Helpers;
using OzzWpf.Core.Commands;
using OzzWpf.Core.ViewModels;
using System;
using System.Collections.Generic;
using static OzzContextGen.Core.Helpers.EnumExtensions;

namespace OzzContextGen.WPF.ViewModels;

public class BulkPackingModeEditVM : AbstractViewModel
{
    private PackingMode _selectedMode = PackingMode.FullPack;

    public IEnumerable<EnumValueItem<PackingMode>> PackingModeValues { get; } = GetValues<PackingMode>();

    public PackingMode SelectedMode
    {
        get => _selectedMode;
        set
        {
            if (_selectedMode != value)
            {
                _selectedMode = value;
                RaisePropertyChanged(nameof(SelectedMode));
            }
        }
    }

    /// <summary>Number of files that will be affected by applying the bulk change.</summary>
    public int AffectedCount { get; }

    public RelayCommand ApplyCommand { get; }
    public RelayCommand CancelCommand { get; }

    public event Action? RequestClose;
    public bool DialogResult { get; private set; }

    public BulkPackingModeEditVM(int affectedCount, PackingMode initialMode = PackingMode.FullPack)
    {
        AffectedCount = affectedCount;
        _selectedMode = initialMode;

        ApplyCommand = new RelayCommand(Apply);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void Apply()
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

