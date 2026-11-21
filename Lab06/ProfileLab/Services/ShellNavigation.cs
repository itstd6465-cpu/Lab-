namespace ProfileLab.Services;

// The only class that knows about Shell. Lives in the app, not in Core.
public class ShellNavigation : INavigationService
{
    public Task GoToAsync(string route) => Shell.Current.GoToAsync(route);

    public Task GoBackAsync() =>
        Shell.Current.Navigation.NavigationStack.Count > 1
            ? Shell.Current.GoToAsync("..")
            : Task.CompletedTask; // a root page has nothing to go back to
}
