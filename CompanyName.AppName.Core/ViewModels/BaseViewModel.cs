using CommunityToolkit.Mvvm.ComponentModel;

namespace CompanyName.AppName.Core.ViewModels;

/// <summary>
/// Base for view models, carrying the loading and status state common to screens.
/// </summary>
/// <remarks>
/// Inherits <see cref="ObservableObject"/> so change notification is generated rather than
/// hand-written. The <c>[ObservableProperty]</c> fields each generate a public property of the
/// PascalCase name, for example <c>isLoading</c> generates <c>IsLoading</c>.
/// </remarks>
public partial class BaseViewModel : ObservableObject
{
    /// <summary>Indicates work is in progress.</summary>
    [ObservableProperty]
    bool isLoading;

    /// <summary>Icon shown by the loading indicator.</summary>
    [ObservableProperty]
    string? loadingIcon;

    /// <summary>Short status label.</summary>
    [ObservableProperty]
    string? status;

    /// <summary>Longer status message.</summary>
    [ObservableProperty]
    string? statusMessage;

    /// <summary>Whether commands bound to this view model may run.</summary>
    [ObservableProperty]
    bool canExecute = true;

    /// <summary>
    /// Sets the loading and status fields together.
    /// </summary>
    /// <param name="loading">Whether work is in progress.</param>
    /// <param name="status">Short status label.</param>
    /// <param name="statusMessage">Longer status message.</param>
    /// <param name="loadingIcon">Loading indicator icon.</param>
    public void SetStatus(bool loading, string? status = null, string? statusMessage = null, string? loadingIcon = null)
    {
        Status = status;
        StatusMessage = statusMessage;
        IsLoading = loading;
        LoadingIcon = loadingIcon;
    }
}
