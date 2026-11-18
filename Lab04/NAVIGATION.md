# Lab 4 · Navigation

## Route map

| Page | Route | Kind | Takes | Reached by |
|---|---|---|---|---|
| NotesPage | //notes | root (ShellContent) | — | tab, app start |
| NoteDetailPage | notedetail | registered | id | tap in list |
| NoteEditPage | noteedit | registered | id | Edit on detail |
| SettingsPage | //settings | root (ShellContent) | — | tab |

## Back stack trace

| # | Action | Call made | Expected stack (top first) | [Nav] line you saw |
|---|---|---|---|---|
| 1 | App starts | — | notes | `[Nav] //notes -> //notes` |
| 2 | Tap The answer | `notedetail?id=42` | detail 42 · notes | `[Nav] notedetail?id=42 -> //notes/notedetail` |
| 3 | Tap Edit | `noteedit?id=42` | edit 42 · detail 42 · notes | `[Nav] noteedit?id=42 -> //notes/notedetail/noteedit` |
| 4 | Change the title, tap Save | `..` | detail 42 · notes | `[Nav] .. -> //notes/notedetail` |
| 5 | Press Android Back | (system back) | notes | `[Nav] (system back) -> //notes` |
| 6 | Open note 1, tap Settings tab, tap Notes tab | — | detail 1 · notes | Each tab maintains its independent stack; switching back returns to note 1 detail |

## Decision: IDs, not objects
We chose Design B (passing only the note ID via route parameters, with navigation managed by ViewModels through `INavigationService`).
This design is justified by two core quality attributes:
1. **Maintainability and Testability:** Decoupling ViewModels from the static `Shell.Current` allows comprehensive unit testing using a mock/fake navigation service without requiring a running MAUI runtime or emulator.
2. **Reliability and Correctness:** Passing only the identifier (`?id=42`) ensures the receiving page always queries and loads the authoritative, current record from `INoteStore`, eliminating stale data synchronization bugs across pages. Furthermore, small IDs are easily serialized for deep linking and process restoration.

Reviewed by: Partner

## Kill on the edit page

| Run | Page shown after reopen | Typed but unsaved text | Saved text | Back stack |
|---|---|---|---|---|
| D2 (unsaved) | Notes list | Lost | — | Cleared |
| D3 (saved) | Notes list | — | Preserved in `notes.json` | Cleared |

- Page shown after reopen: `NotesPage` (root list).
- What was lost: The back stack (detail and edit pages) and any uncommitted text typed into the title or body editors.
- Why the back stack could not come back:
The back stack exists in volatile system memory. When the operating system kills the background process (`am kill`), all in-memory stacks are reset.

## What a Week 2 snapshot would need
To reopen the edit page after a kill, we would save: the target route (`noteedit`), the active note ID (`42`), and the current uncommitted text fields (`Title`, `Body`) into a persistent snapshot. On application startup in `CreateWindow`, if a valid snapshot is detected, the app would navigate hierarchically using `GoToAsync` and restore the draft values into the ViewModel. The snapshot would then be cleared upon calling `Save` or `Cancel`.

## Generated or external code
None. Standard .NET MAUI and xUnit architecture following Lab 4 instructions.
