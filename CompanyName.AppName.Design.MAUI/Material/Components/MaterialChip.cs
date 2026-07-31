using System.Windows.Input;
using CompanyName.AppName.Design.MAUI.Material.Enumerations;
using CompanyName.AppName.Design.MAUI.Material.Tokens;
using Microsoft.Maui.Controls.Shapes;

namespace CompanyName.AppName.Design.MAUI.Material.Components;

/// <summary>
/// A Material Design 3 chip.
/// </summary>
/// <remarks>
/// A compact element for an option, an attribute or an action. Filter chips hold a selected
/// state and show a leading check when selected; the other variants act on tap. Input chips can
/// show a trailing remove affordance via <see cref="IsRemovable"/>.
/// </remarks>
public class MaterialChip : Border
{
    /// <summary>Height of every chip, per the Material spec.</summary>
    const double ChipHeight = 32;

    /// <summary>Horizontal padding either side of the label.</summary>
    const double LabelPadding = 12;

    readonly Label _check = new()
    {
        Text = "✓",
        FontSize = 14,
        VerticalOptions = LayoutOptions.Center,
        IsVisible = false
    };

    readonly LabelLarge _label = new()
    {
        VerticalOptions = LayoutOptions.Center
    };

    readonly Label _remove = new()
    {
        Text = "✕",
        FontSize = 14,
        VerticalOptions = LayoutOptions.Center,
        IsVisible = false
    };

    /// <summary>
    /// Which of the four chip variants to paint.
    /// </summary>
    public MaterialChipVariant Variant
    {
        get => (MaterialChipVariant) GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public static readonly BindableProperty VariantProperty = BindableProperty.Create(
        nameof(Variant),
        typeof(MaterialChipVariant),
        typeof(MaterialChip),
        defaultValue: MaterialChipVariant.Assist,
        propertyChanged: (bindable, _, _) => ((MaterialChip) bindable).ApplyState());

    /// <summary>
    /// The chip's label.
    /// </summary>
    public string Text
    {
        get => (string) GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(MaterialChip),
        propertyChanged: (bindable, _, newValue) =>
            ((MaterialChip) bindable)._label.Text = (string) newValue);

    /// <summary>
    /// Whether the chip is selected. Only filter chips paint a selected state.
    /// </summary>
    public bool IsSelected
    {
        get => (bool) GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(
        nameof(IsSelected),
        typeof(bool),
        typeof(MaterialChip),
        defaultValue: false,
        propertyChanged: (bindable, _, _) => ((MaterialChip) bindable).ApplyState());

    /// <summary>
    /// Whether to show a trailing affordance for removing the chip.
    /// </summary>
    public bool IsRemovable
    {
        get => (bool) GetValue(IsRemovableProperty);
        set => SetValue(IsRemovableProperty, value);
    }

    public static readonly BindableProperty IsRemovableProperty = BindableProperty.Create(
        nameof(IsRemovable),
        typeof(bool),
        typeof(MaterialChip),
        defaultValue: false,
        propertyChanged: (bindable, _, _) => ((MaterialChip) bindable).ApplyState());

    /// <summary>
    /// Command run when the chip is tapped.
    /// </summary>
    public ICommand Command
    {
        get => (ICommand) GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(MaterialChip));

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
        typeof(MaterialChip));

    /// <summary>
    /// Raised when the chip is tapped, after a filter chip has toggled its selection.
    /// </summary>
    public event EventHandler? Clicked;

    /// <summary>
    /// Raised when the remove affordance is tapped.
    /// </summary>
    public event EventHandler? Removed;

    public MaterialChip()
    {
        HeightRequest = ChipHeight;
        Padding = new Thickness(LabelPadding, 0);
        HorizontalOptions = LayoutOptions.Start;
        StrokeShape = new RoundRectangle { CornerRadius = MaterialShape.Small };

        var content = new HorizontalStackLayout
        {
            Spacing = 8,
            VerticalOptions = LayoutOptions.Center,
            Children = { _check, _label, _remove }
        };

        // The state layer sits under the content and inside the Border, which clips it to the
        // chip's rounded shape.
        var stateLayer = MaterialStateLayer.Attach(this, MaterialRole.OnSurfaceVariant);
        Content = new Grid { Children = { stateLayer, content } };

        var tap = new TapGestureRecognizer();
        tap.Tapped += OnTapped;
        GestureRecognizers.Add(tap);

        var removeTap = new TapGestureRecognizer();
        removeTap.Tapped += (_, _) => Removed?.Invoke(this, EventArgs.Empty);
        _remove.GestureRecognizers.Add(removeTap);

        ApplyState();
    }

    void OnTapped(object? sender, TappedEventArgs e)
    {
        if (Variant == MaterialChipVariant.Filter)
        {
            IsSelected = !IsSelected;
        }

        if (Command?.CanExecute(CommandParameter) == true)
        {
            Command.Execute(CommandParameter);
        }

        Clicked?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Paints the chip for its current variant and selection state.
    /// </summary>
    void ApplyState()
    {
        // Only filter chips carry selection, so the check never appears on the others even if
        // IsSelected has been set.
        var selected = Variant == MaterialChipVariant.Filter && IsSelected;

        _check.IsVisible = selected;
        _remove.IsVisible = IsRemovable;

        if (selected)
        {
            this.SetRoleBrush(BackgroundProperty, MaterialRole.SecondaryContainer);
            _label.SetRole(Label.TextColorProperty, MaterialRole.OnSecondaryContainer);
            _check.SetRole(Label.TextColorProperty, MaterialRole.OnSecondaryContainer);
            _remove.SetRole(Label.TextColorProperty, MaterialRole.OnSecondaryContainer);
            StrokeThickness = 0;
            return;
        }

        BackgroundColor = Colors.Transparent;
        this.SetRoleBrush(StrokeProperty, MaterialRole.Outline);
        StrokeThickness = 1;

        _label.SetRole(Label.TextColorProperty, MaterialRole.OnSurfaceVariant);
        _check.SetRole(Label.TextColorProperty, MaterialRole.OnSurfaceVariant);
        _remove.SetRole(Label.TextColorProperty, MaterialRole.OnSurfaceVariant);
    }
}
