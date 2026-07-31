namespace CompanyName.AppName.Design.MAUI.Material.Tokens;

/// <summary>
/// The Material Design 3 elevation scale.
/// </summary>
/// <remarks>
/// Elevation separates a component from the surface behind it. Material expresses it as both a
/// shadow and a surface tone; this library uses the shadow, and components pick the matching
/// surface container role themselves.
/// </remarks>
public static class MaterialElevation
{
    /// <summary>Level 0. Flat against the surface.</summary>
    public const int Level0 = 0;

    /// <summary>Level 1, for elevated cards and buttons at rest.</summary>
    public const int Level1 = 1;

    /// <summary>Level 2, for the resting state of components that lift on press.</summary>
    public const int Level2 = 2;

    /// <summary>Level 3, for floating action buttons and dialogs.</summary>
    public const int Level3 = 3;

    /// <summary>Level 4, for navigation drawers.</summary>
    public const int Level4 = 4;

    /// <summary>Level 5, for a component being dragged.</summary>
    public const int Level5 = 5;

    /// <summary>
    /// Raises an element to one elevation level.
    /// </summary>
    /// <param name="element">Element to elevate.</param>
    /// <param name="level">A level from <see cref="Level0"/> to <see cref="Level5"/>.</param>
    /// <remarks>
    /// Clearing a shadow means assigning null, which MAUI accepts but does not annotate as
    /// nullable. The suppression lives here so no component has to carry it.
    /// </remarks>
    public static void SetElevation(this VisualElement element, int level)
    {
        element.Shadow = Create(level)!;
    }

    /// <summary>
    /// Builds the shadow for one elevation level.
    /// </summary>
    /// <param name="level">A level from <see cref="Level0"/> to <see cref="Level5"/>.</param>
    /// <returns>The shadow, or <c>null</c> at level 0 where nothing should be drawn.</returns>
    public static Shadow? Create(int level)
    {
        if (level <= Level0)
        {
            return null;
        }

        // Material's shadows grow their blur faster than their offset, so a lifted component
        // reads as further from the surface rather than merely lower down it.
        return new Shadow
        {
            Brush = new SolidColorBrush(MaterialColorScheme.Current.Shadow),
            Offset = new Point(0, level),
            Radius = level * 2,
            Opacity = 0.18f + (level * 0.02f)
        };
    }
}
