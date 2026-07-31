using CompanyName.AppName.Extensions.MAUI.Jsx.Enumerations;

namespace CompanyName.AppName.Extensions.MAUI.Jsx.Extensions;

public static class EnumAttributeExtensions
{
    /// <summary>
    /// Gets the attribute from enums.
    /// </summary>
    /// <typeparam name="T">Type of attribute</typeparam>
    /// <param name="enumVal">Enum</param>
    /// <returns>Attribute</returns>
    public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
        return (attributes.Length > 0) ? (T) attributes[0] : null;
    }

    /// <summary>
    /// Matches markdown string to enum type.
    /// </summary>
    /// <typeparam name="T">Enum type to match</typeparam>
    /// <param name="arg">arguments</param>
    /// <returns>Type</returns>
    public static T MatchType<T>(this string arg) where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        foreach (var value in values)
        {
            var match = ((Enum) value).GetAttributeOfType<JsxAttribute>();
            if (match != null)
            {
                if (match.JsxType == arg)
                {
                    return (T) value;
                }
            }
        }
        return default(T);
    }
}

