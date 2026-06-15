using OzzContextGen.Core.Models;
using System.Text;
using System.Text.Json;

namespace OzzContextGen.Core;

public class StateService
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    /// <summary>
    /// .ctxgen uzantılı profil dosyasını yükler
    /// </summary>
    public async Task<ContextStateProfile> LoadProfileAsync(string profilePath)
    {
        if (!File.Exists(profilePath))
            return new ContextStateProfile();

        string json = await File.ReadAllTextAsync(profilePath, Encoding.UTF8);
        return JsonSerializer.Deserialize<ContextStateProfile>(json, JsonOptions) ?? new ContextStateProfile();
    }

    /// <summary>
    /// Mevcut profil durumunu diske (.ctxgen) kaydeder
    /// </summary>
    public async Task SaveProfileAsync(string profilePath, ContextStateProfile profile)
    {
        string json = JsonSerializer.Serialize(profile, JsonOptions);
        await File.WriteAllTextAsync(profilePath, json, Encoding.UTF8);
    }

    /// <summary>
    /// Disk üzerindeki güncel dosyalar ile kayıtlı .ctxgen profilini karşılaştırır, farkları bulur.
    /// </summary>
    public List<FileChangeSummary> AnalyzeChanges(string sourcePath, ContextStateProfile oldProfile, IEnumerable<string> currentDiskFiles)
    {
        var summaries = new List<FileChangeSummary>();
        var diskFileSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // 1. Disk üzerindeki güncel dosyaları incele (Yeni veya Değişmiş olanları bul)
        foreach (var filePath in currentDiskFiles)
        {
            diskFileSet.Add(filePath);
            string relativePath = Path.GetRelativePath(sourcePath, filePath);
            var fileInfo = new FileInfo(filePath);

            // Eğer dosya eski profilde zaten varsa, değişiklik kontrolü yap
            if (oldProfile.TrackedFiles.TryGetValue(relativePath, out var oldState))
            {
                bool isModified = fileInfo.LastWriteTime != oldState.LastWriteTime || fileInfo.Length != oldState.FileSize;

                summaries.Add(new FileChangeSummary
                {
                    RelativePath = relativePath,
                    AbsolutePath = filePath,
                    Change = isModified ? ChangeType.Modified : ChangeType.Unchanged,
                    IsSelected = oldState.IsSelectedForPacking // Kullanıcının önceki seçim tercihini koru
                });
            }
            else
            {
                // Eski profilde yoksa yenidir
                summaries.Add(new FileChangeSummary
                {
                    RelativePath = relativePath,
                    AbsolutePath = filePath,
                    Change = ChangeType.New,
                    IsSelected = true // Yeni dosyalar varsayılan olarak seçili gelsin
                });
            }
        }

        // 2. Eski profilde olup da diskten silinmiş olanları bul
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
                    IsSelected = false
                });
            }
        }

        return summaries;
    }
}
