namespace DreamAssembler.Core.Enums;

/// <summary>
/// Определяет целевой уровень нормальности или абсурдности генерации.
/// </summary>
public enum AbsurdityLevel
{
    /// <summary>
    /// Ближе к обычным и бытовым сочетаниям.
    /// </summary>
    Normal = 0,

    /// <summary>
    /// С небольшим отклонением в странность.
    /// </summary>
    SlightlyStrange = 1,

    /// <summary>
    /// Заметно странные и менее предсказуемые сочетания.
    /// </summary>
    Absurd = 2,

    /// <summary>
    /// Максимально хаотичные и сюрреалистичные сочетания.
    /// </summary>
    Insane = 3
}

