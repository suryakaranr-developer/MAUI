using CompanyName.AppName.Design.MAUI.Material.Tokens;
using Microsoft.Maui.Controls.Shapes;

namespace CompanyName.AppName.Design.MAUI.Material.Components;

/// <summary>
/// A Material Design 3 radio button, with an optional label.
/// </summary>
/// <remarks>
/// For selecting exactly one option from a set. Give every button in a set the same
/// <see cref="GroupName"/> and selecting one clears the rest, which is what distinguishes a
/// radio group from a row of checkboxes.
/// </remarks>
public class MaterialRadioButton : HorizontalStackLayout
{
    /// <summary>Diameter of the outer ring.</summary>
    const double RingSize = 20;

    /// <summary>Diameter of the dot shown when selected.</summary>
    const double DotSize = 10;

    /// <summary>Thickness of the outer ring.</summary>
    const double RingThickness = 2;

    readonly Ellipse _ring;

    readonly Ellipse _dot;

    readonly BodyLarge _label;

    /// <summary>
    /// Whether this option is the selected one.
    /// </summary>
    public bool IsChecked
    {
        get => (bool) GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
        nameof(IsChecked),
        typeof(bool),
        typeof(MaterialRadioButton),
        defaultValue: false,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (bindable, _, newValue) =>
        {
            var control = (MaterialRadioButton) bindable;
            control.ApplyState();

            if ((bool) newValue)
            {
                control.ClearSiblings();
            }

            control.CheckedChanged?.Invoke(control, new CheckedChangedEventArgs((bool) newValue));
        });

    /// <summary>
    /// Name tying this button to the others it is mutually exclusive with.
    /// </summary>
    public string GroupName
    {
        get => (string) GetValue(GroupNameProperty);
        set => SetValue(GroupNameProperty, value);
    }

    public static readonly BindableProperty GroupNameProperty = BindableProperty.Create(
        nameof(GroupName),
        typeof(string),
        typeof(MaterialRadioButton));

    /// <summary>
    /// Text shown beside the button. Leave empty for a bare radio button.
    /// </summary>
    public string Text
    {
        get => (string) GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(MaterialRadioButton),
        propertyChanged: (bindable, _, newValue) =>
        {
            var control = (MaterialRadioButton) bindable;
            control._label.Text = (string) newValue;
            control._label.IsVisible = !string.IsNullOrEmpty((string) newValue);
        });

    /// <summary>
    /// Value this option stands for, for callers reading the selection off a group.
    /// </summary>
    public object? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value),
        typeof(object),
        typeof(MaterialRadioButton));

    /// <summary>
    /// Raised when this button is selected or cleared.
    /// </summary>
    public event EventHandler<CheckedChangedEventArgs>? CheckedChanged;

    public MaterialRadioButton()
    {
        Spacing = 12;
        MinimumHeightRequest = 40;
        VerticalOptions = LayoutOptions.Center;

        _ring = new Ellipse
        {
            WidthRequest = RingSize,
            HeightRequest = RingSize,
            StrokeThickness = RingThickness,
            Fill = Brush.Transparent
        };

        _dot = new Ellipse
        {
            WidthRequest = DotSize,
            HeightRequest = DotSize,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            IsVisible = false
        };

        var ringContainer = new Grid
        {
            WidthRequest = RingSize,
            HeightRequest = RingSize,
            VerticalOptions = LayoutOptions.Center,
            Children = { _ring, _dot }
        };

        _label = new BodyLarge
        {
            VerticalOptions = LayoutOptions.Center,
            IsVisible = false
        };

        Children.Add(ringContainer);
        Children.Add(_label);

        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) =>
        {
            // Tapping the selected option is a no-op: a radio group always has one answer, so
            // there is no way to clear it from the group itself.
            if (IsEnabled && !IsChecked)
            {
                IsChecked = true;
            }
        };
        GestureRecognizers.Add(tap);

        ApplyState();
    }

    /// <summary>
    /// Clears every other button sharing this one's group name.
    /// </summary>
    void ClearSiblings()
    {
        if (string.IsNullOrEmpty(GroupName) || Parent is not Layout parent)
        {
            return;
        }

        foreach (var sibling in parent.Children.OfType<MaterialRadioButton>())
        {
            if (!ReferenceEquals(sibling, this) && sibling.GroupName == GroupName)
            {
                sibling.IsChecked = false;
            }
        }
    }

    /// <summary>
    /// Paints the ring and dot for the current state.
    /// </summary>
    void ApplyState()
    {
        _dot.IsVisible = IsChecked;

        if (IsChecked)
        {
            _ring.SetRoleBrush(Shape.StrokeProperty, MaterialRole.Primary);
            _dot.SetRoleBrush(Shape.FillProperty, MaterialRole.Primary);
        }
        else
        {
            _ring.SetRoleBrush(Shape.StrokeProperty, MaterialRole.OnSurfaceVariant);
        }
    }
}
