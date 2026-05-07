namespace DreamAssembler.Core.Models;

/// <summary>
/// Представляет одну проблему или сообщение по набору данных.
/// </summary>
public sealed class DataValidationIssue
{
    /// <summary>
    /// Получает или задает уровень важности.
    /// </summary>
    public DataValidationSeverity Severity { get; set; }

    /// <summary>
    /// Получает или задает код правила.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задает понятное описание проблемы.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}

