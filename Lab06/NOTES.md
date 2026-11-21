# Lab 6 · Notes

## Design A versus design B

| | Design A (creates its services) | Design B (receives them) |
|---|---|---|
| Testing | Cannot be unit tested without initializing MAUI runtime, live network (`HttpClient`), and static `Shell.Current`. | Fully testable with simple in-memory fakes; fast, deterministic tests execute in milliseconds. |
| Changing storage | Hardcoded to `Preferences`; swapping to SQLite or an API requires modifying and retesting ViewModel code. | ViewModel depends only on `IProfileStore`; storage implementation can be swapped in DI container without touching the ViewModel. |
| Reuse | Tightly coupled to MAUI UI controls and Android platform APIs; cannot be reused in other platforms or headless services. | Clean .NET standard library (`Profile.Core`) decoupled from UI frameworks; reusable across desktop, web, or CLI. |

## Lifetimes in our app

| Service | Lifetime | Reason |
|---|---|---|
| `IProfileStore` → `PreferencesProfileStore` | Singleton | Stateless service that interacts with shared persistent storage (`Preferences`); one instance suffices for the whole application lifetime. |
| `INavigationService` → `ShellNavigation` | Singleton | Acts as an adapter over MAUI's global `Shell.Current`; no local state needed, shared across all pages. |
| `ProfileViewModel` | Transient | Holds mutable state for an individual user interaction/screen session; creating a new instance per page ensures clean initial state. |
| `ProfilePage` | Transient | UI pages in MAUI are created when navigated to and garbage collected when popped; each navigation gets fresh layout and bindings. |

What would go wrong if ProfilePage were a singleton?
If `ProfilePage` were registered as a singleton, navigating back and reopening the screen would reuse the old visual element hierarchy with stale layout caches, bound controls, and old state. Furthermore, popping the page and re-pushing it in Shell could throw exceptions or cause visual glitches because the view would already have an existing parent in the visual tree.

## Lifetimes from the design studio

| Service | Lifetime | Reason |
|---|---|---|
| `IApiClient` wrapping `HttpClient` | Singleton | `HttpClient` instances should be reused to prevent socket exhaustion; managing it as a singleton ensures efficient connection pooling. |
| `ProductDetailViewModel` | Transient | Represents a specific product viewing session; each product opened should get its own fresh ViewModel with dedicated ID and state. |
| `CartViewModel`, shared by two tabs | Singleton (or Scoped) | Shopping cart state must be synchronized and shared consistently across multiple tabs (e.g., Catalog and Checkout tabs) without desynchronization. |

## Our tests

| Test | Behaviour it protects |
|---|---|
| `Save_ValidForm_StoresProfileAndGoesBack` | Verifies that valid form inputs are persisted to `IProfileStore` and navigation initiates back navigation (`..`). |
| `Save_BadEmail_CannotExecuteAndStoresNothing` | Verifies that when email is invalid, `SaveCommand.CanExecute` evaluates to `false`, zero storage calls occur, and no navigation happens. |
| `Email_Invalid_ShowsMessage` | Verifies that invalid email formats (missing domain, missing username, missing dot, etc.) trigger the exact error message `"Enter an email such as name@example.com"`. |
| `Email_Valid_ClearsMessage` | Verifies that correcting an invalid email immediately clears `EmailError` back to empty. |
| `Name_Changed_RaisesSaveCanExecuteChanged` | Verifies that changing the `Name` property raises `CanExecuteChanged` on `SaveCommand`, ensuring the Save button re-evaluates its enabled state. |
| `Save_TrimsSpaces` | Verifies that leading and trailing whitespace is trimmed before persisting the profile. |

## Review

Reviewed by pair: itstd6465-cpu · We reviewed pair: Partner
Comments we received and what we changed (or why not):
- Received feedback to ensure `SaveAsync` does not execute if `CanSave()` is false when triggered via code. Confirmed `if (!CanSave()) return;` guard exists.
- Received suggestion to verify space trimming on inputs. Added `Save_TrimsSpaces` unit test.

The comment we wrote that we are most pleased with (copy it):
`ProfileViewModel.cs, line 48: SaveAsync calls Preferences.Default directly, which couples the ViewModel to MAUI and prevents running unit tests outside an emulator. Moving persistence behind IProfileStore via constructor injection allows injecting FakeProfileStore in test suites.`

## Generated or external code
`CommunityToolkit.Mvvm` source generators produce `ProfileViewModel.g.cs` at build time.
