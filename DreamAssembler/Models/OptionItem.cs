namespace DreamAssembler.App.Models;

/// <summary>
/// Представляет элемент выбора в интерфейсе.
/// </summary>
/// <typeparam name="T">Тип значения.</typeparam>
public sealed class OptionItem<T>
{
    /// <summary>
    /// Получает или задает отображаемое имя.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задает значение элемента.
    /// </summary>
    public T Value { get; set; } = default!;

    /// <inheritdoc />
    public override string ToString()
    {
        return DisplayName;
    }
}

