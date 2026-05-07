namespace DreamAssembler.Core.Models;

/// <summary>
/// Описывает подключенный набор данных генератора.
/// </summary>
public sealed class DataSetManifest
{
    /// <summary>
    /// Получает или задает идентификатор набора данных.
    /// </summary>
    public string Id { get; set; } = "default";

    /// <summary>
    /// Получает или задает версию набора данных.
    /// </summary>
    public string Version { get; set; } = "1.0.0";

    /// <summary>
    /// Получает или задает краткое описание набора данных.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задает список активных словарных наборов.
    /// </summary>
    public IReadOnlyList<string> DictionarySets { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Получает или задает список наборов фрагментов для ассоциативного режима.
    /// </summary>
    public IReadOnlyList<string> AssociationSets { get; set; } = Array.Empty<string>();
}
