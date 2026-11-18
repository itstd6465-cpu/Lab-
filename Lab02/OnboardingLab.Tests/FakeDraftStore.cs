using OnboardingLab;

namespace OnboardingLab.Tests;

// Keeps the snapshot in a field. No emulator, no Preferences.
public class FakeDraftStore : IDraftStore
{
    public DraftSnapshot? Stored { get; private set; }
    public int SaveCount { get; private set; }
    public bool WasCleared { get; private set; }

    public void Save(DraftSnapshot snapshot)
    {
        Stored = snapshot;
        SaveCount++;
    }

    public DraftSnapshot? Load() => Stored;

    public void Clear()
    {
        Stored = null;
        WasCleared = true;
    }
}
