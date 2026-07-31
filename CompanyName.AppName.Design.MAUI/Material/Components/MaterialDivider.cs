using CompanyName.AppName.Design.MAUI.Material.Tokens;

namespace CompanyName.AppName.Design.MAUI.Material.Components;

/// <summary>
/// A Material Design 3 divider.
/// </summary>
/// <remarks>
/// A one-pixel rule separating groups of content. Uses the outline variant role rather than the
/// outline role, because a divider is decorative and should not read as a component boundary.
/// </remarks>
public class MaterialDivider : BoxView
{
    /// <summary>
    /// Inset from the leading edge, for dividers separating items that share an icon column.
    /// </summary>
    public double Inset
    {
        get => (double) GetValue(InsetProperty);
        set => SetValue(InsetProperty, value);
    }

    public static readonly BindableProperty InsetProperty = BindableProperty.Create(
        nameof(Inset),
        typeof(double),
        typeof(MaterialDivider),
        defaultValue: 0.0,
        propertyChanged: (bindable, _, newValue) =>
        {
            var control = (MaterialDivider) bindable;
            control.Margin = new Thickness((double) newValue, 0, 0, 0);
        });

    public MaterialDivider()
    {
        HeightRequest = 1;
        HorizontalOptions = LayoutOptions.Fill;

        this.SetRole(ColorProperty, MaterialRole.OutlineVariant);
    }
}
