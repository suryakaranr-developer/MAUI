using System.Windows.Input;
using CompanyName.AppName.Design.MAUI.Enumerations;
using CompanyName.AppName.Design.MAUI.Extensions;
using Microsoft.Maui.Layouts;

namespace CompanyName.AppName.Design.MAUI.Components.Buttons.IconsButtons;

/// <summary>
/// Secondary Loading Icon Button
/// Displays a loader around a secondary icon button when Loading is set to true
/// </summary>
public class SecondaryLoadingIconButton : AbsoluteLayout
{
    /// <summary>
    /// The button control
    /// </summary>
    SecondaryIconButton _button;

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
    BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(SecondaryLoadingIconButton), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (SecondaryLoadingIconButton) bindable;
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

    //public static readonly BindableProperty SizeProperty =
    //BindableProperty.Create(nameof(Size), typeof(ButtonSize), typeof(SecondaryLoadingIconButton), propertyChanged: (bindable, oldValue, newValue) =>
    //{
    //    var control = (SecondaryLoadingIconButton) bindable;
    //    var size = (ButtonSize) newValue;

    //    control._button.Size = size;
    //});

    public static readonly BindableProperty SizeProperty =
        BindableProperty.Create(
            nameof(Size),
            typeof(ButtonSize),
            typeof(SecondaryLoadingIconButton),
            propertyChanged: OnSizePropertyChanged);

    // Property to get and set the ButtonSize

    // Property changed callback
    private static void OnSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (SecondaryLoadingIconButton) bindable;
        var size = (ButtonSize) newValue;
        // Update the control's size based on the new value
        control.ApplySize(size);
    }

    // Method to update the button size - You need to implement this according to your requirements
    private void ApplySize(ButtonSize size)
    {
        switch (size)
        {
            case ButtonSize.ExtraLarge:
                _button.WidthRequest = 200;
                _button.HeightRequest = 80;
                break;

            case ButtonSize.Large:
                _button.WidthRequest = 160;
                _button.HeightRequest = 60;
                _button.HorizontalOptions = LayoutOptions.Center;
                break;

            case ButtonSize.Small:
                _button.WidthRequest = 120;
                _button.HeightRequest = 40;
                break;

            default:
                throw new NotSupportedException($"Unsupported size: {size}");
        }
    }

    /// <summary>
    /// Sets the loading indicator
    /// </summary>
    public bool Loading
    {
        get => (bool) GetValue(LoadingProperty);
        set => SetValue(LoadingProperty, value);
    }
    public static readonly BindableProperty LoadingProperty =
    BindableProperty.Create(nameof(Loading), typeof(bool), typeof(SecondaryLoadingIconButton), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (SecondaryLoadingIconButton) bindable;
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
    BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(SecondaryLoadingIconButton), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (SecondaryLoadingIconButton) bindable;
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
    BindableProperty.Create(nameof(Command), typeof(object), typeof(SecondaryLoadingIconButton), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (SecondaryLoadingIconButton) bindable;
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
    /// Creates instance of the SecondaryLoadingIconButton
    /// </summary>
    public SecondaryLoadingIconButton()
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

        _button = new SecondaryIconButton()
        {
            Source = Source,
            Size = Size
        };

        // Set the layout flags to position the button proportionally
        AbsoluteLayout.SetLayoutFlags(_button, AbsoluteLayoutFlags.PositionProportional);

        // Set the layout bounds to center the button (0.5, 0.5)
        AbsoluteLayout.SetLayoutBounds(_button, new Rect(0.5, 0.5, AutoSize, AutoSize));

        Children.Add(_button);
    }
}

