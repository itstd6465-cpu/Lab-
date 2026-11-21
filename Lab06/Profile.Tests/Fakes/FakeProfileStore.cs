using ProfileLab.Models;
using ProfileLab.Services;

namespace Profile.Tests.Fakes;

using Profile = ProfileLab.Models.Profile;

// A small, real class that records what happened (lecture slide 11).
public class FakeProfileStore : IProfileStore
{
    public Profile? Saved { get; private set; }
    public int SaveCalls { get; private set; }

    public Task SaveAsync(Profile p)
    {
        Saved = p;
        SaveCalls++;
        return Task.CompletedTask;
    }

    // IProfileStore also has LoadAsync (Lab 5), so the fake must implement it.
    public Task<Profile?> LoadAsync() => Task.FromResult(Saved);

    // Lets a test start with a profile already "on disk".
    public void Seed(Profile p) => Saved = p;
}
