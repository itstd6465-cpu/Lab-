using Notes.Models;
using Notes.Services;

namespace Notes.ViewModels;

public class NoteEditViewModel
{
    readonly INavigationService _nav;
    readonly INoteStore _store;

    public NoteEditViewModel(INavigationService nav, INoteStore store)
    {
        _nav = nav;
        _store = store;
    }

    public int NoteId { get; set; }
    public string Title { get; set; } = "";
    public string Body { get; set; } = "";

    public async Task LoadAsync()
    {
        var note = await _store.GetAsync(NoteId);
        Title = note?.Title ?? "";
        Body = note?.Body ?? "";
    }

    public async Task SaveAsync()
    {
        var note = new Note
        {
            Id = NoteId,
            Title = string.IsNullOrWhiteSpace(Title) ? "(untitled)" : Title.Trim(),
            Body = Body,
            Updated = DateTime.Now,
        };
        await _store.SaveAsync(note); // store first, then leave
        await _nav.GoBackAsync(); // ".." pops back to detail
    }

    public Task CancelAsync() => _nav.GoBackAsync();
}
