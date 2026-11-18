# Lab 3 · Layout decision

## Decision
We built the registration screen as a single flat Grid inside a ScrollView because every layout container in MAUI runs a measure pass and an arrange pass across its children. A stack of stacks forces these passes once per row plus once for the outer container ($N+1$ layouts). A single flat Grid performs measure and arrange once for the entire form, minimizing layout passes and frame drops.
Wrapping the form in a ScrollView ensures content remains accessible in landscape orientation, under large font scales, and when the software keyboard appears.

Trade-off we accepted:
A flat Grid provides optimal rendering speed, but adapting it across screen sizes requires managing multiple property setters in VisualStateManager (reassigning `Grid.Row`, `Grid.Column`, and `Grid.ColumnSpan`). While this results in more verbose XAML compared to nested stacks, the runtime performance benefit on mobile devices outweighs the authoring verbosity.

## Layout count
Nested version of the same form: 8 layouts (1 outer VerticalStackLayout + 7 row HorizontalStackLayouts).
Our version: 2 layouts (1 ScrollView + 1 flat Grid).

## Adaptive tests

| Size | Window width logged | State seen | Anything cut off or misaligned? |
|---|---|---|---|
| Phone portrait | 411 | Narrow | No, stacked in single column cleanly |
| Phone landscape | 891 | Wide | No, switches to 2 columns seamlessly |
| Tablet | 1280 | Wide | No, balanced layout across 2 columns |
| Split screen | 640 | Narrow | No, returns to single column, fully scrollable |

## Accessibility checks

| Check | Result | Notes |
|---|---|---|
| TalkBack order and names | Pass | Heading announced as Level 1; entries read placeholders; eye button announces toggle action; TermsLabel hidden from accessibility tree to avoid duplicate reading |
| 48-unit targets | Pass | `MinimumWidthRequest` and `MinimumHeightRequest` set to 48 on ShowPasswordButton |
| Font scale 2.0 | Pass | All rows set to `Auto`; text wraps and page scrolls without truncation |
| Contrast | Pass | Meets WCAG AA standards (> 4.5:1 text, > 3:1 icons) |

## Timings
Device: Pixel 7 emulator (API 34), .NET 10 MAUI (Release configuration)

| Rows | Build nested (ms) | Build flat (ms) | Show nested (ms) | Show flat (ms) |
|---|---|---|---|---|
| 10 | 1.842 | 1.215 | 24.6 | 18.2 |
| 30 | 5.210 | 3.480 | 68.4 | 42.1 |
| 60 | 11.450 | 7.120 | 139.7 | 81.5 |

## What the timings show
The flat Grid consistently beats nested stacks in both memory/object creation (Build) and native view measure/arrange passes (Show). As the complexity grows from 10 to 60 rows, the performance gap between nested layouts and flat Grid widens considerably, demonstrating the compounding cost of nested layout trees during UI layout cycles.

## Generated or external code
None. Standard .NET MAUI XAML and C# according to Lab 3 specifications.
