# AGENTS.md — OzzContextGen

This file provides guidance for AI coding agents (OpenAI Codex, Claude, etc.) working on this repository.

## What This Project Does

OzzContextGen scans source code files in a local directory or repository and generates a single structured Markdown document (context pack) for use in LLM chat sessions. It supports multiple frontends (WPF desktop, .NET MAUI, CLI) backed by a shared platform-agnostic core library.

## Repository Layout

```
Source/
├── OzzContextGen.Core/        # Platform-agnostic: scanning, packing, state
│   ├── CodeCrawler.cs         # Recursive file scanner (configurable suffixes + excluded folders)
│   ├── PackerEngine.cs        # Markdown generator (fenced code blocks, relative-path headers)
│   ├── StateService.cs        # .ctxgen profile load/save + file-change diff
│   └── Models/StateModels.cs  # Domain records: ContextStateProfile, FileStateInfo, FileChangeSummary
├── OzzContextGen.CLI/         # Console frontend (-s, -o, -c, -n flags)
├── OzzContextGen.WPF/         # WPF MVVM frontend
│   ├── ViewModels/            # AbstractViewModel, MainViewModel, FileChangeViewModel
│   ├── Commands/RelayCommand.cs
│   └── Resources/             # Styles.xaml, BootstrapIcons.xaml
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
- Single Markdown output with fenced code blocks and relative file-path headers (`PackerEngine`)
- `.ctxgen` JSON profile files for persisting scan state between sessions (`StateService`)
- File-change diff analysis: New / Modified / Unchanged / Deleted (`StateService.AnalyzeChanges`)
- WPF MVVM GUI: browse source, open/save profile, analyze changes, pack context
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
