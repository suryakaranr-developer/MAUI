namespace CompanyName.AppName.Design.MAUI.Components.Buttons;

/// <summary>
/// Drives the press and release elevation of a component's <see cref="Shadow"/>.
/// </summary>
/// <remarks>
/// <para>
/// The animation is committed against the element, so it is stepped by the platform ticker on
/// the UI thread. Nothing is marshalled and no background work is started.
/// </para>
/// <para>
/// Every travel is committed under one name, so a release interrupts an in-flight press
/// (and the other way round) rather than the two writing to the same shadow at once.
/// Interpolation starts from the elevation the shadow is actually at, so an interrupted
/// travel carries on smoothly instead of jumping.
/// </para>
/// </remarks>
internal static class ShadowElevation
{
    /// <summary>
    /// Elevation, in device-independent units, of a fully raised shadow.
    /// </summary>
    public const double Raised = 5.0;

    /// <summary>
    /// Elevation of a shadow sitting flat against the surface.
    /// </summary>
    public const double Flat = 0.0;

    /// <summary>
    /// Casts the shadow below the element.
    /// </summary>
    public const double Below = 1.0;

    /// <summary>
    /// Casts the shadow above the element.
    /// </summary>
    public const double Above = -1.0;

    /// <summary>
    /// Name the travel is committed under.
    /// </summary>
    const string AnimationName = "ShadowElevation";

    /// <summary>
    /// Time a full travel between <see cref="Flat"/> and <see cref="Raised"/> takes.
    /// </summary>
    const uint FullTravelMilliseconds = 150;

    /// <summary>
    /// Curve the elevation travels along.
    /// </summary>
    static readonly Easing TravelEasing = Easing.CubicOut;

    /// <summary>
    /// Animates the element's shadow to the given elevation.
    /// </summary>
    /// <param name="element">Element owning the shadow.</param>
    /// <param name="elevation">Elevation to settle on, normally <see cref="Flat"/> or <see cref="Raised"/>.</param>
    /// <param name="direction">Side the shadow is cast on, <see cref="Below"/> or <see cref="Above"/>.</param>
    /// <param name="restRadius">
    /// Radius stamped once the shadow settles at <see cref="Flat"/>, letting a component suppress
    /// its shadow completely at rest.
    /// </param>
    /// <param name="settled">Called on the UI thread once the shadow settles, unless the travel was interrupted.</param>
    public static void AnimateTo(
        VisualElement element,
        double elevation,
        double direction,
        float restRadius = (float) Flat,
        Action? settled = null)
    {
        var shadow = element.Shadow;
        if (shadow == null)
        {
            return;
        }

        element.AbortAnimation(AnimationName);

        // A resting shadow can carry a suppressed radius, which is not an elevation to
        // interpolate from.
        var from = Math.Max(Flat, shadow.Radius);
        var distance = Math.Abs(elevation - from);

        if (distance < double.Epsilon)
        {
            Settle(shadow, elevation, direction, restRadius, settled);
            return;
        }

        var length = Math.Max(1u, (uint) (FullTravelMilliseconds * distance / Raised));

        new Animation(value => Apply(shadow, value, direction), from, elevation, TravelEasing)
            .Commit(element, AnimationName, length: length, finished: (_, interrupted) =>
            {
                if (!interrupted)
                {
                    Settle(shadow, elevation, direction, restRadius, settled);
                }
            });
    }

    /// <summary>
    /// Pins the shadow to its final elevation and hands control back to the caller.
    /// </summary>
    static void Settle(Shadow shadow, double elevation, double direction, float restRadius, Action? settled)
    {
        Apply(shadow, elevation, direction);

        if (elevation <= Flat)
        {
            shadow.Radius = restRadius;
        }

        settled?.Invoke();
    }

    /// <summary>
    /// Positions the shadow for one elevation.
    /// </summary>
    static void Apply(Shadow shadow, double elevation, double direction)
    {
        shadow.Offset = new Point(0, elevation * direction);
        shadow.Radius = (float) elevation;
    }
}
