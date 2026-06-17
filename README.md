# OzzContextGen

OzzContextGen is a GUI-first developer utility that scans code files from a solution directory or local repository and generates structured Markdown context files for use with LLM chats.

It helps developers package relevant source code into organized, attachable context documents such as models, repository contracts, services, and other project layers.

## Purpose

Large language models can provide better answers when they are given clear and focused project context. However, manually copying code files into chat sessions is repetitive and error-prone.

OzzContextGen solves this by allowing developers to select a local solution or repository, choose relevant file groups, and generate Markdown files that can be attached to an LLM chat as structured context.

## Application Type

OzzContextGen is planned as a desktop GUI application.

Possible UI technologies include:

- WPF
- .NET MAUI

The initial version will focus on a graphical interface. CLI execution may be added later through startup parameters for automated or scripted usage.

## Planned Features

- Select a solution directory or local repository
- Scan code files by folder, extension, or naming pattern
- Group files into context categories
- Generates a Markdown file
- Include file paths and fenced code blocks
- Support configurable include and exclude rules
- Preview generated context before saving
- Regenerate context files when source code changes

## Example Output Files

Examples of generated Markdown context files:

- `myapp-models.md`
- `myapp-repository-contracts.md`
- `myapp-services.md`
- `myapp-frontend.md`

## CLI Support

OzzContextGen supports command-line startup parameters.

Example usage:

```bash
OzzContextGen --source "C:\Projects\TradeJournal" --output "C:\LLMContext.md"
```

Or

```bash
OzzContextGen --s "C:\Projects\TradeJournal" --o "C:\LLMContext.md"
```

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for a full history of notable changes.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.
