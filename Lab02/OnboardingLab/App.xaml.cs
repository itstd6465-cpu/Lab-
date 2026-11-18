using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace OnboardingLab;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());

        // APP START: restore once, from storage
        ResolveViewModel()?.RestoreDraft();

        window.Created += (s, e) => Log("Created");
        window.Activated += (s, e) => Log("Activated");
        window.Resumed += (s, e) => Log("Resumed");
        window.Destroying += (s, e) => Log("Destroying"); // logged only; never a save point

        // SAVE POINTS: the last events you are guaranteed before the OS may kill the process
        window.Deactivated += (s, e) => { Log("Deactivated"); ResolveViewModel()?.SaveDraft(); };
        window.Stopped += (s, e) => { Log("Stopped"); ResolveViewModel()?.SaveDraft(); };

        return window;
    }

    static OnboardingViewModel? ResolveViewModel() =>
        IPlatformApplication.Current?.Services.GetService<OnboardingViewModel>();

    // Debug.WriteLine -> Visual Studio Output window (debugger attached).
    // Console.WriteLine -> Android logcat, tag DOTNET (works after the debugger has gone).
    public static void Log(string message)
    {
        var line = $"[Lab2] {DateTime.Now:HH:mm:ss.fff} {message}";
        Debug.WriteLine(line);
        Console.WriteLine(line);
    }
}
