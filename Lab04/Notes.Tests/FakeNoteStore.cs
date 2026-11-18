using Notes.Models;
using Notes.Services;

namespace Notes.Tests;

public class FakeNoteStore : INoteStore
{
    public Dictionary<int, Note> Notes { get; } = new();

    public Task<IReadOnlyList<Note>> GetAllAsync() =>
        Task.FromResult<IReadOnlyList<Note>>(Notes.Values.OrderBy(n => n.Id).ToList());

    public Task<Note?> GetAsync(int id) =>
        Task.FromResult<Note?>(Notes.TryGetValue(id, out var note) ? note : null);

    public Task SaveAsync(Note note)
    {
        Notes[note.Id] = note;
        return Task.CompletedTask;
    }
}
