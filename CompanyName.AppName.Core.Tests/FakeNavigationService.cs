using CompanyName.AppName.Core.Navigation;

namespace CompanyName.AppName.Core.Tests;

/// <summary>
/// Records the routes it is asked to navigate to, in order, so a test can assert what a command
/// did without a Shell or a running app.
/// </summary>
internal sealed class FakeNavigationService : INavigationService
{
    public List<string> Routes { get; } = new();

    public Task GoToAsync(string route)
    {
        Routes.Add(route);
        return Task.CompletedTask;
    }

    public Task GoToAsync(string route, IDictionary<string, object> parameters)
    {
        Routes.Add(route);
        return Task.CompletedTask;
    }

    public Task GoBackAsync()
    {
        Routes.Add("..");
        return Task.CompletedTask;
    }
}
