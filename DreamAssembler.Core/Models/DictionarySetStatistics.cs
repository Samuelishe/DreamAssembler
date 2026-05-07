namespace DreamAssembler.Core.Models;

/// <summary>
/// Содержит статистику по одному JSON-паку словарных данных.
/// </summary>
public sealed class DictionarySetStatistics
{
    /// <summary>
    /// Получает или задает имя набора из manifest.
    /// </summary>
    public string SetName { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задает число корректных записей в наборе.
    /// </summary>
    public int EntryCount { get; set; }

    /// <summary>
    /// Получает или задает количество записей по категориям внутри набора.
    /// </summary>
    public IReadOnlyDictionary<string, int> CategoryCounts { get; set; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Получает или задает количество записей по слотам внутри набора.
    /// </summary>
    public IReadOnlyDictionary<string, int> SlotCounts { get; set; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
}
