using System.Windows.Input;
using CompanyName.AppName.Design.MAUI.Material.Tokens;

namespace CompanyName.AppName.Design.MAUI.Material.Components;

/// <summary>
/// A Material Design 3 list item.
/// </summary>
/// <remarks>
/// Grows from one line to three as <see cref="Headline"/>, <see cref="Supporting"/> and
/// <see cref="Trailing"/> are filled in, matching Material's one-, two- and three-line densities.
/// The leading icon and trailing text are optional and collapse when unset.
/// </remarks>
public class MaterialListItem : Grid
{
    /// <summary>Height of a single-line item.</summary>
    const double OneLineHeight = 56;

    /// <summary>Height of a two-line item.</summary>
    const double TwoLineHeight = 72;

    /// <summary>Size of the leading icon.</summary>
    const double IconSize = 24;

    readonly Image _leading;

    readonly BodyLarge _headline;

    readonly BodyMedium _supporting;

    readonly LabelSmall _trailing;

    /// <summary>
    /// The item's primary line.
    /// </summary>
    public string Headline
    {
        get => (string) GetValue(HeadlineProperty);
        set => SetValue(HeadlineProperty, value);
    }

    public static readonly BindableProperty HeadlineProperty = BindableProperty.Create(
        nameof(Headline),
        typeof(string),
        typeof(MaterialListItem),
        propertyChanged: (bindable, _, _) => ((MaterialListItem) bindable).ApplyState());

    /// <summary>
    /// The item's second line. Leave empty for a single-line item.
    /// </summary>
    public string Supporting
    {
        get => (string) GetValue(SupportingProperty);
        set => SetValue(SupportingProperty, value);
    }

    public static readonly BindableProperty SupportingProperty = BindableProperty.Create(
        nameof(Supporting),
        typeof(string),
        typeof(MaterialListItem),
        propertyChanged: (bindable, _, _) => ((MaterialListItem) bindable).ApplyState());

    /// <summary>
    /// Text shown at the trailing edge, such as a timestamp.
    /// </summary>
    public string Trailing
    {
        get => (string) GetValue(TrailingProperty);
        set => SetValue(TrailingProperty, value);
    }

    public static readonly BindableProperty TrailingProperty = BindableProperty.Create(
        nameof(Trailing),
        typeof(string),
        typeof(MaterialListItem),
        propertyChanged: (bindable, _, _) => ((MaterialListItem) bindable).ApplyState());

    /// <summary>
    /// Icon shown at the leading edge.
    /// </summary>
    public ImageSource LeadingIcon
    {
        get => (ImageSource) GetValue(LeadingIconProperty);
        set => SetValue(LeadingIconProperty, value);
    }

    public static readonly BindableProperty LeadingIconProperty = BindableProperty.Create(
        nameof(LeadingIcon),
        typeof(ImageSource),
        typeof(MaterialListItem),
        propertyChanged: (bindable, _, _) => ((MaterialListItem) bindable).ApplyState());

    /// <summary>
    /// Command run when the item is tapped.
    /// </summary>
    public ICommand Command
    {
        get => (ICommand) GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(MaterialListItem));

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
        typeof(MaterialListItem));

    /// <summary>
    /// Raised when the item is tapped.
    /// </summary>
    public event EventHandler? Tapped;

    public MaterialListItem()
    {
        ColumnDefinitions = new ColumnDefinitionCollection
        {
            new ColumnDefinition { Width = GridLength.Auto },
            new ColumnDefinition { Width = GridLength.Star },
            new ColumnDefinition { Width = GridLength.Auto }
        };
        ColumnSpacing = 16;
        Padding = new Thickness(16, 8);

        this.SetRole(BackgroundColorProperty, MaterialRole.Surface);

        _leading = new Image
        {
            WidthRequest = IconSize,
            HeightRequest = IconSize,
            Aspect = Aspect.AspectFit,
            VerticalOptions = LayoutOptions.Center,
            IsVisible = false
        };

        _headline = new BodyLarge();
        _headline.SetRole(Label.TextColorProperty, MaterialRole.OnSurface);

        _supporting = new BodyMedium { IsVisible = false };
        _supporting.SetRole(Label.TextColorProperty, MaterialRole.OnSurfaceVariant);

        _trailing = new LabelSmall
        {
            VerticalOptions = LayoutOptions.Center,
            IsVisible = false
        };
        _trailing.SetRole(Label.TextColorProperty, MaterialRole.OnSurfaceVariant);

        var lines = new VerticalStackLayout
        {
            VerticalOptions = LayoutOptions.Center,
            Children = { _headline, _supporting }
        };

        SetColumn((BindableObject) _leading, 0);
        SetColumn((BindableObject) lines, 1);
        SetColumn((BindableObject) _trailing, 2);

        // The state layer spans the whole row, under the content, so a tap tints the entire item
        // rather than one column. Padding is negated so it reaches the row's edges.
        var stateLayer = MaterialStateLayer.Attach(this, MaterialRole.OnSurface);
        stateLayer.Margin = new Thickness(-16, -8);
        SetColumn((BindableObject) stateLayer, 0);
        SetColumnSpan((BindableObject) stateLayer, 3);
        Children.Add(stateLayer);

        Children.Add(_leading);
        Children.Add(lines);
        Children.Add(_trailing);

        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) =>
        {
            if (Command?.CanExecute(CommandParameter) == true)
            {
                Command.Execute(CommandParameter);
            }

            Tapped?.Invoke(this, EventArgs.Empty);
        };
        GestureRecognizers.Add(tap);

        ApplyState();
    }

    /// <summary>
    /// Fills the lines and sets the row height for the resulting density.
    /// </summary>
    void ApplyState()
    {
        _headline.Text = Headline;

        _supporting.Text = Supporting;
        _supporting.IsVisible = !string.IsNullOrEmpty(Supporting);

        _trailing.Text = Trailing;
        _trailing.IsVisible = !string.IsNullOrEmpty(Trailing);

        _leading.Source = LeadingIcon;
        _leading.IsVisible = LeadingIcon != null;

        MinimumHeightRequest = _supporting.IsVisible ? TwoLineHeight : OneLineHeight;
    }
}
