namespace CompanyName.AppName.Design.MAUI.Extensions;

public static class ApplicationExtensions
{
    public static T GetResource<T>(this Application app, string key)
    {
        object resource;
        app.Resources.TryGetValue(key, out resource);
        return (T) resource;
    }
}

