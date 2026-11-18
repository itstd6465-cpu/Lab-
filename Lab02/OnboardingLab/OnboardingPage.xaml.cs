using System.Text.Json;

namespace OnboardingLab;

public partial class OnboardingPage : ContentPage
{
    readonly OnboardingViewModel _vm;

    public OnboardingPage(OnboardingViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    void OnBackClicked(object? sender, EventArgs e) => _vm.Back();

    void OnNextClicked(object? sender, EventArgs e)
    {
        if (_vm.Step == OnboardingViewModel.LastStep)
            _vm.Finish();
        else
            _vm.Next();
    }

    // TEST HOOK for T4 only. Remove before any release.
    void OnStoreOldSnapshotClicked(object? sender, EventArgs e)
    {
        var old = new DraftSnapshot(Version: 0, Step: 2, Name: "Old", City: "Version zero");
        Preferences.Default.Set(PreferencesDraftStore.Key, JsonSerializer.Serialize(old));
        App.Log("T4: planted a version 0 snapshot; ending the process");
#if ANDROID
        // End at once, so Deactivated/Stopped cannot overwrite the planted snapshot.
        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#endif
    }
}
