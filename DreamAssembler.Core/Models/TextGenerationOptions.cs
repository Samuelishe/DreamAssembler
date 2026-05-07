using DreamAssembler.Core.Enums;

namespace DreamAssembler.Core.Models;

/// <summary>
/// Описывает настройки генерации текста.
/// </summary>
public sealed class TextGenerationOptions
{
    /// <summary>
    /// Получает или задает режим генерации.
    /// </summary>
    public GenerationMode Mode { get; set; } = GenerationMode.Sentence;

    /// <summary>
    /// Получает или задает целевой уровень абсурдности.
    /// </summary>
    public AbsurdityLevel AbsurdityLevel { get; set; } = AbsurdityLevel.Normal;

    /// <summary>
    /// Получает или задает количество результатов.
    /// </summary>
    public int ResultCount { get; set; } = 1;
}

