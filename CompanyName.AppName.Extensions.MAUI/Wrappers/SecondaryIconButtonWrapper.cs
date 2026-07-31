using CompanyName.AppName.Design.MAUI.Components.Buttons.IconsButtons;
using CompanyName.AppName.Design.MAUI.Enumerations;
using Jint;
using Jint.Native;

namespace CompanyName.AppName.Extensions.MAUI.Wrappers;

public class SecondaryIconButtonWrapper : ComponentWrapperBase
{

    string _source;

    public string source
    {
        get
        {
            return _source;
        }
        set
        {
            _source = Path.Combine(BasePath, value);
            UpdateSource();
        }
    }

    public string getSource()
    {
        return source;
    }

    public void setSource(string source)
    {
        this.source = source;
    }

    string _size;

    public string size
    {
        get
        {
            return _size;
        }
        set
        {
            _size = value;
            UpdateSize();
        }
    }

    public string getSize()
    {
        return size;
    }

    public void setSize(string size)
    {
        this.size = size;
    }

    public Func<JsValue, JsValue[], JsValue> clicked { get; set; }

    public SecondaryIconButtonWrapper(Engine engine, JsValue thisObject, string basePath, string componentName, object attributes, object[] children)
        : base(engine, thisObject, basePath, componentName, attributes, children)
    {
        InitializeNativeView();

        if (Attributes != null)
        {
            foreach (var attr in Attributes)
            {
                if (attr.Key == "source")
                {
                    source = attr.Value.ToString();
                }
                else if (attr.Key == "clicked")
                {
                    clicked = (Func<JsValue, JsValue[], JsValue>) attr.Value;
                }
                else if (attr.Key == "size")
                {
                    size = attr.Value.ToString();
                }
            }
        }
    }

    void InitializeNativeView()
    {
        var button = new SecondaryIconButton();

        button.BindingContext = this;
        button.SetBinding(VisualElement.IsEnabledProperty, nameof(CanExecute));
        button.Command = new Command(() => InvokeScriptCallback(() =>
        {
            var value = JsValue.FromObject(_engine, this);
            clicked?.Invoke(ThisObject, new JsValue[] { value });
        }));
        NativeView = button;
    }

    void UpdateSource()
    {
        var imageButton = (ImageButton) NativeView;
        var darkSource = source.Remove(source.IndexOf(".png")) + "_dark.png";
        if (!File.Exists(darkSource))
        {
            darkSource = source;
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            imageButton.SetAppTheme(ImageButton.SourceProperty, source, darkSource);
        });
    }

    void UpdateSize()
    {
        var iconbutton = (SecondaryIconButton) NativeView;

        ButtonSize buttonSize = ButtonSize.Large;
        switch (size)
        {
            case "small":
                buttonSize = ButtonSize.Small;
                break;
            case "medium":
                buttonSize = ButtonSize.Large;
                break;
            case "large":
                buttonSize = ButtonSize.ExtraLarge;
                break;
        }
        MainThread.BeginInvokeOnMainThread(() =>
        {
            iconbutton.Size = buttonSize;
        });
    }
}

