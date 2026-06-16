# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.0.2] - 2026-06-17

### Added
- Add WPF GUI for context packing with MVVM. Introduced a new OzzContextGen.WPF project featuring a WPF MVVM-based UI for selecting source, profile, and output files, visualizing file changes, and packing selected files.

### Changed
- Refactored `PackerEngine` to separate file scanning and packing.

## [0.0.1] - 2026-06-15

### Added
- `CodeCrawler` — scans solution directories and local repositories for source files
- `PackerEngine` — generates structured Markdown context files from scanned code
- `StateService` and state models for managing scan and generation state
- CLI entry point with `--source` and `--output` startup parameters
- Localization support (English and Turkish) via `OzzContextGen.i18n`

[Unreleased]: https://github.com/ozalpd/OzzContextGen/compare/v0.0.1...HEAD
[0.0.1]: https://github.com/ozalpd/OzzContextGen/releases/tag/v0.0.1
