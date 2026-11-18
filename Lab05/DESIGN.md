# Lab 5 · Where state lives

## State table

| Item | Kind (UI / navigation / domain) | Where (View / ViewModel / Model) | Name in our code | Reason |
|---|---|---|---|---|
| Name and email being typed | UI | ViewModel | `Name`, `Email` | Transient user input bound two-way to entry controls before persistence |
| The saved profile record | domain | Model / Store | `Profile`, `IProfileStore` | Authoritative domain data surviving screen and process lifecycle |
| Save enabled when valid | UI | ViewModel | `SaveCommand` + `CanSave` | Dynamic command execution state evaluated based on current field validity |
| Greeting built from name | UI | ViewModel | `DisplayText` | Derived UI representation computed directly from `Name` |
| Focus moves to email field | UI | View | Entry `ReturnType="Next"` | Purely visual keyboard/focus flow belonging to the View layer |
| Saving spinner is visible | UI | ViewModel | `SaveCommand.IsRunning` | Reactive UI state reflecting whether the async save operation is currently executing |

## Code-behind versus ViewModel
The template's `MainPage` kept `count` in the code-behind. Our design keeps all state and business rules in `ProfileViewModel` because decoupling presentation state from UI rendering enables automated unit testing and maintains clean separation of concerns.

One thing that is easier to test now:
Command execution logic and validation preconditions (`CanSave`) can be tested directly with simple unit tests without requiring a running MAUI runtime, emulator, or windowing system.

One thing that is easier to change now:
The visual presentation, controls, or platform layouts can be redesigned in XAML without modifying or breaking any business rules or data logic.

## Hand-written versus generated
- Lines for Name, Email and DisplayText by hand: ~40 lines.
- With CommunityToolkit.Mvvm generators: ~15 lines.

What the generator writes that we would otherwise have to remember:
The generator writes the backing fields, equality checks, `OnPropertyChanging`/`OnPropertyChanged` notifications, dependent property notifications (`NotifyPropertyChangedFor`), and command execution re-evaluations (`NotifyCanExecuteChangedFor`).

## Our validity rule
Save is enabled when: `!string.IsNullOrWhiteSpace(Name) && Email.Contains('@')`.
Known weaknesses of this rule (Week 6 improves it):
Checking only for `@` is an oversimplification; it permits invalid formats (such as `@`, `user@`, or spaces). It also lacks granular inline error reporting to indicate specifically which input failed validation.

## Generated or external code
`CommunityToolkit.Mvvm` source generators produce `ProfileViewModel.g.cs` at build time.
