using Profile.Tests.Fakes;
using ProfileLab.Models;
using ProfileLab.ViewModels;
using Xunit;

namespace Profile.Tests;

using Profile = ProfileLab.Models.Profile;

public class ProfileViewModelTests
{
    // Command test, happy path (lecture slide 13).
    [Fact]
    public async Task Save_ValidForm_StoresProfileAndGoesBack()
    {
        // Arrange
        var store = new FakeProfileStore();
        var nav = new FakeNavigationService();
        var vm = new ProfileViewModel(store, nav)
            { Name = "Amina", Email = "amina@example.com" };

        // Act
        await vm.SaveCommand.ExecuteAsync(null);

        // Assert
        Assert.Equal("Amina", store.Saved?.Name);
        Assert.Equal("amina@example.com", store.Saved?.Email);
        Assert.Equal("..", nav.LastRoute);
    }

    // Command test, the "no" path: bad email, so CanExecute is false and nothing is stored.
    [Fact]
    public async Task Save_BadEmail_CannotExecuteAndStoresNothing()
    {
        var store = new FakeProfileStore();
        var nav = new FakeNavigationService();
        var vm = new ProfileViewModel(store, nav)
            { Name = "Amina", Email = "amina@" };

        Assert.False(vm.SaveCommand.CanExecute(null));
        await vm.SaveCommand.ExecuteAsync(null); // even if something calls it anyway

        Assert.Equal(0, store.SaveCalls);
        Assert.Empty(nav.Routes);
    }

    // Validation test: the rule and its message, checked together.
    [Theory]
    [InlineData("amina")]
    [InlineData("amina@")]
    [InlineData("@example.com")]
    [InlineData("amina@example")]
    public void Email_Invalid_ShowsMessage(string email)
    {
        var vm = new ProfileViewModel(new FakeProfileStore(), new FakeNavigationService());
        vm.Email = email;

        Assert.Equal("Enter an email such as name@example.com", vm.EmailError);
    }

    [Fact]
    public void Email_Valid_ClearsMessage()
    {
        var vm = new ProfileViewModel(new FakeProfileStore(), new FakeNavigationService());
        vm.Email = "amina"; // error shown
        vm.Email = "amina@example.com"; // fixed

        Assert.Equal(string.Empty, vm.EmailError);
    }

    // Custom test 1: Name change raises SaveCommand.CanExecuteChanged
    [Fact]
    public void Name_Changed_RaisesSaveCanExecuteChanged()
    {
        var vm = new ProfileViewModel(new FakeProfileStore(), new FakeNavigationService());
        int raised = 0;
        vm.SaveCommand.CanExecuteChanged += (_, _) => raised++;

        vm.Name = "Amina";

        Assert.True(raised > 0);
    }

    // Custom test 2: Save trims leading and trailing spaces
    [Fact]
    public async Task Save_TrimsSpaces()
    {
        var store = new FakeProfileStore();
        var nav = new FakeNavigationService();
        var vm = new ProfileViewModel(store, nav)
            { Name = "  Amina  ", Email = "  amina@example.com  " };

        await vm.SaveCommand.ExecuteAsync(null);

        Assert.Equal("Amina", store.Saved?.Name);
        Assert.Equal("amina@example.com", store.Saved?.Email);
    }
}
