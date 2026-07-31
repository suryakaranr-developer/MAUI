using CompanyName.AppName.Design.MAUI.SystemDesigns;
using CompanyName.AppName.Design.MAUI.SystemDesigns.Material;

namespace CompanyName.AppName.MAUI.Navigation;

/// <summary>
/// The single source of truth for detail routes and the pages they resolve to.
/// </summary>
/// <remarks>
/// Consumed in two places that would otherwise drift: <see cref="AppShell"/> registers each route
/// with the Shell, and <c>MauiProgram</c> registers each page with the container so it can be
/// constructed with its dependencies. Adding a page means adding one line here.
/// </remarks>
public static class AppRoutes
{
    /// <summary>
    /// Detail pages reached by pushing onto the active tab, keyed by route name.
    /// </summary>
    public static IReadOnlyList<(string Route, Type Page)> Details { get; } = new[]
    {
        ("Buttons", typeof(Buttons)),

        ("MaterialColorRoles", typeof(MaterialColorRoles)),
        ("MaterialTypeScale", typeof(MaterialTypeScale)),
        ("MaterialButtons", typeof(MaterialButtons)),
        ("MaterialSelection", typeof(MaterialSelection)),
        ("MaterialTextFields", typeof(MaterialTextFields)),
        ("MaterialContainment", typeof(MaterialContainment)),
        ("MaterialProgress", typeof(MaterialProgress))
    };
}
