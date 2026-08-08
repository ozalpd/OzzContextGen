# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).


## [Unreleased]

### Added
- Added an About button to the main toolbar with a localized tooltip.
- Added Batch and Pascal language definitions to PrismJS for improved code block highlighting. Updated minified assets accordingly.
- Added support for opening a profile file passed by the OS (e.g. via double-click on an associated `.md` file): `App.OnStartup` now reads the launch arguments and passes the file path to a new `MainWindow(string? filePathToOpen)` constructor overload, which loads it through a new `MainViewModel.OpenProfile` method once the view model is initialized.

### Changed
- `MainViewModel`: Packing allowed if files are selected; prompts for output path if missing.
- Refined link colors in all built-in Markdown themes for better consistency and accessibility.
- Added `.iss` (Inno Setup) and `.bat` (Batch) to SourceLanguages, expanding built-in types to 32.
- Moved AppVersion to OzzWpf.Core/Models, improved version reporting using Assembly.GetEntryAssembly() with fallback.
- Updated namespace imports to use `OzzMarkdown.Core.Models` for shared types. Added `MainWindowPosition` property to `AppSettings` for saving window geometry, utilizing the `WindowPosition` type. Restored `OzzWpf.Core.Models` import for compatibility. 
- Replaced old icons with new `CtxGen-Icon.ico` and `CtxGen-Icon-832.png`. Updated project and window to use the new `.ico` file as the application icon.

### Fixed
- Fixed `AppVersion` (in `OzzWpf.Core`) reporting its own assembly's version instead of the hosting application's; now uses `Assembly.GetEntryAssembly()` with a fallback to `Assembly.GetExecutingAssembly()`.

### Planned
- **Source trimming** — `TrimComments` and `TrimXmlDocs` options on `PackerEngine` to optionally strip inline/block comments and XML documentation lines, reducing output size and token count.

## [0.1.6] - 2026-07-22

### Added
- Added `OzzMarkdown` as a submodule and projects `OzzWpf.Core` and `OzzMarkdown.Core` reference to `OzzContextGen.WPF`.
- Added `MarkdownView` to display Markdown content in a WPF window using `OzzMarkdown.Core` for rendering.
- Added `SelectedTheme` for markdown preview to `AppSettings`.

### Changed
- Updated `MainViewModel` to show preview after packing.

## [0.1.5] - 2026-06-30

### Added
- Added new application icon and WPF window icon.

### Changed
- Auto-assign note and `MetadataOnly` mode for `.resx` files in `GetDefaultPackingMode`.
- Reorder and deduplicate `SourceLanguages` suffix mappings.
- Updated `MainWindow` DataGrid to show a color-coded FileSize column and apply packing mode coloring with newly added WPF value converters and cell styles for file size and packing mode coloring.

## [0.1.4] - 2026-06-29

### Added
- Added new property `PackingMode` to model type `FileContextEntry` and view model `FileChangeViewModel`.
- Added Packing Mode combo box to WPF UI for user selection. Update `PackerEngine` to respect `PackingMode` for file output. Persist `PackingMode` in `StateService` and `FileChangeSummary`.
- Added `EnumExtensions` and `EnumValueItem<T>` for localized, ordered enum binding.
- Added `AppSettings` for saving window position and UI culture to AppData. `MainWindow` now loads/saves position and culture.

### Changed
- Refactor `FileContextEntry` to use `PackingMode`; `IsSelected` is now a computed property in the view model.
- Show app version in window title via new `AppVersion` helper.
- Added a save button to `MainWindow's` toolbar and `SaveProfileCommand` to `MainViewModel`.

## [0.1.3] - 2026-06-28

### Added
- Added "Remove deleted files" toolbar button with icon and localization
- Added `BindingProxy` helper for DataGrid header binding
- Implemented select all/none/indeterminate checkbox in DataGrid header

### Changed
- Synced selection state and commands in MainViewModel

## [0.1.2] - 2026-06-27

### Changed
- Replaced `FileStateInfo` with `FileContextEntry` (adds `ContextNote`)
- Moved/split model records into `Models/` directory
- Added `Helpers/FileExtensions.cs` for file size formatting
- WPF: DataGrid status coloring, file detail panel, context note editing
- Added localized strings for file size and context note (en/tr)
- `PackerEngine` outputs context notes in Markdown

## [0.1.1] - 2026-06-27

### Added
- Added `FileInclusionMode` enum for future packaging options with localized new file state strings.

### Changed
- Replaced `FileStateInfo` with `FileContextEntry` across the codebase, adding support for context notes and selection state.
- Updated `ContextStateProfile`, `StateService`, `MainViewModel`, and `FileChangeViewModel` to use the new model.
- Refactored `FileChangeSummary` to inherit from `FileContextEntry`.
- Enhanced `SourceLanguages` to support `.resx` and `.ctxgen` files.
- Updated `PackerEngine` to operate on `FileContextEntry` objects.
- Relocating the `FileChangeSummary` record and its logic from `StateModels.cs` to a new `FileChangeSummary.cs` file.

## [0.1.0] - 2026-06-27

### Added
- `SourceLanguage` record — describes a file type with its suffix, Markdown fence identifier, line-comment prefix, block-comment delimiters, and XML doc prefix (`XmlDocPrefix`). Lives in `OzzContextGen.Core`.
- `SourceLanguages` static registry — 13 built-in `SourceLanguage` definitions (`.cs`, `.xaml`, `.html`, `.cshtml`, `.sql`, `.js`, `.ts`, `.css`, `.json`, `.xml`, `.md`, `.py`, `.pine`) with `TryGet(suffix)` and `All` dictionary.
- `SelectedSuffixes` property on `ContextStateProfile` — persists the file types a profile should scan. An empty list falls back to all suffixes registered in `SourceLanguages`.
- Profile-aware `PackerEngine.PackSourceCodeAsync` overload — accepts a `ContextStateProfile` and uses its `SelectedSuffixes` to drive `CodeCrawler`.
- `ContextStateProfile` moved to its own file (`Models/ContextStateProfile.cs`) with full XML documentation per property.

### Changed
- `PackerEngine` now resolves the Markdown fence language dynamically via `SourceLanguages.TryGet`, replacing the hardcoded `` ```csharp `` fence. Falls back to `` `text` `` for unregistered suffixes.
- `PackerEngine` instance overload now crawls **all suffixes** registered in `SourceLanguages` instead of only `.cs`.
- `CodeCrawler` XML documentation updated to be suffix-agnostic.
- `MainViewModel` tracks the loaded `ContextStateProfile` and round-trips `ProfileName` and `SelectedSuffixes` on save.

## [0.0.3] - 2026-06-20

### Added
- `Styles.xaml` — shared WPF resource dictionary with reusable control styles: validation-aware `TextBox` and `ComboBox` styles (inline error display, red border on error, tooltip), a read-only `TextBox` style, right-aligned and read-only `TextBlock` styles, and two icon `Button` sizes (22×18, 28×24).
- `BootstrapIcons.xaml` — Bootstrap Icons v1.13.1 (MIT) as WPF `Geometry` resources, available application-wide for use in `Path` elements.

## [0.0.2] - 2026-06-17

### Added
- Add WPF GUI for context packing with MVVM. Introduced a new OzzContextGen.WPF project featuring a WPF MVVM-based UI for selecting source, profile, and output files, visualizing file changes, and packing selected files.

### Changed
- Refactored `PackerEngine` to separate file scanning and packing.
- Refactored `MainViewModel` to use strongly-typed `RelayCommand` properties and dynamic `CanExecute` logic.
- Added `SelectedFile` binding and event handling for `DataGrid` selection.
- Improved `PackContext` button styling and feedback.

## [0.0.1] - 2026-06-15

### Added
- `CodeCrawler` — scans solution directories and local repositories for source files
- `PackerEngine` — generates structured Markdown context files from scanned code
- `StateService` and state models for managing scan and generation state
- CLI entry point with `--source` and `--output` startup parameters
- Localization support (English and Turkish) via `OzzContextGen.i18n`

[Unreleased]: https://github.com/ozalpd/OzzContextGen/compare/v0.1.0...HEAD
[0.1.0]: https://github.com/ozalpd/OzzContextGen/compare/v0.0.3...v0.1.0
[0.0.3]: https://github.com/ozalpd/OzzContextGen/compare/v0.0.2...v0.0.3
[0.0.2]: https://github.com/ozalpd/OzzContextGen/compare/v0.0.1...v0.0.2
[0.0.1]: https://github.com/ozalpd/OzzContextGen/releases/tag/v0.0.1
