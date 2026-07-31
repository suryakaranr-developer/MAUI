using CompanyName.AppName.Extensions.MAUI.Contracts;
using CompanyName.AppName.Extensions.MAUI.Jsx.Extensions;
using CompanyName.AppName.Extensions.MAUI.Markdown.Contracts;
using CompanyName.AppName.Extensions.MAUI.Markdown.Enumerations;
using Jint;
using Jint.Native;

namespace CompanyName.AppName.Extensions.MAUI.Markdown.Models;

public class MarkdownBranding : IMarkdownElement
{
    readonly string _markdown;

    XamlBrandingType _type;

    public XamlBrandingType Type
    {
        get
        {
            if (_type == XamlBrandingType.Unknown)
            {
                _type = _markdown.MatchType<XamlBrandingType>();
            }
            return _type;
        }
    }

    public MarkdownBranding(string markdown)
    {
        _markdown = markdown;
    }

    public string ToXaml()
    {
        switch (this.Type)
        {
            case XamlBrandingType.Logo:
                return @"<AppLogo/>";
            default:
                return string.Empty;
        }
    }

    public IComponentWrapper ToComponentWrapper(Engine engine, JsValue thisObject)
    {
        return new LogoWrapper(engine, thisObject, "logo", null, null);
    }
}

