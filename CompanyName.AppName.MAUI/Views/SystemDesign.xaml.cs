using CompanyName.AppName.Core.ViewModels;

namespace CompanyName.AppName.MAUI.Views;

public partial class SystemDesign : ContentPage
{
    public SystemDesign(SystemDesignViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}
