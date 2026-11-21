using ProfileLab.Services;

namespace Profile.Tests.Fakes;

// Records the route instead of moving.
public class FakeNavigationService : INavigationService
{
    public List<string> Routes { get; } = new();
    public string? LastRoute => Routes.LastOrDefault();

    public Task GoToAsync(string route)
    {
        Routes.Add(route);
        return Task.CompletedTask;
    }

    public Task GoBackAsync() => GoToAsync("..");
}
