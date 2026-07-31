using Jint;
using Jint.Native;


public class ImageWrapper : ComponentWrapperBase
{
    double _width;

    public double width
    {
        get
        {
            return _width;
        }
        set
        {
            _width = value;
            UpdateWidth();
        }
    }

    public double getWidth()
    {
        return width;
    }

    public void setWidth(double width)
    {
        this.width = width;
    }

    double _height;

    public double height
    {
        get
        {
            return _height;
        }
        set
        {
            _height = value;
            UpdateHeight();
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

    string _source;

    public string source
    {
        get
        {
            return _source;
        }
        set
        {
            _source = value;
            if (!value.Contains("https", StringComparison.CurrentCultureIgnoreCase))
            {
                _source = Path.Combine(BasePath, value);
            }
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

    public ImageWrapper(Engine engine, JsValue thisObject, string basePath, string componentName, object attributes, object[] children)
        : base(engine, thisObject, basePath, componentName, attributes, children)
    {
        NativeView = new Image();

        if (Attributes != null)
        {
            foreach (var attr in Attributes)
            {
                if (attr.Key == "width")
                {
                    width = (double) attr.Value;
                }
                else if (attr.Key == "height")
                {
                    height = (double) attr.Value;
                }
                else if (attr.Key == "source")
                {
                    source = attr.Value.ToString();
                }
            }
        }
    }

    void UpdateWidth()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var image = (Image) NativeView;
            image.WidthRequest = width;
        });
    }

    void UpdateHeight()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var image = (Image) NativeView;
            image.HeightRequest = height;
        });
    }

    void UpdateSource()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var image = (Image) NativeView;
            image.Source = source;
        });
    }
}

