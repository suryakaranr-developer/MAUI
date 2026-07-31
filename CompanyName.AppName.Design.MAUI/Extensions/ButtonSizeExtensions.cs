using CompanyName.AppName.Design.MAUI.Enumerations;

namespace CompanyName.AppName.Design.MAUI.Extensions;

public static class ButtonSizeExtensions
{
    /// <summary>
    /// Gets the width based on button size
    /// </summary>
    /// <param name="size">Button size</param>
    /// <returns>Width</returns>
    public static double GetWidth(this ButtonSize size)
    {
        switch (size)
        {
            case ButtonSize.Small:
                return 40;
            case ButtonSize.ExtraLarge:
                return 88;
            default:
                return 64;
        }
    }

    /// <summary>
    /// Gets the height based on button size
    /// </summary>
    /// <param name="size">Button size</param>
    /// <returns>Height</returns>
    public static double GetHeight(this ButtonSize size)
    {
        switch (size)
        {
            case ButtonSize.Small:
                return 40;
            case ButtonSize.ExtraLarge:
                return 88;
            default:
                return 64;
        }
    }
}

