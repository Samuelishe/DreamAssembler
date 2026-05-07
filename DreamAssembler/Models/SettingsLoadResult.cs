namespace DreamAssembler.App.Models;

/// <summary>
/// Содержит результат загрузки пользовательских настроек.
/// </summary>
public sealed class SettingsLoadResult
{
    /// <summary>
    /// Получает или задает загруженные настройки.
    /// </summary>
    public AppSettings Settings { get; set; } = new();

    /// <summary>
    /// Получает или задает признак использования значений по умолчанию.
    /// </summary>
    public bool UsedDefaults { get; set; }

    /// <summary>
    /// Получает или задает сообщение о загрузке настроек.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}

