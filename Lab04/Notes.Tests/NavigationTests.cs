using Notes.Models;
using Notes.ViewModels;
using Xunit;

namespace Notes.Tests;

public class NavigationTests
{
    [Fact]
    public async Task OpenAsync_GoesToDetail()
    {
        var nav = new FakeNavigationService();
        var vm = new NotesViewModel(nav, new FakeNoteStore());

        await vm.OpenAsync(42);

        // The literal string on purpose: renaming the route must break this test.
        Assert.Equal("notedetail?id=42", nav.Routes.Single());
    }

    [Fact]
    public async Task SaveAsync_StoresNoteThenGoesBack()
    {
        var nav = new FakeNavigationService();
        var store = new FakeNoteStore();
        store.Notes[42] = new Note { Id = 42, Title = "Old", Body = "" };
        var vm = new NoteEditViewModel(nav, store) { NoteId = 42 };

        await vm.LoadAsync();
        vm.Title = "New";
        await vm.SaveAsync();

        Assert.Equal("..", nav.Routes.Single());
        Assert.Equal("New", store.Notes[42].Title);
    }
}
