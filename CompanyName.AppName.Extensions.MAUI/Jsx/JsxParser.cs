using CompanyName.AppName.Extensions.MAUI.Contracts;
using CompanyName.AppName.Extensions.MAUI.Enumerations;
using CompanyName.AppName.Extensions.MAUI.Jsx.Extensions;
using CompanyName.AppName.Extensions.MAUI.Markdown;
using CompanyName.AppName.Extensions.MAUI.Wrappers;
using Jint;
using Jint.Native;
using Jint.Native.Object; 

namespace CompanyName.AppName.Extensions.MAUI.Jsx;

public class JsxParser
{
    //static readonly ILogger Log = LoggerFactory.GetLogger(nameof(JsxParser));

    Engine _engine;

    JsValue _thisObject;

    readonly string _basePath;

    public string ReferencedMarkdownFile { get; set; }

    public JsxParser(string basePath)
    {
        _basePath = basePath;
    }

    public void SetEngine(Engine engine)
    {
        _engine = engine;
    }

    public IComponentWrapper[] Jsx(object arg, object attributes, params object[] children)
    {
        List<IComponentWrapper> result = new List<IComponentWrapper>();
        if (arg.GetType() == typeof(Func<JsValue, JsValue[], JsValue>))
        {
            Func<JsValue, JsValue[], JsValue> jsClass = (Func<JsValue, JsValue[], JsValue>) arg;
            var jsConstructor = JsValue.FromObject(_engine, jsClass.Target);

            var arguments = new JsValue[] { JsValue.FromObject(_engine, attributes) };

            var jsClassInstance = _engine.Construct(jsConstructor, arguments);
            //if(attributes != null && attributes is ExpandoObject)
            //{
            //  var attr = attributes as ExpandoObject;
            //  foreach(var a in attr)
            //  {
            //     jsClassInstance.FastSetDataProperty(a.Key, JsValue.FromObject(_engine, a.Value));
            //  }
            //}

            _thisObject = JsValue.FromObject(_engine, jsClassInstance);

            // Corrected line: Use Get instead of GetProperty on the JsValue object
            var render = jsClassInstance.Prototype.Get("render"); 
            
            if (render != null && render != JsValue.Undefined) // Check if the property exists
            {
                var jsx = _engine.Invoke(render, _thisObject, new object[0]);
                //var jsx = _engine.CallFunction(jsClassInstance, "render", children);
                if (jsx is JsArray)
                {
                    var jsxArray = jsx as JsArray;
                    foreach (var component in jsxArray)
                    {
                        var jsxComponent = component.ToObject();
                        if (jsxComponent is IComponentWrapper)
                        {
                            result.Add((IComponentWrapper) jsxComponent);
                        }
                    }
                }
            }
            else if (!string.IsNullOrEmpty(ReferencedMarkdownFile))
            {
                var markdownCode = File.ReadAllText(ReferencedMarkdownFile);
                var markdown = MarkdownParser.Parse(markdownCode);
                var mdChildren = new List<IComponentWrapper>();
                foreach (var component in markdown)
                {
                    mdChildren.Add(component.ToComponentWrapper(_engine, _thisObject));
                }

                result.Add(new ContentWrapper(_engine, _thisObject, "content", null, (object[]) mdChildren.ToArray()));
            }
        }
        else
        {
            var name = (string) arg;
            var xamlType = name.MatchType<XamlType>();
            switch (xamlType)
            {
                case XamlType.ContentView:
                    result.Add(new ContentWrapper(_engine, _thisObject, name, attributes, children));
                    break;
                case XamlType.VerticalStackLayout:
                case XamlType.HorizontalStackLayout:
                    result.Add(new SectionWrapper(_engine, _thisObject, name, attributes, children));
                    break;
                case XamlType.FlexLayout:
                    result.Add(new FlexLayoutWrapper(_engine, _thisObject, name, attributes, children));
                    break;
                case XamlType.PrimaryButton:
                case XamlType.SecondaryButton:
                case XamlType.TertiaryButton:
                    result.Add(new ButtonWrapper(_engine, _thisObject, name, attributes, children));
                    break;
                case XamlType.PrimaryIconButton:
                    result.Add(new PrimaryIconButtonWrapper(_engine, _thisObject, _basePath, name, attributes, children));
                    break;
                case XamlType.SecondaryIconButton:
                    result.Add(new SecondaryIconButtonWrapper(_engine, _thisObject, _basePath, name, attributes, children));
                    break;
                case XamlType.PrimaryLoadingButton:
                    result.Add(new PrimaryLoadingButtonWrapper(_engine, _thisObject, _basePath, name, attributes, children));
                    break;
                case XamlType.SecondaryLoadingButton:
                    result.Add(new SecondaryLoadingButtonWrapper(_engine, _thisObject, _basePath, name, attributes, children));
                    break;
                case XamlType.ExtraLargeTitle:
                case XamlType.LargeTitle:
                case XamlType.Title1:
                case XamlType.Title2:
                case XamlType.Title3:
                case XamlType.Title4:
                case XamlType.Subtitle:
                case XamlType.Body1:
                case XamlType.Body2:
                case XamlType.Caption:
                case XamlType.Display1:
                case XamlType.Display2:
                case XamlType.Display3:
                    result.Add(new LabelWrapper(_engine, _thisObject, name, attributes, children));
                    break;
                case XamlType.Span:
                case XamlType.HyperLink:
                    result.Add(new SpanWrapper(_engine, _thisObject, name, attributes, children));
                    break;
                case XamlType.NewLine:
                    result.Add(new NlSpanWrapper(_engine, _thisObject, name, attributes, children));
                    break;
                case XamlType.Image:
                    result.Add(new ImageWrapper(_engine, _thisObject, _basePath, name, attributes, children));
                    break;
                case XamlType.Input:
                    result.Add(new InputWrapper(_engine, _thisObject, name, attributes, children));
                    break;
                case XamlType.Editor:
                    result.Add(new EditorWrapper(_engine, _thisObject, name, attributes, children));
                    break;
                case XamlType.ProgressLoader:
                    result.Add(new ProgressLoaderWrapper(_engine, _thisObject, _basePath, name, attributes, children));
                    break;
                case XamlType.ProgressBar:
                    result.Add(new ProgressBarWrapper(_engine, _thisObject, _basePath, name, attributes, children));
                    break;
            }
        }
        return result.ToArray();
    }
}