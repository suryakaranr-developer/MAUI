using System.Text.RegularExpressions;
using CompanyName.AppName.Extensions.MAUI.Markdown.Enumerations;

namespace CompanyName.AppName.Extensions.MAUI.Markdown.Extensions;

/// <summary>
/// String extensions
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Matches markdown string to enum type.
    /// </summary>
    /// <typeparam name="T">Enum type to match</typeparam>
    /// <param name="markdown">Markdown</param>
    /// <returns>Type</returns>
    public static T MatchType<T>(this string markdown) where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        foreach (var value in values)
        {
            var match = ((Enum) value).GetAttributeOfType<MatchAttribute>();
            if (match != null)
            {
                if (Regex.IsMatch(markdown, match.Regex))
                {
                    return (T) value;
                }
            }
        }
        return default(T);
    }
}

