using Notes.Services;

namespace Notes.Tests;

public class FakeNavigationService : INavigationService
{
    public List<string> Routes { get; } = new();

    public Task GoToAsync(string route)
    {
        Routes.Add(route);
        return Task.CompletedTask;
    }

    public Task GoBackAsync() => GoToAsync("..");
}
