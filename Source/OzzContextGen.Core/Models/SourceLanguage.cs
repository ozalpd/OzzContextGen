namespace OzzContextGen.Core.Models;

/// <summary>
/// Describes a source file type: its file suffix, Markdown fence language identifier,
/// and comment delimiters used for source trimming (TrimComments / TrimXmlDocs).
/// </summary>
public record SourceLanguage
{
    /// <summary>File extension including the dot, e.g. ".cs", ".xaml", ".sql".</summary>
    public string Suffix { get; init; } = string.Empty;

    /// <summary>Markdown fenced code block language tag, e.g. "csharp", "xml", "sql".</summary>
    public string MarkdownFence { get; init; } = string.Empty;

    /// <summary>Single-line comment prefix, e.g. "//" or "--". Null if the language has no line comments.</summary>
    public string? LineComment { get; init; }

    /// <summary>Opening delimiter for block comments, e.g. "/*" or "&lt;!--". Null if not supported.</summary>
    public string? BlockCommentStart { get; init; }

    /// <summary>Closing delimiter for block comments, e.g. "*/" or "--&gt;". Null if not supported.</summary>
    public string? BlockCommentEnd { get; init; }

    /// <summary>
    /// XML documentation comment prefix. Only set for languages that support
    /// structured doc comments (e.g. "///" for C#). Used by the TrimXmlDocs feature
    /// to strip doc lines independently of regular line comments.
    /// </summary>
    public string? XmlDocPrefix { get; init; }
}
