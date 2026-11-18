using System.Text.Json;
using Notes.Models;

namespace Notes.Services;

// JSON file in the app's private folder. Week 10 replaces this with SQLite behind the same interface.
public class NoteStore : INoteStore
{
    readonly string _path = Path.Combine(FileSystem.Current.AppDataDirectory, "notes.json");
    List<Note>? _notes;

    public async Task<IReadOnlyList<Note>> GetAllAsync()
    {
        var notes = await LoadAsync();
        return notes.OrderBy(n => n.Id).Select(Copy).ToList();
    }

    public async Task<Note?> GetAsync(int id)
    {
        var notes = await LoadAsync();
        var note = notes.FirstOrDefault(n => n.Id == id);
        return note is null ? null : Copy(note);
    }

    public async Task SaveAsync(Note note)
    {
        var notes = await LoadAsync();
        notes.RemoveAll(n => n.Id == note.Id);
        notes.Add(Copy(note));
        await using var stream = File.Create(_path);
        await JsonSerializer.SerializeAsync(stream, notes);
    }

    async Task<List<Note>> LoadAsync()
    {
        if (_notes is not null)
            return _notes;

        if (File.Exists(_path))
        {
            await using var stream = File.OpenRead(_path);
            _notes = await JsonSerializer.DeserializeAsync<List<Note>>(stream) ?? new List<Note>();
        }
        else
        {
            _notes = Seed();
        }
        return _notes;
    }

    static Note Copy(Note n) => new() { Id = n.Id, Title = n.Title, Body = n.Body, Updated = n.Updated };

    static List<Note> Seed() =>
    [
        new() { Id = 1, Title = "Shopping list", Body = "Milk, bread, a USB-C cable.", Updated = DateTime.Now },
        new() { Id = 2, Title = "Lab 4 questions", Body = "Why pass an ID and not the note?", Updated = DateTime.Now },
        new() { Id = 42, Title = "The answer", Body = "Forty-two. Now what was the question?", Updated = DateTime.Now },
    ];
}
