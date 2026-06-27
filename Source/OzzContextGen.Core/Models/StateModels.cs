namespace OzzContextGen.Core.Models;

public record FileStateInfo
{
    public string RelativePath { get; init; } = string.Empty;
    public DateTime LastWriteTime { get; init; }
    public long FileSize { get; init; }
    public bool IsSelectedForPacking { get; init; } = true; // GUI için varsayılan seçim durumu
}

// GUI ve CLI'da listelemek için değişim analiz sonucu
public record FileChangeSummary
{
    public string RelativePath { get; init; } = string.Empty;
    public string AbsolutePath { get; init; } = string.Empty;
    public ChangeType Change { get; init; }
    public bool IsSelected { get; set; } = true;
}

public enum ChangeType
{
    New,        // Yeni eklenmiş dosya
    Modified,   // En son paketten beri güncellenmiş dosya
    Unchanged,  // Değişmemiş dosya
    Deleted     // Diskten silinmiş ama eski profilde kayıtlı dosya
}