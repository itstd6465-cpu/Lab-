using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OnboardingLab;

public class OnboardingViewModel : INotifyPropertyChanged
{
    public const int LastStep = 3;
    readonly IDraftStore _store;
    int _step = 1;
    string _name = string.Empty;
    string _city = string.Empty;
    string _status = string.Empty;

    // Depends on the interface only. Tests pass a FakeDraftStore here.
    public OnboardingViewModel(IDraftStore store)
    {
        _store = store;
    }

    public int Step
    {
        get => _step;
        set
        {
            if (!Set(ref _step, value)) return;
            OnPropertyChanged(nameof(IsStep1));
            OnPropertyChanged(nameof(IsStep2));
            OnPropertyChanged(nameof(IsStep3));
            OnPropertyChanged(nameof(StepTitle));
            OnPropertyChanged(nameof(CanGoBack));
            OnPropertyChanged(nameof(NextText));
            OnPropertyChanged(nameof(Summary));
        }
    }

    public string Name { get => _name; set => Set(ref _name, value ?? string.Empty); }
    public string City { get => _city; set => Set(ref _city, value ?? string.Empty); }
    public string Status { get => _status; set => Set(ref _status, value ?? string.Empty); }

    public bool IsStep1 => Step == 1;
    public bool IsStep2 => Step == 2;
    public bool IsStep3 => Step == 3;
    public bool CanGoBack => Step > 1;

    public string StepTitle => $"Onboarding · {Step} of {LastStep}";
    public string NextText => Step == LastStep ? "Finish" : "Next";
    public string Summary => $"Name: {Name}\nCity: {City}";

    bool IsPristine => Step == 1 && Name.Length == 0 && City.Length == 0;

    // --- Memento: save often, restore once, clear when done ---
    public void SaveDraft()
    {
        if (IsPristine)
        {
            _store.Clear(); // nothing worth keeping; do not leave an empty draft behind
            return;
        }
        _store.Save(new DraftSnapshot(DraftSnapshot.CurrentVersion, Step, Name, City));
    }

    public void RestoreDraft()
    {
        var s = _store.Load();
        if (s is null) return; // no draft, or old/corrupt: keep defaults
        if (s.Step < 1 || s.Step > LastStep) return; // nonsense value: keep defaults

        Step = s.Step;
        Name = s.Name ?? string.Empty;
        City = s.City ?? string.Empty;
        Status = "Draft restored.";
    }

    public void Next()
    {
        if (Step >= LastStep) return;
        Step++;
        Status = string.Empty;
        SaveDraft(); // each step done
    }

    public void Back()
    {
        if (Step <= 1) return;
        Step--;
        SaveDraft();
    }

    public void Finish()
    {
        _store.Clear(); // form finished: the draft is no longer needed
        Step = 1;
        Name = string.Empty;
        City = string.Empty;
        Status = "Account set up.";
    }

    // --- INotifyPropertyChanged plumbing ---
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    protected bool Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(name);
        return true;
    }
}
