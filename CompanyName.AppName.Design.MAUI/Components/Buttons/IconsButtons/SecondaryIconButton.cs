using CompanyName.AppName.Design.MAUI.Enumerations;

namespace CompanyName.AppName.Design.MAUI.Components.Buttons.IconsButtons;

/// <summary>
/// The Secondary Icon Button container can only include an icon, supporting text can be added near the container.
/// </summary>
public class SecondaryIconButton : ImageButton
{
    /// <summary>
    /// Colour the shadow is cast in while the button is held.
    /// </summary>
    static readonly Color CastColor = Color.FromArgb("#ff000000");

    /// <summary>
    /// Colour the shadow rests in, hiding it while the button is idle.
    /// </summary>
    static readonly Color RestColor = Color.FromArgb("#00000000");

    /// <summary>
    /// Gets and sets the size of the secondary icon button. Default is Large.
    /// </summary>
    public ButtonSize Size
    {
        get
        {
            return (ButtonSize) GetValue(SizeProperty);
        }
        set
        {
            SetValue(SizeProperty, value);
        }
    }

    public static readonly BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(ButtonSize), typeof(SecondaryIconButton), propertyChanged: SizePropertyChanged);

    public SecondaryIconButton() : base()
    {
        Shadow = new Shadow()
        {
            Brush = new SolidColorBrush(RestColor),
            Offset = new Point(0, 0),
            Radius = 0,
            Opacity = 0.5f
        };
        Pressed += SecondaryIconButton_Pressed;
        Released += SecondaryIconButton_Released;
    }

    public static void SizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (SecondaryIconButton) bindable;

        switch (control.Size)
        {
            case ButtonSize.Large:
                control.Padding = 16;
                control.CornerRadius = 32;
                control.HeightRequest = 64;
                control.WidthRequest = 64;
                break;
            case ButtonSize.Small:
                control.Padding = 8;
                control.CornerRadius = 20;
                control.HeightRequest = 40;
                control.WidthRequest = 40;
                break;
            case ButtonSize.ExtraLarge:
                control.Padding = 22;
                control.CornerRadius = 44;
                control.HeightRequest = 88;
                control.WidthRequest = 88;
                break;
        }
    }

    void SecondaryIconButton_Pressed(object? sender, EventArgs e)
    {
        Shadow.Brush = new SolidColorBrush(CastColor);

        ShadowElevation.AnimateTo(this, ShadowElevation.Raised, ShadowElevation.Above);
    }

    void SecondaryIconButton_Released(object? sender, EventArgs e)
    {
        // The shadow is only hidden once it has finished sinking, otherwise it would blink
        // out rather than fade away. A press arriving mid-travel cancels the callback.
        ShadowElevation.AnimateTo(
            this,
            ShadowElevation.Flat,
            ShadowElevation.Above,
            settled: () => Shadow.Brush = new SolidColorBrush(RestColor));
    }
}
