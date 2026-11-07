# LifecycleLab

## Overview

**LifecycleLab** is a .NET MAUI application demonstrating Android application lifecycle events for SE442 Mobile Software Engineering Lab 1.

The app logs six Window lifecycle events to Visual Studio Debug Output with precise timestamps:
- `Created`
- `Activated`
- `Deactivated`
- `Stopped`
- `Resumed`
- `Destroying`

## Getting Started

### Prerequisites
- Visual Studio 2022+ (or Visual Studio 2026)
- .NET 10.0 SDK
- Android SDK (API 21+)
- Android Emulator or physical device

### Build & Run
```bash
# Build Android project
dotnet build -f net10.0-android

# Run on emulator/device
dotnet run -f net10.0-android
```

### View Lifecycle Logs
1. Open Visual Studio `Output` window (`View` → `Output`)
2. Select `Debug` from the dropdown pane
3. Filter by `[Lifecycle]` to see timestamped events

## Lab 1 Requirements

✅ **Implemented**
- 6 Window lifecycle event handlers
- Log format: `[Lifecycle] HH:mm:ss.fff EventName`
- Correct `CreateWindow()` override
- Counter button (UI state demonstration)
- AppShell navigation
- No duplicate event subscriptions

✅ **Tested**
- Home press (5-second background)
- Device rotation
- Settings background (5-second)
- Swipe away & restart
- Lock/unlock (extra observation)

## Architecture

- **App.xaml.cs** — Window lifecycle events
- **MainPage.xaml** — UI: Hello World label + counter button
- **MainPage.xaml.cs** — Counter logic
- **MauiProgram.cs** — MAUI build configuration
- **Platforms/Android/** — Android-specific entry points

## Findings

See `Lab01/FINDINGS.md` for detailed experiment results, including:
- Event sequences with timestamps
- Counter state preservation analysis
- Lifecycle constraints and state saving best practices
- Why `Destroying` is unsafe for data persistence

## Key Learnings

1. **Process vs. Activity Lifecycle** — Activities can pause/resume while the process survives. Process termination requires persistent storage.
2. **State Preservation** — In-memory state (e.g., counter) survives pause/resume but not process termination.
3. **Save Point Selection** — Use `OnStopped()` or `OnDeactivated()`, not `Destroying`, for reliable persistence.
4. **Rotation Handling** — MVVM pattern preserves state across orientation changes via SaveInstanceState.

## Technologies

- **.NET MAUI** 10.0
- **C#** 14.0
- **Android** 21+ (API Level 21)
- **XAML** for UI

## License

Educational use only — SE442 Mobile Software Engineering Lab

---

**Created:** January 2025  
**Status:** ✅ Lab Complete  
**Test Environment:** Visual Studio 2026, Android Emulator
