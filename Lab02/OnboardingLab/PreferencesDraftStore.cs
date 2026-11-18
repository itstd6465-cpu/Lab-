using System.Text.Json;

namespace OnboardingLab;

// JSON under one Preferences key. Preferences survive process death; memory does not.
public class PreferencesDraftStore : IDraftStore
{
    public const string Key = "onboarding.draft";

    public void Save(DraftSnapshot snapshot)
    {
        var json = JsonSerializer.Serialize(snapshot);
        Preferences.Default.Set(Key, json);
        App.Log($"Store: saved step {snapshot.Step} ({json.Length} chars)");
    }

    // Safe to fail: missing, old or corrupt data means "start fresh", never a crash.
    public DraftSnapshot? Load()
    {
        var json = Preferences.Default.Get(Key, string.Empty);
        if (string.IsNullOrEmpty(json))
        {
            App.Log("Store: no draft");
            return null;
        }

        try
        {
            var s = JsonSerializer.Deserialize<DraftSnapshot>(json);
            if (s?.Version == DraftSnapshot.CurrentVersion)
            {
                App.Log($"Store: loaded step {s.Step}");
                return s;
            }
            App.Log($"Store: ignored snapshot with version {s?.Version}");
            return null;
        }
        catch (JsonException ex)
        {
            App.Log($"Store: corrupt draft ignored ({ex.Message})");
            return null;
        }
    }

    public void Clear()
    {
        Preferences.Default.Remove(Key);
        App.Log("Store: draft cleared");
    }
}
