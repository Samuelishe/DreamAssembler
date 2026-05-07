namespace DreamAssembler.Core.Models;

/// <summary>
/// Содержит загруженные словари и шаблоны для генератора.
/// </summary>
public sealed class GeneratorDataBundle
{
    /// <summary>
    /// Получает или задает список записей словаря.
    /// </summary>
    public IReadOnlyList<DictionaryEntry> DictionaryEntries { get; set; } = Array.Empty<DictionaryEntry>();

    /// <summary>
    /// Получает или задает список шаблонов.
    /// </summary>
    public IReadOnlyList<TemplateDefinition> Templates { get; set; } = Array.Empty<TemplateDefinition>();

    /// <summary>
    /// Получает или задает словарные записи для словесных режимов.
    /// </summary>
    public IReadOnlyList<AssociationFragmentEntry> AssociationFragments { get; set; } = Array.Empty<AssociationFragmentEntry>();

    /// <summary>
    /// Получает или задает признак использования fallback-данных.
    /// </summary>
    public bool UsedFallback { get; set; }

    /// <summary>
    /// Получает или задает итоговое сообщение по загрузке.
    /// </summary>
    public string StatusMessage { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задает manifest набора данных, если он был найден.
    /// </summary>
    public DataSetManifest? Manifest { get; set; }

    /// <summary>
    /// Получает или задает корневой путь папки данных.
    /// </summary>
    public string DataRootPath { get; set; } = string.Empty;
}
