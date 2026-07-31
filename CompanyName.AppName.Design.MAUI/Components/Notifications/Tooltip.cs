using CompanyName.AppName.Design.MAUI.Extensions;
using CompanyName.AppName.Design.MAUI.Typography;

namespace CompanyName.AppName.Design.MAUI.Components.Notifications;

public class Tooltip : ContentPage
{
    readonly VisualElement _element;
    readonly Frame _frame;

    Point _position;

    bool _repositioned;

    public Tooltip(VisualElement element, string text)
    {
        _element = element;

        ControlTemplate = null;
        _position = element.GetLocationInWindow();
        _position.Y -= 15;


        var layout = new AbsoluteLayout();

        var grid = new Grid();
        _frame = new TooltipFrame(this)
        {
            BackgroundColor = Application.Current.RequestedTheme == AppTheme.Light ?
                Application.Current.GetResource<Color>("SurfaceHighlight") : Application.Current.GetResource<Color>("SurfaceHighlight_Dark"),
            Padding = new Thickness(10, 8),
            Content = grid
        };

        grid.Add(new Body2()
        {
            Text = text,
            MaximumWidthRequest = Application.Current.MainPage.Window.Width - 40
        }, 0, 0);

        AbsoluteLayout.SetLayoutBounds(_frame, new Rect(_position.X, _position.Y, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        layout.Add(_frame);

        TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += (s, e) =>
        {
            Navigation.PopModalAsync(false);
        };
        layout.GestureRecognizers.Add(tapGestureRecognizer);
        Content = layout;
    }

    private void OnFrameSizeAllocated(double width, double height)
    {
        if (width > 0 && height > 0 && !_repositioned)
        {
            _repositioned = true;

            var windowWidth = Application.Current.MainPage.Window.Width;
            var windowHeight = Application.Current.MainPage.Window.Height;

            _position.Y -= (height - 46); // Account for multiline tool tip

            if (_position.X + width > windowWidth - 10)
            {
                _position.X = (windowWidth - width) - 10;
            }
            if (_position.Y + height > ((windowHeight / 2) + 20))
            {
                _position.Y = _position.Y - _element.Height - height - 10;
            }
            AbsoluteLayout.SetLayoutBounds(_frame, new Rect(_position.X, _position.Y, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        }
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        if (Handler != null)
        {
            var platformView = Handler.PlatformView;
        }
    }

    private class TooltipFrame : Frame
    {
        readonly Tooltip _parent;

        public TooltipFrame(Tooltip parent)
        {
            _parent = parent;
            BorderColor = Application.Current.RequestedTheme == AppTheme.Light ?
                Application.Current.GetResource<Color>("Divider") : Application.Current.GetResource<Color>("Divider_Dark");
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            _parent.OnFrameSizeAllocated(width, height);
        }
    }
}

