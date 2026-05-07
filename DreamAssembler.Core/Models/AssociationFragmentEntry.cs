namespace DreamAssembler.Core.Models;

/// <summary>
/// Представляет фрагмент для генерации короткой ассоциативной фразы.
/// </summary>
public sealed class AssociationFragmentEntry
{
    /// <summary>
    /// Получает или задает уникальный идентификатор фрагмента.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задает текст фрагмента.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задает тип фрагмента внутри ассоциативной генерации.
    /// </summary>
    public string Kind { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задает список тегов фрагмента.
    /// </summary>
    public IReadOnlyList<string> Tags { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Получает или задает базовый вес выбора.
    /// </summary>
    public double Weight { get; set; } = 1d;
}
