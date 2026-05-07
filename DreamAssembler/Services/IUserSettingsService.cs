using DreamAssembler.App.Models;

namespace DreamAssembler.App.Services;

/// <summary>
/// Определяет сервис загрузки и сохранения пользовательских настроек.
/// </summary>
public interface IUserSettingsService
{
    /// <summary>
    /// Загружает настройки пользователя.
    /// </summary>
    /// <returns>Результат загрузки настроек.</returns>
    SettingsLoadResult Load();

    /// <summary>
    /// Сохраняет настройки пользователя.
    /// </summary>
    /// <param name="settings">Настройки для сохранения.</param>
    /// <returns><see langword="true"/>, если сохранение прошло успешно.</returns>
    bool Save(AppSettings settings);
}

