using System.Text;
using DreamAssembler.Core.Enums;
using DreamAssembler.Core.Models;

namespace DreamAssembler.Core.Services;

/// <summary>
/// Выполняет генерацию предложений, коротких текстов и идей.
/// </summary>
public sealed class TextGeneratorService
{
    private const int RecentTemplateLimit = 8;
    private const int RecentEntryLimitPerCategory = 6;

    private readonly IReadOnlyList<DictionaryEntry> _dictionaryEntries;
    private readonly IReadOnlyList<TemplateDefinition> _templates;
    private readonly WeightedRandomSelector _selector;
    private readonly TemplateEngine _templateEngine;
    private readonly Queue<string> _recentTemplateIds = new();
    private readonly Dictionary<string, Queue<string>> _recentEntryIdsByCategory = new(StringComparer.OrdinalIgnoreCase);

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
        var context = new GenerationContext();

        for (var index = 0; index < resultCount; index++)
        {
            var text = options.Mode == GenerationMode.ShortText
                ? GenerateShortText(options, context)
                : GenerateSingleTemplateText(options.Mode, options.AbsurdityLevel, context);

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

    private string GenerateShortText(TextGenerationOptions options, GenerationContext context)
    {
        var sentenceCount = Random.Shared.Next(2, 6);
        var builder = new StringBuilder();

        for (var index = 0; index < sentenceCount; index++)
        {
            if (builder.Length > 0)
            {
                builder.Append(' ');
            }

            builder.Append(GenerateSingleTemplateText(GenerationMode.ShortText, options.AbsurdityLevel, context));
        }

        return builder.ToString();
    }

    private string GenerateSingleTemplateText(GenerationMode mode, AbsurdityLevel absurdityLevel, GenerationContext context)
    {
        var targetAbsurdity = (int)absurdityLevel;
        var candidates = _templates
            .Where(template => template.Mode == mode)
            .ToList();

        if (candidates.Count == 0)
        {
            throw new InvalidOperationException($"Не найдено шаблонов для режима {mode}.");
        }

        var template = _selector.Select(candidates, item => CalculateTemplateWeight(item, targetAbsurdity, context));
        var values = new Dictionary<string, DictionaryEntry>(StringComparer.OrdinalIgnoreCase);

        foreach (var category in template.RequiredCategories)
        {
            var entry = SelectEntry(category, template, targetAbsurdity, context);
            values[category] = entry;
        }

        RememberTemplate(template.Id, context);

        return _templateEngine.Render(template, values);
    }

    private DictionaryEntry SelectEntry(string category, TemplateDefinition template, int targetAbsurdity, GenerationContext context)
    {
        var candidates = _dictionaryEntries
            .Where(entry => string.Equals(entry.Category, category, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (candidates.Count == 0)
        {
            throw new InvalidOperationException($"Не найдены словарные записи для категории '{category}'.");
        }

        var selectedEntry = _selector.Select(candidates, item => CalculateEntryWeight(item, template, targetAbsurdity, context));
        RememberEntry(selectedEntry, context);
        return selectedEntry;
    }

    private double CalculateTemplateWeight(TemplateDefinition template, int targetAbsurdity, GenerationContext context)
    {
        var baseWeight = Math.Max(0.1d, template.Weight);
        var inRangeBoost = targetAbsurdity >= template.MinAbsurdity && targetAbsurdity <= template.MaxAbsurdity
            ? 1.6d
            : 0.45d;

        var center = (template.MinAbsurdity + template.MaxAbsurdity) / 2d;
        var distance = Math.Abs(center - targetAbsurdity);
        var closenessBoost = Math.Max(0.3d, 1.4d - (distance * 0.3d));
        var batchPenalty = context.UsedTemplateIds.Contains(template.Id) ? 0.18d : 1d;
        var recentPenalty = _recentTemplateIds.Contains(template.Id) ? 0.45d : 1d;

        return baseWeight * inRangeBoost * closenessBoost * batchPenalty * recentPenalty;
    }

    private double CalculateEntryWeight(DictionaryEntry entry, TemplateDefinition template, int targetAbsurdity, GenerationContext context)
    {
        var baseWeight = Math.Max(0.1d, entry.Weight);
        var absurdityDistance = Math.Abs(entry.Absurdity - targetAbsurdity);
        var absurdityBoost = Math.Max(0.25d, 1.75d - (absurdityDistance * 0.4d));
        var tagMatches = entry.Tags.Intersect(template.Tags, StringComparer.OrdinalIgnoreCase).Count();
        var tagBoost = 1d + (tagMatches * 0.2d);
        var batchPenalty = context.IsEntryUsed(entry.Category, entry.Id) ? 0.22d : 1d;
        var recentPenalty = IsRecentlyUsed(entry.Category, entry.Id) ? 0.5d : 1d;

        return baseWeight * absurdityBoost * tagBoost * batchPenalty * recentPenalty;
    }

    private void RememberTemplate(string templateId, GenerationContext context)
    {
        context.UsedTemplateIds.Add(templateId);
        EnqueueWithLimit(_recentTemplateIds, templateId, RecentTemplateLimit);
    }

    private void RememberEntry(DictionaryEntry entry, GenerationContext context)
    {
        context.MarkEntryAsUsed(entry.Category, entry.Id);

        if (!_recentEntryIdsByCategory.TryGetValue(entry.Category, out var queue))
        {
            queue = new Queue<string>();
            _recentEntryIdsByCategory[entry.Category] = queue;
        }

        EnqueueWithLimit(queue, entry.Id, RecentEntryLimitPerCategory);
    }

    private bool IsRecentlyUsed(string category, string entryId)
    {
        return _recentEntryIdsByCategory.TryGetValue(category, out var queue) && queue.Contains(entryId);
    }

    private static void EnqueueWithLimit(Queue<string> queue, string value, int limit)
    {
        queue.Enqueue(value);
        while (queue.Count > limit)
        {
            queue.Dequeue();
        }
    }

    private sealed class GenerationContext
    {
        public HashSet<string> UsedTemplateIds { get; } = new(StringComparer.OrdinalIgnoreCase);

        private Dictionary<string, HashSet<string>> UsedEntryIdsByCategory { get; } = new(StringComparer.OrdinalIgnoreCase);

        public bool IsEntryUsed(string category, string entryId)
        {
            return UsedEntryIdsByCategory.TryGetValue(category, out var entries) && entries.Contains(entryId);
        }

        public void MarkEntryAsUsed(string category, string entryId)
        {
            if (!UsedEntryIdsByCategory.TryGetValue(category, out var entries))
            {
                entries = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                UsedEntryIdsByCategory[category] = entries;
            }

            entries.Add(entryId);
        }
    }
}
