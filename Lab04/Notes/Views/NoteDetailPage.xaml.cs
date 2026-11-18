using Notes.ViewModels;

namespace Notes.Views;

// IQueryAttributable: Shell calls ApplyQueryAttributes with the values from "?id=42"
// after creating the page and before it appears.
public partial class NoteDetailPage : ContentPage, IQueryAttributable
{
    readonly NoteDetailViewModel _vm;

    public NoteDetailPage(NoteDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        // Values from a route string arrive as strings. Keys are matched exactly: "id", not "Id".
        if (query.TryGetValue("id", out var value) && int.TryParse(value?.ToString(), out var id))
            _vm.NoteId = id;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync(); // also runs when Edit is popped: shows the saved text
        TitleLabel.Text = _vm.Title; // Week 5: bindings
        BodyLabel.Text = _vm.Body;
        EditButton.IsEnabled = _vm.Found;
    }

    async void OnEditClicked(object? sender, EventArgs e) => await _vm.EditAsync();
}
