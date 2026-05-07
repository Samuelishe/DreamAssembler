namespace DreamAssembler.Core.Enums;

/// <summary>
/// Определяет режим генерации текста.
/// </summary>
public enum GenerationMode
{
    /// <summary>
    /// Генерирует одно предложение.
    /// </summary>
    Sentence,

    /// <summary>
    /// Генерирует короткий текст из нескольких предложений.
    /// </summary>
    ShortText,

    /// <summary>
    /// Генерирует идею, концепт или странный замысел.
    /// </summary>
    Idea,

    /// <summary>
    /// Генерирует словосочетание из двух слов.
    /// </summary>
    WordPair,

    /// <summary>
    /// Генерирует короткую фразу из трех-четырех слов.
    /// </summary>
    WordCluster
}
