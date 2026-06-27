namespace OzzContextGen.Core.Models;

public record FileContextEntry
{
    public string RelativePath { get; init; } = string.Empty;
    public DateTime LastWriteTime { get; init; }
    public long FileSize { get; init; }
    public bool IsSelected { get; set; } = true;

    /// <summary>
    /// Optional note or comment about the file's context, which can be used for additional information or instructions related to the source file.
    /// </summary>
    public string ContextNote { get; set; } = string.Empty;
}