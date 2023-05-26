using Microsoft.Extensions.Logging;

namespace WaterTrackerMAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
                fonts.AddFont("Rambla-Bold.ttf", "Black");
                fonts.AddFont("Rambla-Regular.ttf", "Regular");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
