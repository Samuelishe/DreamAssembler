namespace DreamAssembler.Core.Services;

/// <summary>
/// Выполняет выбор одного элемента из списка по весам.
/// </summary>
public sealed class WeightedRandomSelector
{
    private readonly Random _random;

    /// <summary>
    /// Создает селектор с указанным генератором случайных чисел.
    /// </summary>
    /// <param name="random">Генератор случайных чисел.</param>
    public WeightedRandomSelector(Random random)
    {
        _random = random;
    }

    /// <summary>
    /// Выбирает один элемент из списка кандидатов по рассчитанному весу.
    /// </summary>
    /// <typeparam name="T">Тип элемента.</typeparam>
    /// <param name="items">Список кандидатов.</param>
    /// <param name="weightSelector">Функция получения веса.</param>
    /// <returns>Выбранный элемент.</returns>
    /// <exception cref="InvalidOperationException">Выбрасывается, если нет кандидатов с положительным весом.</exception>
    public T Select<T>(IReadOnlyList<T> items, Func<T, double> weightSelector)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(weightSelector);

        var weightedItems = items
            .Select(item => new { Item = item, Weight = Math.Max(0d, weightSelector(item)) })
            .Where(item => item.Weight > 0d)
            .ToList();

        if (weightedItems.Count == 0)
        {
            throw new InvalidOperationException("Нет элементов с положительным весом для выбора.");
        }

        var totalWeight = weightedItems.Sum(item => item.Weight);
        var threshold = _random.NextDouble() * totalWeight;
        var cumulative = 0d;

        foreach (var item in weightedItems)
        {
            cumulative += item.Weight;
            if (threshold <= cumulative)
            {
                return item.Item;
            }
        }

        return weightedItems[^1].Item;
    }
}

