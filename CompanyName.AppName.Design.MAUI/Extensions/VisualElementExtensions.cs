namespace CompanyName.AppName.Design.MAUI.Extensions;

public static class VisualElementExtensions
{
    /// <summary>
    /// Gets the location of the element in the window
    /// </summary>
    /// <param name="element">Element</param>
    /// <returns>Location of element</returns>
    public static Point GetLocationInWindow(this VisualElement element)
    {
        var x = element.X;
        var y = element.Y;

        var parent = (VisualElement) element.Parent;
        while (parent != null)
        {
            x += parent.X;
            y += parent.Y;

            parent = parent.Parent is VisualElement ? (VisualElement) parent.Parent : null;
        }

        return new Point(x, y);
    }
}

