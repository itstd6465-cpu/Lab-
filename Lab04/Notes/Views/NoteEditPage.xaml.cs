using Notes.ViewModels;

namespace Notes.Views;

public partial class NoteEditPage : ContentPage, IQueryAttributable
{
    readonly NoteEditViewModel _vm;
    bool _loaded; // load once: reloading would wipe what the user typed

    public NoteEditPage(NoteEditViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var value) && int.TryParse(value?.ToString(), out var id))
            _vm.NoteId = id;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_loaded) return;
        _loaded = true;
        await _vm.LoadAsync();
        TitleEntry.Text = _vm.Title; // Week 5: TwoWay bindings
        BodyEditor.Text = _vm.Body;
    }

    async void OnSaveClicked(object? sender, EventArgs e)
    {
        _vm.Title = TitleEntry.Text ?? "";
        _vm.Body = BodyEditor.Text ?? "";
        await _vm.SaveAsync();
    }

    async void OnCancelClicked(object? sender, EventArgs e) => await _vm.CancelAsync();
}
