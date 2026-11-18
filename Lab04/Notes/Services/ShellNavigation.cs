using System.Diagnostics;

namespace Notes.Services;

public class ShellNavigation : INavigationService
{
    public async Task GoToAsync(string route)
    {
        await Shell.Current.GoToAsync(route);
        // Lets you watch the stack in the Output window. The exact text can differ by MAUI version.
        Debug.WriteLine($"[Nav] {route} -> {Shell.Current.CurrentState.Location}");
    }

    public Task GoBackAsync() => GoToAsync(Routes.Back);
}
