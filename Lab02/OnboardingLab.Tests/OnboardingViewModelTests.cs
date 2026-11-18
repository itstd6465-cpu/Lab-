using OnboardingLab;
using Xunit;

namespace OnboardingLab.Tests;

public class OnboardingViewModelTests
{
    [Fact]
    public void RestoreDraft_SetsStepAndFields_FromStore()
    {
        var store = new FakeDraftStore();
        store.Save(new DraftSnapshot(
            DraftSnapshot.CurrentVersion, 2, "Amina", "Benghazi"));
        var vm = new OnboardingViewModel(store);

        vm.RestoreDraft();

        Assert.Equal(2, vm.Step);
        Assert.Equal("Amina", vm.Name);
        Assert.Equal("Benghazi", vm.City);
    }

    [Fact]
    public void SaveDraft_WritesCurrentState_ToStore()
    {
        var store = new FakeDraftStore();
        var vm = new OnboardingViewModel(store) { Name = "Amina", City = "Benghazi" };
        vm.Next(); // step 2, also saves
        vm.SaveDraft();

        Assert.NotNull(store.Stored);
        Assert.Equal(DraftSnapshot.CurrentVersion, store.Stored!.Version);
        Assert.Equal(2, store.Stored.Step);
        Assert.Equal("Amina", store.Stored.Name);
        Assert.Equal("Benghazi", store.Stored.City);
    }

    [Fact]
    public void RestoreDraft_WithNoSnapshot_LeavesDefaults()
    {
        var vm = new OnboardingViewModel(new FakeDraftStore());

        vm.RestoreDraft();

        Assert.Equal(1, vm.Step);
        Assert.Equal(string.Empty, vm.Name);
        Assert.Equal(string.Empty, vm.City);
    }

    [Fact]
    public void Finish_ClearsTheDraft()
    {
        var store = new FakeDraftStore();
        var vm = new OnboardingViewModel(store) { Name = "Amina", City = "Benghazi" };
        vm.Next();
        vm.Next(); // step 3
        vm.Finish();

        Assert.True(store.WasCleared);
        Assert.Null(store.Stored);
        Assert.Equal(1, vm.Step);
    }
}
