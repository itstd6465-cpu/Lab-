namespace OnboardingLab;

// Small (well under 50 KB), versioned, holds identifiers and short text only.
public record DraftSnapshot(int Version, int Step, string Name, string City)
{
    public const int CurrentVersion = 1;
}
