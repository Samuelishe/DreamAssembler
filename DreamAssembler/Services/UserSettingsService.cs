using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using DreamAssembler.App.Models;

namespace DreamAssembler.App.Services;

/// <summary>
/// Загружает и сохраняет пользовательские настройки в JSON-файле.
/// </summary>
public sealed class UserSettingsService : IUserSettingsService
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    private readonly string _settingsFilePath;

    /// <summary>
    /// Инициализирует сервис пользовательских настроек.
    /// </summary>
    public UserSettingsService()
    {
        var applicationDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "DreamAssembler");

        _settingsFilePath = Path.Combine(applicationDirectory, "settings.json");
    }

    /// <summary>
    /// Загружает настройки пользователя.
    /// </summary>
    /// <returns>Результат загрузки настроек.</returns>
    public SettingsLoadResult Load()
    {
        if (!File.Exists(_settingsFilePath))
        {
            return new SettingsLoadResult
            {
                Settings = new AppSettings(),
                UsedDefaults = true,
                Message = "Пользовательские настройки еще не созданы. Использованы значения по умолчанию."
            };
        }

        try
        {
            var content = File.ReadAllText(_settingsFilePath);
            var settings = JsonSerializer.Deserialize<AppSettings>(content, _jsonOptions);

            if (settings is null)
            {
                return new SettingsLoadResult
                {
                    Settings = new AppSettings(),
                    UsedDefaults = true,
                    Message = "Файл настроек пуст. Использованы значения по умолчанию."
                };
            }

            settings.ResultCount = Math.Clamp(settings.ResultCount, 1, 10);

            return new SettingsLoadResult
            {
                Settings = settings,
                UsedDefaults = false,
                Message = "Пользовательские настройки загружены."
            };
        }
        catch (IOException)
        {
            return new SettingsLoadResult
            {
                Settings = new AppSettings(),
                UsedDefaults = true,
                Message = "Не удалось прочитать пользовательские настройки. Использованы значения по умолчанию."
            };
        }
        catch (UnauthorizedAccessException)
        {
            return new SettingsLoadResult
            {
                Settings = new AppSettings(),
                UsedDefaults = true,
                Message = "Нет доступа к пользовательским настройкам. Использованы значения по умолчанию."
            };
        }
        catch (JsonException)
        {
            return new SettingsLoadResult
            {
                Settings = new AppSettings(),
                UsedDefaults = true,
                Message = "Файл пользовательских настроек поврежден. Использованы значения по умолчанию."
            };
        }
    }

    /// <summary>
    /// Сохраняет настройки пользователя.
    /// </summary>
    /// <param name="settings">Настройки для сохранения.</param>
    /// <returns><see langword="true"/>, если сохранение прошло успешно.</returns>
    public bool Save(AppSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        try
        {
            var directoryPath = Path.GetDirectoryName(_settingsFilePath);
            if (!string.IsNullOrWhiteSpace(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var content = JsonSerializer.Serialize(settings, _jsonOptions);
            File.WriteAllText(_settingsFilePath, content);
            return true;
        }
        catch (IOException)
        {
            return false;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
    }
}
