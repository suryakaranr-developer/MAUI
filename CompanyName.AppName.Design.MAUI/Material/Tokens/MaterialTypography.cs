namespace CompanyName.AppName.Design.MAUI.Material.Tokens;

/// <summary>
/// Base for the Material Design 3 type scale.
/// </summary>
/// <remarks>
/// Each style in the scale carries its own size, line height, weight and tracking. Applying them
/// in the constructor means a page can place <c>&lt;material:BodyLarge Text="..."/&gt;</c> and get
/// the right metrics without a style lookup, which is what keeps the scale hard to use wrongly.
/// </remarks>
public abstract class MaterialLabel : Label
{
    /// <summary>Roboto at weight 400, the Material default.</summary>
    protected const string Regular = MaterialFont.Regular;

    /// <summary>Roboto at weight 500, used by titles and labels.</summary>
    protected const string Medium = MaterialFont.Medium;

    /// <param name="size">Font size in scale-independent pixels.</param>
    /// <param name="lineHeight">Line height in scale-independent pixels.</param>
    /// <param name="tracking">Letter spacing.</param>
    /// <param name="family">Registered font family name.</param>
    protected MaterialLabel(double size, double lineHeight, double tracking, string family)
    {
        FontSize = size;
        FontFamily = family;
        CharacterSpacing = tracking;

        // Material publishes line height in absolute units while MAUI takes a multiple of the
        // font size, so the scale's figures are converted rather than restated.
        LineHeight = lineHeight / size;

        this.SetRole(TextColorProperty, MaterialRole.OnSurface);
    }
}

/// <summary>Largest display style. For short, high-impact text on its own.</summary>
public class DisplayLarge : MaterialLabel
{
    public DisplayLarge() : base(57, 64, -0.25, Regular) { }
}

/// <summary>Display style for hero text that shares the screen with other content.</summary>
public class DisplayMedium : MaterialLabel
{
    public DisplayMedium() : base(45, 52, 0, Regular) { }
}

/// <summary>Smallest display style, for hero text on compact screens.</summary>
public class DisplaySmall : MaterialLabel
{
    public DisplaySmall() : base(36, 44, 0, Regular) { }
}

/// <summary>For high-emphasis text at the top of a screen or a major section.</summary>
public class HeadlineLarge : MaterialLabel
{
    public HeadlineLarge() : base(32, 40, 0, Regular) { }
}

/// <summary>The default headline. For text introducing a section of content.</summary>
public class HeadlineMedium : MaterialLabel
{
    public HeadlineMedium() : base(28, 36, 0, Regular) { }
}

/// <summary>For headlines where vertical space is tight.</summary>
public class HeadlineSmall : MaterialLabel
{
    public HeadlineSmall() : base(24, 32, 0, Regular) { }
}

/// <summary>For medium-emphasis text, such as an app bar title.</summary>
public class TitleLarge : MaterialLabel
{
    public TitleLarge() : base(22, 28, 0, Regular) { }
}

/// <summary>The default title. For the primary line of a list item or card.</summary>
public class TitleMedium : MaterialLabel
{
    public TitleMedium() : base(16, 24, 0.15, Medium) { }
}

/// <summary>For titles subordinate to the content they label.</summary>
public class TitleSmall : MaterialLabel
{
    public TitleSmall() : base(14, 20, 0.1, Medium) { }
}

/// <summary>For longer passages of body text.</summary>
public class BodyLarge : MaterialLabel
{
    public BodyLarge() : base(16, 24, 0.5, Regular) { }
}

/// <summary>The default body style, for most running text.</summary>
public class BodyMedium : MaterialLabel
{
    public BodyMedium() : base(14, 20, 0.25, Regular) { }
}

/// <summary>For supporting text and captions.</summary>
public class BodySmall : MaterialLabel
{
    public BodySmall() : base(12, 16, 0.4, Regular) { }
}

/// <summary>For text inside components, such as a button's label.</summary>
public class LabelLarge : MaterialLabel
{
    public LabelLarge() : base(14, 20, 0.1, Medium) { }
}

/// <summary>For text inside compact components.</summary>
public class LabelMedium : MaterialLabel
{
    public LabelMedium() : base(12, 16, 0.5, Medium) { }
}

/// <summary>Smallest label, for the tightest components such as badges.</summary>
public class LabelSmall : MaterialLabel
{
    public LabelSmall() : base(11, 16, 0.5, Medium) { }
}
