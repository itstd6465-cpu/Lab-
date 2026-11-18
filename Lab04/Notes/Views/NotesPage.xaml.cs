using Notes.Models;
using Notes.ViewModels;

namespace Notes.Views;

public partial class NotesPage : ContentPage
{
    readonly NotesViewModel _vm;
    bool _busy; // stops a double tap from pushing two detail pages

    public NotesPage(NotesViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync(); // reload: titles may have changed on the edit page
        NotesList.ItemsSource = _vm.Items; // Week 5: becomes a binding
    }

    async void OnNoteTapped(object? sender, TappedEventArgs e)
    {
        if (_busy || (sender as BindableObject)?.BindingContext is not Note note) return;
        _busy = true;
        try { await _vm.OpenAsync(note.Id); } // only the ID leaves this page
        finally { _busy = false; }
    }
}
