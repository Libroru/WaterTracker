namespace WaterTrackerMaui2;

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

        window.MaximumHeight = window.Height;
        window.MaximumWidth = window.Width;

        window.MinimumHeight = window.Height;
        window.MinimumWidth = window.Width;

        window.Destroying += (s, e) =>
        {
            mainPage.OnDestroying();
        };

        return window;
    }
}
