using CompanyName.AppName.Core.Navigation;

namespace CompanyName.AppName.MAUI.Navigation;

/// <summary>
/// Navigates by driving the application <see cref="Shell"/>.
/// </summary>
/// <remarks>
/// The dependency on the static <c>Shell.Current</c> is contained here, at the platform boundary,
/// so view models stay clear of it. Every call is marshalled to the UI thread because a command
/// may be raised from a background continuation, and Shell navigation must run on the UI thread.
/// </remarks>
public sealed class ShellNavigationService : INavigationService
{
    public Task GoToAsync(string route) =>
        MainThread.InvokeOnMainThreadAsync(() => Shell.Current.GoToAsync(route));

    public Task GoToAsync(string route, IDictionary<string, object> parameters) =>
        MainThread.InvokeOnMainThreadAsync(() => Shell.Current.GoToAsync(route, parameters));

    public Task GoBackAsync() =>
        MainThread.InvokeOnMainThreadAsync(() => Shell.Current.GoToAsync(".."));
}
