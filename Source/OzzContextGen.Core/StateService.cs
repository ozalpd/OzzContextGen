using OzzContextGen.Core.Models;
using System.Text;
using System.Text.Json;

namespace OzzContextGen.Core;

/// <summary>
/// Provides methods for loading, saving, and analyzing the state of a context generation profile.
/// </summary>
public class StateService
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    /// <summary>Loads a <c>.ctxgen</c> profile from disk. Returns an empty profile if the file does not exist.</summary>
    public async Task<ContextStateProfile> LoadProfileAsync(string profilePath)
    {
        if (!File.Exists(profilePath))
            return new ContextStateProfile();

        string json = await File.ReadAllTextAsync(profilePath, Encoding.UTF8);
        return JsonSerializer.Deserialize<ContextStateProfile>(json, JsonOptions) ?? new ContextStateProfile();
    }

    /// <summary>Serializes and writes the profile state to a <c>.ctxgen</c> file.</summary>
    public async Task SaveProfileAsync(string profilePath, ContextStateProfile profile)
    {
        string json = JsonSerializer.Serialize(profile, JsonOptions);
        await File.WriteAllTextAsync(profilePath, json, Encoding.UTF8);
    }

    /// <summary>
    /// Diffs <paramref name="currentDiskFiles"/> against <paramref name="oldProfile"/> and returns
    /// a <see cref="FileChangeSummary"/> for every file (New, Modified, Unchanged, or Deleted).
    /// </summary>
    public List<FileChangeSummary> AnalyzeChanges(string sourcePath, ContextStateProfile oldProfile, IEnumerable<string> currentDiskFiles)
    {
        var summaries = new List<FileChangeSummary>();
        var diskFileSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // 1. Walk current disk files — classify as New or Modified/Unchanged
        foreach (var filePath in currentDiskFiles)
        {
            diskFileSet.Add(filePath);
            string relativePath = Path.GetRelativePath(sourcePath, filePath);
            var fileInfo = new FileInfo(filePath);

            // File exists in old profile — check for changes
            if (oldProfile.TrackedFiles.TryGetValue(relativePath, out var oldState))
            {
                bool isModified = fileInfo.LastWriteTime != oldState.LastWriteTime || fileInfo.Length != oldState.FileSize;

                summaries.Add(new FileChangeSummary
                {
                    RelativePath = relativePath,
                    AbsolutePath = filePath,
                    Change = isModified ? ChangeType.Modified : ChangeType.Unchanged,
                    ContextNote = oldState.ContextNote,
                    FileSize = fileInfo.Length,
                    LastWriteTime = fileInfo.LastWriteTime,
                    PackingMode = oldState.PackingMode // preserve previous user selection
                });
            }
            else
            {
                // Not in old profile — mark as New
                summaries.Add(new FileChangeSummary
                {
                    RelativePath = relativePath,
                    AbsolutePath = filePath,
                    Change = ChangeType.New
                });
            }
        }

        // 2. Find profile entries no longer present on disk
        foreach (var oldRelativePath in oldProfile.TrackedFiles.Keys)
        {
            string fullPath = Path.Combine(sourcePath, oldRelativePath);
            if (!diskFileSet.Contains(fullPath))
            {
                summaries.Add(new FileChangeSummary
                {
                    RelativePath = oldRelativePath,
                    AbsolutePath = fullPath,
                    Change = ChangeType.Deleted,
                    PackingMode = PackingMode.Excluded
                });
            }
        }

        return summaries;
    }
}
