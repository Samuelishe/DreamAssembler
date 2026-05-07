using DreamAssembler.Core.Enums;

namespace DreamAssembler.Core.Models;

/// <summary>
/// Представляет шаблон генерации текста.
/// </summary>
public sealed class TemplateDefinition
{
    /// <summary>
    /// Получает или задает идентификатор шаблона.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задает текст шаблона с плейсхолдерами.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задает режим генерации, для которого подходит шаблон.
    /// </summary>
    public GenerationMode Mode { get; set; }

    /// <summary>
    /// Получает или задает список обязательных категорий.
    /// </summary>
    public IReadOnlyList<string> RequiredCategories { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Получает или задает теги шаблона.
    /// </summary>
    public IReadOnlyList<string> Tags { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Получает или задает минимальный поддерживаемый уровень абсурдности.
    /// </summary>
    public int MinAbsurdity { get; set; }

    /// <summary>
    /// Получает или задает максимальный поддерживаемый уровень абсурдности.
    /// </summary>
    public int MaxAbsurdity { get; set; } = 3;

    /// <summary>
    /// Получает или задает базовый вес шаблона.
    /// </summary>
    public double Weight { get; set; } = 1d;
}

