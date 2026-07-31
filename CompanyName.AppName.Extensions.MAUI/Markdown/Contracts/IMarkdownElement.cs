using CompanyName.AppName.Extensions.MAUI.Contracts;
using Jint;
using Jint.Native;

namespace CompanyName.AppName.Extensions.MAUI.Markdown.Contracts;

/// <summary>
/// Markdown Element Interface
/// </summary>
public interface IMarkdownElement
{
    /// <summary>
    /// Converts the markdown element to the matched XAML element string.
    /// </summary>
    /// <returns>XAML string</returns>
    string ToXaml();

    /// <summary>
    /// Converts the markdown element to a component wrapper
    /// </summary>
    /// <param name="engine">Script engine</param>
    /// <param name="thisObject">Javascript this object</param>
    /// <returns></returns>
    IComponentWrapper ToComponentWrapper(Engine engine, JsValue thisObject);
}

