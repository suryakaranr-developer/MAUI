using CompanyName.AppName.Design.MAUI.Material.Tokens;
using Microsoft.Maui.Controls.Shapes;

namespace CompanyName.AppName.Design.MAUI.SystemDesigns.Material;

public partial class MaterialColorRoles : ContentPage
{
    /// <summary>
    /// A role paired with the content role guaranteed to be readable on it.
    /// </summary>
    /// <param name="Name">Role name as it appears in the Material spec.</param>
    /// <param name="Role">The container role.</param>
    /// <param name="OnRole">The role for content drawn on it.</param>
    record Swatch(string Name, MaterialRole Role, MaterialRole OnRole);

    public MaterialColorRoles()
    {
        InitializeComponent();

        foreach (var swatch in BuildSwatches())
        {
            Swatches.Add(Render(swatch));
        }
    }

    /// <summary>
    /// The roles worth showing, grouped the way the Material spec presents them.
    /// </summary>
    static IEnumerable<Swatch> BuildSwatches()
    {
        yield return new Swatch("Primary", MaterialRole.Primary, MaterialRole.OnPrimary);
        yield return new Swatch("Primary container", MaterialRole.PrimaryContainer, MaterialRole.OnPrimaryContainer);
        yield return new Swatch("Secondary", MaterialRole.Secondary, MaterialRole.OnSecondary);
        yield return new Swatch("Secondary container", MaterialRole.SecondaryContainer, MaterialRole.OnSecondaryContainer);
        yield return new Swatch("Tertiary", MaterialRole.Tertiary, MaterialRole.OnTertiary);
        yield return new Swatch("Tertiary container", MaterialRole.TertiaryContainer, MaterialRole.OnTertiaryContainer);
        yield return new Swatch("Error", MaterialRole.Error, MaterialRole.OnError);
        yield return new Swatch("Error container", MaterialRole.ErrorContainer, MaterialRole.OnErrorContainer);
        yield return new Swatch("Surface", MaterialRole.Surface, MaterialRole.OnSurface);
        yield return new Swatch("Surface variant", MaterialRole.SurfaceVariant, MaterialRole.OnSurfaceVariant);
        yield return new Swatch("Surface container lowest", MaterialRole.SurfaceContainerLowest, MaterialRole.OnSurface);
        yield return new Swatch("Surface container low", MaterialRole.SurfaceContainerLow, MaterialRole.OnSurface);
        yield return new Swatch("Surface container", MaterialRole.SurfaceContainer, MaterialRole.OnSurface);
        yield return new Swatch("Surface container high", MaterialRole.SurfaceContainerHigh, MaterialRole.OnSurface);
        yield return new Swatch("Surface container highest", MaterialRole.SurfaceContainerHighest, MaterialRole.OnSurface);
        yield return new Swatch("Inverse surface", MaterialRole.InverseSurface, MaterialRole.InverseOnSurface);
        yield return new Swatch("Outline", MaterialRole.Outline, MaterialRole.Surface);
        yield return new Swatch("Outline variant", MaterialRole.OutlineVariant, MaterialRole.OnSurface);
    }

    /// <summary>
    /// Draws one role as a filled block labelled with its name and its resolved hex value.
    /// </summary>
    static View Render(Swatch swatch)
    {
        var name = new LabelLarge { Text = swatch.Name };
        name.SetRole(Label.TextColorProperty, swatch.OnRole);

        // The hex is read from the live scheme so the page shows what is actually being applied
        // in the theme currently in force.
        var hex = new BodySmall { Text = MaterialColorScheme.Current[swatch.Role].ToArgbHex() };
        hex.SetRole(Label.TextColorProperty, swatch.OnRole);

        var block = new Border
        {
            Padding = new Thickness(16, 12),
            StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = MaterialShape.Medium },
            Content = new VerticalStackLayout { Spacing = 2, Children = { name, hex } }
        };
        block.SetRoleBrush(Border.BackgroundProperty, swatch.Role);

        return block;
    }
}
