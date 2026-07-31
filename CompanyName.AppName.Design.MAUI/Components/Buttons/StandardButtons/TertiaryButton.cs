using System.Runtime.CompilerServices;

namespace CompanyName.AppName.Design.MAUI.Components.Buttons.StandardButtons;

/// <summary>
/// <para>Also Known as Ghost Button</para>
/// <para>This is a subtle call to action used for lower priority actions and can be paired with the Primary and Secondary Button.The Tertiary Button can be only text, or a combination of text with an icon.</para>
/// </summary>
public class TertiaryButton : Button
{
    /// <summary>
    /// Radius the ghost button's shadow rests at, suppressing it entirely until pressed.
    /// </summary>
    const float SuppressedRadius = float.MinValue;

    bool _sizeAllocated;

    double _oldCharacterSpacing;

    public TertiaryButton() : base()
    {
        //Radius of the Button Can be set Here
        //CornerRadius = 16;
        Shadow = new Shadow()
        {
            Brush = new SolidColorBrush(Color.FromArgb("#ff000000")),
            Offset = new Point(0, 0),
            Radius = SuppressedRadius,
            Opacity = 0.5f
        };

        Pressed += TertiaryButton_Pressed;
        Released += TertiaryButton_Released;
    }

    void TertiaryButton_Pressed(object? sender, EventArgs e)
    {
        if (IsEnabled)
        {
            ShadowElevation.AnimateTo(this, ShadowElevation.Raised, ShadowElevation.Above);
        }
    }

    void TertiaryButton_Released(object? sender, EventArgs e)
    {
        if (IsEnabled)
        {
            ShadowElevation.AnimateTo(this, ShadowElevation.Flat, ShadowElevation.Above, SuppressedRadius);
        }
    }

    // TODO: This can be removed when updating to MAUI 8. See MAUI issue #10293
    protected override void OnSizeAllocated(double width, double height)
    {
        if (!_sizeAllocated && WidthRequest == -1 && HorizontalOptions.Alignment != LayoutAlignment.Fill)
        {
            _sizeAllocated = true;
            this.WidthRequest = width + ((Text.Length - 1) * CharacterSpacing);
            return;
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

