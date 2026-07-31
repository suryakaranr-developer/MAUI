using Jint;
using Jint.Native;


public class LogoWrapper : ComponentWrapperBase
{
    public LogoWrapper(Engine engine, JsValue thisObject, string componentName, object attributes, object[] children) :
        base(engine, thisObject, null, componentName, attributes, children)
    {
    }
}

