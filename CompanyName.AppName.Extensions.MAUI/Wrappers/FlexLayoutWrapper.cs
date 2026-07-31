using CompanyName.AppName.Extensions.MAUI.Contracts;
using Jint;
using Jint.Native;
using Microsoft.Maui.Layouts;


public class FlexLayoutWrapper : ComponentWrapperBase
{
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
        return this.alignment;
    }

    public void setAlignment(string alignment)
    {
        this.alignment = alignment;
    }

    public FlexLayoutWrapper(Engine engine, JsValue thisObject, string componentName, object attributes, object[] children)
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
            }
        }
    }

    void InitializeNativeView()
    {
        FlexLayout layout = new FlexLayout
        {
            JustifyContent = FlexJustify.Start,
            AlignItems = FlexAlignItems.Start,
            AlignContent = FlexAlignContent.Start,
            Direction = FlexDirection.Row,
            Wrap = FlexWrap.Wrap,
            Margin = new Thickness(20, 10)
        };

        if (Children != null)
        {
            foreach (var child in Children)
            {
                layout.Children.Add((IView) ((IComponentWrapper) child).GetNativeView());
            }
        }

        NativeView = layout;
    }

    void UpdateAlignment()
    {
        FlexJustify justify = FlexJustify.Start;
        FlexAlignItems items = FlexAlignItems.Start;
        FlexAlignContent content = FlexAlignContent.Start;
        switch (alignment)
        {
            case "center":
                justify = FlexJustify.Center;
                items = FlexAlignItems.Center;
                content = FlexAlignContent.Center;
                break;
            case "end":
                justify = FlexJustify.End;
                items = FlexAlignItems.End;
                content = FlexAlignContent.End;
                break;
        }
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var layout = (FlexLayout) NativeView;
            layout.JustifyContent = justify;
            layout.AlignItems = items;
            layout.AlignContent = content;
        });
    }
}

