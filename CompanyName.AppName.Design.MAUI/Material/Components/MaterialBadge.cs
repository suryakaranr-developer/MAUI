using CompanyName.AppName.Design.MAUI.Material.Tokens;
using Microsoft.Maui.Controls.Shapes;

namespace CompanyName.AppName.Design.MAUI.Material.Components;

/// <summary>
/// A Material Design 3 badge.
/// </summary>
/// <remarks>
/// Marks that something needs attention. With no <see cref="Count"/> it is a small dot; with one
/// it grows to a pill carrying the number. Counts past <see cref="MaxCount"/> are shown as
/// "99+" rather than widening the badge indefinitely.
/// </remarks>
public class MaterialBadge : Border
{
    /// <summary>Diameter of the badge when it carries no number.</summary>
    const double DotSize = 6;

    /// <summary>Height of the badge when it carries a number.</summary>
    const double LabelledHeight = 16;

    /// <summary>Largest number shown before the badge switches to an overflow label.</summary>
    const int MaxCount = 99;

    readonly LabelSmall _label;

    /// <summary>
    /// Number shown in the badge. Zero or less renders the dot form.
    /// </summary>
    public int Count
    {
        get => (int) GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    public static readonly BindableProperty CountProperty = BindableProperty.Create(
        nameof(Count),
        typeof(int),
        typeof(MaterialBadge),
        defaultValue: 0,
        propertyChanged: (bindable, _, _) => ((MaterialBadge) bindable).ApplyState());

    public MaterialBadge()
    {
        Padding = 0;
        StrokeThickness = 0;
        HorizontalOptions = LayoutOptions.Start;
        VerticalOptions = LayoutOptions.Start;

        _label = new LabelSmall
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            IsVisible = false
        };

        Content = _label;

        this.SetRoleBrush(BackgroundProperty, MaterialRole.Error);
        _label.SetRole(Label.TextColorProperty, MaterialRole.OnError);

        ApplyState();
    }

    /// <summary>
    /// Switches between the dot and numbered forms.
    /// </summary>
    void ApplyState()
    {
        if (Count <= 0)
        {
            _label.IsVisible = false;
            WidthRequest = DotSize;
            HeightRequest = DotSize;
            Padding = 0;
            StrokeShape = new RoundRectangle { CornerRadius = DotSize / 2 };
            return;
        }

        _label.IsVisible = true;
        _label.Text = Count > MaxCount ? $"{MaxCount}+" : Count.ToString();

        // Width is left to the layout so a three-character overflow label still fits, while the
        // corner radius keeps the pill fully rounded at any width.
        WidthRequest = -1;
        HeightRequest = LabelledHeight;
        MinimumWidthRequest = LabelledHeight;
        Padding = new Thickness(4, 0);
        StrokeShape = new RoundRectangle { CornerRadius = LabelledHeight / 2 };
    }
}
