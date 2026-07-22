# AGENTS.md — OzzContextGen

This file provides guidance for AI coding agents (OpenAI Codex, Claude, etc.) working on this repository.

## What This Project Does

OzzContextGen scans source code files in a local directory or repository and generates a single structured Markdown document (context pack) for use in LLM chat sessions. It supports multiple frontends (WPF desktop, .NET MAUI, CLI) backed by a shared platform-agnostic core library.

**The generated output is consumed by LLMs, not humans.** Human readability is not a goal. When working on `PackerEngine` or any output-formatting code, optimise for token efficiency and LLM parseability — not visual prettiness. Concreteness over whitespace, no decorative separators, no padding.

## Repository Layout

```
Source/
├── OzzContextGen.Core/        # Platform-agnostic: scanning, packing, state
│   ├── CodeCrawler.cs             # Recursive file scanner (suffixes driven by SourceLanguages)
│   ├── PackerEngine.cs            # Markdown generator; resolves fence via SourceLanguages.TryGet
│   ├── StateService.cs            # .ctxgen profile load/save + file-change diff
│   ├── Helpers/
│   │   ├── EnumExtensions.cs          # Extension methods on Enum: GetDisplayValue, GetAttribute<T>, GetValues<T>, GetOrderedValues<T>, GetDisplayOrder; includes EnumValueItem<T>
│   │   └── FileExtensions.cs          # ToFileSize extension on int/long (Bytes / KB / MB / GB)
│   └── Models/
│       ├── ContextStateProfile.cs     # Root profile record; includes SelectedSuffixes
│       ├── Enums.cs                   # ChangeType (New/Modified/Unchanged/Deleted) + FileInclusionMode (FullPack/MetadataOnly/Excluded)
│       ├── FileChangeSummary.cs       # Diff result record; inherits FileContextEntry
│       ├── FileContextEntry.cs        # Per-file metadata: RelativePath, LastWriteTime, FileSize, ContextNote, InclusionMode (lazy-defaults by size)
│       ├── SourceLanguage.cs          # Record: Suffix, MarkdownFence, comment delimiters, XmlDocPrefix
│       └── SourceLanguages.cs         # Static registry of 30 built-in SourceLanguage definitions
├── OzzContextGen.CLI/         # Console frontend (-s, -o, -c, -n flags)
├── OzzContextGen.WPF/         # WPF MVVM frontend
│   ├── Commands/RelayCommand.cs       # Sync/async ICommand with optional CanExecute
│   ├── Controls/
│   │   └── MarkdownViewer.xaml        # Uses MarkdownHtmlRenderer from OzzMarkdown.Core
│   ├── Helpers/
│   │   └── BindingProxy.cs            # Freezable bridge for bindings outside the visual tree (e.g. DataGridColumn.Header)
│   ├── Models/
│   │   ├── AppSettings.cs             # Singleton; persists UiCulture + MainWindowPosition to %AppData%/OzzContextGen/wpfsettings.json
│   │   ├── AppVersion.cs              # Static helper; exposes Version, FullVersion, Product, Copyright, Description from assembly metadata
│   │   └── WindowPosition.cs          # Window geometry (Top/Left/Width/Height); GetWindowPositions/SetWindowPositions helpers; namespace TD.WPF.Models
│   ├── ViewModels/                    # AbstractViewModel, MainViewModel, FileChangeViewModel
│   ├── Views/
│   │   └── MarkdownView.xaml
│   └── Resources/                     # Styles.xaml, BootstrapIcons.xaml
├── OzzMarkdown/               # Git submodule — github.com/ozalpd/OzzMarkdown
│   └── OzzMarkdown.Core/          # Markdown-to-HTML rendering library (Markdig-based)
│       ├── MarkdownHtmlRenderer.cs    # Renders markdown to a temp HTML file, returns virtual URL; handles WebView2 2MB NavigateToString limit
│       ├── MarkdownTheme.cs           # Theme model (CSS string)
│       └── MarkdownThemeProvider.cs   # Provides built-in themes by name
├── OzzContextGen.MAUI/        # .NET MAUI frontend (planned — does not exist yet)
└── OzzContextGen.i18n/        # Shared .resx localization (en + tr)
```

## Hard Rules

1. **Never add UI or platform-specific dependencies to `OzzContextGen.Core` or `OzzContextGen.CLI`.**  
   These projects must compile and run on any platform that supports .NET 10.

2. **Business logic belongs in Core.**  
   If a feature is useful to both GUI and CLI, implement it in `OzzContextGen.Core` and call it from the frontend.

3. **Each frontend is responsible for its own MVVM layer.**  
   ViewModels call Core APIs; they do not contain file I/O or domain logic themselves.

4. **Localized strings go into `OzzContextGen.i18n`.**  
   Add keys to both `LocalizedStrings.resx` (English) and `LocalizedStrings.tr.resx` (Turkish).

5. **Use `record` types for all new domain models.**

6. **All file I/O must be async.**  
   Use `File.ReadAllTextAsync`, `File.WriteAllTextAsync`, etc.

## Implemented Features

- Recursive source file scanning with configurable suffixes and excluded folder list (`CodeCrawler`)
- `SourceLanguage` record + `SourceLanguages` static registry — 14 built-in file type definitions mapping suffix → Markdown fence + comment delimiters + `XmlDocPrefix`
- Single Markdown output with per-file fenced code blocks; fence language resolved dynamically via `SourceLanguages.TryGet` (`PackerEngine`)
- Profile-aware `PackerEngine` overload — uses `ContextStateProfile.SelectedSuffixes` (falls back to all registered suffixes when empty)
- `.ctxgen` JSON profile files with `SelectedSuffixes` persisting the user’s file-type selection between sessions (`StateService`, `ContextStateProfile`)
- File-change diff analysis: New / Modified / Unchanged / Deleted (`StateService.AnalyzeChanges`); per-file state stored as `FileContextEntry` (includes `ContextNote` for optional per-file annotations)
- WPF MVVM GUI: browse source, open/save profile, analyze changes, pack context; round-trips `ProfileName` and `SelectedSuffixes` on save
- CLI: `--source`, `--output`, `--config`, `--nohistory` parameters

## Planned Features (Not Yet Implemented)

- `OzzContextGen.MAUI` project — mirrors WPF frontend for cross-platform support
- Configurable include/exclude rules for file scanning
- Live preview of the generated Markdown before saving
- File-watcher to automatically regenerate when source changes
- **Source trimming** — `TrimComments` and `TrimXmlDocs` bool options on `PackerEngine`; strips inline/block comments (`//`, `/* */`) and XML doc lines (`///`) respectively from packed output to reduce file size and LLM token count. Both flags are opt-in and independent.

## Coding Style

- Short, concise type names (avoid verbose names like `LookupServiceTemplateType`)
- `HashSet<string>` with `StringComparer.OrdinalIgnoreCase` for path sets
- `RelayCommand` (in WPF) supports both sync/async delegates and `CanExecute`
- Target framework: `net10.0` (all projects)

## Testing Instructions

There are currently no automated tests. Manual testing is done by running `OzzContextGen.WPF` or `OzzContextGen.CLI` from Visual Studio.

## Build

Open `Source/OzzContextGen.slnx` in Visual Studio 2026 and build the solution. All projects target `net10.0`.
