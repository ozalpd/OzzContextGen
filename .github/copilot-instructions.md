# Copilot Instructions for OzzContextGen

## Project Overview

OzzContextGen is a .NET 10 developer utility that scans source code files and generates structured Markdown context documents for use with LLM chats. It has multiple frontends (WPF, MAUI, CLI) backed by a shared platform-agnostic core.

## Architecture

### Projects

| Project | Role |
|---|---|
| `OzzContextGen.Core` | Platform-agnostic business logic. No UI or platform dependencies. |
| `OzzContextGen.CLI` | Console frontend. References Core and i18n only. |
| `OzzContextGen.WPF` | WPF desktop frontend using MVVM. References Core and i18n. |
| `OzzContextGen.MAUI` | *(Planned)* .NET MAUI cross-platform frontend. Will mirror WPF's MVVM structure. |
| `OzzContextGen.i18n` | Shared localization (English + Turkish `.resx` files). |

### Core Types

| Type | Responsibility |
|---|---|
| `CodeCrawler` | Recursively scans a directory for files matching configured suffixes. Excludes `bin`, `obj`, `.git`, `.vs`, `packages`, `node_modules`. Suffixes are driven by `SourceLanguages.All.Keys` by default. |
| `PackerEngine` | Produces a single Markdown document with fenced code blocks and relative-path headers. Resolves the fence language via `SourceLanguages.TryGet`. Profile-aware overload uses `ContextStateProfile.SelectedSuffixes`. Planned: `TrimComments` and `TrimXmlDocs` flags for source trimming. |
| `SourceLanguage` | Immutable record describing one file type: `Suffix`, `MarkdownFence`, `LineComment`, `BlockCommentStart`, `BlockCommentEnd`, `XmlDocPrefix`. |
| `SourceLanguages` | Static registry of 13 built-in `SourceLanguage` definitions, keyed by suffix (case-insensitive). Exposes `All` dictionary and `TryGet(suffix)`. |
| `StateService` | Loads/saves `.ctxgen` JSON profile files and computes `FileChangeSummary` diffs. |
| `ContextStateProfile` | Root profile model (record, own file). Contains `TrackedFiles`, `SelectedSuffixes` (persisted suffix selection), `ProfileName`, `TargetSourcePath`, `LastPackedAt`. |
| `FileStateInfo` | Per-file state snapshot (record): relative path, last write time, file size, selection flag. |
| `FileChangeSummary` | Diff result per file: `ChangeType` (New / Modified / Unchanged / Deleted) + `IsSelected`. |

### Profile Files

Scan state is persisted as `.ctxgen` files (JSON). `StateService.LoadProfileAsync` / `SaveProfileAsync` handle serialization. `ContextStateProfile` lives in its own file (`Models/ContextStateProfile.cs`). The `SelectedSuffixes` property stores the user’s suffix selection per profile; an empty list means all `SourceLanguages`-registered suffixes are used.

## Key Constraints

- **`OzzContextGen.Core` and `OzzContextGen.CLI` must stay platform-agnostic.** Never add WPF, MAUI, Windows-only APIs, or any UI framework reference to these projects.
- **New reusable logic goes into Core**, not into a frontend project.
- **Each frontend owns its own MVVM layer** (ViewModels, Commands, Views/Pages). Keep ViewModels thin — push logic down to Core.
- When adding a feature that applies to both WPF and the future MAUI project, implement it in Core and expose a clean API.

## Coding Conventions

- Prefer `record` types for immutable data/state models.
- Use `async/await` for all file I/O operations.
- Use `HashSet<string>` with `StringComparer.OrdinalIgnoreCase` for file path collections.
- Prefer short, concise type names.
- User-facing strings go into `OzzContextGen.i18n` (`LocalizedStrings.resx` for English, `LocalizedStrings.tr.resx` for Turkish). Access via the generated `LocalizedStrings` static class.

## WPF Frontend Notes

- Base ViewModel: `AbstractViewModel` (implements `INotifyPropertyChanged`).
- `RelayCommand` supports sync and async delegates with optional `CanExecute`.
- Shared styles: `Resources/Styles.xaml` — validation-aware TextBox/ComboBox styles, read-only styles, icon Button sizes.
- Icons: `Resources/BootstrapIcons.xaml` — Bootstrap Icons v1.13.1 (MIT) as `Geometry` resources for use in `Path` elements.

## MAUI Frontend Notes (Planned)

- Must mirror WPF's MVVM structure so ViewModels can eventually be shared or easily ported.
- Do not use WPF-specific types (`PresentationCore`, etc.) anywhere that MAUI will also reference.
