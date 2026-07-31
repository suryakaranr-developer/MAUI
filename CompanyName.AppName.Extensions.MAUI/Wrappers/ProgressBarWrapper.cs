using CompanyName.AppName.Design.MAUI.Components.Notifications;
using Jint;
using Jint.Native;


public class ProgressBarWrapper : ComponentWrapperBase
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

    public ProgressBarWrapper(Engine engine, JsValue thisObject, string basePath, string componentName, object attributes, object[] children)
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
            }
        }
    }

    void InitializeNativeView()
    {
        var loader = new ProgressBarLoader();
        loader.BindingContext = this;
        loader.SetBinding(ProgressBarLoader.ProgressProperty, nameof(progress));

        NativeView = loader;
    }
}

