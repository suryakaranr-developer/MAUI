using CompanyName.AppName.Design.MAUI.Material.Tokens;
using Microsoft.Maui.Controls.Shapes;

namespace CompanyName.AppName.Design.MAUI.Material.Components;

/// <summary>
/// A Material Design 3 checkbox, with an optional label.
/// </summary>
/// <remarks>
/// For selecting any number of options from a set. The box itself is 18dp but the control claims
/// a 40dp row so the touch target stays comfortable, which is why the label is part of the
/// control rather than something the caller places beside it.
/// </remarks>
public class MaterialCheckBox : HorizontalStackLayout
{
    /// <summary>Side of the box.</summary>
    const double BoxSize = 18;

    /// <summary>Thickness of the box outline while unchecked.</summary>
    const double OutlineThickness = 2;

    readonly Border _box;

    readonly Label _mark;

    readonly BodyLarge _label;

    /// <summary>
    /// Whether the box is checked.
    /// </summary>
    public bool IsChecked
    {
        get => (bool) GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
        nameof(IsChecked),
        typeof(bool),
        typeof(MaterialCheckBox),
        defaultValue: false,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (bindable, _, newValue) =>
        {
            var control = (MaterialCheckBox) bindable;
            control.ApplyState();
            control.CheckedChanged?.Invoke(control, new CheckedChangedEventArgs((bool) newValue));
        });

    /// <summary>
    /// Text shown beside the box. Leave empty for a bare checkbox.
    /// </summary>
    public string Text
    {
        get => (string) GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(MaterialCheckBox),
        propertyChanged: (bindable, _, newValue) =>
        {
            var control = (MaterialCheckBox) bindable;
            control._label.Text = (string) newValue;
            control._label.IsVisible = !string.IsNullOrEmpty((string) newValue);
        });

    /// <summary>
    /// Raised when the box is checked or unchecked.
    /// </summary>
    public event EventHandler<CheckedChangedEventArgs>? CheckedChanged;

    public MaterialCheckBox()
    {
        Spacing = 12;
        MinimumHeightRequest = 40;
        VerticalOptions = LayoutOptions.Center;

        _mark = new Label
        {
            Text = "✓",
            FontSize = 12,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            IsVisible = false
        };

        _box = new Border
        {
            WidthRequest = BoxSize,
            HeightRequest = BoxSize,
            Padding = 0,
            VerticalOptions = LayoutOptions.Center,
            StrokeShape = new RoundRectangle { CornerRadius = 2 },
            Content = _mark
        };

        _label = new BodyLarge
        {
            VerticalOptions = LayoutOptions.Center,
            IsVisible = false
        };

        Children.Add(_box);
        Children.Add(_label);

        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) =>
        {
            if (IsEnabled)
            {
                IsChecked = !IsChecked;
            }
        };
        GestureRecognizers.Add(tap);

        ApplyState();
    }

    /// <summary>
    /// Paints the box for the current state.
    /// </summary>
    void ApplyState()
    {
        _mark.IsVisible = IsChecked;

        if (IsChecked)
        {
            _box.SetRoleBrush(Border.BackgroundProperty, MaterialRole.Primary);
            _box.SetRoleBrush(Border.StrokeProperty, MaterialRole.Primary);
            _mark.SetRole(Label.TextColorProperty, MaterialRole.OnPrimary);
            _box.StrokeThickness = 0;
        }
        else
        {
            _box.BackgroundColor = Colors.Transparent;
            _box.SetRoleBrush(Border.StrokeProperty, MaterialRole.OnSurfaceVariant);
            _box.StrokeThickness = OutlineThickness;
        }
    }
}
