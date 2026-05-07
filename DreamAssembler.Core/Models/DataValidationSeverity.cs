namespace DreamAssembler.Core.Models;

/// <summary>
/// Определяет уровень важности проблемы в наборе данных.
/// </summary>
public enum DataValidationSeverity
{
    /// <summary>
    /// Информационное сообщение.
    /// </summary>
    Info,

    /// <summary>
    /// Предупреждение, не блокирующее использование набора данных.
    /// </summary>
    Warning,

    /// <summary>
    /// Критическая ошибка набора данных.
    /// </summary>
    Error
}

