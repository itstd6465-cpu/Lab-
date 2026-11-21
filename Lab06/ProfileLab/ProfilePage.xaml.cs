using ProfileLab.ViewModels;

namespace ProfileLab;

public partial class ProfilePage : ContentPage
{
    // The container passes the ViewModel in
    public ProfilePage(ProfileViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
