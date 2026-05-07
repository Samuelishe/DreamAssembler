using DreamAssembler.Core.Enums;

namespace DreamAssembler.App.Models;

/// <summary>
/// Хранит пользовательские настройки приложения.
/// </summary>
public sealed class AppSettings
{
    /// <summary>
    /// Получает или задает активную тему оформления.
    /// </summary>
    public AppTheme Theme { get; set; } = AppTheme.WarmLight;

    /// <summary>
    /// Получает или задает активный шрифт чтения результатов.
    /// </summary>
    public ReadingFontOption ReadingFont { get; set; } = ReadingFontOption.Literata;

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
