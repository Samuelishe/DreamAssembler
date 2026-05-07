using System.Text;
using DreamAssembler.Core.Enums;
using DreamAssembler.Core.Models;

namespace DreamAssembler.Core.Services;

/// <summary>
/// Выполняет генерацию предложений, коротких текстов и идей.
/// </summary>
public sealed class TextGeneratorService
{
    private readonly IReadOnlyList<DictionaryEntry> _dictionaryEntries;
    private readonly IReadOnlyList<TemplateDefinition> _templates;
    private readonly WeightedRandomSelector _selector;
    private readonly TemplateEngine _templateEngine;

    /// <summary>
    /// Создает сервис генерации текста.
    /// </summary>
    /// <param name="dictionaryEntries">Список словарных записей.</param>
    /// <param name="templates">Список шаблонов.</param>
    /// <param name="selector">Селектор случайного выбора по весам.</param>
    /// <param name="templateEngine">Движок подстановки значений.</param>
    public TextGeneratorService(
        IReadOnlyList<DictionaryEntry> dictionaryEntries,
        IReadOnlyList<TemplateDefinition> templates,
        WeightedRandomSelector selector,
        TemplateEngine templateEngine)
    {
        _dictionaryEntries = dictionaryEntries;
        _templates = templates;
        _selector = selector;
        _templateEngine = templateEngine;
    }

    /// <summary>
    /// Генерирует набор результатов по указанным настройкам.
    /// </summary>
    /// <param name="options">Настройки генерации.</param>
    /// <returns>Список результатов.</returns>
    public IReadOnlyList<TextGenerationResult> Generate(TextGenerationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var resultCount = Math.Clamp(options.ResultCount, 1, 20);
        var results = new List<TextGenerationResult>(resultCount);

        for (var index = 0; index < resultCount; index++)
        {
            var text = options.Mode == GenerationMode.ShortText
                ? GenerateShortText(options)
                : GenerateSingleTemplateText(options.Mode, options.AbsurdityLevel);

            results.Add(new TextGenerationResult
            {
                Text = text,
                Mode = options.Mode,
                AbsurdityLevel = options.AbsurdityLevel,
                CreatedAt = DateTimeOffset.Now
            });
        }

        return results;
    }

    private string GenerateShortText(TextGenerationOptions options)
    {
        var sentenceCount = Random.Shared.Next(2, 6);
        var builder = new StringBuilder();

        for (var index = 0; index < sentenceCount; index++)
        {
            if (builder.Length > 0)
            {
                builder.Append(' ');
            }

            builder.Append(GenerateSingleTemplateText(GenerationMode.ShortText, options.AbsurdityLevel));
        }

        return builder.ToString();
    }

    private string GenerateSingleTemplateText(GenerationMode mode, AbsurdityLevel absurdityLevel)
    {
        var targetAbsurdity = (int)absurdityLevel;
        var candidates = _templates
            .Where(template => template.Mode == mode)
            .ToList();

        if (candidates.Count == 0)
        {
            throw new InvalidOperationException($"Не найдено шаблонов для режима {mode}.");
        }

        var template = _selector.Select(candidates, item => CalculateTemplateWeight(item, targetAbsurdity));
        var values = new Dictionary<string, DictionaryEntry>(StringComparer.OrdinalIgnoreCase);

        foreach (var category in template.RequiredCategories)
        {
            var entry = SelectEntry(category, template, targetAbsurdity);
            values[category] = entry;
        }

        return _templateEngine.Render(template, values);
    }

    private DictionaryEntry SelectEntry(string category, TemplateDefinition template, int targetAbsurdity)
    {
        var candidates = _dictionaryEntries
            .Where(entry => string.Equals(entry.Category, category, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (candidates.Count == 0)
        {
            throw new InvalidOperationException($"Не найдены словарные записи для категории '{category}'.");
        }

        return _selector.Select(candidates, item => CalculateEntryWeight(item, template, targetAbsurdity));
    }

    private static double CalculateTemplateWeight(TemplateDefinition template, int targetAbsurdity)
    {
        var baseWeight = Math.Max(0.1d, template.Weight);
        var inRangeBoost = targetAbsurdity >= template.MinAbsurdity && targetAbsurdity <= template.MaxAbsurdity
            ? 1.6d
            : 0.45d;

        var center = (template.MinAbsurdity + template.MaxAbsurdity) / 2d;
        var distance = Math.Abs(center - targetAbsurdity);
        var closenessBoost = Math.Max(0.3d, 1.4d - (distance * 0.3d));

        return baseWeight * inRangeBoost * closenessBoost;
    }

    private static double CalculateEntryWeight(DictionaryEntry entry, TemplateDefinition template, int targetAbsurdity)
    {
        var baseWeight = Math.Max(0.1d, entry.Weight);
        var absurdityDistance = Math.Abs(entry.Absurdity - targetAbsurdity);
        var absurdityBoost = Math.Max(0.25d, 1.75d - (absurdityDistance * 0.4d));
        var tagMatches = entry.Tags.Intersect(template.Tags, StringComparer.OrdinalIgnoreCase).Count();
        var tagBoost = 1d + (tagMatches * 0.2d);

        return baseWeight * absurdityBoost * tagBoost;
    }
}

