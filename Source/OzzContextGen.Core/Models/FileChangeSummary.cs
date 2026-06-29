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
            PackingMode = this.PackingMode,
            ContextNote = this.ContextNote
        };
    }
}
