using Notes.Views;

namespace Notes;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(Routes.NoteDetail, typeof(NoteDetailPage));
        Routing.RegisterRoute(Routes.NoteEdit, typeof(NoteEditPage));
    }
}
