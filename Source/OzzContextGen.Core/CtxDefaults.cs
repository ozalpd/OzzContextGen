namespace OzzContextGen.Core;

/// <summary>
/// Centralized default values and fallback configurations for context generation profiles and scanners.
/// </summary>
public static class CtxDefaults
{
    /// <summary>
    /// Default folder names excluded during source scanning across all frontends.
    /// </summary>
    public static readonly string[] ExcludedFolders =
    [
        "bin", "obj", ".git", ".vs", ".vscode", "packages", "node_modules", "GeneratedCodes"
    ];
}
