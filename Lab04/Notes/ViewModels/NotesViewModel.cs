using Notes.Models;
using Notes.Services;

namespace Notes.ViewModels;

public class NotesViewModel
{
    readonly INavigationService _nav;
    readonly INoteStore _store;

    public NotesViewModel(INavigationService nav, INoteStore store)
    {
        _nav = nav;
        _store = store;
    }

    public IReadOnlyList<Note> Items { get; private set; } = [];

    public async Task LoadAsync() => Items = await _store.GetAllAsync();

    // Pass the ID, not the note.
    public Task OpenAsync(int id) =>
        _nav.GoToAsync($"{Routes.NoteDetail}?id={id}");
}
