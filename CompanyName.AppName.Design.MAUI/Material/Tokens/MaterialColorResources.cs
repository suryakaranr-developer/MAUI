namespace CompanyName.AppName.Design.MAUI.Material.Tokens;

/// <summary>
/// Projects every Material colour role into application resources, as both a colour and a brush.
/// </summary>
/// <remarks>
/// <para>
/// Merging this into <c>Application.Resources</c> is what lets XAML reach a role with
/// <c>{DynamicResource MaterialPrimary}</c> and code reach it with
/// <see cref="MaterialColorExtensions.SetRole"/>, from one source of truth.
/// </para>
/// <para>
/// The entries are refreshed when the system theme changes. Because consumers bind with
/// <c>DynamicResource</c>, replacing the values here repaints them live, without rebuilding the
/// tree — which a set of fixed colours applied in a constructor could not do.
/// </para>
/// </remarks>
public sealed class MaterialColorResources : ResourceDictionary
{
    public MaterialColorResources()
    {
        Populate(MaterialColorScheme.Current);

        // The dictionary outlives no window, so unsubscription is not a concern; it lives for the
        // life of the application it themes.
        if (Application.Current is not null)
        {
            Application.Current.RequestedThemeChanged += (_, _) => Populate(MaterialColorScheme.Current);
        }
    }

    /// <summary>
    /// The resource key a role's colour is stored under, for example <c>MaterialPrimary</c>.
    /// </summary>
    public static string ColorKey(MaterialRole role) => $"Material{role}";

    /// <summary>
    /// The resource key a role's brush is stored under, for example <c>MaterialPrimaryBrush</c>.
    /// </summary>
    public static string BrushKey(MaterialRole role) => $"Material{role}Brush";

    /// <summary>
    /// Writes every role from a scheme into this dictionary, replacing what is there.
    /// </summary>
    void Populate(MaterialColorScheme scheme)
    {
        foreach (var role in Enum.GetValues<MaterialRole>())
        {
            var color = scheme[role];
            this[ColorKey(role)] = color;
            this[BrushKey(role)] = new SolidColorBrush(color);
        }
    }
}
