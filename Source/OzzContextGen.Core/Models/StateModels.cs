using OzzContextGen.i18n;
using System.ComponentModel.DataAnnotations;

namespace OzzContextGen.Core.Models;


// Analysis result for a single file, indicating its relative path, absolute path, and the type of change detected.
public record FileChangeSummary : FileContextEntry
{
    public string AbsolutePath { get; init; } = string.Empty;
    public ChangeType Change { get; init; }

    public FileContextEntry ToFileContextEntry()
    {
        return new FileContextEntry
        {
            RelativePath = this.RelativePath,
            LastWriteTime = this.LastWriteTime,
            FileSize = this.FileSize,
            IsSelected = this.IsSelected,
            ContextNote = this.ContextNote
        };
    }
}

public enum ChangeType
{
    /// <summary>
    /// File newly added since the last package.
    /// </summary>
    [Display(ResourceType = typeof(LocalizedStrings), Name = "New", Order = 10)]
    New = 10,
    /// <summary>
    /// File modified since the last package.
    /// </summary>
    [Display(ResourceType = typeof(LocalizedStrings), Name = "Modified", Order = 20)]
    Modified = 20,
    /// <summary>
    /// File unchanged since the last package.
    /// </summary>
    [Display(ResourceType = typeof(LocalizedStrings), Name = "Unchanged", Order = 30)]
    Unchanged = 30,
    /// <summary>
    /// File deleted from disk but still recorded in the old profile.
    /// This state indicates that the file was present in the previous scan but is no longer found in the current source directory.
    /// </summary>
    [Display(ResourceType = typeof(LocalizedStrings), Name = "Deleted", Order = 40)]
    Deleted = 40
}

public enum FileInclusionMode
{
    /// <summary>
    /// File content entirely included in the markdown file
    /// </summary>
    [Display(ResourceType = typeof(LocalizedStrings), Name = "FullPack", Order = 10)]
    FullPack = 10,
    /// <summary>
    /// Only metadata of the file will be included in the markdown
    /// </summary>
    [Display(ResourceType = typeof(LocalizedStrings), Name = "MetadataOnly", Order = 20)]
    MetadataOnly = 20,
    /// <summary>
    /// File that will not be included in the package and in the markdown file
    /// </summary>
    [Display(ResourceType = typeof(LocalizedStrings), Name = "Excluded", Order = 30)]
    Excluded = 0
}