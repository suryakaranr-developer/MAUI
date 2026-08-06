using CommunityToolkit.Maui;
using CompanyName.AppName.Design.MAUI.Extensions;
using CompanyName.AppName.MAUI.Configuration;
using CompanyName.AppName.MAUI.Navigation;
using CompanyName.AppName.MAUI.Views;
using CompanyName.AppName.Core.Configuration;
using CompanyName.AppName.Core.Navigation;
using CompanyName.AppName.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace CompanyName.AppName.MAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseApplicationDesignSystem();

		RegisterConfiguration(builder.Services);
		RegisterNavigation(builder.Services);
		RegisterViewModels(builder.Services);
		RegisterPages(builder.Services);

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	/// <summary>
	/// Registers the build environment the app was compiled for. Singleton because the values are
	/// stamped into the assembly and cannot change while the process is alive.
	/// </summary>
	static void RegisterConfiguration(IServiceCollection services)
	{
		services.AddSingleton<IAppEnvironment, BuildAppEnvironment>();
	}

	/// <summary>
	/// Registers the navigation service and the shell, and maps every detail route to its page.
	/// </summary>
	static void RegisterNavigation(IServiceCollection services)
	{
		services.AddSingleton<INavigationService, ShellNavigationService>();
		services.AddSingleton<AppShell>();

		foreach (var (route, page) in AppRoutes.Details)
		{
			Routing.RegisterRoute(route, page);
		}
	}

	/// <summary>
	/// Registers view models. Transient so each page gets a fresh one.
	/// </summary>
	static void RegisterViewModels(IServiceCollection services)
	{
		services.AddTransient<SystemDesignViewModel>();
	}

	/// <summary>
	/// Registers pages so the container constructs them with their dependencies injected. The tab
	/// pages are resolved by the shell; the detail pages are resolved when their route is pushed.
	/// </summary>
	static void RegisterPages(IServiceCollection services)
	{
		services.AddTransient<MainPage>();
		services.AddTransient<SystemDesign>();

		foreach (var (_, page) in AppRoutes.Details)
		{
			services.AddTransient(page);
		}
	}
}
