using System.Windows.Input;
using CompanyName.AppName.Design.MAUI.Material.Enumerations;
using CompanyName.AppName.Design.MAUI.Material.Tokens;
using Microsoft.Maui.Controls.Shapes;

namespace CompanyName.AppName.Design.MAUI.Material.Components;

/// <summary>
/// A Material Design 3 floating action button.
/// </summary>
/// <remarks>
/// <para>
/// Carries the single most common action on a screen, which is why it floats above the content
/// rather than sitting in it. Give it a <see cref="Text"/> to get the extended form, which pairs
/// the icon with a label and is the variant to use when the icon alone would be ambiguous.
/// </para>
/// <para>
/// Built as a <see cref="Border"/> wrapping its own content so the icon and label can sit
/// together, which a plain <c>ImageButton</c> cannot express.
/// </para>
/// </remarks>
public class MaterialFab : Border
{
    /// <summary>Gap between the icon and label of an extended button.</summary>
    const double ExtendedSpacing = 12;

    /// <summary>Horizontal padding of an extended button.</summary>
    const double ExtendedPadding = 16;

    readonly Image _icon = new()
    {
        Aspect = Aspect.AspectFit,
        VerticalOptions = LayoutOptions.Center,
        HorizontalOptions = LayoutOptions.Center
    };

    readonly LabelLarge _label = new()
    {
        VerticalOptions = LayoutOptions.Center,
        IsVisible = false
    };

    readonly HorizontalStackLayout _content;

    /// <summary>
    /// Icon shown in the button.
    /// </summary>
    public ImageSource Source
    {
        get => (ImageSource) GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly BindableProperty SourceProperty = BindableProperty.Create(
        nameof(Source),
        typeof(ImageSource),
        typeof(MaterialFab),
        propertyChanged: (bindable, _, newValue) =>
            ((MaterialFab) bindable)._icon.Source = (ImageSource) newValue);

    /// <summary>
    /// Label shown beside the icon. Setting it switches the button to its extended form.
    /// </summary>
    public string Text
    {
        get => (string) GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(MaterialFab),
        propertyChanged: (bindable, _, _) => ((MaterialFab) bindable).ApplyShape());

    /// <summary>
    /// Size of the button. Ignored once <see cref="Text"/> makes it extended.
    /// </summary>
    public FabSize Size
    {
        get => (FabSize) GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public static readonly BindableProperty SizeProperty = BindableProperty.Create(
        nameof(Size),
        typeof(FabSize),
        typeof(MaterialFab),
        defaultValue: FabSize.Medium,
        propertyChanged: (bindable, _, _) => ((MaterialFab) bindable).ApplyShape());

    /// <summary>
    /// Colour role the button is painted in.
    /// </summary>
    public FabColor Color
    {
        get => (FabColor) GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public static readonly BindableProperty ColorProperty = BindableProperty.Create(
        nameof(Color),
        typeof(FabColor),
        typeof(MaterialFab),
        defaultValue: FabColor.Primary,
        propertyChanged: (bindable, _, _) => ((MaterialFab) bindable).ApplyColor());

    /// <summary>
    /// Command run when the button is tapped.
    /// </summary>
    public ICommand Command
    {
        get => (ICommand) GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(MaterialFab));

    /// <summary>
    /// Parameter passed to <see cref="Command"/>.
    /// </summary>
    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        nameof(CommandParameter),
        typeof(object),
        typeof(MaterialFab));

    /// <summary>
    /// Raised when the button is tapped.
    /// </summary>
    public event EventHandler? Clicked;

    public MaterialFab()
    {
        _content = new HorizontalStackLayout
        {
            Spacing = ExtendedSpacing,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Children = { _icon, _label }
        };

        // The state layer sits under the icon and label and inside the Border, which clips it to
        // the button's rounded shape.
        var stateLayer = MaterialStateLayer.Attach(this, MaterialRole.OnPrimaryContainer);
        Content = new Grid { Children = { stateLayer, _content } };
        Padding = 0;
        StrokeThickness = 0;
        HorizontalOptions = LayoutOptions.Start;
        this.SetElevation(MaterialElevation.Level3);

        var tap = new TapGestureRecognizer();
        tap.Tapped += OnTapped;
        GestureRecognizers.Add(tap);

        ApplyShape();
        ApplyColor();
    }

    void OnTapped(object? sender, TappedEventArgs e)
    {
        if (Command?.CanExecute(CommandParameter) == true)
        {
            Command.Execute(CommandParameter);
        }

        Clicked?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Sizes the container and its corners for the current size, or for the extended form.
    /// </summary>
    void ApplyShape()
    {
        var extended = !string.IsNullOrEmpty(Text);

        _label.Text = Text;
        _label.IsVisible = extended;
        _icon.IsVisible = Source != null || !extended;

        if (extended)
        {
            // An extended button keeps the medium height but grows to fit its label, so its
            // width is left to the layout rather than requested.
            WidthRequest = -1;
            HeightRequest = 56;
            Padding = new Thickness(ExtendedPadding, 0);
            StrokeShape = new RoundRectangle { CornerRadius = MaterialShape.Large };
            _icon.WidthRequest = 24;
            _icon.HeightRequest = 24;
            return;
        }

        Padding = 0;

        var (side, radius, iconSize) = Size switch
        {
            FabSize.Small => (40.0, MaterialShape.Medium, 24.0),
            FabSize.Large => (96.0, MaterialShape.ExtraLarge, 36.0),
            _ => (56.0, MaterialShape.Large, 24.0)
        };

        WidthRequest = side;
        HeightRequest = side;
        StrokeShape = new RoundRectangle { CornerRadius = radius };
        _icon.WidthRequest = iconSize;
        _icon.HeightRequest = iconSize;
    }

    /// <summary>
    /// Paints the container and label for the current colour role.
    /// </summary>
    void ApplyColor()
    {
        switch (Color)
        {
            case FabColor.Secondary:
                this.SetRoleBrush(BackgroundProperty, MaterialRole.SecondaryContainer);
                _label.SetRole(Label.TextColorProperty, MaterialRole.OnSecondaryContainer);
                break;

            case FabColor.Tertiary:
                this.SetRoleBrush(BackgroundProperty, MaterialRole.TertiaryContainer);
                _label.SetRole(Label.TextColorProperty, MaterialRole.OnTertiaryContainer);
                break;

            case FabColor.Surface:
                this.SetRoleBrush(BackgroundProperty, MaterialRole.SurfaceContainerHigh);
                _label.SetRole(Label.TextColorProperty, MaterialRole.Primary);
                break;

            default:
                this.SetRoleBrush(BackgroundProperty, MaterialRole.PrimaryContainer);
                _label.SetRole(Label.TextColorProperty, MaterialRole.OnPrimaryContainer);
                break;
        }
    }
}
