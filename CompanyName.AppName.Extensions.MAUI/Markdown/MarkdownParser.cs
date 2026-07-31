using CompanyName.AppName.Extensions.MAUI.Markdown.Contracts;
using CompanyName.AppName.Extensions.MAUI.Markdown.Enumerations;
using CompanyName.AppName.Extensions.MAUI.Markdown.Extensions;
using CompanyName.AppName.Extensions.MAUI.Markdown.Models;

namespace CompanyName.AppName.Extensions.MAUI.Markdown;

public class MarkdownParser
{
   // static readonly ILogger Log = LoggerFactory.GetLogger(nameof(MarkdownParser));

    /// <summary>
    /// Parses the steps from the markdown
    /// </summary>
    /// <returns>List of markdown steps</returns>
    public static IList<IMarkdownElement> Parse(string markdown)
    {
        var result = new List<IMarkdownElement>();

        var markdownLines = markdown.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        for (var i = 0; i < markdownLines.Length; i++)
        {
            var line = markdownLines[i];

            var type = line.MatchType<MarkdownElementType>();

            switch (type)
            {
                case MarkdownElementType.Unknown:
                    result.Add(new MarkdownBody(line));
                    break;
                case MarkdownElementType.MarkdownHeader:
                    result.Add(new MarkdownHeader(line));
                    break;
                case MarkdownElementType.MarkdownButton:
                    result.Add(new MarkdownButton(line));
                    break;
                case MarkdownElementType.MarkdownBranding:
                    result.Add(new MarkdownBranding(line));
                    break;
            }
        }

        return result;
    }
}

