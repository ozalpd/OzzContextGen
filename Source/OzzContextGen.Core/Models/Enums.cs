using OzzContextGen.i18n;
using System.ComponentModel.DataAnnotations;

namespace OzzContextGen.Core.Models;

public enum ChangeType
{
    /// <summary>File not present in the previous profile.</summary>
    [Display(ResourceType = typeof(LocalizedStrings), Name = "New", Order = 10)]
    New = 10,
    /// <summary>File changed since the last pack (size or timestamp differs).</summary>
    [Display(ResourceType = typeof(LocalizedStrings), Name = "Modified", Order = 20)]
    Modified = 20,
    /// <summary>File identical to the last pack.</summary>
    [Display(ResourceType = typeof(LocalizedStrings), Name = "Unchanged", Order = 30)]
    Unchanged = 30,
    /// <summary>Deleted from disk but still recorded in the saved profile.</summary>
    [Display(ResourceType = typeof(LocalizedStrings), Name = "Deleted", Order = 40)]
    Deleted = 40
}

public enum PackingMode
{
    /// <summary>File omitted from the markdown output entirely.</summary>
    [Display(ResourceType = typeof(LocalizedStrings), Name = "Excluded", Order = 0)]
    Excluded = 0,
    /// <summary>Only file metadata written; content is omitted.</summary>
    [Display(ResourceType = typeof(LocalizedStrings), Name = "MetadataOnly", Order = 10)]
    MetadataOnly = 10,
    /// <summary>Full file content written to the markdown output.</summary>
    [Display(ResourceType = typeof(LocalizedStrings), Name = "FullPack", Order = 20)]
    FullPack = 20
}