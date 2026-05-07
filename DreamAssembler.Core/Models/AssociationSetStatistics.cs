namespace DreamAssembler.Core.Models;

/// <summary>
/// Содержит обзор по одному внешнему CSV-источнику словесных режимов.
/// </summary>
public sealed class AssociationSetStatistics
{
    /// <summary>
    /// Получает или задает имя источника из manifest.
    /// </summary>
    public string SetName { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задает логический тип источника.
    /// </summary>
    public string SourceKind { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задает число непустых строк данных без учета header.
    /// </summary>
    public int RowCount { get; set; }
}
