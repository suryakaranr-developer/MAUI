using System.Runtime.CompilerServices;

namespace CompanyName.AppName.Design.MAUI.Components.Buttons.StandardButtons;

/// <summary>
/// <para>The Secondary Button is for medium-priority actions. Use the Primary Button for high-priority actions and the Tertiary Button for low-priority actions.</para>
/// <para>Use the Secondary Button when several buttons will appear together or as lower priority companion to the Primary Button.</para>
/// </summary>
public class SecondaryButton : Button
{
    bool _sizeAllocated;

    double _oldCharacterSpacing;

    public SecondaryButton() : base()
    {
        //Radius of the Button Can be set Here
        //CornerRadius = 16;
        Shadow = new Shadow()
        {
            Brush = new SolidColorBrush(Color.FromArgb("#ff000000")),
            Offset = new Point(0, 0),
            Radius = 0,
            Opacity = 0.5f
        };

        Pressed += SecondaryButton_Pressed;
        Released += SecondaryButton_Released;
    }

    void SecondaryButton_Pressed(object? sender, EventArgs e)
    {
        if (IsEnabled)
        {
            ShadowElevation.AnimateTo(this, ShadowElevation.Raised, ShadowElevation.Above);
        }
    }

    void SecondaryButton_Released(object? sender, EventArgs e)
    {
        if (IsEnabled)
        {
            ShadowElevation.AnimateTo(this, ShadowElevation.Flat, ShadowElevation.Above);
        }
    }

    // TODO: This can be removed when updating to MAUI 8. See MAUI issue #10293
    protected override void OnSizeAllocated(double width, double height)
    {
        if (!string.IsNullOrEmpty(Text))
        {
            if (!_sizeAllocated && WidthRequest == -1 && HorizontalOptions.Alignment != LayoutAlignment.Fill)
            {
                _sizeAllocated = true;
                this.WidthRequest = width + ((Text.Length - 1) * CharacterSpacing);
                return;
            }
        }
        base.OnSizeAllocated(width, height);
    }

    protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanging(propertyName);
        if (propertyName == "Text")
        {
            if (_sizeAllocated)
            {
                WidthRequest = -1;
                _sizeAllocated = false;
            }
        }
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (_sizeAllocated && HorizontalOptions.Alignment != LayoutAlignment.Fill)
        {
            if (propertyName == "CharacterSpacing")
            {
                if (CharacterSpacing < _oldCharacterSpacing)
                {
                    WidthRequest = Width - ((Text.Length - 1) * (_oldCharacterSpacing - CharacterSpacing));
                }
                if (CharacterSpacing > _oldCharacterSpacing)
                {
                    WidthRequest = Width + ((Text.Length - 1) * (CharacterSpacing - _oldCharacterSpacing));
                }
            }
        }
        _oldCharacterSpacing = CharacterSpacing;
    }
}

