namespace CompanyName.AppName.Design.MAUI.Material.Tokens;

/// <summary>
/// The font families the Material layer draws with.
/// </summary>
/// <remarks>
/// These names must match the families registered at startup. Referencing them through constants
/// rather than as string literals means a mismatch is a compile error at the point of use, instead
/// of a silent fall back to the system face at runtime.
/// </remarks>
public static class MaterialFont
{
    /// <summary>Roboto at weight 400, the Material default for body text.</summary>
    public const string Regular = "RobotoRegular";

    /// <summary>Roboto at weight 500, used by titles, labels and buttons.</summary>
    public const string Medium = "RobotoMedium";

    /// <summary>Roboto at weight 700, for the heaviest emphasis.</summary>
    public const string Bold = "RobotoBold";
}
