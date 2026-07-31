using System.Windows.Input;
using CompanyName.AppName.Design.MAUI.Enumerations;
using CompanyName.AppName.Design.MAUI.Extensions;

namespace CompanyName.AppName.Design.MAUI.Components.Buttons.IconsButtons;

/// <summary>
/// Primary Loading Icon Button
/// Displays a loader around a primary icon button when Loading is set to true
/// </summary>
public class PrimaryLoadingIconButton : AbsoluteLayout
{
    /// <summary>
    /// The button control
    /// </summary>
    PrimaryIconButton _button;

    /// <summary>
    /// Graphics View
    /// </summary>
    GraphicsView _graphicsView;

    /// <summary>
    /// Icon image source
    /// </summary>
    public ImageSource Source
    {
        get => (ImageSource) GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }
    public static readonly BindableProperty SourceProperty =
    BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(PrimaryLoadingIconButton), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (PrimaryLoadingIconButton) bindable;
        control._button.Source = (ImageSource) newValue;
    });

    /// <summary>
    /// Button size
    /// </summary>
    public ButtonSize Size
    {
        get => (ButtonSize) GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
    public static readonly BindableProperty SizeProperty =
    BindableProperty.Create(nameof(Size), typeof(ButtonSize), typeof(PrimaryLoadingIconButton), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (PrimaryLoadingIconButton) bindable;
        var size = (ButtonSize) newValue;

        control._button.Size = size;
    });

    /// <summary>
    /// Sets the loading indicator
    /// </summary>
    public bool Loading
    {
        get => (bool) GetValue(LoadingProperty);
        set => SetValue(LoadingProperty, value);
    }
    public static readonly BindableProperty LoadingProperty =
    BindableProperty.Create(nameof(Loading), typeof(bool), typeof(PrimaryLoadingIconButton), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (PrimaryLoadingIconButton) bindable;
        if ((bool) newValue)
        {
            control._graphicsView = new GraphicsView()
            {
                WidthRequest = control.Size.GetWidth(),
                HeightRequest = control.Size.GetHeight(),
                BackgroundColor = Colors.Transparent
            };
            control._graphicsView.Drawable = new ButtonLoaderDrawable(control._graphicsView);

            SetLayoutBounds(control._graphicsView, new Rect(0, 0, AutoSize, AutoSize));
            control.Children.Add(control._graphicsView);
        }
        else if (control._graphicsView != null)
        {
            // Stops the loader's frame timer. Dropping the view alone would leave the timer
            // running and holding the discarded loader alive.
            (control._graphicsView.Drawable as IDisposable)?.Dispose();

            control.Children.Remove(control._graphicsView);
            control._graphicsView = null;
        }
    });

    /// <summary>
    /// Command
    /// </summary>
    public ICommand Command
    {
        get => (ICommand) GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    public static readonly BindableProperty CommandProperty =
    BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(PrimaryLoadingIconButton), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (PrimaryLoadingIconButton) bindable;
        var command = (ICommand) newValue;
        control._button.Command = command;
    });

    /// <summary>
    /// Command parameter
    /// </summary>
    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    public static readonly BindableProperty CommandParameterProperty =
    BindableProperty.Create(nameof(Command), typeof(object), typeof(PrimaryLoadingIconButton), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (PrimaryLoadingIconButton) bindable;
        control._button.CommandParameter = newValue;
    });

    /// <summary>
    /// Clicked event
    /// </summary>
    public event EventHandler Clicked
    {
        add
        {
            _button.Clicked += value;
        }
        remove
        {
            _button.Clicked -= value;
        }
    }

    /// <summary>
    /// Creates instance of the PrimaryLoadingIconButton
    /// </summary>
    public PrimaryLoadingIconButton()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Initializes the component
    /// </summary>
    void InitializeComponent()
    {
        Children.Clear();

        BackgroundColor = Colors.Transparent;

        _button = new PrimaryIconButton()
        {
            Source = Source,
            Size = Size
        };

        AbsoluteLayout.SetLayoutBounds(_button, new Rect(0, 0, AutoSize, AutoSize));

        Children.Add(_button);
    }
}
