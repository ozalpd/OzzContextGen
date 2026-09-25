namespace OzzContextGen.Core.Models;

/// <summary>
/// Root model for a <c>.ctxgen</c> profile file. Stores the target source directory,
/// per-file state snapshots, and the suffix selection used during scanning.
/// Serialized as JSON by <see cref="StateService"/>.
/// </summary>
public record ContextStateProfile
{
    /// <summary>Display name for this profile.</summary>
    public string ProfileName { get; init; } = "Default Profile";

    /// <summary>Absolute path to the root source directory this profile targets.</summary>
    public string TargetSourcePath { get; init; } = string.Empty;

    /// <summary>
    /// Timestamp of the most recent successful pack operation.
    /// A default (zero) value indicates the profile has never been packed.
    /// </summary>
    public DateTime LastPackedAt { get; init; }

    /// <summary>Repository URL (e.g. GitHub, GitLab, Azure DevOps).</summary>
    public string RepoUrl { get; set; } = string.Empty;

    /// <summary>License or source model (e.g. "MIT", "Apache-2.0", "Proprietary").</summary>
    public string License { get; set; } = string.Empty;

    /// <summary>Indicates whether the project is open source or proprietary/closed source.</summary>
    public bool? IsOpenSource { get; set; }

    /// <summary>Overview, architecture notes, or system instructions for the LLM.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Custom directives or instructions for the LLM when consuming this context pack.</summary>
    public string SystemPrompt { get; set; } = string.Empty;

    /// <summary>
    /// File suffixes to scan for this profile (e.g. <c>".cs"</c>, <c>".xaml"</c>).
    /// An empty list means all suffixes registered in <see cref="SourceLanguages"/> are used.
    /// </summary>
    public List<string> SelectedSuffixes { get; init; } = new();

    private List<string> _excludedFolders = [.. CtxDefaults.ExcludedFolders];

    /// <summary>
    /// Folder names to exclude during scanning (e.g. <c>"bin"</c>, <c>"obj"</c>).
    /// If empty or omitted, defaults from <see cref="CtxDefaults.ExcludedFolders"/> are used.
    /// </summary>
    public List<string> ExcludedFolders
    {
        get => _excludedFolders;
        set => _excludedFolders = (value == null || value.Count == 0)
            ? [.. CtxDefaults.ExcludedFolders]
            : value;
    }

    /// <summary>
    /// Per-file state snapshots from the last scan, keyed by relative path.
    /// Used by <see cref="StateService.AnalyzeChanges"/> to compute
    /// New / Modified / Unchanged / Deleted diffs on the next run.
    /// </summary>
    public Dictionary<string, FileContextEntry> TrackedFiles { get; init; } = new();
}
