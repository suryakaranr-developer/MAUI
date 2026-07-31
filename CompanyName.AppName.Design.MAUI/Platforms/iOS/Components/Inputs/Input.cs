using CoreGraphics;
using Microsoft.Maui.Platform;
using UIKit;

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
        if (input.Handler?.PlatformView != null)
        {
            (input.Handler.PlatformView as UITextField).Layer.CornerRadius = (float) newValue;
        }
    }

    public static void BorderWidthPropertyChanged(BindableObject sender, object oldValue, object newValue)
    {
        var input = (Input) sender;
        if (input.Handler?.PlatformView != null)
        {
            (input.Handler.PlatformView as UITextField).Layer.BorderWidth = (float) newValue;
        }
    }

    public static void BorderColorPropertyChanged(BindableObject sender, object oldValue, object newValue)
    {
        var input = (Input) sender;
        if (input.Handler?.PlatformView != null)
        {
            (input.Handler.PlatformView as UITextField).Layer.BorderColor = ((Color) newValue).ToCGColor();
        }
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
            var nativeView = input.Handler.PlatformView as UITextField;

            var height = (float) (padding.VerticalThickness + input.FontSize);

            nativeView.LeftView = new UIView(new CGRect(0, 0, padding.Left, height));
            nativeView.LeftViewMode = UITextFieldViewMode.Always;

            nativeView.RightView = new UIView(new CGRect(0, 0, padding.Right, height));
            nativeView.RightViewMode = UITextFieldViewMode.Always;

            input.HeightRequest = height;
        }
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        var nativeView = Handler.PlatformView as UITextField;

        nativeView.Layer.CornerRadius = CornerRadius;
        nativeView.Layer.BorderColor = BorderColor?.ToCGColor();
        nativeView.Layer.BorderWidth = BorderWidth;
        nativeView.Layer.MasksToBounds = true;
        nativeView.EditingDidBegin += NativeView_EditingDidBegin;
        nativeView.EditingDidEnd += NativeView_EditingDidEnd;


        PaddingPropertyChanged(this, null, this.Padding);
    }

    private void NativeView_EditingDidBegin(object sender, EventArgs e)
    {
        if (ActiveBorderColor != null)
        {
            var nativeView = Handler.PlatformView as UITextField;

            nativeView.Layer.BorderColor = ActiveBorderColor.ToCGColor();
        }
    }

    private void NativeView_EditingDidEnd(object sender, EventArgs e)
    {
        if (ActiveBorderColor != null)
        {
            var nativeView = Handler.PlatformView as UITextField;

            nativeView.Layer.BorderColor = BorderColor.ToCGColor();
        }
    }
}

