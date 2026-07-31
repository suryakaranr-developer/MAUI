using System.Text;
using System.Text.RegularExpressions;
using CompanyName.AppName.Extensions.MAUI.Contracts;
using CompanyName.AppName.Extensions.MAUI.Jsx.Extensions;
using CompanyName.AppName.Extensions.MAUI.Markdown.Contracts;
using CompanyName.AppName.Extensions.MAUI.Markdown.Enumerations;
using Jint;
using Jint.Native;

namespace CompanyName.AppName.Extensions.MAUI.Markdown.Models;

/// <summary>
/// Markdown Header
/// </summary>
public class MarkdownHeader : IMarkdownElement
{
    /// <summary>
    /// Markdown
    /// </summary>
    readonly string _markdown;

    XamlHeaderType _type = XamlHeaderType.Unknown;

    /// <summary>
    /// Xaml header type
    /// </summary>
    public XamlHeaderType Type
    {
        get
        {
            if (_type == XamlHeaderType.Unknown)
            {
                _type = _markdown.MatchType<XamlHeaderType>();
            }
            return _type;
        }
    }

    string _text;

    /// <summary>
    /// Header text
    /// </summary>
    public string Text
    {
        get
        {
            if (_text == null)
            {
                _text = Regex.Replace(_markdown, @"^(#{1,6}\s)(\{.*?\})?", "").Trim();
            }
            return _text;
        }
    }

    string _name;

    /// <summary>
    /// Header name
    /// </summary>
    public string Name
    {
        get
        {
            if (_name == null)
            {
                var match = Regex.Match(_markdown, @"\((.*?)\)");
                if (match != null && match.Groups.Count > 1 && match.Groups.ContainsKey("1"))
                {
                    _name = match.Groups["1"].Value;
                }
            }
            return _name;
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

    /// <summary>
    /// Creates instance of MarkdownHeader
    /// </summary>
    /// <param name="markdown">The markdown representing the header element.</param>
    public MarkdownHeader(string markdown)
    {
        _markdown = markdown;
    }

    public string ToXaml()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append($"<{Type.ToString()}");

        if (!string.IsNullOrEmpty(Text))
        {
            stringBuilder.Append(string.Format(@" Text=""{0}""", Text));
        }

        if (!string.IsNullOrEmpty(Name))
        {
            stringBuilder.Append(string.Format(@" x:Name=""{0}""", Name));
        }

        stringBuilder.Append("/>");

        return stringBuilder.ToString();
    }

    public IComponentWrapper ToComponentWrapper(Engine engine, JsValue thisObject)
    {
        var children = !string.IsNullOrEmpty(Text) ? new object[] { Text } : null;

        switch (Type)
        {
            case XamlHeaderType.LargeTitle:
                return new LabelWrapper(engine, thisObject, "lgtitle", Attributes, children);
            case XamlHeaderType.Title1:
                return new LabelWrapper(engine, thisObject, "title1", Attributes, children);
            case XamlHeaderType.Title2:
                return new LabelWrapper(engine, thisObject, "title2", Attributes, children);
            case XamlHeaderType.Title3:
                return new LabelWrapper(engine, thisObject, "title3", Attributes, children);
            case XamlHeaderType.Title4:
                return new LabelWrapper(engine, thisObject, "title4", Attributes, children);
            case XamlHeaderType.Subtitle:
                return new LabelWrapper(engine, thisObject, "subtitle", Attributes, children);
            default:
                return new LabelWrapper(engine, thisObject, "body1", Attributes, children);
        }
    }
}
