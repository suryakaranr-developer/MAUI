namespace CompanyName.AppName.Extensions.MAUI.Markdown.Enumerations;

/// <summary>
/// Markdown Element Types
/// </summary>
public enum MarkdownElementType
{
    /// <summary>
    /// Unknown
    /// </summary>
    Unknown,
    /// <summary>
    /// Markdown step
    /// <para>This is used to incorporate steps for the state engine.</para>
    /// </summary>
    [Match(@"^:Step \[[0-9]+\]")]
    Step,
    /// <summary>
    /// Markdown script
    /// <para>This is used to match the script section of the markdown</para>
    /// </summary>
    [Match(@"^(:Script\s)(.*)")]
    Script,
    /// <summary>
    /// Markdown header
    /// </summary>
    [Match(@"^(#{1,6}\s)(.*)")]
    MarkdownHeader,
    /// <summary>
    /// Markdown button
    /// </summary>
    [Match(@"^(:Button[2-3]?\s)(.*)")]
    MarkdownButton,
    /// <summary>
    /// Markdown branding
    /// </summary>
    [Match(@":Branding\s\(.*?\)")]
    MarkdownBranding
}

/// <summary>
/// Xaml Header Type
/// <para>Used to match markdown to corresponding FDS text component.</para>
/// </summary>
public enum XamlHeaderType
{
    /// <summary>
    /// Unknown
    /// </summary>
    Unknown,
    /// <summary>
    /// Large Title
    /// <para>Matches Header 1 to <see cref="Design.LargeTitle"/>.</para>
    /// </summary>
    [Match(@"^(#{1}\s)(.*)")]
    LargeTitle,
    /// <summary>
    /// Title 1
    /// <para>Matches Header 2 to <see cref="Design.Title1"/>.</para>
    /// </summary>
    [Match(@"^(#{2}\s)(.*)")]
    Title1,
    /// <summary>
    /// Title 2
    /// <para>Matches Header 3 to <see cref="Design.Title2"/>.</para>
    /// </summary>
    [Match(@"^(#{3}\s)(.*)")]
    Title2,
    /// <summary>
    /// Title 3
    /// <para>Matches Header 4 to <see cref="Design.Title3"/>.</para>
    /// </summary>
    [Match(@"^(#{4}\s)(.*)")]
    Title3,
    /// <summary>
    /// Title 4
    /// <para>Matches Header 5 to <see cref="Design.Title4"/>.</para>
    /// </summary>
    [Match(@"^(#{5}\s)(.*)")]
    Title4,
    /// <summary>
    /// Subtitle
    /// <para>Matches Header 6 to <see cref="Design.Subtitle"/>.</para>
    /// </summary>
    [Match(@"^(#{6}\s)(.*)")]
    Subtitle,
}

/// <summary>
/// Xaml Button type
/// <para>Used to match markdown to corresponding FDS button component.</para>
/// </summary>
public enum XamlButtonType
{
    /// <summary>
    /// Unknown
    /// </summary>
    Unknown,
    /// <summary>
    /// Primary Button
    /// <para>Matches domain specific markdown button to <see cref="Design.PrimaryButton"/></para>
    /// </summary>
    [Match(@"^(:Button\s)(.*)")]
    PrimaryButton,
    /// <summary>
    /// Secondary Button
    /// <para>Matches domain specific markdown button to <see cref="Design.SecondaryButton"/></para>
    /// </summary>
    [Match(@"^(:Button2\s)(.*)")]
    SecondaryButton,
    /// <summary>
    /// Tertiary Button
    /// <para>Matches domain specific markdown button to <see cref="Design.TertiaryButton"/></para>
    /// </summary>
    [Match(@"^(:Button3\s)(.*)")]
    TertiaryButton
}

public enum XamlBrandingType
{
    /// <summary>
    /// Unkown
    /// </summary>
    Unknown,
    /// <summary>
    /// Logo
    /// <para>Matches domain specific markdown branding to <see cref="Design.AppLogo"/></para>
    /// </summary>
    [Match(@"\(logo\)")]
    Logo
}

/// <summary>
/// Match Attribute
/// <para>This is a helper class to allow regex match attributes on enum types.</para>
/// </summary>
internal class MatchAttribute : Attribute
{
    public string Regex { get; set; }

    public MatchAttribute(string regex)
    {
        Regex = regex;
    }
}
