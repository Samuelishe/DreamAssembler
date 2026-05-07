using DreamAssembler.Core.Enums;

namespace DreamAssembler.App.Models;

/// <summary>
/// Хранит пользовательские настройки приложения.
/// </summary>
public sealed class AppSettings
{
    /// <summary>
    /// Получает или задает последний выбранный режим генерации.
    /// </summary>
    public GenerationMode Mode { get; set; } = GenerationMode.Sentence;

    /// <summary>
    /// Получает или задает последний выбранный уровень абсурдности.
    /// </summary>
    public AbsurdityLevel AbsurdityLevel { get; set; } = AbsurdityLevel.SlightlyStrange;

    /// <summary>
    /// Получает или задает последнее выбранное количество результатов.
    /// </summary>
    public int ResultCount { get; set; } = 3;
}

