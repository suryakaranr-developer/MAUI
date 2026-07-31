using Jint;
using Jint.Native;


public class NlSpanWrapper : ComponentWrapperBase
{
    public NlSpanWrapper(Engine engine, JsValue thisObject, string componentName, object attributes, object[] children)
        : base(engine, thisObject, null, componentName, attributes, children)
    {
        NativeView = new Span
        {
            Text = Environment.NewLine
        };
    }
}

