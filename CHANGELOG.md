# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Planned
- **Source trimming** — `TrimComments` and `TrimXmlDocs` options on `PackerEngine` to optionally strip inline/block comments and XML documentation lines, reducing output size and token count.

## [0.0.3]

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

[Unreleased]: https://github.com/ozalpd/OzzContextGen/compare/v0.0.3...HEAD
[0.0.3]: https://github.com/ozalpd/OzzContextGen/compare/v0.0.2...v0.0.3
[0.0.2]: https://github.com/ozalpd/OzzContextGen/compare/v0.0.1...v0.0.2
[0.0.1]: https://github.com/ozalpd/OzzContextGen/releases/tag/v0.0.1
