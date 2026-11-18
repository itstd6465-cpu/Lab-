using ProfileLab.Models;

namespace ProfileLab.Services;

// The ViewModel depends on this interface, never on Preferences (slide 11).
public interface IProfileStore
{
    Task SaveAsync(Profile profile);
    Task<Profile?> LoadAsync(); // null when nothing has been saved yet
}
