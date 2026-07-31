namespace CompanyName.AppName.MAUI;

public partial class App : Application
{
	readonly AppShell _shell;

	public App(AppShell shell)
	{
		InitializeComponent();

		_shell = shell;
	}

	// The shell is injected and handed to the window here rather than assigned to the obsolete
	// MainPage property, which is the supported way to set the root on MAUI 9.
	protected override Window CreateWindow(IActivationState? activationState)
		=> new Window(_shell);
}

