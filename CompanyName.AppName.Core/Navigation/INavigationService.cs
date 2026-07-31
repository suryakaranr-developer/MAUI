namespace CompanyName.AppName.Core.Navigation;

/// <summary>
/// Moves between application routes.
/// </summary>
/// <remarks>
/// View models depend on this rather than on the Shell so they can be exercised in tests with a
/// fake, and so navigation is not a static reached through <c>Shell.Current</c>. The concrete
/// implementation lives in the app head, which is the only layer that knows about the Shell.
/// </remarks>
public interface INavigationService
{
    /// <summary>
    /// Pushes a registered route onto the active navigation stack.
    /// </summary>
    /// <param name="route">A route relative to the current page. A leading "//" resets the
    /// stack instead of pushing, so pass routes relative unless that is the intent.</param>
    Task GoToAsync(string route);

    /// <summary>
    /// Pushes a route, passing parameters the destination can receive as query attributes.
    /// </summary>
    /// <param name="route">The route to push.</param>
    /// <param name="parameters">Values keyed by the name the destination expects.</param>
    Task GoToAsync(string route, IDictionary<string, object> parameters);

    /// <summary>
    /// Pops the current page off the stack.
    /// </summary>
    Task GoBackAsync();
}
