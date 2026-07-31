using CompanyName.AppName.Design.MAUI.Extensions;
using Jint;
using Jint.Native;


public class EditorWrapper : ComponentWrapperBase
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

    double _heightRequest;

    public double height
    {
        get
        {
            return _heightRequest;
        }
        set
        {
            _heightRequest = value;
            OnPropertyChanged();
        }
    }

    public double getHeight()
    {
        return height;
    }

    public void setHeight(double height)
    {
        this.height = height;
    }

    public EditorWrapper(Engine engine, JsValue thisObject, string componentName, object attributes, object[] children)
        : base(engine, thisObject, null, componentName, attributes, children)
    {
        InitializeNativeView();

        ParseAttributes();
    }

    void InitializeNativeView()
    {
        var editor = new Editor();
        editor.BackgroundColor = Application.Current.RequestedTheme == AppTheme.Light ?
            Application.Current.GetResource<Color>("SurfaceHighlight") : Application.Current.GetResource<Color>("SurfaceHighlight_Dark");
        editor.TextColor = Application.Current.RequestedTheme == AppTheme.Light ?
            Application.Current.GetResource<Color>("PrimaryInfo") : Application.Current.GetResource<Color>("PrimaryInfo_Dark");
        editor.FontFamily = "MontserratLight";
        editor.FontSize = 14;


        editor.BindingContext = this;
        editor.SetBinding(Editor.TextProperty, nameof(text), BindingMode.TwoWay);
        editor.SetBinding(VisualElement.HeightRequestProperty, nameof(height), BindingMode.TwoWay);

        // TODO: implement OnTextChanged event

        NativeView = editor;
    }

    void ParseAttributes()
    {
        if (Attributes != null)
        {
            foreach (var attr in Attributes)
            {
                if (attr.Key == "text")
                {
                    text = attr.Value.ToString();
                }
                else if (attr.Key == "height")
                {
                    height = ((double) attr.Value);
                }
            }
        }
    }
}

