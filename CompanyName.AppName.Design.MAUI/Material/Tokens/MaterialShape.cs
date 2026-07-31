namespace CompanyName.AppName.Design.MAUI.Material.Tokens;

/// <summary>
/// The Material Design 3 corner radius scale.
/// </summary>
/// <remarks>
/// Shape carries meaning in Material: the larger the radius, the more a component reads as a
/// self-contained object rather than part of the surface behind it.
/// </remarks>
public static class MaterialShape
{
    /// <summary>Square corners, for components that meet the edge of the screen.</summary>
    public const double None = 0;

    /// <summary>4dp, for text fields and small containers.</summary>
    public const double ExtraSmall = 4;

    /// <summary>8dp, for chips and menus.</summary>
    public const double Small = 8;

    /// <summary>12dp, for cards and small floating action buttons.</summary>
    public const double Medium = 12;

    /// <summary>16dp, for floating action buttons and larger containers.</summary>
    public const double Large = 16;

    /// <summary>28dp, for dialogs, large floating action buttons and bottom sheets.</summary>
    public const double ExtraLarge = 28;

    /// <summary>
    /// A stadium, where the radius is always half the height.
    /// </summary>
    /// <remarks>
    /// Corner radius clamps to half the shorter side, so any value past that produces the same
    /// shape. Used by buttons, chip-like components and switches.
    /// </remarks>
    public const double Full = 1000;
}
