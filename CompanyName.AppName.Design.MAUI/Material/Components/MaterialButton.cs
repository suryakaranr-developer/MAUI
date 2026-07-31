using CompanyName.AppName.Design.MAUI.Material.Enumerations;
using CompanyName.AppName.Design.MAUI.Material.Tokens;

namespace CompanyName.AppName.Design.MAUI.Material.Components;

/// <summary>
/// A Material Design 3 common button.
/// </summary>
/// <remarks>
/// The five variants differ only in how they are painted, so they are one control with a
/// <see cref="Variant"/> rather than five classes. Setting the variant re-paints the button
/// against the colour roles for that variant, in both themes.
/// </remarks>
public class MaterialButton : Button
{
    /// <summary>Height of every common button, per the Material spec.</summary>
    const double ButtonHeight = 40;

    /// <summary>Horizontal padding for buttons with a container.</summary>
    const double ContainedPadding = 24;

    /// <summary>Horizontal padding for text buttons, which have no container to inset from.</summary>
    const double TextPadding = 12;

    /// <summary>
    /// Which of the five common button variants to paint.
    /// </summary>
    public MaterialButtonVariant Variant
    {
        get => (MaterialButtonVariant) GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public static readonly BindableProperty VariantProperty = BindableProperty.Create(
        nameof(Variant),
        typeof(MaterialButtonVariant),
        typeof(MaterialButton),
        defaultValue: MaterialButtonVariant.Filled,
        propertyChanged: (bindable, _, _) => ((MaterialButton) bindable).ApplyVariant());

    public MaterialButton()
    {
        HeightRequest = ButtonHeight;
        MinimumHeightRequest = ButtonHeight;
        CornerRadius = (int) MaterialShape.Full;
        HorizontalOptions = LayoutOptions.Start;

        FontFamily = MaterialFont.Medium;
        FontSize = 14;
        CharacterSpacing = 0.1;

        ApplyVariant();
    }

    /// <summary>
    /// Paints the button for its current variant.
    /// </summary>
    void ApplyVariant()
    {
        Padding = new Thickness(Variant == MaterialButtonVariant.Text ? TextPadding : ContainedPadding, 0);
        BorderWidth = Variant == MaterialButtonVariant.Outlined ? 1 : 0;
        this.SetElevation(Variant == MaterialButtonVariant.Elevated ? MaterialElevation.Level1 : MaterialElevation.Level0);

        switch (Variant)
        {
            case MaterialButtonVariant.Filled:
                this.SetRole(BackgroundColorProperty, MaterialRole.Primary);
                this.SetRole(TextColorProperty, MaterialRole.OnPrimary);
                break;

            case MaterialButtonVariant.Elevated:
                this.SetRole(BackgroundColorProperty, MaterialRole.SurfaceContainerLow);
                this.SetRole(TextColorProperty, MaterialRole.Primary);
                break;

            case MaterialButtonVariant.FilledTonal:
                this.SetRole(BackgroundColorProperty, MaterialRole.SecondaryContainer);
                this.SetRole(TextColorProperty, MaterialRole.OnSecondaryContainer);
                break;

            case MaterialButtonVariant.Outlined:
                BackgroundColor = Colors.Transparent;
                this.SetRole(TextColorProperty, MaterialRole.Primary);
                this.SetRole(BorderColorProperty, MaterialRole.Outline);
                break;

            case MaterialButtonVariant.Text:
                BackgroundColor = Colors.Transparent;
                this.SetRole(TextColorProperty, MaterialRole.Primary);
                break;
        }
    }
}
