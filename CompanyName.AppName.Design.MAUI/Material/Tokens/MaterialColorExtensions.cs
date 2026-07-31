namespace CompanyName.AppName.Design.MAUI.Material.Tokens;

/// <summary>
/// Binds component properties to Material colour roles.
/// </summary>
public static class MaterialColorExtensions
{
    /// <summary>
    /// Points a colour property at a Material role.
    /// </summary>
    /// <param name="element">Element to colour.</param>
    /// <param name="property">Colour property to set.</param>
    /// <param name="role">The role to draw the property in.</param>
    /// <remarks>
    /// Binds through <c>DynamicResource</c> to the role's entry in <see cref="MaterialColorResources"/>,
    /// so the property follows a theme change live and a value set locally in XAML still wins over
    /// this default.
    /// </remarks>
    public static void SetRole(this VisualElement element, BindableProperty property, MaterialRole role)
    {
        element.SetDynamicResource(property, MaterialColorResources.ColorKey(role));
    }

    /// <summary>
    /// Points a brush property at a Material colour role.
    /// </summary>
    /// <param name="element">Element to colour.</param>
    /// <param name="property">Brush property to set.</param>
    /// <param name="role">The role to draw the property in.</param>
    public static void SetRoleBrush(this VisualElement element, BindableProperty property, MaterialRole role)
    {
        element.SetDynamicResource(property, MaterialColorResources.BrushKey(role));
    }

    /// <summary>
    /// Returns the role at a given opacity, for the state layers Material draws over a component
    /// while it is hovered, focused, pressed or disabled.
    /// </summary>
    /// <param name="color">Colour to fade.</param>
    /// <param name="opacity">Fraction of full opacity, 0 to 1.</param>
    public static Color At(this Color color, double opacity)
    {
        return color.WithAlpha((float) opacity);
    }
}
