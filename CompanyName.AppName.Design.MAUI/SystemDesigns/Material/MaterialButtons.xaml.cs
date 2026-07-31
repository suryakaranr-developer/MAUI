using CompanyName.AppName.Design.MAUI.Material.Components;

namespace CompanyName.AppName.Design.MAUI.SystemDesigns.Material;

public partial class MaterialButtons : ContentPage
{
    public MaterialButtons()
    {
        InitializeComponent();

        // Segments are populated in code because the control builds its row from the collection,
        // which XAML cannot add to declaratively without a bindable items source.
        ViewSegments.Items.Add(new SegmentedButtonItem { Text = "Day", Value = "day" });
        ViewSegments.Items.Add(new SegmentedButtonItem { Text = "Week", Value = "week" });
        ViewSegments.Items.Add(new SegmentedButtonItem { Text = "Month", Value = "month" });
    }
}
