namespace CompanyName.AppName.Design.MAUI.Material.Tokens;

/// <summary>
/// The Material Design 3 colour roles for one theme.
/// </summary>
/// <remarks>
/// <para>
/// A role says what a colour is for rather than what it looks like, which is what lets a
/// component be re-themed without touching the component. Every "On" role is the colour of
/// content drawn on top of the role it is named after, and is guaranteed to contrast with it.
/// </para>
/// <para>
/// This is the single source of truth for Material colour in the library. Components read it
/// through <see cref="MaterialColorExtensions.SetRole"/> rather than holding literals, so a
/// change here reaches every component.
/// </para>
/// </remarks>
public sealed class MaterialColorScheme
{
    /// <summary>
    /// The baseline light scheme.
    /// </summary>
    public static MaterialColorScheme Light { get; } = new()
    {
        Primary = Color.FromArgb("#6750A4"),
        OnPrimary = Color.FromArgb("#FFFFFF"),
        PrimaryContainer = Color.FromArgb("#EADDFF"),
        OnPrimaryContainer = Color.FromArgb("#21005D"),

        Secondary = Color.FromArgb("#625B71"),
        OnSecondary = Color.FromArgb("#FFFFFF"),
        SecondaryContainer = Color.FromArgb("#E8DEF8"),
        OnSecondaryContainer = Color.FromArgb("#1D192B"),

        Tertiary = Color.FromArgb("#7D5260"),
        OnTertiary = Color.FromArgb("#FFFFFF"),
        TertiaryContainer = Color.FromArgb("#FFD8E4"),
        OnTertiaryContainer = Color.FromArgb("#31111D"),

        Error = Color.FromArgb("#B3261E"),
        OnError = Color.FromArgb("#FFFFFF"),
        ErrorContainer = Color.FromArgb("#F9DEDC"),
        OnErrorContainer = Color.FromArgb("#410E0B"),

        Surface = Color.FromArgb("#FEF7FF"),
        OnSurface = Color.FromArgb("#1D1B20"),
        SurfaceVariant = Color.FromArgb("#E7E0EC"),
        OnSurfaceVariant = Color.FromArgb("#49454F"),

        SurfaceContainerLowest = Color.FromArgb("#FFFFFF"),
        SurfaceContainerLow = Color.FromArgb("#F7F2FA"),
        SurfaceContainer = Color.FromArgb("#F3EDF7"),
        SurfaceContainerHigh = Color.FromArgb("#ECE6F0"),
        SurfaceContainerHighest = Color.FromArgb("#E6E0E9"),

        Outline = Color.FromArgb("#79747E"),
        OutlineVariant = Color.FromArgb("#CAC4D0"),

        InverseSurface = Color.FromArgb("#322F35"),
        InverseOnSurface = Color.FromArgb("#F5EFF7"),
        InversePrimary = Color.FromArgb("#D0BCFF"),

        Scrim = Color.FromArgb("#000000"),
        Shadow = Color.FromArgb("#000000")
    };

    /// <summary>
    /// The baseline dark scheme.
    /// </summary>
    public static MaterialColorScheme Dark { get; } = new()
    {
        Primary = Color.FromArgb("#D0BCFF"),
        OnPrimary = Color.FromArgb("#381E72"),
        PrimaryContainer = Color.FromArgb("#4F378B"),
        OnPrimaryContainer = Color.FromArgb("#EADDFF"),

        Secondary = Color.FromArgb("#CCC2DC"),
        OnSecondary = Color.FromArgb("#332D41"),
        SecondaryContainer = Color.FromArgb("#4A4458"),
        OnSecondaryContainer = Color.FromArgb("#E8DEF8"),

        Tertiary = Color.FromArgb("#EFB8C8"),
        OnTertiary = Color.FromArgb("#492532"),
        TertiaryContainer = Color.FromArgb("#633B48"),
        OnTertiaryContainer = Color.FromArgb("#FFD8E4"),

        Error = Color.FromArgb("#F2B8B5"),
        OnError = Color.FromArgb("#601410"),
        ErrorContainer = Color.FromArgb("#8C1D18"),
        OnErrorContainer = Color.FromArgb("#F9DEDC"),

        Surface = Color.FromArgb("#141218"),
        OnSurface = Color.FromArgb("#E6E0E9"),
        SurfaceVariant = Color.FromArgb("#49454F"),
        OnSurfaceVariant = Color.FromArgb("#CAC4D0"),

        SurfaceContainerLowest = Color.FromArgb("#0F0D13"),
        SurfaceContainerLow = Color.FromArgb("#1D1B20"),
        SurfaceContainer = Color.FromArgb("#211F26"),
        SurfaceContainerHigh = Color.FromArgb("#2B2930"),
        SurfaceContainerHighest = Color.FromArgb("#36343B"),

        Outline = Color.FromArgb("#938F99"),
        OutlineVariant = Color.FromArgb("#49454F"),

        InverseSurface = Color.FromArgb("#E6E0E9"),
        InverseOnSurface = Color.FromArgb("#322F35"),
        InversePrimary = Color.FromArgb("#6750A4"),

        Scrim = Color.FromArgb("#000000"),
        Shadow = Color.FromArgb("#000000")
    };

    /// <summary>
    /// The scheme matching the theme the application is currently rendering in.
    /// </summary>
    public static MaterialColorScheme Current =>
        Application.Current?.RequestedTheme == AppTheme.Dark ? Dark : Light;

    /// <summary>
    /// The colour this scheme assigns to a role.
    /// </summary>
    /// <param name="role">The role to resolve.</param>
    /// <remarks>
    /// Lets the scheme be walked by role, which is what the resource dictionary does when it
    /// projects every role into a resource, rather than each caller naming properties by hand.
    /// </remarks>
    public Color this[MaterialRole role] => role switch
    {
        MaterialRole.Primary => Primary,
        MaterialRole.OnPrimary => OnPrimary,
        MaterialRole.PrimaryContainer => PrimaryContainer,
        MaterialRole.OnPrimaryContainer => OnPrimaryContainer,
        MaterialRole.Secondary => Secondary,
        MaterialRole.OnSecondary => OnSecondary,
        MaterialRole.SecondaryContainer => SecondaryContainer,
        MaterialRole.OnSecondaryContainer => OnSecondaryContainer,
        MaterialRole.Tertiary => Tertiary,
        MaterialRole.OnTertiary => OnTertiary,
        MaterialRole.TertiaryContainer => TertiaryContainer,
        MaterialRole.OnTertiaryContainer => OnTertiaryContainer,
        MaterialRole.Error => Error,
        MaterialRole.OnError => OnError,
        MaterialRole.ErrorContainer => ErrorContainer,
        MaterialRole.OnErrorContainer => OnErrorContainer,
        MaterialRole.Surface => Surface,
        MaterialRole.OnSurface => OnSurface,
        MaterialRole.SurfaceVariant => SurfaceVariant,
        MaterialRole.OnSurfaceVariant => OnSurfaceVariant,
        MaterialRole.SurfaceContainerLowest => SurfaceContainerLowest,
        MaterialRole.SurfaceContainerLow => SurfaceContainerLow,
        MaterialRole.SurfaceContainer => SurfaceContainer,
        MaterialRole.SurfaceContainerHigh => SurfaceContainerHigh,
        MaterialRole.SurfaceContainerHighest => SurfaceContainerHighest,
        MaterialRole.Outline => Outline,
        MaterialRole.OutlineVariant => OutlineVariant,
        MaterialRole.InverseSurface => InverseSurface,
        MaterialRole.InverseOnSurface => InverseOnSurface,
        MaterialRole.InversePrimary => InversePrimary,
        MaterialRole.Scrim => Scrim,
        MaterialRole.Shadow => Shadow,
        _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
    };

    /// <summary>The highest emphasis colour, for the primary action on a screen.</summary>
    public required Color Primary { get; init; }

    /// <summary>Content drawn on <see cref="Primary"/>.</summary>
    public required Color OnPrimary { get; init; }

    /// <summary>A lower emphasis fill standing in for <see cref="Primary"/>.</summary>
    public required Color PrimaryContainer { get; init; }

    /// <summary>Content drawn on <see cref="PrimaryContainer"/>.</summary>
    public required Color OnPrimaryContainer { get; init; }

    /// <summary>For less prominent components alongside the primary action.</summary>
    public required Color Secondary { get; init; }

    /// <summary>Content drawn on <see cref="Secondary"/>.</summary>
    public required Color OnSecondary { get; init; }

    /// <summary>A lower emphasis fill standing in for <see cref="Secondary"/>.</summary>
    public required Color SecondaryContainer { get; init; }

    /// <summary>Content drawn on <see cref="SecondaryContainer"/>.</summary>
    public required Color OnSecondaryContainer { get; init; }

    /// <summary>A contrasting accent, for balancing primary and secondary.</summary>
    public required Color Tertiary { get; init; }

    /// <summary>Content drawn on <see cref="Tertiary"/>.</summary>
    public required Color OnTertiary { get; init; }

    /// <summary>A lower emphasis fill standing in for <see cref="Tertiary"/>.</summary>
    public required Color TertiaryContainer { get; init; }

    /// <summary>Content drawn on <see cref="TertiaryContainer"/>.</summary>
    public required Color OnTertiaryContainer { get; init; }

    /// <summary>For values and states that need attention.</summary>
    public required Color Error { get; init; }

    /// <summary>Content drawn on <see cref="Error"/>.</summary>
    public required Color OnError { get; init; }

    /// <summary>A lower emphasis fill standing in for <see cref="Error"/>.</summary>
    public required Color ErrorContainer { get; init; }

    /// <summary>Content drawn on <see cref="ErrorContainer"/>.</summary>
    public required Color OnErrorContainer { get; init; }

    /// <summary>The background behind scrollable content.</summary>
    public required Color Surface { get; init; }

    /// <summary>Content drawn on <see cref="Surface"/>.</summary>
    public required Color OnSurface { get; init; }

    /// <summary>An alternate surface, for differentiating areas of a screen.</summary>
    public required Color SurfaceVariant { get; init; }

    /// <summary>Lower emphasis content drawn on a surface.</summary>
    public required Color OnSurfaceVariant { get; init; }

    /// <summary>The surface tone furthest from the surrounding surface.</summary>
    public required Color SurfaceContainerLowest { get; init; }

    /// <summary>A surface tone one step from the surrounding surface.</summary>
    public required Color SurfaceContainerLow { get; init; }

    /// <summary>The default fill for components sitting on a surface.</summary>
    public required Color SurfaceContainer { get; init; }

    /// <summary>A surface tone for components needing more separation.</summary>
    public required Color SurfaceContainerHigh { get; init; }

    /// <summary>The surface tone with the most separation from the surface.</summary>
    public required Color SurfaceContainerHighest { get; init; }

    /// <summary>Borders on components that need a clear boundary.</summary>
    public required Color Outline { get; init; }

    /// <summary>Decorative boundaries, such as dividers.</summary>
    public required Color OutlineVariant { get; init; }

    /// <summary>A surface that inverts against the current theme.</summary>
    public required Color InverseSurface { get; init; }

    /// <summary>Content drawn on <see cref="InverseSurface"/>.</summary>
    public required Color InverseOnSurface { get; init; }

    /// <summary>An accent readable on <see cref="InverseSurface"/>.</summary>
    public required Color InversePrimary { get; init; }

    /// <summary>The wash drawn over content behind a modal.</summary>
    public required Color Scrim { get; init; }

    /// <summary>The colour a component's shadow is cast in.</summary>
    public required Color Shadow { get; init; }
}
