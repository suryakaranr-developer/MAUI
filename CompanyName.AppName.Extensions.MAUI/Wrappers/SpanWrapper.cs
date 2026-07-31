using CompanyName.AppName.Design.MAUI.Extensions;
using CompanyName.AppName.Extensions.MAUI.Enumerations;
using CompanyName.AppName.Design.MAUI.Typography;
using Jint;
using Jint.Native;


public class SpanWrapper : ComponentWrapperBase
{
    string _text;

    public string text
    {
        get
        {
            return _text;
        }
        set
        {
            _text = value;
            OnPropertyChanged();
        }
    }

    public string getText()
    {
        return text;
    }

    public void setText(string text)
    {
        this.text = text;
    }

    string _color;

    public string color
    {
        get
        {
            return _color;
        }
        set
        {
            _color = value;
            UpdateColor();
        }
    }

    public string getColor()
    {
        return color;
    }

    public void setColor(string color)
    {
        this.color = color;
    }

    string _url;

    public string url
    {
        get
        {
            return _url;
        }
        set
        {
            _url = value;
            ((Hyperlink) NativeView).Url = _url;
        }
    }

    public string getUrl()
    {
        return url;
    }

    public void setUrl(string url)
    {
        this.url = url;
    }

    string _decoration;

    public string decoration
    {
        get
        {
            return _decoration;
        }
        set
        {
            _decoration = value;
            UpdateDecoration();
        }
    }

    public string getDecoration()
    {
        return decoration;
    }

    public void setDecoration(string decoration)
    {
        this.decoration = decoration;
    }

    string _style;

    public string style
    {
        get
        {
            return _style;
        }
        set
        {
            _style = value;
            UpdateStyle();
        }
    }

    public string getStyle()
    {
        return style;
    }

    public void setStyle(string style)
    {
        this.style = style;
    }

    public string DefaultFont { get; set; } = "MontserratRegular";

    public SpanWrapper(Engine engine, JsValue thisObject, string componentName, object attributes, object[] children)
        : base(engine, thisObject, null, componentName, attributes, children)
    {
        InitializeNativeView();

        if (Attributes != null)
        {
            foreach (var attr in Attributes)
            {
                if (attr.Key == "color")
                {
                    color = attr.Value.ToString();
                }
                else if (attr.Key == "url")
                {
                    url = attr.Value.ToString();
                }
                else if (attr.Key == "decoration")
                {
                    decoration = attr.Value.ToString();
                }
                else if (attr.Key == "style")
                {
                    style = attr.Value.ToString();
                }
            }
        }

        if (children != null && children.Length > 0)
        {
            text = string.Empty;
            foreach (var child in children)
            {
                text += child == null ? string.Empty : child.ToString();
            }
        }
        else
        {
            text = string.Empty;
        }
    }

    void InitializeNativeView()
    {
        switch (XamlType)
        {
            case XamlType.Span:
                NativeView = new Span();
                break;
            case XamlType.HyperLink:
                NativeView = new Hyperlink();
                break;
        }

        if (NativeView == null)
        {
            throw new InvalidCastException($"Error casting object {ComponentName} to {XamlType.ToString()}");
        }

        var span = (Span) NativeView;
        span.BindingContext = this;
        span.SetBinding(Span.TextProperty, nameof(text));
    }

    void UpdateColor()
    {
        string cl;
        switch (color)
        {
            case "primaryblue":
                cl = "PrimaryBlue";
                break;
            case "disabled":
                cl = "Disabled";
                break;
            case "secondaryinfo":
                cl = "SecondaryInfo";
                break;
            case "success":
                cl = "Success";
                break;
            case "warning":
                cl = "Warning";
                break;
            case "error":
                cl = "Error";
                break;
            default:
                cl = "PrimaryInfo";
                break;
        }
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var span = (Span) NativeView;
            span.SetAppThemeColor(Label.TextColorProperty, Application.Current.GetResource<Color>(cl), Application.Current.GetResource<Color>(cl + "_Dark"));
        });
    }

    void UpdateDecoration()
    {
        TextDecorations decorations = TextDecorations.None;
        switch (_decoration)
        {
            case "underline":
                decorations = TextDecorations.Underline;
                break;
            case "strikethrough":
                decorations = TextDecorations.Strikethrough;
                break;
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            var span = (Span) NativeView;
            span.TextDecorations = decorations;
        });
    }

    void UpdateStyle()
    {
        string font = DefaultFont;
        switch (style)
        {
            case "bold":
                font = "MontserratBold";
                break;
            case "medium":
                font = "MontserratMedium";
                break;
            case "cond-bold":
                font = "MontserratBold";
                break;
            case "cond-medium":
                font = "MontserratMedium";
                break;
            case "cond-regular":
                font = "MontserratRegular";
                break;
            case "light":
                font = "MontserratLight";
                break;
            case "italic":
                font = "MontserratLightItalic";
                break;
        }
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var span = (Span) NativeView;
            span.FontFamily = font;
        });
    }
}

