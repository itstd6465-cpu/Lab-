namespace Notes;

// Every route name in the app, in one place. Pages, ViewModels and tests use these.
public static class Routes
{
    // Root pages: ShellContent in AppShell.xaml. Reach them with "//" + name.
    public const string Notes = "notes";
    public const string Settings = "settings";

    // Pushed pages: registered in AppShell.xaml.cs with Routing.RegisterRoute.
    public const string NoteDetail = "notedetail";
    public const string NoteEdit = "noteedit";

    // Pop one page.
    public const string Back = "..";
}
