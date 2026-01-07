using Microsoft.Extensions.Logging;

namespace noten;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("NewYork.ttf", "NewYork");
				fonts.AddFont("SF-Pro.ttf", "SFPro");
				fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
