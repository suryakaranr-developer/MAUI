using CompanyName.AppName.Design.MAUI.Material.Tokens;
using Microsoft.Maui.Controls.Shapes;

namespace CompanyName.AppName.Design.MAUI.Material.Components;

/// <summary>
/// A Material Design 3 switch.
/// </summary>
/// <remarks>
/// Toggles a single setting on or off, taking effect immediately. Material's switch grows its
/// thumb when selected as well as moving it, so the state is legible without relying on colour
/// alone. Built from a track and thumb rather than the platform switch, which cannot express
/// that shape change.
/// </remarks>
public class MaterialSwitch : Grid
{
    /// <summary>Width of the track.</summary>
    const double TrackWidth = 52;

    /// <summary>Height of the track.</summary>
    const double TrackHeight = 32;

    /// <summary>Diameter of the thumb while off.</summary>
    const double ThumbOff = 16;

    /// <summary>Diameter of the thumb while on.</summary>
    const double ThumbOn = 24;

    /// <summary>Name the thumb travel is committed under, so a fast toggle cancels the last one.</summary>
    const string ToggleAnimation = "MaterialSwitchToggle";

    /// <summary>Time the thumb takes to cross the track.</summary>
    const uint ToggleDuration = 180;

    readonly Border _track;

    readonly Ellipse _thumb;

    /// <summary>
    /// Whether the switch is on.
    /// </summary>
    public bool IsToggled
    {
        get => (bool) GetValue(IsToggledProperty);
        set => SetValue(IsToggledProperty, value);
    }

    public static readonly BindableProperty IsToggledProperty = BindableProperty.Create(
        nameof(IsToggled),
        typeof(bool),
        typeof(MaterialSwitch),
        defaultValue: false,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (bindable, _, newValue) =>
        {
            var control = (MaterialSwitch) bindable;
            control.ApplyState(animated: true);
            control.Toggled?.Invoke(control, new ToggledEventArgs((bool) newValue));
        });

    /// <summary>
    /// Raised when the switch changes state.
    /// </summary>
    public event EventHandler<ToggledEventArgs>? Toggled;

    public MaterialSwitch()
    {
        WidthRequest = TrackWidth;
        HeightRequest = TrackHeight;
        HorizontalOptions = LayoutOptions.Start;
        VerticalOptions = LayoutOptions.Center;

        _track = new Border
        {
            StrokeShape = new RoundRectangle { CornerRadius = TrackHeight / 2 },
            StrokeThickness = 2,
            Padding = 0
        };

        _thumb = new Ellipse
        {
            WidthRequest = ThumbOff,
            HeightRequest = ThumbOff,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Center
        };

        Children.Add(_track);
        Children.Add(_thumb);

        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) =>
        {
            if (IsEnabled)
            {
                IsToggled = !IsToggled;
            }
        };
        GestureRecognizers.Add(tap);

        ApplyState(animated: false);
    }

    /// <summary>
    /// Moves and paints the thumb and track for the current state.
    /// </summary>
    /// <param name="animated">Whether to travel to the new position or jump to it.</param>
    void ApplyState(bool animated)
    {
        var size = IsToggled ? ThumbOn : ThumbOff;

        // The thumb is inset from whichever end it rests against by the same margin, so the
        // larger selected thumb sits closer to the edge than the smaller unselected one.
        var inset = (TrackHeight - size) / 2;
        var target = IsToggled ? TrackWidth - size - inset : inset;

        if (IsToggled)
        {
            _track.SetRoleBrush(Border.BackgroundProperty, MaterialRole.Primary);
            _track.SetRoleBrush(Border.StrokeProperty, MaterialRole.Primary);
            _thumb.SetRoleBrush(Shape.FillProperty, MaterialRole.OnPrimary);
        }
        else
        {
            _track.SetRoleBrush(Border.BackgroundProperty, MaterialRole.SurfaceContainerHighest);
            _track.SetRoleBrush(Border.StrokeProperty, MaterialRole.Outline);
            _thumb.SetRoleBrush(Shape.FillProperty, MaterialRole.Outline);
        }

        this.AbortAnimation(ToggleAnimation);

        if (!animated)
        {
            _thumb.WidthRequest = size;
            _thumb.HeightRequest = size;
            _thumb.TranslationX = target;
            return;
        }

        var fromX = _thumb.TranslationX;
        var fromSize = _thumb.WidthRequest;

        new Animation(progress =>
        {
            _thumb.TranslationX = fromX + ((target - fromX) * progress);

            var current = fromSize + ((size - fromSize) * progress);
            _thumb.WidthRequest = current;
            _thumb.HeightRequest = current;
        })
        .Commit(this, ToggleAnimation, length: ToggleDuration, easing: Easing.CubicInOut);
    }
}
