using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProfileLab.Models;
using ProfileLab.Services;

namespace ProfileLab.ViewModels;

public partial class ProfileViewModel(
    IProfileStore store,
    INavigationService nav) : ObservableObject
{
    private readonly IProfileStore _store = store;
    private readonly INavigationService _nav = nav;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayText))]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string name = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string email = string.Empty;

    [ObservableProperty]
    private string nameError = string.Empty;

    [ObservableProperty]
    private string emailError = string.Empty;

    [ObservableProperty]
    private string statusMessage = string.Empty;

    public string DisplayText =>
        string.IsNullOrWhiteSpace(Name) ? "Hello" : $"Hello, {Name.Trim()}";

    // Generated hooks: run after the property has changed.
    // An untouched empty form shows no errors; a field shows its message once the user has typed in it.
    partial void OnNameChanged(string value) =>
        NameError = string.IsNullOrWhiteSpace(value)
            ? "Enter your name."
            : string.Empty;

    partial void OnEmailChanged(string value) =>
        EmailError = IsValidEmail(value)
            ? string.Empty
            : "Enter an email such as name@example.com";

    // Generates SaveCommand (IAsyncRelayCommand). While CanSave() is false, a bound button disables itself.
    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync()
    {
        if (!CanSave()) return; // ExecuteAsync can be called from code even when CanExecute is false
        await _store.SaveAsync(new Profile(Name.Trim(), Email.Trim()));
        StatusMessage = "Profile saved.";
        await _nav.GoBackAsync();
    }

    private bool CanSave() =>
        !string.IsNullOrWhiteSpace(Name) && IsValidEmail(Email);

    [RelayCommand]
    private async Task LoadAsync()
    {
        Profile? saved = await _store.LoadAsync();
        if (saved is null)
        {
            StatusMessage = "No saved profile yet.";
            return;
        }

        Name = saved.Name;
        Email = saved.Email;
        StatusMessage = "Loaded the saved profile.";
    }

    // Deliberately simple: one @, something before it, a dot inside the part after it, no spaces.
    private static bool IsValidEmail(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;
        string s = value.Trim();
        if (s.Contains(' ')) return false;
        int at = s.IndexOf('@');
        if (at <= 0 || at != s.LastIndexOf('@')) return false;
        string domain = s[(at + 1)..];
        int dot = domain.LastIndexOf('.');
        return dot > 0 && dot < domain.Length - 1;
    }
}
