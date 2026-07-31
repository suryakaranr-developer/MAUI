namespace CompanyName.AppName.Extensions.MAUI.Jsx.Enumerations;

/// <summary>
/// Jsx Attribute
/// <para>This is a helper class to assist matching JSX and enum types.</para>
/// </summary>
internal class JsxAttribute : Attribute
{
    public string JsxType { get; set; }

    public JsxAttribute(string xamlType)
    {
        JsxType = xamlType;
    }
}
