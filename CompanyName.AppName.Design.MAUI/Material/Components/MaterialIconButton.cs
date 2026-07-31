using CompanyName.AppName.Design.MAUI.Material.Enumerations;
using CompanyName.AppName.Design.MAUI.Material.Tokens;

namespace CompanyName.AppName.Design.MAUI.Material.Components;

/// <summary>
/// A Material Design 3 icon button.
/// </summary>
/// <remarks>
/// The container is 40dp so the button meets the minimum touch target even though the icon
/// inside it is 24dp. Set <see cref="IsToggle"/> to have the button hold a selected state, which
/// Material paints at a higher emphasis than the unselected one.
/// </remarks>
public class MaterialIconButton : ImageButton
{
    /// <summary>Diameter of the container, and the minimum comfortable touch target.</summary>
    const double ContainerSize = 40;

    /// <summary>Inset that leaves the Material standard 24dp icon inside the container.</summary>
    const double IconInset = 8;

    /// <summary>
    /// Which of the four icon button variants to paint.
    /// </summary>
    public MaterialIconButtonVariant Variant
    {
        get => (MaterialIconButtonVariant) GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public static readonly BindableProperty VariantProperty = BindableProperty.Create(
        nameof(Variant),
        typeof(MaterialIconButtonVariant),
        typeof(MaterialIconButton),
        defaultValue: MaterialIconButtonVariant.Standard,
        propertyChanged: (bindable, _, _) => ((MaterialIconButton) bindable).ApplyVariant());

    /// <summary>
    /// Whether tapping the button toggles <see cref="IsSelected"/> rather than only raising a click.
    /// </summary>
    public bool IsToggle
    {
        get => (bool) GetValue(IsToggleProperty);
        set => SetValue(IsToggleProperty, value);
    }

    public static readonly BindableProperty IsToggleProperty = BindableProperty.Create(
        nameof(IsToggle),
        typeof(bool),
        typeof(MaterialIconButton),
        defaultValue: false);

    /// <summary>
    /// Whether a toggle button is currently selected.
    /// </summary>
    public bool IsSelected
    {
        get => (bool) GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(
        nameof(IsSelected),
        typeof(bool),
        typeof(MaterialIconButton),
        defaultValue: false,
        propertyChanged: (bindable, _, _) => ((MaterialIconButton) bindable).ApplyVariant());

    public MaterialIconButton()
    {
        WidthRequest = ContainerSize;
        HeightRequest = ContainerSize;
        CornerRadius = (int) (ContainerSize / 2);
        Padding = IconInset;
        Aspect = Aspect.AspectFit;

        Clicked += OnClicked;

        ApplyVariant();
    }

    void OnClicked(object? sender, EventArgs e)
    {
        if (IsToggle)
        {
            IsSelected = !IsSelected;
        }
    }

    /// <summary>
    /// Paints the container for the current variant and selection state.
    /// </summary>
    void ApplyVariant()
    {
        // An unselected toggle drops to the low emphasis painting of its variant, which is what
        // makes the selected state legible without a second control.
        var selected = !IsToggle || IsSelected;

        BorderWidth = Variant == MaterialIconButtonVariant.Outlined ? 1 : 0;

        switch (Variant)
        {
            case MaterialIconButtonVariant.Standard:
                BackgroundColor = Colors.Transparent;
                break;

            case MaterialIconButtonVariant.Filled:
                if (selected)
                {
                    this.SetRole(BackgroundColorProperty, MaterialRole.Primary);
                }
                else
                {
                    this.SetRole(BackgroundColorProperty, MaterialRole.SurfaceContainerHighest);
                }
                break;

            case MaterialIconButtonVariant.Tonal:
                if (selected)
                {
                    this.SetRole(BackgroundColorProperty, MaterialRole.SecondaryContainer);
                }
                else
                {
                    this.SetRole(BackgroundColorProperty, MaterialRole.SurfaceContainerHighest);
                }
                break;

            case MaterialIconButtonVariant.Outlined:
                if (selected && IsToggle)
                {
                    this.SetRole(BackgroundColorProperty, MaterialRole.InverseSurface);
                }
                else
                {
                    BackgroundColor = Colors.Transparent;
                }
                this.SetRole(BorderColorProperty, MaterialRole.Outline);
                break;
        }
    }
}
