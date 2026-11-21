using System.Text.Json;
using ProfileLab.Models;

namespace ProfileLab.Services;

// The only class that knows the profile lives in Preferences.
public class PreferencesProfileStore : IProfileStore
{
    const string Key = "profile.v1";

    public Task SaveAsync(Profile profile)
    {
        Preferences.Default.Set(Key, JsonSerializer.Serialize(profile));
        return Task.CompletedTask;
    }

    public Task<Profile?> LoadAsync()
    {
        string json = Preferences.Default.Get(Key, string.Empty);
        if (string.IsNullOrEmpty(json))
            return Task.FromResult<Profile?>(null);

        try
        {
            return Task.FromResult(JsonSerializer.Deserialize<Profile>(json));
        }
        catch (JsonException)
        {
            return Task.FromResult<Profile?>(null); // corrupt data: start fresh (Week 2 rule)
        }
    }
}
