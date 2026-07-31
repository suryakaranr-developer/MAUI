namespace CompanyName.AppName.Design.MAUI.Material.Tokens;

/// <summary>
/// The Material Design 3 colour roles, named the way the spec names them.
/// </summary>
/// <remarks>
/// A role is a strongly typed key rather than a literal, so a component asks for
/// <see cref="Primary"/> and the theme decides what that is. It is the identity shared by the
/// <see cref="MaterialColorScheme"/> that defines each role's value and the resource dictionary
/// that exposes it to XAML.
/// </remarks>
public enum MaterialRole
{
    Primary,
    OnPrimary,
    PrimaryContainer,
    OnPrimaryContainer,

    Secondary,
    OnSecondary,
    SecondaryContainer,
    OnSecondaryContainer,

    Tertiary,
    OnTertiary,
    TertiaryContainer,
    OnTertiaryContainer,

    Error,
    OnError,
    ErrorContainer,
    OnErrorContainer,

    Surface,
    OnSurface,
    SurfaceVariant,
    OnSurfaceVariant,

    SurfaceContainerLowest,
    SurfaceContainerLow,
    SurfaceContainer,
    SurfaceContainerHigh,
    SurfaceContainerHighest,

    Outline,
    OutlineVariant,

    InverseSurface,
    InverseOnSurface,
    InversePrimary,

    Scrim,
    Shadow
}
