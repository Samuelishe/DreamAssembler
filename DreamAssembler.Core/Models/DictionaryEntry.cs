namespace DreamAssembler.Core.Models;

/// <summary>
/// Представляет одну запись словаря.
/// </summary>
public sealed class DictionaryEntry
{
    /// <summary>
    /// Получает или задает уникальный идентификатор записи.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задает текст записи.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задает категорию записи.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задает теги записи.
    /// </summary>
    public IReadOnlyList<string> Tags { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Получает или задает уровень абсурдности записи.
    /// </summary>
    public int Absurdity { get; set; }

    /// <summary>
    /// Получает или задает базовый вес записи.
    /// </summary>
    public double Weight { get; set; } = 1d;
}

