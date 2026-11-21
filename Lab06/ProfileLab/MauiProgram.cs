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

        // Services: one instance for the whole run of the app.
        builder.Services.AddSingleton<IProfileStore, PreferencesProfileStore>();
        builder.Services.AddSingleton<INavigationService, ShellNavigation>();

        // Pages and their ViewModels: a new instance each time one is resolved.
        builder.Services.AddTransient<ProfileViewModel>();
        builder.Services.AddTransient<ProfilePage>();

        return builder.Build();
    }
}
