using Microsoft.Extensions.Logging;

namespace OnboardingLab;

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
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<IDraftStore, PreferencesDraftStore>();
        builder.Services.AddSingleton<OnboardingViewModel>(); // ONE instance: the page and App share it
        builder.Services.AddTransient<OnboardingPage>();

        return builder.Build();
    }
}
