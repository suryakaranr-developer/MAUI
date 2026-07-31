using System.Text.RegularExpressions;
using CompanyName.AppName.Extensions.MAUI.Contracts;
using CompanyName.AppName.Extensions.MAUI.Markdown.Contracts;
using CompanyName.AppName.Extensions.MAUI.Markdown.Enumerations;
using Jint;
using Jint.Native;

namespace CompanyName.AppName.Extensions.MAUI.Markdown.Models;

/// <summary>
/// Markdown body
/// </summary>
public class MarkdownBody : IMarkdownElement
{
    readonly string _markdown;

    /// <summary>
    /// Header type
    /// </summary>
    public XamlHeaderType Type
    {
        get
        {
            return XamlHeaderType.Unknown;
        }
    }

    string _text;

    /// <summary>
    /// Text
    /// </summary>
    public string Text
    {
        get
        {
            if (_text == null)
            {
                _text = Regex.Replace(_markdown, @"^(\{.*?\})", "").Trim();
            }
            return _text;
        }
    }

    Dictionary<string, object> _attributes;

    public Dictionary<string, object> Attributes
    {
        get
        {
            if (_attributes == null)
            {
                var match = Regex.Match(_markdown, @"(?<=\{).*?(?=\})")?.Value;
                if (!string.IsNullOrEmpty(match))
                {
                    _attributes = new Dictionary<string, object>();
                    var attrList = match.Split(',');
                    foreach (var attr in attrList)
                    {
                        var attrKeyValue = attr.Split("=");
                        if (attrKeyValue.Length > 1)
                        {
                            _attributes.Add(attrKeyValue[0].Trim(), attrKeyValue[1].Trim());
                        }
                    }
                }

            }
            return _attributes;
        }
    }

    public MarkdownBody(string markdown)
    {
        _markdown = markdown;
    }

    public string ToXaml()
    {
        return string.Format(@"<Body1 Text=""{0}""/>", Text);
    }

    public IComponentWrapper ToComponentWrapper(Engine engine, JsValue thisObject)
    {
        return new LabelWrapper(engine, thisObject, "body1", Attributes, new object[] { Text });
    }
}

