using AndroidX.AppCompat.Widget;

namespace CompanyName.AppName.Design.MAUI.Components.Inputs;

public partial class Input : Entry
{
    static Input()
    {
        CornerRadiusProperty = BindableProperty.Create(
            nameof(CornerRadius), typeof(float), typeof(Input), propertyChanged: CornerRadiusPropertyChanged);

        BorderWidthProperty = BindableProperty.Create(
            nameof(BorderWidth), typeof(float), typeof(Input), propertyChanged: BorderWidthPropertyChanged);

        BorderColorProperty = BindableProperty.Create(
            nameof(BorderColor), typeof(Color), typeof(Input), propertyChanged: BorderColorPropertyChanged);

        ActiveBorderColorProperty = BindableProperty.Create(
            nameof(ActiveBorderColor), typeof(Color), typeof(Input), propertyChanged: ActiveBorderColorPropertyChanged);

        PaddingProperty = BindableProperty.Create(
            nameof(Padding), typeof(Thickness), typeof(Input), propertyChanged: PaddingPropertyChanged);
    }

    public static void CornerRadiusPropertyChanged(BindableObject sender, object oldValue, object newValue)
    {
        var input = (Input) sender;
    }

    public static void BorderWidthPropertyChanged(BindableObject sender, object oldValue, object newValue)
    {
        var input = (Input) sender;
    }

    public static void BorderColorPropertyChanged(BindableObject sender, object oldValue, object newValue)
    {
        var input = (Input) sender;
    }

    public static void ActiveBorderColorPropertyChanged(BindableObject sender, object oldValue, object newValue)
    {

    }

    public static void PaddingPropertyChanged(BindableObject sender, object oldValue, object newValue)
    {
        var input = (Input) sender;
        if (input.Handler?.PlatformView != null && newValue != null)
        {
            var padding = (Thickness) newValue;
            var nativeView = input.Handler.PlatformView as AppCompatEditText;
            nativeView.SetPadding((int) padding.Left, (int) padding.Top, (int) padding.Right, (int) padding.Bottom);
        }
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        var nativeView = Handler.PlatformView as AppCompatEditText;
        nativeView.SetPadding((int) Padding.Left, (int) Padding.Top, (int) Padding.Right, (int) Padding.Bottom);
        nativeView.SetBackgroundResource(0);

        //nativeView.Layer.CornerRadius = CornerRadius;
        //nativeView.Layer.BorderColor = BorderColor?.ToCGColor();
        //nativeView.Layer.BorderWidth = BorderWidth;
        //nativeView.Layer.MasksToBounds = true;
        //nativeView.EditingDidBegin += NativeView_EditingDidBegin;
        //nativeView.EditingDidEnd += NativeView_EditingDidEnd;


        //PaddingPropertyChanged(this, null, this.Padding);
    }
}
