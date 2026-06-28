using System.Windows;

namespace OzzContextGen.WPF.Helpers;

/// <summary>
/// Bridges bindings for elements outside the visual tree (e.g. DataGridColumn headers).
/// Declare as a resource and bind Data to the window's DataContext.
/// </summary>
public class BindingProxy : Freezable
{
    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register(nameof(Data), typeof(object), typeof(BindingProxy));

    public object Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    protected override Freezable CreateInstanceCore() => new BindingProxy();
}
