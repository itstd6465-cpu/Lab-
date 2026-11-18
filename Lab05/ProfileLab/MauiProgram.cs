using Microsoft.Extensions.Logging;
using ProfileLab.Services;
using ProfileLab.ViewModels;

namespace ProfileLab;

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

        builder.Services
            .AddSingleton<IProfileStore, PreferencesProfileStore>()
            .AddTransient<ProfileViewModel>()
            .AddTransient<ProfilePage>();

        return builder.Build();
    }
}
