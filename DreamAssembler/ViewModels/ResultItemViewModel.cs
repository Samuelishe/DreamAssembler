using CommunityToolkit.Mvvm.ComponentModel;
using DreamAssembler.Core.Models;

namespace DreamAssembler.App.ViewModels;

/// <summary>
/// Представляет один элемент результата в списке UI.
/// </summary>
public partial class ResultItemViewModel : ObservableObject
{
    /// <summary>
    /// Инициализирует модель представления результата.
    /// </summary>
    /// <param name="result">Результат генерации.</param>
    /// <param name="index">Порядковый номер результата.</param>
    public ResultItemViewModel(TextGenerationResult result, int index)
    {
        Text = result.Text;
        Header = GetHeader(result.Mode);
        GeneratedAt = result.CreatedAt;
        AtmosphereLabel = GetAtmosphereLabel(result.AtmosphereKey);
        AtmosphereDescription = GetAtmosphereDescription(result.AtmosphereKey);
    }

    /// <summary>
    /// Получает текст результата.
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// Получает заголовок результата.
    /// </summary>
    public string Header { get; }

    /// <summary>
    /// Получает время генерации.
    /// </summary>
    public DateTimeOffset GeneratedAt { get; }

    /// <summary>
    /// Получает тихую reader-facing подпись atmospheric-кластера.
    /// </summary>
    public string AtmosphereLabel { get; }

    /// <summary>
    /// Получает короткое пояснение локального atmospheric-пространства.
    /// </summary>
    public string AtmosphereDescription { get; }

    private static string GetHeader(Core.Enums.GenerationMode mode)
    {
        return mode switch
        {
            Core.Enums.GenerationMode.Sentence => "Фраза",
            Core.Enums.GenerationMode.ShortText => "Короткий текст",
            Core.Enums.GenerationMode.Idea => "Идея",
            Core.Enums.GenerationMode.WordPair => "Фрагмент",
            Core.Enums.GenerationMode.WordCluster => "Фрагмент",
            _ => "Фрагмент"
        };
    }

    private static string GetAtmosphereLabel(string? atmosphereKey)
    {
        return atmosphereKey switch
        {
            "archive" => "Архивная тишина",
            "rainy-city" => "Дождливый город",
            "night-route" => "Ночной маршрут",
            _ => string.Empty
        };
    }

    private static string GetAtmosphereDescription(string? atmosphereKey)
    {
        return atmosphereKey switch
        {
            "archive" => "Пыль, бумага, коридоры, поздние лампы.",
            "rainy-city" => "Сырые витрины, пустые переходы, холодный свет.",
            "night-route" => "Платформы, депо, маршрутные огни, медленное движение.",
            _ => string.Empty
        };
    }
}

