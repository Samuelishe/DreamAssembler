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
        Header = $"Результат {index}";
        GeneratedAt = result.CreatedAt;
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
}

