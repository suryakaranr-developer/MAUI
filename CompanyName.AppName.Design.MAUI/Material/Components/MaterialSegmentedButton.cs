using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CompanyName.AppName.Design.MAUI.Material.Tokens;
using Microsoft.Maui.Controls.Shapes;

namespace CompanyName.AppName.Design.MAUI.Material.Components;

/// <summary>
/// One option within a <see cref="MaterialSegmentedButton"/>.
/// </summary>
public class SegmentedButtonItem : BindableObject
{
    /// <summary>
    /// Label shown on the segment.
    /// </summary>
    public string Text
    {
        get => (string) GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(SegmentedButtonItem));

    /// <summary>
    /// Value this segment stands for.
    /// </summary>
    public object? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value),
        typeof(object),
        typeof(SegmentedButtonItem));
}

/// <summary>
/// A Material Design 3 segmented button.
/// </summary>
/// <remarks>
/// For choosing between a small set of related options, where seeing all of them at once matters.
/// Material caps a segmented button at five options; past that a menu is the better control.
/// </remarks>
public class MaterialSegmentedButton : Border
{
    /// <summary>Height of the control, per the Material spec.</summary>
    const double SegmentHeight = 40;

    readonly Grid _segments;

    /// <summary>
    /// The options, in the order they are shown.
    /// </summary>
    public IList<SegmentedButtonItem> Items { get; } = new ObservableCollection<SegmentedButtonItem>();

    /// <summary>
    /// Index of the chosen option, or -1 when nothing is chosen.
    /// </summary>
    public int SelectedIndex
    {
        get => (int) GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(
        nameof(SelectedIndex),
        typeof(int),
        typeof(MaterialSegmentedButton),
        defaultValue: 0,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (bindable, _, _) =>
        {
            var control = (MaterialSegmentedButton) bindable;
            control.PaintSegments();
            control.SelectionChanged?.Invoke(control, EventArgs.Empty);
        });

    /// <summary>
    /// Value of the chosen option.
    /// </summary>
    public object? SelectedValue =>
        SelectedIndex >= 0 && SelectedIndex < Items.Count ? Items[SelectedIndex].Value : null;

    /// <summary>
    /// Raised when the chosen option changes.
    /// </summary>
    public event EventHandler? SelectionChanged;

    public MaterialSegmentedButton()
    {
        HeightRequest = SegmentHeight;
        Padding = 0;
        StrokeThickness = 1;
        HorizontalOptions = LayoutOptions.Start;
        StrokeShape = new RoundRectangle { CornerRadius = MaterialShape.Full };

        this.SetRoleBrush(StrokeProperty, MaterialRole.Outline);
        BackgroundColor = Colors.Transparent;

        _segments = new Grid { ColumnSpacing = 0 };
        Content = _segments;

        ((ObservableCollection<SegmentedButtonItem>) Items).CollectionChanged += OnItemsChanged;
    }

    void OnItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        BuildSegments();
    }

    /// <summary>
    /// Rebuilds the segment row from the current items.
    /// </summary>
    void BuildSegments()
    {
        _segments.Children.Clear();
        _segments.ColumnDefinitions.Clear();

        for (var i = 0; i < Items.Count; i++)
        {
            _segments.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            var index = i;

            var label = new LabelLarge
            {
                Text = Items[i].Text,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            var cell = new Grid
            {
                Padding = new Thickness(12, 0)
            };

            // The state layer fills the cell under the label; the negative margin lets it reach
            // the cell's edges past its padding. The outer Border clips the end cells' corners.
            var stateLayer = MaterialStateLayer.Attach(cell, MaterialRole.OnSurface);
            stateLayer.Margin = new Thickness(-12, 0);
            cell.Children.Add(stateLayer);
            cell.Children.Add(label);

            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, _) => SelectedIndex = index;
            cell.GestureRecognizers.Add(tap);

            // Every segment but the first carries a leading rule, which is what produces the
            // dividers between them without drawing a separate set of lines.
            if (i > 0)
            {
                var rule = new BoxView
                {
                    WidthRequest = 1,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Fill
                };
                rule.SetRole(BoxView.ColorProperty, MaterialRole.Outline);
                cell.Children.Add(rule);
            }

            _segments.Add(cell, i, 0);
        }

        PaintSegments();
    }

    /// <summary>
    /// Paints each segment against the current selection.
    /// </summary>
    void PaintSegments()
    {
        for (var i = 0; i < _segments.Children.Count; i++)
        {
            if (_segments.Children[i] is not Grid cell)
            {
                continue;
            }

            var selected = i == SelectedIndex;

            if (selected)
            {
                cell.SetRole(BackgroundColorProperty, MaterialRole.SecondaryContainer);
            }
            else
            {
                cell.BackgroundColor = Colors.Transparent;
            }

            foreach (var label in cell.Children.OfType<LabelLarge>())
            {
                if (selected)
                {
                    label.SetRole(Label.TextColorProperty, MaterialRole.OnSecondaryContainer);
                }
                else
                {
                    label.SetRole(Label.TextColorProperty, MaterialRole.OnSurface);
                }
            }
        }
    }
}
