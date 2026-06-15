namespace OzzContextGen.Core.Models;

// .ctxgen dosyasının ana gövdesi
public record ContextStateProfile
{
    public string ProfileName { get; init; } = "Default Profile";
    public string TargetSourcePath { get; init; } = string.Empty;
    public DateTime LastPackedAt { get; init; }
    public Dictionary<string, FileStateInfo> TrackedFiles { get; init; } = new();
}

// Her bir dosyanın durum bilgisi
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