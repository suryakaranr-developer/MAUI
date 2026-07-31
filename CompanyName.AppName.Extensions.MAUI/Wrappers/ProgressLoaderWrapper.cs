using CompanyName.AppName.Design.MAUI.Components.Notifications;
using CompanyName.AppName.Design.MAUI.Enumerations;
using Jint;
using Jint.Native;


public class ProgressLoaderWrapper : ComponentWrapperBase
{
    float _progress;

    public float progress
    {
        get
        {
            return _progress;
        }
        set
        {
            _progress = value;
            OnPropertyChanged();
        }
    }

    public float getProgress()
    {
        return progress;
    }

    public void setProgress(float progress)
    {
        this.progress = progress;
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

    public ProgressLoaderWrapper(Engine engine, JsValue thisObject, string basePath, string componentName, object attributes, object[] children)
        : base(engine, thisObject, basePath, componentName, attributes, children)
    {
        InitializeNativeView();

        if (Attributes != null)
        {
            foreach (var attr in Attributes)
            {
                if (attr.Key == "progress")
                {
                    progress = (float) ((double) attr.Value);
                }
                else if (attr.Key == "size")
                {
                    size = attr.Value.ToString();
                }
            }
        }

        if (size == null)
        {
            UpdateSize();
        }
    }

    void InitializeNativeView()
    {
        var loader = new Loader();
        loader.BindingContext = this;
        //loader.SetBinding(ProgressLoader.ProgressProperty, nameof(progress));

        NativeView = loader;
    }

    void UpdateSize()
    {
        var loadingButton = (Loader) NativeView;

        LoaderSize buttonSize = LoaderSize.Large;
        switch (size)
        {
            case "small":
                buttonSize = LoaderSize.Small;
                break;
            case "medium":
                buttonSize = LoaderSize.Large;
                break;
            case "large":
                buttonSize = LoaderSize.ExtraLarge;
                break;
            default:
                buttonSize = LoaderSize.ExtraLarge;
                break;
        }
        MainThread.BeginInvokeOnMainThread(() =>
        {
            loadingButton.Size = buttonSize;
        });
    }
}

