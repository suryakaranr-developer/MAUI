namespace CompanyName.AppName.Design.MAUI.Components.Notifications;

public class ProgressBarLoader : StackLayout
{
    GraphicsView _graphicsView;

    ProgressBarDrawable _drawable;

    public static readonly BindableProperty ProgressProperty =
    BindableProperty.Create(nameof(Progress), typeof(float), typeof(ProgressBarLoader), propertyChanged: ProgressPropertyChanged);

    public float Progress
    {
        get
        {
            return (float) GetValue(ProgressProperty);
        }
        set
        {
            SetValue(ProgressProperty, value);
        }
    }

    public ProgressBarLoader() : base()
    {
        InitializeComponent();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        _graphicsView.WidthRequest = width;
        _graphicsView.HeightRequest = height;

        _drawable.SizeAllocated(width, height);
    }

    private void InitializeComponent()
    {
        this.Children.Clear();

        HeightRequest = 32;
        HorizontalOptions = LayoutOptions.Fill;
        BackgroundColor = Colors.Transparent;
        Margin = 10;

        _graphicsView = new GraphicsView()
        {
            HeightRequest = 0,
            WidthRequest = 0,
            BackgroundColor = Colors.Transparent
        };

        _drawable = new ProgressBarDrawable(_graphicsView);
        _graphicsView.Drawable = _drawable;

        this.Children.Add(_graphicsView);
    }

    public static void ProgressPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var loader = (ProgressBarLoader) bindable;
        var type = newValue.GetType();
        loader._drawable.SetProgress((float) newValue);
    }

    class ProgressBarDrawable : IDrawable
    {
        readonly GraphicsView _view;

        float _progress = 0;

        float progress
        {
            get
            {
                if (_progress < 1)
                {
                    return (float) (_width * _progress) - 1.5f;
                }
                else
                {
                    return (float) _width - 1.5f;
                }
            }
        }

        double _width;

        double _height;

        public ProgressBarDrawable(GraphicsView view) : base()
        {
            _view = view;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeSize = 1;

            var theme = Application.Current.RequestedTheme;

            LinearGradientPaint linearGradientPaint;

            // TODO: Move these to resource dictionary
            if (theme == AppTheme.Dark)
            {
                linearGradientPaint = new LinearGradientPaint
                {
                    StartColor = Color.FromArgb("#212830"),
                    EndColor = Color.FromArgb("#1a1f26"),
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1)
                };
            }
            else
            {
                linearGradientPaint = new LinearGradientPaint
                {
                    StartColor = Color.FromArgb("#ffffff"),
                    EndColor = Color.FromArgb("#f4f4f4"),
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1)
                };
            }

            var width = (float) _width - 3;
            var height = (float) _height - 3;

            var x = 1.5f;
            var y = 1.5f;

            RectF linearRectangle = new RectF(x, y, width, height);
            canvas.SetFillPaint(linearGradientPaint, linearRectangle);
            canvas.FillRoundedRectangle(x, y, width, height, 16);

            var fillColor = theme == AppTheme.Dark ? Color.FromArgb("#0275eb") : Color.FromArgb("#0275eb");
            canvas.SetFillPaint(new SolidPaint(fillColor), linearRectangle);
            canvas.FillRoundedRectangle(x, y, progress, height, 16);

            canvas.StrokeColor = theme == AppTheme.Dark ? Color.FromArgb("#f2f2f2") : Color.FromArgb("#4d4d4d");
            canvas.DrawRoundedRectangle(x, y, width, height, 16);
        }

        public void SetProgress(float progress)
        {
            _progress = progress;
            if (_width > 0)
            {
                _view.Invalidate();
            }
        }

        public void SizeAllocated(double width, double height)
        {
            _width = width;
            _height = height;

            _view.Invalidate();
        }
    }
}

