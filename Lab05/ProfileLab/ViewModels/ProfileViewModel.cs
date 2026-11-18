using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProfileLab.Models;
using ProfileLab.Services;

namespace ProfileLab.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    private readonly IProfileStore _store;

    // The ViewModel asks for its store; it never creates one.
    public ProfileViewModel(IProfileStore store) => _store = store;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayText))]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))] // re-check CanSave when Name changes
    private string name = "";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))] // ...and when Email changes
    private string email = "";

    [ObservableProperty]
    private string statusMessage = "";

    public string DisplayText =>
        string.IsNullOrWhiteSpace(Name) ? "Hello" : $"Hello, {Name.Trim()}";

    // Generates SaveCommand (the "Async" suffix is dropped), an IAsyncRelayCommand.
    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync()
    {
        if (!CanSave()) return; // a command can still be executed from code; guard it
        await _store.SaveAsync(new Profile(Name.Trim(), Email.Trim()));
        StatusMessage = "Profile saved.";
    }

    private bool CanSave() =>
        !string.IsNullOrWhiteSpace(Name) && Email.Contains('@');

    // Generates LoadCommand.
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
}
