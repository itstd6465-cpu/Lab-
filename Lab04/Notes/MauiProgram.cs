using Microsoft.Extensions.Logging;
using Notes.Services;
using Notes.ViewModels;
using Notes.Views;

namespace Notes;

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

        var s = builder.Services;
        s.AddSingleton<INavigationService, ShellNavigation>();
        s.AddSingleton<INoteStore, NoteStore>();
        s.AddTransient<NotesPage>();
        s.AddTransient<NotesViewModel>();
        s.AddTransient<NoteDetailPage>();
        s.AddTransient<NoteDetailViewModel>();
        s.AddTransient<NoteEditPage>();
        s.AddTransient<NoteEditViewModel>();
        s.AddTransient<SettingsPage>();

        return builder.Build();
    }
}
