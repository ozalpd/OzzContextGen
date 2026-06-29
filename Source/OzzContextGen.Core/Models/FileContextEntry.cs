namespace OzzContextGen.Core.Models;

/// <summary>
/// A single file entry passed to <see cref="PackerEngine"/>. Carries metadata and packing
/// instructions for one source file.
/// </summary>
public record FileContextEntry
{
    /// <summary>Path relative to the scanned root directory.</summary>
    public string RelativePath { get; init; } = string.Empty;

    /// <summary>Last modified timestamp; used for change detection.</summary>
    public DateTime LastWriteTime { get; init; }

    /// <summary>File size in bytes; drives the default <see cref="PackingMode"/>.</summary>
    public long FileSize { get; init; }

    /// <summary>
    /// Controls how this file is packed into the markdown output.
    /// Lazy-initialized via <see cref="GetDefaultPackingMode"/> if not explicitly set.
    /// </summary>
    public PackingMode PackingMode
    {
        get
        {
            if (_fileInclusionMode == null)
            {
                _fileInclusionMode = GetDefaultPackingMode();
            }
            return _fileInclusionMode.Value;
        }

        set => _fileInclusionMode = value;
    }
    PackingMode? _fileInclusionMode;

    /// <summary>
    /// Returns <see cref="PackingMode.FullPack"/> for files under 32,678 bytes,
    /// <see cref="PackingMode.MetadataOnly"/> otherwise.
    /// </summary>
    public PackingMode GetDefaultPackingMode()
    {
        return FileSize < 32678 ? PackingMode.FullPack : PackingMode.MetadataOnly;
    }

    /// <summary>Optional free-text note injected into the markdown output alongside this file.</summary>
    public string ContextNote { get; set; } = string.Empty;
}