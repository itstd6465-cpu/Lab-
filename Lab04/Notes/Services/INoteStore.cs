using Notes.Models;

namespace Notes.Services;

public interface INoteStore
{
    Task<IReadOnlyList<Note>> GetAllAsync();
    Task<Note?> GetAsync(int id); // null if there is no note with this ID
    Task SaveAsync(Note note);
}
