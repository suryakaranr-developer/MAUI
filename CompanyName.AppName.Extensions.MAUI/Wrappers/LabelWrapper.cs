
using CompanyName.AppName.Design.MAUI.Extensions;
using CompanyName.AppName.Extensions.MAUI.Enumerations;
using CompanyName.AppName.Design.MAUI.Typography;
using CompanyName.AppName.Extensions.MAUI.Contracts;
using Jint;
using Jint.Native;


public class LabelWrapper : ComponentWrapperBase
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

    string _alignment;

    public string alignment
    {
        get
        {
            return _alignment;
        }
        set
        {
            _alignment = value;
            UpdateAlignment();
        }
    }

    public string getAlignment()
    {
        return alignment;
    }

    public void setAlignment(string alignment)
    {
        this.alignment = alignment;
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

    Label _label;

    Label _nativeLabel
    {
        get
        {
            if (_label == null)
            {
                _label = (Label) NativeView;
            }
            return _label;
        }
    }

    FormattedString _formattedString;

    public LabelWrapper(Engine engine, JsValue thisObject, string componentName, object attributes, object[] children)
        : base(engine, thisObject, null, componentName, attributes, children)
    {
        InitializeNativeView();

        if (Attributes != null)
        {
            foreach (var attr in Attributes)
            {
                if (attr.Key == "alignment")
                {
                    alignment = attr.Value.ToString();
                }
                else if (attr.Key == "color")
                {
                    color = attr.Value.ToString();
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

        if (children != null)
        {
            if (children == null)
            {
                text = string.Empty;
            }
            else if (children.Length == 1 && children[0].GetType() == typeof(string))
            {
                text = children[0].ToString();
            }
            else
            {
                UpdateFormattedText(children);
                _nativeLabel.FormattedText = _formattedString;
            }
        }
    }

    void InitializeNativeView()
    {
        switch (XamlType)
        {
            case  XamlType.ExtraLargeTitle:
                NativeView = new ExtraLargeTitle();
                break;
            case  XamlType.LargeTitle:
                NativeView = new LargeTitle();
                break;
            case  XamlType.Title1:
                NativeView = new Title1();
                break;
            case  XamlType.Title2:
                NativeView = new Title2();
                break;
            case  XamlType.Title3:
                NativeView = new Title3();
                break;
            case  XamlType.Title4:
                NativeView = new Title4();
                break;
            case  XamlType.Subtitle:
                NativeView = new Title4();
                break;
            case  XamlType.Body1:
                NativeView = new Body1();
                break;
            case  XamlType.Body2:
                NativeView = new Body2();
                break;
            case  XamlType.Caption:
                NativeView = new Caption();
                break;
            case  XamlType.Display1:
                NativeView = new Display1();
                break;
            case  XamlType.Display2:
                NativeView = new Display2();
                break;
            case  XamlType.Display3:
                NativeView = new Display3();
                break;
        }

        if (NativeView == null)
        {
            throw new InvalidCastException($"Error casting object {ComponentName} to {XamlType.ToString()}");
        }

        var label = (Label) NativeView;
        label.BindingContext = this;
        label.SetBinding(Label.TextProperty, nameof(text));
    }

    void UpdateFormattedText(object[] children)
    {
        if (_formattedString == null)
        {
            _formattedString = new FormattedString();
        }

        foreach (var child in children)
        {
            if (child is string)
            {
                var spanWrapper = new SpanWrapper(_engine, ThisObject, "span", null, new object[] { child.ToString() });
                var span = (Span) spanWrapper.NativeView;
                span.CharacterSpacing = _nativeLabel.CharacterSpacing;
                span.FontSize = _nativeLabel.FontSize;
                span.FontFamily = _nativeLabel.FontFamily;
                _formattedString.Spans.Add((Span) spanWrapper.GetNativeView());
            }
            else if (child is IComponentWrapper)
            {
                var spanWrapper = child as IComponentWrapper;
                var span = (Span) spanWrapper.GetNativeView();

                span.CharacterSpacing = _nativeLabel.CharacterSpacing;
                span.FontSize = _nativeLabel.FontSize;

                if (spanWrapper.GetType() == typeof(SpanWrapper))
                {
                    var spanWrap = (SpanWrapper) spanWrapper;
                    spanWrap.DefaultFont = _nativeLabel.FontFamily;
                    spanWrap.style = spanWrap.style;
                }

                _formattedString.Spans.Add(span);
            }
            else if (child is object[])
            {
                UpdateFormattedText(child as object[]);
            }
        }
    }

    void UpdateAlignment()
    {
        TextAlignment al;
        switch (alignment)
        {
            case "center":
                al = TextAlignment.Center;
                break;
            case "end":
                al = TextAlignment.End;
                break;
            default:
                al = TextAlignment.Start;
                break;
        }
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var label = (Label) NativeView;
            label.HorizontalTextAlignment = al;
        });
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
            var label = (Label) NativeView;
            label.SetAppThemeColor(Label.TextColorProperty, Application.Current.GetResource<Color>(cl), Application.Current.GetResource<Color>(cl + "_Dark"));
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
            var label = (Label) NativeView;
            label.TextDecorations = decorations;
        });
    }

    void UpdateStyle()
    {
        string font = "MontserratRegular";
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
            var label = (Label) NativeView;
            label.FontFamily = font;
        });
    }
}

