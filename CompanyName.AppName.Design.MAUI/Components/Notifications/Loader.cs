using CompanyName.AppName.Design.MAUI.Enumerations;
using Microsoft.Maui.Dispatching;

namespace CompanyName.AppName.Design.MAUI.Components.Notifications;

public class Loader : AbsoluteLayout
{
    Image _image;

    LoaderDrawable _drawable;

    double _size
    {
        get
        {
            return GetSize();
        }
    }

    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(string), typeof(Loader), propertyChanged: IconPropertyChanged);

    public static readonly BindableProperty SizeProperty =
        BindableProperty.Create(nameof(Size), typeof(LoaderSize), typeof(Loader), propertyChanged: SizePropertyChanged);

    public string Icon
    {
        get
        {
            return (string) GetValue(IconProperty);
        }
        set
        {
            SetValue(IconProperty, value);
        }
    }

    public LoaderSize Size
    {
        get
        {
            return (LoaderSize) GetValue(SizeProperty);
        }
        set
        {
            SetValue(SizeProperty, value);
        }
    }

    public Loader() : base()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        // Resizing rebuilds the whole visual tree, so the arc left over from the previous size
        // has to be shut down or its timer keeps ticking against a discarded view.
        _drawable?.Dispose();

        this.Children.Clear();

        HeightRequest = _size;
        WidthRequest = _size;

        BackgroundColor = Colors.Transparent;
        var graphicsView = new GraphicsView()
        {
            HeightRequest = _size,
            WidthRequest = _size,
            BackgroundColor = Colors.Transparent
        };

        _drawable = new LoaderDrawable(graphicsView);
        graphicsView.Drawable = _drawable;

        AbsoluteLayout.SetLayoutBounds(graphicsView, new Rect(0, 0, _size, _size));

        this.Children.Add(graphicsView);

        _image = new Image()
        {
            Source = Icon
        };

        AbsoluteLayout.SetLayoutBounds(_image, new Rect(_size / 4, _size / 4, _size / 2, _size / 2));

        this.Children.Add(_image);
    }

    public static void IconPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((Loader) bindable)._image.Source = (string) newValue;
    }

    double GetSize()
    {
        switch (Size)
        {
            case LoaderSize.Large:
                return 64;
            case LoaderSize.Small:
                return 40;
            case LoaderSize.ExtraLarge:
                return 88;
            case LoaderSize.ExtraSmall:
                return 24;
            default:
                return 108;
        }
    }

    public static void SizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var loader = (Loader) bindable;

        loader.InitializeComponent();
    }

    /// <summary>
    /// Spinning arc drawn behind the loader icon.
    /// </summary>
    /// <remarks>
    /// <see cref="Draw"/> renders the current frame and nothing else. The arc is advanced by a
    /// dispatcher timer bound to the view's lifetime, so drawing never re-enters itself and the
    /// animation stops as soon as the view it draws into leaves the tree.
    /// </remarks>
    sealed class LoaderDrawable : IDrawable, IDisposable
    {
        /// <summary>
        /// Angle both ends of the arc start from and reset to.
        /// </summary>
        const int ResetAngle = 90;

        /// <summary>
        /// Angle both ends sweep to before the arc resets.
        /// </summary>
        const int SweepLimit = -270;

        /// <summary>
        /// Gap between frames. One frame per display refresh is as fine grained as is worth drawing.
        /// </summary>
        static readonly TimeSpan FrameInterval = TimeSpan.FromMilliseconds(16);

        readonly GraphicsView _view;

        readonly IDispatcherTimer _timer;

        int _start = ResetAngle;
        int _stop = ResetAngle;

        public LoaderDrawable(GraphicsView view) : base()
        {
            _view = view;

            _timer = view.Dispatcher.CreateTimer();
            _timer.Interval = FrameInterval;
            _timer.Tick += OnTick;

            view.Loaded += OnViewLoaded;
            view.Unloaded += OnViewUnloaded;

            if (view.IsLoaded)
            {
                _timer.Start();
            }
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            var strokeSize = 8;
            if (_view.Height < 108)
            {
                strokeSize = 3;
            }
            canvas.StrokeSize = strokeSize;

            var theme = Application.Current?.RequestedTheme ?? AppTheme.Light;

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

            var width = (float) _view.WidthRequest - strokeSize;
            var height = (float) _view.HeightRequest - strokeSize;

            var x = 1.5f;
            var y = 1.5f;

            RectF linearRectangle = new RectF(x, y, width, height);
            canvas.SetFillPaint(linearGradientPaint, linearRectangle);
            canvas.FillEllipse(x, y, width, height);

            canvas.StrokeColor = theme == AppTheme.Dark ? Color.FromArgb("#1a1f26") : Color.FromArgb("#f4f4f4");
            canvas.DrawEllipse(x, y, width, height);

            canvas.StrokeLineCap = LineCap.Round;
            canvas.StrokeColor = Color.FromArgb("#0275eb");
            canvas.DrawArc(x, y, width, height, _start, _stop, true, false);
        }

        /// <summary>
        /// Advances the arc by one frame and asks for a redraw.
        /// </summary>
        void OnTick(object? sender, EventArgs e)
        {
            if (_stop > -35)
            {
                _start -= 2;
            }
            else
            {
                _start = _start - 6 > SweepLimit ? _start - 6 : SweepLimit;
            }

            if (_stop > -230)
            {
                _stop -= 6;
            }
            else
            {
                _stop = _stop - 2 > SweepLimit ? _stop - 2 : SweepLimit;
            }

            if (_start <= SweepLimit && _stop <= SweepLimit)
            {
                _start = ResetAngle;
                _stop = ResetAngle;
            }

            _view.Invalidate();
        }

        void OnViewLoaded(object? sender, EventArgs e)
        {
            _timer.Start();
        }

        void OnViewUnloaded(object? sender, EventArgs e)
        {
            _timer.Stop();
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Tick -= OnTick;

            _view.Loaded -= OnViewLoaded;
            _view.Unloaded -= OnViewUnloaded;
        }
    }
}

