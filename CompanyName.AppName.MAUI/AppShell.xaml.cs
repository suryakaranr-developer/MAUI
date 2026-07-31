using CompanyName.AppName.MAUI.Views;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.AppName.MAUI;

public partial class AppShell : Shell
{
	readonly IServiceProvider _services;

	public AppShell(IServiceProvider services)
	{
		InitializeComponent();

		_services = services;
		BuildTabs();
	}

	/// <summary>
	/// Builds the tab bar, resolving each root page from the container through a factory template.
	/// </summary>
	/// <remarks>
	/// The factory form of <see cref="DataTemplate"/> is what routes construction through the
	/// service provider, so a tab page receives its injected view model. The page is still created
	/// lazily, only when its tab is first shown.
	/// </remarks>
	void BuildTabs()
	{
		var tabs = new TabBar();

		tabs.Items.Add(new ShellContent
		{
			Route = "MainPage",
			ContentTemplate = new DataTemplate(() => _services.GetRequiredService<MainPage>())
		});

		tabs.Items.Add(new ShellContent
		{
			Title = "System Design",
			Icon = ImageSource.FromFile("system_design.png"),
			Route = "SystemDesign",
			ContentTemplate = new DataTemplate(() => _services.GetRequiredService<SystemDesign>())
		});

		Items.Add(tabs);
	}
}
