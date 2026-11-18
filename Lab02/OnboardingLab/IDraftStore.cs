namespace OnboardingLab;

public interface IDraftStore
{
    void Save(DraftSnapshot snapshot);
    DraftSnapshot? Load();
    void Clear();
}
