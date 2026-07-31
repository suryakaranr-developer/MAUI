namespace CompanyName.AppName.Design.MAUI.Extensions;

public static class AppBuilderExtensions
{
    /// <summary>
    /// Configures the MAUI application to use the Application Design System Library
    /// </summary>
    /// <param name="builder"><see cref="MauiAppBuilder"/></param>
    /// <returns><see cref="MauiAppBuilder"/></returns>
    public static MauiAppBuilder UseApplicationDesignSystem(this MauiAppBuilder builder)
    {
        return builder.ConfigureFonts(fonts =>
        {
            fonts.AddFont("Montserrat_Bold.ttf", "MontserratBold");
            fonts.AddFont("Montserrat_Medium.ttf", "MontserratMedium");
            fonts.AddFont("Montserrat_Regular.ttf", "MontserratRegular");
            fonts.AddFont("Montserrat_Light.ttf", "MontserratLight");
            fonts.AddFont("Montserrat_LightItalic.ttf", "MontserratLightItalic");
            fonts.AddFont("Roboto_Bold.ttf", "RobotoBold");
            fonts.AddFont("Roboto_Medium.ttf", "RobotoMedium");
            fonts.AddFont("Roboto_Regular.ttf", "RobotoRegular");
        });
    }
}