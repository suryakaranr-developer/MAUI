using CompanyName.AppName.Design.MAUI.Material.Enumerations;
using CompanyName.AppName.Design.MAUI.Material.Tokens;
using Microsoft.Maui.Controls.Shapes;

namespace CompanyName.AppName.Design.MAUI.Material.Components;

/// <summary>
/// A Material Design 3 card.
/// </summary>
/// <remarks>
/// A container for content and actions about a single subject. The three variants differ in how
/// strongly they separate from the surface behind them: elevated casts a shadow, outlined draws a
/// border, and filled relies on tone alone.
/// </remarks>
public class MaterialCard : Border
{
    /// <summary>
    /// Which of the three card variants to paint.
    /// </summary>
    public MaterialCardVariant Variant
    {
        get => (MaterialCardVariant) GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public static readonly BindableProperty VariantProperty = BindableProperty.Create(
        nameof(Variant),
        typeof(MaterialCardVariant),
        typeof(MaterialCard),
        defaultValue: MaterialCardVariant.Elevated,
        propertyChanged: (bindable, _, _) => ((MaterialCard) bindable).ApplyVariant());

    public MaterialCard()
    {
        Padding = 16;
        StrokeShape = new RoundRectangle { CornerRadius = MaterialShape.Medium };

        ApplyVariant();
    }

    /// <summary>
    /// Paints the card for its current variant.
    /// </summary>
    void ApplyVariant()
    {
        switch (Variant)
        {
            case MaterialCardVariant.Elevated:
                this.SetRoleBrush(BackgroundProperty, MaterialRole.SurfaceContainerLow);
                StrokeThickness = 0;
                this.SetElevation(MaterialElevation.Level1);
                break;

            case MaterialCardVariant.Filled:
                this.SetRoleBrush(BackgroundProperty, MaterialRole.SurfaceContainerHighest);
                StrokeThickness = 0;
                this.SetElevation(MaterialElevation.Level0);
                break;

            case MaterialCardVariant.Outlined:
                this.SetRoleBrush(BackgroundProperty, MaterialRole.Surface);
                this.SetRoleBrush(StrokeProperty, MaterialRole.OutlineVariant);
                StrokeThickness = 1;
                this.SetElevation(MaterialElevation.Level0);
                break;
        }
    }
}
