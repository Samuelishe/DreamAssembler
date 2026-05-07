using DreamAssembler.Core.Enums;

namespace DreamAssembler.Core.Models;

/// <summary>
/// Представляет итог одного сгенерированного результата.
/// </summary>
public sealed class TextGenerationResult
{
    /// <summary>
    /// Получает или задает итоговый текст.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задает режим, в котором был получен текст.
    /// </summary>
    public GenerationMode Mode { get; set; }

    /// <summary>
    /// Получает или задает уровень абсурдности, с которым был получен текст.
    /// </summary>
    public AbsurdityLevel AbsurdityLevel { get; set; }

    /// <summary>
    /// Получает или задает время генерации.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    /// <summary>
    /// Получает или задает ключ локального atmospheric-пространства для lexical-режимов.
    /// </summary>
    public string? AtmosphereKey { get; set; }
}

