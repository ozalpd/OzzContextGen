# OzzContextGen

OzzContextGen is a developer utility that scans source code files from a solution directory or local repository and generates structured Markdown context documents for use with LLM chats.

It helps developers package relevant source code into organized, attachable context documents such as models, repository contracts, services, and other project layers.

## Purpose

Large language models can provide better answers when they are given clear and focused project context. However, manually copying code files into chat sessions is repetitive and error-prone.

OzzContextGen solves this by allowing developers to select a local solution or repository, choose relevant file groups, and generate Markdown files that can be attached to an LLM chat as structured context.

## Application Type

OzzContextGen is a multi-frontend desktop utility targeting **.NET 10**.

| Frontend | Status | Technology |
|---|---|---|
| `OzzContextGen.WPF` | ✅ Available | WPF, MVVM |
| `OzzContextGen.MAUI` | 🔜 Planned | .NET MAUI |
| `OzzContextGen.CLI` | ✅ Available | Console |

The core scanning and packing logic lives in `OzzContextGen.Core`, which is **platform-agnostic** and shared by all frontends. `OzzContextGen.i18n` provides shared localization (English and Turkish) across all projects.

The WPF frontend uses MVVM and ships with a shared `Styles.xaml` resource dictionary. Shared, reusable WPF assets — [Bootstrap Icons v1.13.1](https://icons.getbootstrap.com) (MIT) as `Geometry` resources in `BootstrapIcons.xaml`, plus `AppVersion` — live in `OzzWpf.Core` (from the `OzzMarkdown` submodule) so they can be reused by other WPF-based frontends. The MAUI frontend will mirror the same MVVM structure.

## Project Structure

```
OzzContextGen/
├── Source/
│   ├── OzzContextGen.Core/       # Platform-agnostic: scanning, packing, state
│   │   ├── CodeCrawler.cs
│   │   ├── PackerEngine.cs
│   │   ├── StateService.cs
│   │   ├── TokenEstimator.cs
│   │   ├── Helpers/
│   │   │   ├── EnumExtensions.cs
│   │   │   └── FileExtensions.cs
│   │   └── Models/
│   │       ├── ContextStateProfile.cs
│   │       ├── Enums.cs
│   │       ├── FileChangeSummary.cs
│   │       ├── FileContextEntry.cs
│   │       ├── SourceLanguage.cs
│   │       ├── SourceLanguages.cs
│   │       └── TokenEstimate.cs
│   ├── OzzContextGen.CLI/        # Command-line frontend
│   ├── OzzContextGen.WPF/        # WPF desktop frontend (MVVM)
│   │   ├── MainWindow.xaml
│   │   ├── Commands/
│   │   │   └── RelayCommand.cs
│   │   ├── Converters/
│   │   │   ├── FileSizeToColor.cs
│   │   │   └── PackingModeToColor.cs
│   │   ├── Helpers/
│   │   │   └── BindingProxy.cs
│   │   ├── Models/
│   │   │   ├── AppSettings.cs
│   │   │   └── ReleaseSource.cs
│   │   ├── Resources/
│   │   │   ├── Converters.xaml
│   │   │   └── Styles.xaml
│   │   ├── ViewModels/
│   │   │   ├── FileChangeViewModel.cs
│   │   │   └── MainViewModel.cs
│   │   └── Views/
│   │       └── MarkdownView.xaml
│   ├── OzzMarkdown/              # Git submodule — github.com/ozalpd/OzzMarkdown
│   │   ├── OzzMarkdown.Core/     # Markdown-to-HTML rendering library
│   │   │   ├── MarkdownHtmlRenderer.cs
│   │   │   ├── MarkdownTheme.cs
│   │   │   ├── MarkdownThemeProvider.cs
│   │   │   └── Models/
│   │   │       ├── AbstractAppSettings.cs
│   │   │       └── AppVersion.cs
│   │   └── OzzWpf.Core/           # Shared WPF assets reused across frontends
│   │       ├── Controls/
│   │       │   └── MarkdownViewer.xaml
│   │       ├── Converters/
│   │       │   └── FileNameConverter.cs
│   │       ├── Dialogs/
│   │       │   └── AboutDialog.xaml
│   │       ├── Models/
│   │       │   └── WindowPosition.cs
│   │       ├── Resources/
│   │       │   ├── BootstrapIcons.xaml
│   │       │   └── Styles.xaml
│   │       └── ViewModels/
│   │           └── AbstractViewModel.cs
│   ├── OzzContextGen.MAUI/       # .NET MAUI frontend (planned)
│   └── OzzContextGen.i18n/       # Shared localization (en, tr)
├── CHANGELOG.md
└── README.md
```

## Features

- Select a solution directory or local repository as the source
- Scan code files by folder, extension, or naming pattern
- Group files into context categories via `.ctxgen` profile files
- Generate a single Markdown file with fenced code blocks per file
- Include relative file paths as section headers
- Per-profile file type selection — choose which suffixes each `.ctxgen` profile scans; persisted between sessions
- WPF toolbar picker for up to 10 recently opened or saved `.ctxgen` profiles, persisted in application settings; displays the active profile's filename
- Profile-relative source paths — a profile's source path may be stored relative to the `.ctxgen` file and is resolved to an absolute path at runtime, keeping profiles portable

## Supported File Types

OzzContextGen recognises the following file types out of the box. Each is scanned automatically and wrapped in the correct Markdown fence in the output. Comment delimiters are stored for the planned **source trimming** feature.

| Suffix | Markdown Fence | Line Comment | Block Comment | XML Doc |
|---|---|---|---|---|
| `.cs` | `csharp` | `//` | `/* */` | `///` |
| `.cshtml` | `html` | `//` | `/* */` | — |
| `.css` | `css` | — | `/* */` | — |
| `.js` | `javascript` | `//` | `/* */` | — |
| `.json` | `json` | — | — | — |
| `.html` | `html` | — | `<!-- -->` | — |
| `.sql` | `sql` | `--` | `/* */` | — |
| `.ts` | `typescript` | `//` | `/* */` | `///` |
| `.xaml` | `xml` | — | `<!-- -->` | — |
| `.xml` | `xml` | — | `<!-- -->` | — |
| `.ctxgen` | `json` | — | — | — |
| `.md` | `markdown` | — | — | — |
| `.pine` | `pine` | `//` | — | — |
| `.py` | `python` | `#` | `""" """` | — |
| `.csproj` | `xml` | — | `<!-- -->` | — |
| `.resx` | `xml` | — | `<!-- -->` | — |
| `.sln` | `text` | — | — | — |
| `.slnx` | `xml` | — | `<!-- -->` | — |
| `.OzzGen` | `xml` | — | `<!-- -->` | — |
| `.settings` | `xml` | — | `<!-- -->` | — |
| `.cpp` | `cpp` | `//` | `/* */` | — |
| `.h` | `cpp` | `//` | `/* */` | — |
| `.hlsl` | `hlsl` | `//` | `/* */` | — |
| `.shader` | `hlsl` | `//` | `/* */` | — |
| `.usf` | `hlsl` | `//` | `/* */` | — |
| `.ush` | `hlsl` | `//` | `/* */` | — |
| `.uxml` | `xml` | — | `<!-- -->` | — |
| `.uss` | `css` | — | `/* */` | — |
| `.uproject` | `json` | — | — | — |
| `.uplugin` | `json` | — | — | — |
| `.asmdef` | `json` | — | — | — |
| `.ini` | `ini` | `;` | — | — |
| `.iss` | `inno` | `;` | — | — |
| `.bat` | `bat` | `REM` | — | — |
| `.txt` | `text` | — | — | — |
| `.ps1` | `powershell` | `#` | `<# #>` | — |
| `.psm1` | `powershell` | `#` | `<# #>` | — |
| `.psd1` | `powershell` | `#` | `<# #>` | — |
| `.ps1xml` | `xml` | — | `<!-- -->` | — |
| `.pssc` | `powershell` | `#` | `<# #>` | — |
| `.psc1` | `xml` | — | `<!-- -->` | — |

## Planned Features

- Configurable include and exclude rules
- Preview generated context before saving
- Regenerate context files automatically when source code changes
- **Source trimming** — optionally strip comments (`TrimComments`) and/or XML documentation lines (`TrimXmlDocs`) from packed output to reduce file size and token count

## Example Output Files

Examples of generated Markdown context files:

- `myapp-models.md`
- `myapp-repository-contracts.md`
- `myapp-services.md`
- `myapp-frontend.md`

## CLI Support

OzzContextGen supports command-line startup parameters.

```bash
OzzContextGen --source "C:\Projects\TradeJournal" --output "C:\LLMContext.md"
```

Short aliases are also supported:

```bash
OzzContextGen -s "C:\Projects\TradeJournal" -o "C:\LLMContext.md"
```

Additional parameters:

| Parameter | Alias | Description |
|---|---|---|
| `--source` | `-s` | Root directory of the project to scan |
| `--output` | `-o` | Path for the generated Markdown file |
| `--config` | `-c` | Path to a `.ctxgen` profile file |
| `--nohistory` | `-n` | Skip loading previous scan state |

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for a full history of notable changes.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.

