using CommunityToolkit.Mvvm.Input;
using CompanyName.AppName.Core.Navigation;

namespace CompanyName.AppName.Core.ViewModels;

/// <summary>
/// Drives the System Design catalog index, opening the component showcase pages.
/// </summary>
/// <remarks>
/// Navigation arrives through <see cref="INavigationService"/> rather than a static, so the
/// commands can be verified in a test with a fake navigator. Each <c>[RelayCommand]</c> method
/// generates a command of the PascalCase name plus "Command"; for example <c>ViewButtons</c>
/// generates <c>ViewButtonsCommand</c>, which the XAML binds to.
/// </remarks>
public partial class SystemDesignViewModel : BaseViewModel
{
    readonly INavigationService _navigation;

    public SystemDesignViewModel(INavigationService navigation)
    {
        _navigation = navigation;
    }

    /// <summary>Opens the existing application-system buttons page.</summary>
    [RelayCommand]
    Task ViewButtons() => _navigation.GoToAsync("Buttons");

    /// <summary>
    /// Opens a Material catalog page by route.
    /// </summary>
    /// <param name="route">Route of the page to open, passed as the command parameter.</param>
    [RelayCommand]
    Task ViewMaterial(string route) => _navigation.GoToAsync(route);
}
