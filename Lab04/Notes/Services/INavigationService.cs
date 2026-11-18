namespace Notes.Services;

// ViewModels depend on this, never on Shell directly.
public interface INavigationService
{
    Task GoToAsync(string route);
    Task GoBackAsync();
}
