namespace ProfileLab.Services;

// ViewModels navigate through this, never through Shell.Current (Week 4).
public interface INavigationService
{
    Task GoToAsync(string route);
    Task GoBackAsync();
}
