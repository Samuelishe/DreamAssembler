namespace DreamAssembler.Core.Models;

/// <summary>
/// Содержит данные, загруженные репозиторием, и информацию о fallback-поведении.
/// </summary>
/// <typeparam name="T">Тип полезных данных.</typeparam>
public sealed class RepositoryLoadResult<T>
{
    /// <summary>
    /// Получает или задает загруженные данные.
    /// </summary>
    public T Data { get; set; } = default!;

    /// <summary>
    /// Получает или задает признак использования fallback-данных.
    /// </summary>
    public bool UsedFallback { get; set; }

    /// <summary>
    /// Получает или задает сообщение для пользователя или журнала.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}

