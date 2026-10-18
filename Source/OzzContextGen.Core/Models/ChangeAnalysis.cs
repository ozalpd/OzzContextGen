namespace OzzContextGen.Core.Models;

/// <summary>
/// Result of <see cref="StateService.AnalyzeChangesAsync"/>: per-file diffs plus the
/// newest file write time observed on disk during the scan.
/// </summary>
public record ChangeAnalysis(List<FileChangeSummary> Changes, DateTime LatestSourceWriteAt);
