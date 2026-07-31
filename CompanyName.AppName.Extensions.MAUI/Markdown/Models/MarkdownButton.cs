

using System.Text;
using System.Text.RegularExpressions;
using CompanyName.AppName.Extensions.MAUI.Contracts;
using CompanyName.AppName.Extensions.MAUI.Markdown.Contracts;
using CompanyName.AppName.Extensions.MAUI.Markdown.Enumerations;
using CompanyName.AppName.Extensions.MAUI.Markdown.Extensions;
using Jint;
using Jint.Native;
using Jint.Native.Object;

namespace CompanyName.AppName.Extensions.MAUI.Markdown.Models;

/// <summary>
/// Markdown button
/// </summary>
public class MarkdownButton : IMarkdownElement
{
    /// <summary>
    /// Markdown
    /// </summary>
    readonly string _markdown;

    XamlButtonType _type = XamlButtonType.Unknown;

    /// <summary>
    /// <para>The xaml button type</para>
    /// </summary>
    public XamlButtonType Type
    {
        get
        {
            if (_type == XamlButtonType.Unknown)
            {
                _type = _markdown.MatchType<XamlButtonType>();
            }
            return _type;
        }
    }

    string _text;

    /// <summary>
    /// Button text
    /// </summary>
    public string Text
    {
        get
        {
            if (_text == null)
            {
                _text = Regex.Match(_markdown, @"(?<=\[).*?(?=\])")?.Value;
            }
            return _text;
        }
    }

    string _commandParameter;

    /// <summary>
    /// Command parameters
    /// </summary>
    public string CommandParameter
    {
        get
        {
            if (_commandParameter == null)
            {
                _commandParameter = Regex.Match(_markdown, @"(?<=\().*?(?=\))")?.Value;
                if (_commandParameter.Contains(","))
                {
                    _commandParameter = _commandParameter.Substring(0, _commandParameter.IndexOf(","));
                }
            }
            return _commandParameter;
        }
    }

    string _name;

    /// <summary>
    /// Command parameters
    /// </summary>
    public string Name
    {
        get
        {
            if (_name == null)
            {
                _name = Regex.Match(_markdown, @"(?<="").*?(?="")")?.Value;
            }
            return _name;
        }
    }

    /// <summary>
    /// Creates instance of Markdown button
    /// </summary>
    /// <param name="markdown">The markdown representing the button element.</param>
    public MarkdownButton(string markdown)
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

        if (!string.IsNullOrEmpty(CommandParameter))
        {
            stringBuilder.Append($" Command=\"{{Binding ButtonClicked}}\" CommandParameter=\"{CommandParameter}\"");
        }

        stringBuilder.Append("/>");

        return stringBuilder.ToString();
    }

    public IComponentWrapper ToComponentWrapper(Engine engine, JsValue thisObject)
    {
        Dictionary<string, object> attributes = new Dictionary<string, object>();
        var match = Regex.Match(_markdown, @"(?<=\{).*?(?=\})")?.Value;
        if (!string.IsNullOrEmpty(match))
        {
            var attrList = match.Split(',');
            foreach (var attr in attrList)
            {
                var attrKeyValue = attr.Split("=");
                if (attrKeyValue.Length > 1)
                {
                    if (attrKeyValue[0] == "clicked")
                    {
                        var classInstance = (ObjectInstance) thisObject;

                        var prototype = classInstance.Get("prototype").AsObject();
                        var clicked = prototype.Get(attrKeyValue[1].Trim(), classInstance);
                        attributes.Add(attrKeyValue[0], clicked);

                    }
                    else
                    {
                        attributes.Add(attrKeyValue[0].Trim(), attrKeyValue[1].Trim());
                    }
                }
            }
        }
        if (!string.IsNullOrEmpty(Text))
        {
            attributes.Add("text", Text);
        }

        switch (Type)
        {
            case XamlButtonType.SecondaryButton:
                return new ButtonWrapper(engine, thisObject, "secondarybutton", attributes, null);
            case XamlButtonType.TertiaryButton:
                return new ButtonWrapper(engine, thisObject, "tertiarybutton", attributes, null);
            default:
                return new ButtonWrapper(engine, thisObject, "primarybutton", attributes, null);
        }
    }
}

