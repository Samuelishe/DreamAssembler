namespace DreamAssembler.Core.Models;

/// <summary>
/// Содержит результат анализа набора данных.
/// </summary>
public sealed class DataValidationReport
{
    /// <summary>
    /// Получает или задает идентификатор набора данных.
    /// </summary>
    public string DataSetId { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задает версию набора данных.
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задает общее число словарных записей.
    /// </summary>
    public int EntryCount { get; set; }

    /// <summary>
    /// Получает или задает общее число шаблонов.
    /// </summary>
    public int TemplateCount { get; set; }

    /// <summary>
    /// Получает или задает общее число словарных записей словесных режимов.
    /// </summary>
    public int AssociationFragmentCount { get; set; }

    /// <summary>
    /// Получает или задает количество записей по категориям.
    /// </summary>
    public IReadOnlyDictionary<string, int> CategoryCounts { get; set; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Получает или задает количество записей по слотам.
    /// </summary>
    public IReadOnlyDictionary<string, int> SlotCounts { get; set; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Получает или задает количество словарных записей словесных режимов по типам.
    /// </summary>
    public IReadOnlyDictionary<string, int> AssociationKindCounts { get; set; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Получает или задает статистику по JSON-пакам словарей.
    /// </summary>
    public IReadOnlyList<DictionarySetStatistics> DictionarySetStats { get; set; } = Array.Empty<DictionarySetStatistics>();

    /// <summary>
    /// Получает или задает статистику по CSV-источникам словесных режимов.
    /// </summary>
    public IReadOnlyList<AssociationSetStatistics> AssociationSetStats { get; set; } = Array.Empty<AssociationSetStatistics>();

    /// <summary>
    /// Получает или задает список замечаний и ошибок.
    /// </summary>
    public IReadOnlyList<DataValidationIssue> Issues { get; set; } = Array.Empty<DataValidationIssue>();

    /// <summary>
    /// Возвращает признак наличия критических ошибок.
    /// </summary>
    public bool HasErrors => Issues.Any(issue => issue.Severity == DataValidationSeverity.Error);
}
