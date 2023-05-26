namespace WaterTrackerMAUI;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

    protected override Window CreateWindow(IActivationState activationState)
    {
		var window = base.CreateWindow(activationState);
		MainPage mainPage = new MainPage();

		window.Height = 800;
		window.Width = 400;

		window.Destroying += (s, e) =>
		{
			mainPage.OnDestroying();
		};

		return window;
    }
}
