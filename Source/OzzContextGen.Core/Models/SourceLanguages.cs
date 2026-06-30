namespace OzzContextGen.Core.Models;

/// <summary>
/// Registry of known <see cref="SourceLanguage"/> definitions, keyed by file suffix.
/// Used by <see cref="PackerEngine"/> to resolve the correct Markdown fence and comment
/// delimiters for each source file.
/// </summary>
public static class SourceLanguages
{
    public static readonly SourceLanguage CSharp = new()
    {
        Suffix = ".cs",
        MarkdownFence = "csharp",
        LineComment = "//",
        BlockCommentStart = "/*",
        BlockCommentEnd = "*/",
        XmlDocPrefix = "///"
    };

    public static readonly SourceLanguage Xaml = new()
    {
        Suffix = ".xaml",
        MarkdownFence = "xml",
        BlockCommentStart = "<!--",
        BlockCommentEnd = "-->"
    };

    public static readonly SourceLanguage Html = new()
    {
        Suffix = ".html",
        MarkdownFence = "html",
        BlockCommentStart = "<!--",
        BlockCommentEnd = "-->"
    };

    public static readonly SourceLanguage CssHtml = new()
    {
        Suffix = ".cshtml",
        MarkdownFence = "html",
        LineComment = "//",
        BlockCommentStart = "/*",
        BlockCommentEnd = "*/"
    };

    public static readonly SourceLanguage Sql = new()
    {
        Suffix = ".sql",
        MarkdownFence = "sql",
        LineComment = "--",
        BlockCommentStart = "/*",
        BlockCommentEnd = "*/"
    };

    public static readonly SourceLanguage JavaScript = new()
    {
        Suffix = ".js",
        MarkdownFence = "javascript",
        LineComment = "//",
        BlockCommentStart = "/*",
        BlockCommentEnd = "*/"
    };

    public static readonly SourceLanguage TypeScript = new()
    {
        Suffix = ".ts",
        MarkdownFence = "typescript",
        LineComment = "//",
        BlockCommentStart = "/*",
        BlockCommentEnd = "*/",
        XmlDocPrefix = "///"
    };

    public static readonly SourceLanguage Css = new()
    {
        Suffix = ".css",
        MarkdownFence = "css",
        BlockCommentStart = "/*",
        BlockCommentEnd = "*/"
    };

    public static readonly SourceLanguage Json = new()
    {
        Suffix = ".json",
        MarkdownFence = "json"
    };

    public static readonly SourceLanguage Xml = new()
    {
        Suffix = ".xml",
        MarkdownFence = "xml",
        BlockCommentStart = "<!--",
        BlockCommentEnd = "-->"
    };

    public static readonly SourceLanguage Markdown = new()
    {
        Suffix = ".md",
        MarkdownFence = "markdown"
    };

    public static readonly SourceLanguage Python = new()
    {
        Suffix = ".py",
        MarkdownFence = "python",
        LineComment = "#",
        BlockCommentStart = "\"\"\"",
        BlockCommentEnd = "\"\"\""
    };

    public static readonly SourceLanguage PineScript = new()
    {
        Suffix = ".pine",
        MarkdownFence = "pine",
        LineComment = "//"
    };

    public static readonly SourceLanguage Resx = new()
    {
        Suffix = ".resx",
        MarkdownFence = "xml",
        BlockCommentStart = "<!--",
        BlockCommentEnd = "-->"
    };

    public static readonly SourceLanguage CtxGen = new()
    {
        Suffix = ".ctxgen",
        MarkdownFence = "json"
    };

    public static readonly SourceLanguage CsProj = new()
    {
        Suffix = ".csproj",
        MarkdownFence = "xml",
        BlockCommentStart = "<!--",
        BlockCommentEnd = "-->"
    };

    public static readonly SourceLanguage Sln = new()
    {
        Suffix = ".sln",
        MarkdownFence = "text"
    };

    public static readonly SourceLanguage SlnX = new()
    {
        Suffix = ".slnx",
        MarkdownFence = "xml",
        BlockCommentStart = "<!--",
        BlockCommentEnd = "-->"
    };

    // C++
    public static readonly SourceLanguage Cpp = new()
    {
        Suffix = ".cpp",
        MarkdownFence = "cpp",
        LineComment = "//",
        BlockCommentStart = "/*",
        BlockCommentEnd = "*/"
    };

    public static readonly SourceLanguage CppHeader = new()
    {
        Suffix = ".h",
        MarkdownFence = "cpp",
        LineComment = "//",
        BlockCommentStart = "/*",
        BlockCommentEnd = "*/"
    };

    // Shaders (Unity + Unreal)
    public static readonly SourceLanguage Hlsl = new()
    {
        Suffix = ".hlsl",
        MarkdownFence = "hlsl",
        LineComment = "//",
        BlockCommentStart = "/*",
        BlockCommentEnd = "*/"
    };

    public static readonly SourceLanguage UnityShader = new()
    {
        Suffix = ".shader",
        MarkdownFence = "hlsl",
        LineComment = "//",
        BlockCommentStart = "/*",
        BlockCommentEnd = "*/"
    };

    public static readonly SourceLanguage UnrealShaderFile = new()
    {
        Suffix = ".usf",
        MarkdownFence = "hlsl",
        LineComment = "//",
        BlockCommentStart = "/*",
        BlockCommentEnd = "*/"
    };

    public static readonly SourceLanguage UnrealShaderHeader = new()
    {
        Suffix = ".ush",
        MarkdownFence = "hlsl",
        LineComment = "//",
        BlockCommentStart = "/*",
        BlockCommentEnd = "*/"
    };

    // Unity UI Toolkit
    public static readonly SourceLanguage UnityUxml = new()
    {
        Suffix = ".uxml",
        MarkdownFence = "xml",
        BlockCommentStart = "<!--",
        BlockCommentEnd = "-->"
    };

    public static readonly SourceLanguage UnityUss = new()
    {
        Suffix = ".uss",
        MarkdownFence = "css",
        BlockCommentStart = "/*",
        BlockCommentEnd = "*/"
    };

    // Unreal + Unity project descriptors and config
    public static readonly SourceLanguage UnrealProject = new()
    {
        Suffix = ".uproject",
        MarkdownFence = "json"
    };

    public static readonly SourceLanguage UnrealPlugin = new()
    {
        Suffix = ".uplugin",
        MarkdownFence = "json"
    };

    public static readonly SourceLanguage UnityAsmDef = new()
    {
        Suffix = ".asmdef",
        MarkdownFence = "json"
    };

    public static readonly SourceLanguage IniConfig = new()
    {
        Suffix = ".ini",
        MarkdownFence = "ini",
        LineComment = ";"
    };

    private static readonly Dictionary<string, SourceLanguage> _map =
        new(StringComparer.OrdinalIgnoreCase)
        {
            [".sln"]    = Sln,
            [".slnx"]   = SlnX,
            [".csproj"] = CsProj,
            [".xaml"]   = Xaml,
            [".cs"]     = CSharp,
            [".html"]   = Html,
            [".cshtml"] = CssHtml,
            [".sql"]    = Sql,
            [".js"]     = JavaScript,
            [".ts"]     = TypeScript,
            [".css"]    = Css,
            [".json"]   = Json,
            [".xml"]    = Xml,
            [".md"]     = Markdown,
            [".py"]     = Python,
            [".pine"]   = PineScript,
            [".resx"]   = Resx,
            [".ctxgen"] = CtxGen,
            [".cpp"]      = Cpp,
            [".h"]        = CppHeader,
            [".hlsl"]     = Hlsl,
            [".shader"]   = UnityShader,
            [".usf"]      = UnrealShaderFile,
            [".ush"]      = UnrealShaderHeader,
            [".uxml"]     = UnityUxml,
            [".uss"]      = UnityUss,
            [".uproject"] = UnrealProject,
            [".uplugin"]  = UnrealPlugin,
            [".asmdef"]   = UnityAsmDef,
            [".ini"]      = IniConfig,
        };

    /// <summary>All registered language definitions, keyed by suffix (case-insensitive).</summary>
    public static IReadOnlyDictionary<string, SourceLanguage> All => _map;

    /// <summary>
    /// Returns the <see cref="SourceLanguage"/> for the given file suffix, or <c>null</c>
    /// if the suffix is not registered.
    /// </summary>
    public static SourceLanguage? TryGet(string suffix)
        => _map.TryGetValue(suffix, out var lang) ? lang : null;
}
