namespace CompanyName.AppName.Design.MAUI.Components.Inputs;

public partial class Input : Entry
{
    public static readonly BindableProperty CornerRadiusProperty;

    public static readonly BindableProperty BorderWidthProperty;

    public static readonly BindableProperty BorderColorProperty;

    public static readonly BindableProperty ActiveBorderColorProperty;

    public static readonly BindableProperty PaddingProperty;

    public float CornerRadius
    {
        get
        {
            return (float) GetValue(CornerRadiusProperty);
        }
        set
        {
            SetValue(CornerRadiusProperty, value);
        }
    }

    public float BorderWidth
    {
        get
        {
            return (float) GetValue(BorderWidthProperty);
        }
        set
        {
            SetValue(BorderWidthProperty, value);
        }
    }

    public Color BorderColor
    {
        get
        {
            return GetValue(BorderColorProperty) != null ?
                (Color) GetValue(BorderColorProperty) :
                null;
        }
        set
        {
            SetValue(BorderColorProperty, value);
        }
    }

    public Color ActiveBorderColor
    {
        get
        {
            return GetValue(ActiveBorderColorProperty) != null ?
                (Color) GetValue(ActiveBorderColorProperty) :
                null;
        }
        set
        {
            SetValue(ActiveBorderColorProperty, value);
        }
    }

    public Thickness Padding
    {
        get
        {
            return (Thickness) GetValue(PaddingProperty);
        }
        set
        {
            SetValue(PaddingProperty, value);
        }
    }
}   