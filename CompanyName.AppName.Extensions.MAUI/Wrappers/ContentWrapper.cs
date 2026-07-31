using CompanyName.AppName.Extensions.MAUI.Contracts;
using Jint;
using Jint.Native;

public class ContentWrapper : ComponentWrapperBase
{
    public ContentWrapper(Engine engine, JsValue thisObject, string componentName, object attributes, object[] children)
        : base(engine, thisObject, null, componentName, attributes, children)
    {
        InitializeNativeView();
    }

    void InitializeNativeView()
    {
        var contentView = new ContentView();

        var scrollView = new ScrollView();

        var stackLayout = new VerticalStackLayout
        {
            Padding = new Thickness(10, 20)
        };

        if (Children != null)
        {
            foreach (var child in Children)
            {
                stackLayout.Children.Add((IView) ((IComponentWrapper) child).GetNativeView());
            }
        }

        scrollView.Content = stackLayout;
        contentView.Content = scrollView;
        NativeView = contentView;
    }
}

