namespace CompanyName.AppName.Design.MAUI.Typography;

/// <summary>
/// Reserved for future use cases
/// </summary>
public class ExtraLargeTitle : Label { }

/// <summary>
/// Reserved for future use cases
/// </summary>
public class LargeTitle : Label { }

/// <summary>
/// Reserved for future use cases
/// </summary>
public class Title1 : Label { }

/// <summary>
/// For page titles located at the beginning of a view.
/// </summary>
public class Title2 : Label { }

/// <summary>
/// For section titles at the middle of a view.
/// </summary>
public class Title3 : Label { }

/// <summary>
/// For lower-level titles within a view or a section.
/// </summary>
public class Title4 : Label { }

/// <summary>
/// <list type="number">
/// <item>For subordinate tiles which compliment main titles.</item>
/// <item>For small titles when positioning and spacing constraints apply.</item>
/// </list>
/// </summary>
public class Subtitle : Label { }

/// <summary>
/// <list type="number">
/// <item>The base text style for regular text elements.</item>
/// <item>For descriptive text when pairing with <see cref="Title2"/> or larger title styles.</item>
/// </list>
/// </summary>
public class Body1 : Label { }

/// <summary>
/// <list type="number">
/// <item>The base text style for regular text elements when the screen real estate is limited.</item>
/// <item>Body2 can be used together with <see cref="Body1"/> for increasing clarity and effectiveness in terms of sectioning.</item>
/// </list>
/// </summary>
public class Body2 : Label { }

/// <summary>
/// For ancillary pieces of information related to main content.
/// </summary>
public class Caption : Label { }

/// <summary>
/// For primary information in graphical elements.
/// </summary>
public class Display1 : Label { }

/// <summary>
/// For secondary information in graphical elements.
/// </summary>
public class Display2 : Label { }

/// <summary>
/// For tertiary information in graphical elements.
/// </summary>
public class Display3 : Label { }

/// <summary>
/// For displaying a hyperlink inside a label
/// </summary>
public class Hyperlink : Span
{
    private string _url;

    /// <summary>
    /// Gets and sets the source of the hyperlink.
    /// </summary>
    public string Url
    {
        get
        {
            return _url;
        }
        set
        {
            _url = value;
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ToolTipProperties.SetText(this, _url);
            });
        }
    }

    /// <summary>
    /// Gets and sets the browser launch mode.
    /// </summary>
    public BrowserLaunchMode LaunchMode { get; set; }

    /// <summary>
    /// Creates and instance of hyperlink
    /// </summary>
    public Hyperlink() : base()
    {
        // FIXME: Span gesture recognizers are broken in MAUI Issue #4734
        TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += (s, e) =>
        {
            var uri = new Uri(Url);
            Browser.Default.OpenAsync(uri, LaunchMode);
        };
        GestureRecognizers.Add(tapGestureRecognizer);
    }
}