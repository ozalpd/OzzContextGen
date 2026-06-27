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

    /// <summary>
    /// Per-file state snapshots from the last scan, keyed by relative path.
    /// Used by <see cref="StateService.AnalyzeChanges"/> to compute
    /// New / Modified / Unchanged / Deleted diffs on the next run.
    /// </summary>
    public Dictionary<string, FileContextEntry> TrackedFiles { get; init; } = new();

    /// <summary>
    /// File suffixes to scan for this profile (e.g. <c>".cs"</c>, <c>".xaml"</c>).
    /// An empty list means all suffixes registered in <see cref="SourceLanguages"/> are used.
    /// </summary>
    public List<string> SelectedSuffixes { get; init; } = new();
}
