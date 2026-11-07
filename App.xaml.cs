
using System.Diagnostics;

namespace LifecycleLab;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());

        window.Created += (s, e) => Log("Created");
        window.Activated += (s, e) => Log("Activated");
        window.Deactivated += (s, e) => Log("Deactivated");
        window.Stopped += (s, e) => Log("Stopped");
        window.Resumed += (s, e) => Log("Resumed");
        window.Destroying += (s, e) => Log("Destroying");

        return window;
    }

    static void Log(string name) =>
        Debug.WriteLine($"[Lifecycle] {DateTime.Now:HH:mm:ss.fff} {name}");
}