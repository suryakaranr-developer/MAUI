using CompanyName.AppName.Design.MAUI.Material.Tokens;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Dispatching;

namespace CompanyName.AppName.Design.MAUI.Material.Components;

/// <summary>
/// A Material Design 3 linear progress indicator.
/// </summary>
/// <remarks>
/// Set <see cref="IsIndeterminate"/> when the length of the wait is unknown; otherwise bind
/// <see cref="Progress"/> between 0 and 1. Material draws a gap between the indicator and the
/// remaining track, which is what distinguishes it from a plain progress bar.
/// </remarks>
public class LinearProgressIndicator : Grid
{
    /// <summary>Height of the track, per the Material spec.</summary>
    const double TrackHeight = 4;

    /// <summary>Name the indeterminate sweep is committed under.</summary>
    const string SweepAnimation = "MaterialLinearSweep";

    /// <summary>Time one indeterminate sweep takes to cross the track.</summary>
    const uint SweepDuration = 1400;

    /// <summary>Fraction of the track the indeterminate indicator occupies.</summary>
    const double IndeterminateWidth = 0.4;

    readonly Border _track;

    readonly Border _indicator;

    /// <summary>
    /// How far along the work is, from 0 to 1. Ignored while indeterminate.
    /// </summary>
    public double Progress
    {
        get => (double) GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }

    public static readonly BindableProperty ProgressProperty = BindableProperty.Create(
        nameof(Progress),
        typeof(double),
        typeof(LinearProgressIndicator),
        defaultValue: 0.0,
        propertyChanged: (bindable, _, _) => ((LinearProgressIndicator) bindable).ApplyProgress());

    /// <summary>
    /// Whether the indicator sweeps rather than filling to a known point.
    /// </summary>
    public bool IsIndeterminate
    {
        get => (bool) GetValue(IsIndeterminateProperty);
        set => SetValue(IsIndeterminateProperty, value);
    }

    public static readonly BindableProperty IsIndeterminateProperty = BindableProperty.Create(
        nameof(IsIndeterminate),
        typeof(bool),
        typeof(LinearProgressIndicator),
        defaultValue: false,
        propertyChanged: (bindable, _, _) => ((LinearProgressIndicator) bindable).ApplyProgress());

    public LinearProgressIndicator()
    {
        HeightRequest = TrackHeight;
        HorizontalOptions = LayoutOptions.Fill;

        _track = new Border
        {
            Padding = 0,
            StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = TrackHeight / 2 }
        };

        _indicator = new Border
        {
            Padding = 0,
            StrokeThickness = 0,
            HorizontalOptions = LayoutOptions.Start,
            StrokeShape = new RoundRectangle { CornerRadius = TrackHeight / 2 }
        };

        _track.SetRoleBrush(BackgroundProperty, MaterialRole.SecondaryContainer);
        _indicator.SetRoleBrush(BackgroundProperty, MaterialRole.Primary);

        Children.Add(_track);
        Children.Add(_indicator);

        SizeChanged += (_, _) => ApplyProgress();

        Loaded += (_, _) => ApplyProgress();
        Unloaded += (_, _) => this.AbortAnimation(SweepAnimation);
    }

    /// <summary>
    /// Sizes and positions the indicator for the current mode.
    /// </summary>
    void ApplyProgress()
    {
        if (Width <= 0)
        {
            return;
        }

        this.AbortAnimation(SweepAnimation);

        if (!IsIndeterminate)
        {
            _indicator.TranslationX = 0;
            _indicator.WidthRequest = Width * Math.Clamp(Progress, 0, 1);
            return;
        }

        var span = Width * IndeterminateWidth;
        _indicator.WidthRequest = span;

        // The sweep starts fully off the leading edge and ends fully off the trailing one, so the
        // indicator enters and leaves rather than appearing and vanishing at the ends.
        new Animation(progress => _indicator.TranslationX = -span + ((Width + span) * progress))
            .Commit(this, SweepAnimation, length: SweepDuration, easing: Easing.SinInOut, repeat: () => IsIndeterminate);
    }
}

/// <summary>
/// A Material Design 3 circular progress indicator.
/// </summary>
/// <remarks>
/// Drawn on a <see cref="GraphicsView"/> and advanced by a dispatcher timer bound to the view's
/// lifetime, so the spin stops as soon as the indicator leaves the tree.
/// </remarks>
public class CircularProgressIndicator : GraphicsView
{
    /// <summary>Diameter of the indicator, per the Material spec.</summary>
    const double Diameter = 48;

    /// <summary>Thickness of the arc.</summary>
    const float Stroke = 4;

    /// <summary>
    /// How far along the work is, from 0 to 1. Ignored while indeterminate.
    /// </summary>
    public double Progress
    {
        get => (double) GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }

    public static readonly BindableProperty ProgressProperty = BindableProperty.Create(
        nameof(Progress),
        typeof(double),
        typeof(CircularProgressIndicator),
        defaultValue: 0.0,
        propertyChanged: (bindable, _, _) => ((CircularProgressIndicator) bindable).Invalidate());

    /// <summary>
    /// Whether the indicator spins rather than filling to a known point.
    /// </summary>
    public bool IsIndeterminate
    {
        get => (bool) GetValue(IsIndeterminateProperty);
        set => SetValue(IsIndeterminateProperty, value);
    }

    public static readonly BindableProperty IsIndeterminateProperty = BindableProperty.Create(
        nameof(IsIndeterminate),
        typeof(bool),
        typeof(CircularProgressIndicator),
        defaultValue: true);

    public CircularProgressIndicator()
    {
        WidthRequest = Diameter;
        HeightRequest = Diameter;
        BackgroundColor = Colors.Transparent;
        HorizontalOptions = LayoutOptions.Start;

        Drawable = new CircularDrawable(this);
    }

    /// <summary>
    /// Renders the arc. Holds no animation logic of its own: the timer owns the state and this
    /// only paints whatever frame it is currently on.
    /// </summary>
    sealed class CircularDrawable : IDrawable, IDisposable
    {
        /// <summary>Gap between frames, one per display refresh.</summary>
        static readonly TimeSpan FrameInterval = TimeSpan.FromMilliseconds(16);

        /// <summary>Degrees the arc advances each frame.</summary>
        const float Step = 4;

        /// <summary>Length of the indeterminate arc, in degrees.</summary>
        const float ArcSweep = 90;

        readonly CircularProgressIndicator _view;

        readonly IDispatcherTimer _timer;

        float _rotation;

        public CircularDrawable(CircularProgressIndicator view)
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
            var scheme = MaterialColorScheme.Current;

            var inset = Stroke / 2;
            var size = (float) Diameter - Stroke;

            canvas.StrokeSize = Stroke;
            canvas.StrokeLineCap = LineCap.Round;

            if (!_view.IsIndeterminate)
            {
                canvas.StrokeColor = scheme.SecondaryContainer;
                canvas.DrawEllipse(inset, inset, size, size);

                canvas.StrokeColor = scheme.Primary;
                canvas.DrawArc(
                    inset,
                    inset,
                    size,
                    size,
                    90,
                    90 - (float) (Math.Clamp(_view.Progress, 0, 1) * 360),
                    true,
                    false);
                return;
            }

            canvas.StrokeColor = scheme.Primary;
            canvas.DrawArc(inset, inset, size, size, 90 - _rotation, 90 - _rotation - ArcSweep, true, false);
        }

        void OnTick(object? sender, EventArgs e)
        {
            if (!_view.IsIndeterminate)
            {
                return;
            }

            _rotation = (_rotation + Step) % 360;
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
