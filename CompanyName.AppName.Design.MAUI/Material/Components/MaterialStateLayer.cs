using CompanyName.AppName.Design.MAUI.Material.Tokens;

namespace CompanyName.AppName.Design.MAUI.Material.Components;

/// <summary>
/// The translucent overlay Material draws over a component while it is hovered or pressed.
/// </summary>
/// <remarks>
/// <para>
/// The state layer is what gives a component the sense of responding to touch. It is a fill in the
/// content colour whose opacity rises on interaction — 8% hovered, 10% pressed, per the Material
/// spec — and nothing at rest.
/// </para>
/// <para>
/// The controls that need one are composed from <see cref="Border"/> and layouts rather than a
/// native button, so they get no press feedback for free; this supplies it. The overlay is added
/// as the lowest child of the content so it tints the container behind the label rather than the
/// label itself, and it sits inside the shape so the parent clips it to the same corners.
/// </para>
/// </remarks>
internal static class MaterialStateLayer
{
    /// <summary>Overlay opacity while the pointer is over the control.</summary>
    const double HoverOpacity = 0.08;

    /// <summary>Overlay opacity while the control is pressed.</summary>
    const double PressOpacity = 0.10;

    /// <summary>Time the overlay takes to fade between states.</summary>
    const uint FadeDuration = 100;

    /// <summary>Name the fade is committed under, so a new state cancels the last.</summary>
    const string FadeAnimation = "MaterialStateLayerFade";

    /// <summary>
    /// Builds a state-layer overlay tinted with a role and wires it to a control's interaction.
    /// </summary>
    /// <param name="control">The element whose hover and press drive the overlay.</param>
    /// <param name="tint">Role the overlay is filled with, normally the control's content colour.</param>
    /// <returns>
    /// The overlay to add as the lowest child of the control's content, sized to fill it.
    /// </returns>
    public static BoxView Attach(View control, MaterialRole tint)
    {
        var overlay = new BoxView
        {
            Opacity = 0,
            InputTransparent = true
        };
        overlay.SetRole(BoxView.ColorProperty, tint);

        // Hover is a desktop and pointer concept; on touch only the press events fire, and both
        // resolve to opacity, so no platform check is needed.
        var pointer = new PointerGestureRecognizer();
        pointer.PointerEntered += (_, _) => FadeTo(overlay, HoverOpacity);
        pointer.PointerExited += (_, _) => FadeTo(overlay, 0);
        pointer.PointerPressed += (_, _) => FadeTo(overlay, PressOpacity);
        pointer.PointerReleased += (_, _) => FadeTo(overlay, 0);
        control.GestureRecognizers.Add(pointer);

        return overlay;
    }

    /// <summary>
    /// Fades the overlay to a target opacity, cancelling any fade already running.
    /// </summary>
    static void FadeTo(BoxView overlay, double opacity)
    {
        overlay.AbortAnimation(FadeAnimation);
        overlay.Animate(
            FadeAnimation,
            new Animation(value => overlay.Opacity = value, overlay.Opacity, opacity),
            length: FadeDuration);
    }
}
