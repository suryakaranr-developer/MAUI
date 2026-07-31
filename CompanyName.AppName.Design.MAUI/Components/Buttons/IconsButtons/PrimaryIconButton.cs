using CompanyName.AppName.Design.MAUI.Enumerations;

namespace CompanyName.AppName.Design.MAUI.Components.Buttons.IconsButtons;

/// <summary>
/// The Primary Icon Button allows users to make a choice with a click or tap. The button’s container can only include an icon, supporting text can be added near container.
/// </summary>
public class PrimaryIconButton : ImageButton
{
    /// <summary>
    /// Gets and sets the size of the primary icon button. Default is Large.
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

    public static readonly BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(ButtonSize), typeof(PrimaryIconButton), propertyChanged: SizePropertyChanged);

    public PrimaryIconButton() : base()
    {
        Shadow = new Shadow()
        {
            Brush = new SolidColorBrush(Color.FromArgb("#ff000000")),
            Offset = new Point(0, ShadowElevation.Raised),
            Radius = (float) ShadowElevation.Raised,
            Opacity = 0.5f
        };
        Pressed += PrimaryIconButton_Pressed;
        Released += PrimaryIconButton_Released;
    }

    public static void SizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (PrimaryIconButton) bindable;

        if(DeviceInfo.Current.Platform == DevicePlatform.Android)
        {

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
                    control.Padding = 16;
                    control.CornerRadius = 28;
                    control.HeightRequest = 88;
                    control.WidthRequest = 88;
                    break;
            }
        }
        else
        {
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

    }

    void PrimaryIconButton_Pressed(object? sender, EventArgs e)
    {
        ShadowElevation.AnimateTo(this, ShadowElevation.Flat, ShadowElevation.Below);
    }

    void PrimaryIconButton_Released(object? sender, EventArgs e)
    {
        ShadowElevation.AnimateTo(this, ShadowElevation.Raised, ShadowElevation.Below);
    }
}

