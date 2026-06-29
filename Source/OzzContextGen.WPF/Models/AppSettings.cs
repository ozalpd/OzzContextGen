using System.IO;
using System.Text.Json;
using TD.WPF.Models;
using static System.Environment;

namespace OzzContextGen.WPF.Models;

public class AppSettings
{
    private static AppSettings? _instance;
    private static readonly object _syncRoot = new();

    private static string ozzContextGen = "OzzContextGen";
    private static string settingsFileName = "wpfsettings.json";


    /// <summary>
    /// Gets or sets the position and size of the main application window.
    /// </summary>
    public WindowPosition MainWindowPosition { get; set; } = new WindowPosition();

    /// <summary>
    /// Gets or sets the BCP-47 culture name used for the application UI (e.g. <c>"en-US"</c>, <c>"tr-TR"</c>).
    /// </summary>
    /// <remarks>When empty, the operating system's current culture is used.</remarks>
    public string UiCulture { get; set; } = string.Empty;


    public static AppSettings GetAppSettings()
    {
        if (_instance is not null)
        {
            return _instance;
        }

        lock (_syncRoot)
        {
            if (_instance is not null)
            {
                return _instance;
            }

            var settingsFilePath = GetSettingsFilePath();
            if (File.Exists(settingsFilePath))
            {
                var settingsJson = File.ReadAllText(settingsFilePath);
                if (!string.IsNullOrWhiteSpace(settingsJson))
                {
                    try
                    {
                        _instance = JsonSerializer.Deserialize<AppSettings>(settingsJson);
                    }
                    catch (JsonException)
                    {
                    }
                    catch (NotSupportedException)
                    {
                    }
                }
            }

            _instance ??= new AppSettings();
            return _instance;
        }
    }

    private static string GetSettingsFilePath()
    {
        var settingsFolder = Path.Combine(GetFolderPath(SpecialFolder.ApplicationData), ozzContextGen);
        Directory.CreateDirectory(settingsFolder);

        return Path.Combine(settingsFolder, settingsFileName);
    }

    public void Save()
    {
        var settingsFilePath = GetSettingsFilePath();
        var settingsJson = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(settingsFilePath, settingsJson);
    }
}
