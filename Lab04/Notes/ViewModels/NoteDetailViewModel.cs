using Notes.Services;

namespace Notes.ViewModels;

public class NoteDetailViewModel
{
    readonly INavigationService _nav;
    readonly INoteStore _store;

    public NoteDetailViewModel(INavigationService nav, INoteStore store)
    {
        _nav = nav;
        _store = store;
    }

    public int NoteId { get; set; } // set from the route: notedetail?id=42
    public string Title { get; private set; } = "";
    public string Body { get; private set; } = "";
    public bool Found { get; private set; }

    // Called every time the page appears, so a saved edit shows on Back.
    public async Task LoadAsync()
    {
        var note = await _store.GetAsync(NoteId);
        Found = note is not null;
        Title = note?.Title ?? "Note not found";
        Body = note?.Body ?? $"There is no note with ID {NoteId}.";
    }

    public Task EditAsync() =>
        _nav.GoToAsync($"{Routes.NoteEdit}?id={NoteId}");
}
