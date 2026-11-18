# Lab 2 · Surviving process death

Pair: Suliman, Partner
Device: Pixel 7 emulator, Android 14 (API 34) · .NET / MAUI: .NET 10
Package: com.companyname.onboardinglab

## Part A · Seeded defect

| What was lost | Kind of state (UI / navigation / domain) |
|---|---|
| Current step (Step 2) | navigation |
| Name ("Amina") and City ("Benghazi") | UI |

Events before the kill (from the log):
- Created
- Activated
- Deactivated
- Stopped

Why Destroying never ran:
The OS ended the process directly while it was in the background (`adb shell am kill`). When Android kills a background process under memory pressure, no lifecycle callback follows `Stopped`; `Destroying` is never invoked.

Did Destroying run when we pressed Back / swiped away?
Yes, pressing Back or swiping the card away from Recents is treated as user-initiated exit, which runs `Destroying`. That is why the initial flawed implementation appeared to work during manual superficial testing.

## Fix
- Snapshot: `DraftSnapshot(Version, Step, Name, City)`, about 65 characters of JSON.
- Store: `IDraftStore` -> `PreferencesDraftStore`, one key (`onboarding.draft`).
- Save points: Next/Back, Deactivated, Stopped. Restore: once in CreateWindow. Clear: Finish.
- Why the ViewModel is a singleton (B9 symptom and cause):
When `OnboardingViewModel` was transient, `ResolveViewModel()` in `App.xaml.cs` resolved a newly instantiated, blank ViewModel rather than the one the page was bound to. As a result, pressing Home saved empty fields. Registering as a singleton ensures `App.xaml.cs` and `OnboardingPage` share the same instance.

## Kill-and-relaunch tests

| Test | Run 1 | Run 2 | Log evidence / notes |
|---|---|---|---|
| T1 kill at step 2 | Pass | Pass | Restored step 2, name 'Amina', city 'Benghazi' via Store: loaded step 2 |
| T2 Home and return | Pass | Pass | State unchanged, logged Resumed, no Store: loaded |
| T3 finish, kill | Pass | Pass | Store: draft cleared; relaunched to empty Step 1 |
| T4 version 0 snapshot | Pass | Pass | Store: ignored snapshot with version 0; safe startup at Step 1 |

## Unit tests
- `RestoreDraft_SetsStepAndFields_FromStore` (Passed)
- `SaveDraft_WritesCurrentState_ToStore` (Passed)
- `RestoreDraft_WithNoSnapshot_LeavesDefaults` (Passed)
- `Finish_ClearsTheDraft` (Passed)

## Generated or external code
None. Standard .NET MAUI and xUnit implementation according to Lab 2 guidelines.
