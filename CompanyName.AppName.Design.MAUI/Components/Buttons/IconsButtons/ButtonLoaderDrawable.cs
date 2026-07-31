using Microsoft.Maui.Dispatching;

namespace CompanyName.AppName.Design.MAUI.Components.Buttons.IconsButtons;

/// <summary>
/// Button loader
/// </summary>
/// <remarks>
/// <see cref="Draw"/> renders the current frame and nothing else. The arc is advanced by a
/// dispatcher timer bound to the view's lifetime, so drawing never re-enters itself and the
/// animation stops as soon as the view it draws into leaves the tree.
/// </remarks>
internal sealed class ButtonLoaderDrawable : IDrawable, IDisposable
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

    public ButtonLoaderDrawable(GraphicsView view) : base()
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

        var width = (float) _view.WidthRequest - strokeSize;
        var height = (float) _view.HeightRequest - strokeSize;

        var x = 1.5f;
        var y = 1.5f;

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
