using System.Runtime.CompilerServices;

namespace CompanyName.AppName.Design.MAUI.Components.Buttons.StandardButtons;

/// <summary>
/// The Primary Button can be used to guide users to action through a click or tap. The Primary Button should be the most prominent visual element.
/// <list type="number">
///	<item>Use to Create Focus: There should be no more than one Primary Button per section or scroll, as this button is meant to be a focus point.</item>
///	<item>Alignment: Within any experience the primary button should always be center aligned.</item>
/// </list>
/// </summary>
public class PrimaryButton : Button
{
    bool _sizeAllocated;

    double _oldCharacterSpacing;

    public PrimaryButton() : base()
    {
        //Radius of the Button Can be set Here
        //CornerRadius = 16;
        Shadow = new Shadow()
        {
            Brush = new SolidColorBrush(Color.FromArgb("#ff000000")),
            Offset = new Point(0, ShadowElevation.Raised),
            Radius = (float) ShadowElevation.Raised,
            Opacity = 0.5f
        };

        Pressed += PrimaryButton_Pressed;
        Released += PrimaryButton_Released;
    }

    void PrimaryButton_Pressed(object? sender, EventArgs e)
    {
        if (IsEnabled)
        {
            ShadowElevation.AnimateTo(this, ShadowElevation.Flat, ShadowElevation.Below);
        }
    }

    void PrimaryButton_Released(object? sender, EventArgs e)
    {
        if (IsEnabled)
        {
            ShadowElevation.AnimateTo(this, ShadowElevation.Raised, ShadowElevation.Below);
        }
    }

    // TODO: This can be removed when updating to MAUI 8. See MAUI issue #10293
    protected override void OnSizeAllocated(double width, double height)
    {
        if (!_sizeAllocated && WidthRequest == -1 && HorizontalOptions.Alignment != LayoutAlignment.Fill)
        {
            _sizeAllocated = true;
            this.WidthRequest = width + ((string.IsNullOrEmpty(Text) ? 0 : Text.Length - 1) * CharacterSpacing);
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

