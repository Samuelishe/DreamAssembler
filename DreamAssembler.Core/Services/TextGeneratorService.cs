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
    private static readonly string[] PreferredOpeningRoles = ["setup", "scene"];
    private static readonly string[] OpeningRoles = ["setup", "scene"];
    private static readonly string[] LateRoles = ["reflection", "interpretation", "meta"];
    private static readonly string[] RevealRoles = ["observation", "reaction"];

    private readonly IReadOnlyList<DictionaryEntry> _dictionaryEntries;
    private readonly IReadOnlyList<TemplateDefinition> _templates;
    private readonly WeightedRandomSelector _selector;
    private readonly TemplateEngine _templateEngine;
    private readonly Random _random;
    private readonly Queue<string> _recentTemplateIds = new();
    private readonly Dictionary<string, Queue<string>> _recentEntryIdsByCategory = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Создает сервис генерации текста.
    /// </summary>
    /// <param name="dictionaryEntries">Список словарных записей.</param>
    /// <param name="templates">Список шаблонов.</param>
    /// <param name="selector">Селектор случайного выбора по весам.</param>
    /// <param name="templateEngine">Движок подстановки значений.</param>
    /// <param name="random">Источник случайности для композиции коротких текстов.</param>
    public TextGeneratorService(
        IReadOnlyList<DictionaryEntry> dictionaryEntries,
        IReadOnlyList<TemplateDefinition> templates,
        WeightedRandomSelector selector,
        TemplateEngine templateEngine,
        Random? random = null)
    {
        _dictionaryEntries = dictionaryEntries;
        _templates = templates;
        _selector = selector;
        _templateEngine = templateEngine;
        _random = random ?? Random.Shared;
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
        var sentenceCount = _random.Next(2, 6);
        var builder = new StringBuilder();
        var roleUsageCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        string? previousRole = null;

        for (var index = 0; index < sentenceCount; index++)
        {
            if (builder.Length > 0)
            {
                builder.Append(' ');
            }

            var template = SelectTemplate(
                GenerationMode.ShortText,
                (int)options.AbsurdityLevel,
                context,
                roleUsageCounts,
                previousRole,
                index,
                sentenceCount);

            builder.Append(RenderTemplate(template, (int)options.AbsurdityLevel, context));

            previousRole = GetCompositionRole(template);
            IncrementRoleUsage(roleUsageCounts, previousRole);
        }

        return builder.ToString();
    }

    private string GenerateSingleTemplateText(GenerationMode mode, AbsurdityLevel absurdityLevel, GenerationContext context)
    {
        var targetAbsurdity = (int)absurdityLevel;
        var template = SelectTemplate(mode, targetAbsurdity, context);
        return RenderTemplate(template, targetAbsurdity, context);
    }

    private TemplateDefinition SelectTemplate(
        GenerationMode mode,
        int targetAbsurdity,
        GenerationContext context,
        IReadOnlyDictionary<string, int>? roleUsageCounts = null,
        string? previousRole = null,
        int sentenceIndex = -1,
        int sentenceCount = -1)
    {
        var candidates = _templates
            .Where(template => template.Mode == mode)
            .ToList();

        if (candidates.Count == 0)
        {
            throw new InvalidOperationException($"Не найдено шаблонов для режима {mode}.");
        }

        if (mode == GenerationMode.ShortText)
        {
            candidates = FilterShortTextCandidates(candidates, roleUsageCounts, previousRole, sentenceIndex, sentenceCount);
        }

        return _selector.Select(candidates, item => CalculateTemplateWeight(item, targetAbsurdity, context));
    }

    private string RenderTemplate(TemplateDefinition template, int targetAbsurdity, GenerationContext context)
    {
        var values = new Dictionary<string, DictionaryEntry>(StringComparer.OrdinalIgnoreCase);

        foreach (var category in template.RequiredCategories)
        {
            var entry = SelectEntry(category, template, targetAbsurdity, context, values);
            values[category] = entry;
        }

        RememberTemplate(template.Id, context);

        return _templateEngine.Render(template, values);
    }

    private static List<TemplateDefinition> FilterShortTextCandidates(
        IReadOnlyList<TemplateDefinition> candidates,
        IReadOnlyDictionary<string, int>? roleUsageCounts,
        string? previousRole,
        int sentenceIndex,
        int sentenceCount)
    {
        var usage = roleUsageCounts ?? new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var filtered = candidates.ToList();

        if (sentenceIndex == 0)
        {
            var openingCandidates = filtered
                .Where(template => PreferredOpeningRoles.Contains(GetCompositionRole(template), StringComparer.OrdinalIgnoreCase))
                .ToList();

            if (openingCandidates.Count > 0)
            {
                filtered = openingCandidates;
            }
        }
        else
        {
            filtered = TryFilterByRole(filtered, template => !IsMetaRole(template), candidates);
        }

        filtered = TryFilterByRole(filtered, template => CanUseShortTextRole(template, usage, previousRole, sentenceIndex, sentenceCount), candidates);
        return filtered;
    }

    private static List<TemplateDefinition> TryFilterByRole(
        IReadOnlyList<TemplateDefinition> currentCandidates,
        Func<TemplateDefinition, bool> predicate,
        IReadOnlyList<TemplateDefinition> fallbackCandidates)
    {
        var filtered = currentCandidates.Where(predicate).ToList();
        return filtered.Count > 0 ? filtered : fallbackCandidates.ToList();
    }

    private static bool CanUseShortTextRole(
        TemplateDefinition template,
        IReadOnlyDictionary<string, int> roleUsageCounts,
        string? previousRole,
        int sentenceIndex,
        int sentenceCount)
    {
        var role = GetCompositionRole(template);

        if (sentenceIndex == 0 && IsRoleIn(role, LateRoles))
        {
            return false;
        }

        if (sentenceIndex > 0
            && IsRoleIn(role, OpeningRoles)
            && roleUsageCounts.Keys.Any(existingRole => IsRoleIn(existingRole, OpeningRoles)))
        {
            return false;
        }

        if (sentenceIndex == 1
            && IsRoleIn(role, LateRoles)
            && sentenceCount > 2)
        {
            return false;
        }

        if (sentenceIndex == 1
            && IsRoleIn(role, RevealRoles)
            && !roleUsageCounts.ContainsKey("setup")
            && !roleUsageCounts.ContainsKey("scene"))
        {
            return false;
        }

        if (string.Equals(role, "meta", StringComparison.OrdinalIgnoreCase)
            && roleUsageCounts.TryGetValue(role, out var metaCount)
            && metaCount >= 1)
        {
            return false;
        }

        if (string.Equals(role, "reflection", StringComparison.OrdinalIgnoreCase)
            && roleUsageCounts.TryGetValue(role, out var reflectionCount)
            && reflectionCount >= 1)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(previousRole)
            && string.Equals(role, previousRole, StringComparison.OrdinalIgnoreCase)
            && roleUsageCounts.Count > 1)
        {
            return false;
        }

        return true;
    }

    private static bool IsRoleIn(string role, IEnumerable<string> supportedRoles)
    {
        return supportedRoles.Contains(role, StringComparer.OrdinalIgnoreCase);
    }

    private static bool IsMetaRole(TemplateDefinition template)
    {
        return string.Equals(GetCompositionRole(template), "meta", StringComparison.OrdinalIgnoreCase);
    }

    private static string GetCompositionRole(TemplateDefinition template)
    {
        return string.IsNullOrWhiteSpace(template.CompositionRole) ? "default" : template.CompositionRole;
    }

    private static void IncrementRoleUsage(IDictionary<string, int> roleUsageCounts, string role)
    {
        roleUsageCounts.TryGetValue(role, out var count);
        roleUsageCounts[role] = count + 1;
    }

    private DictionaryEntry SelectEntry(
        string category,
        TemplateDefinition template,
        int targetAbsurdity,
        GenerationContext context,
        IReadOnlyDictionary<string, DictionaryEntry> selectedValues)
    {
        template.SlotRequirements.TryGetValue(category, out var requiredSlot);

        var candidates = _dictionaryEntries
            .Where(entry => string.Equals(entry.Category, category, StringComparison.OrdinalIgnoreCase))
            .Where(entry => string.IsNullOrWhiteSpace(requiredSlot)
                            || string.Equals(entry.Slot, requiredSlot, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (candidates.Count == 0)
        {
            var slotSuffix = string.IsNullOrWhiteSpace(requiredSlot) ? string.Empty : $" со слотом '{requiredSlot}'";
            throw new InvalidOperationException($"Не найдены словарные записи для категории '{category}'{slotSuffix}.");
        }

        var selectedEntry = _selector.Select(candidates, item => CalculateEntryWeight(item, template, targetAbsurdity, context, selectedValues));
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

    private double CalculateEntryWeight(
        DictionaryEntry entry,
        TemplateDefinition template,
        int targetAbsurdity,
        GenerationContext context,
        IReadOnlyDictionary<string, DictionaryEntry> selectedValues)
    {
        var baseWeight = Math.Max(0.1d, entry.Weight);
        var absurdityDistance = Math.Abs(entry.Absurdity - targetAbsurdity);
        var absurdityBoost = Math.Max(0.25d, 1.75d - (absurdityDistance * 0.4d));
        var tagMatches = entry.Tags.Intersect(template.Tags, StringComparer.OrdinalIgnoreCase).Count();
        var tagBoost = 1d + (tagMatches * 0.2d);
        var batchPenalty = context.IsEntryUsed(entry.Category, entry.Id) ? 0.22d : 1d;
        var recentPenalty = IsRecentlyUsed(entry.Category, entry.Id) ? 0.5d : 1d;
        var compatibilityBoost = CalculateCompatibilityBoost(entry, selectedValues.Values);

        return baseWeight * absurdityBoost * tagBoost * batchPenalty * recentPenalty * compatibilityBoost;
    }

    private static double CalculateCompatibilityBoost(DictionaryEntry entry, IEnumerable<DictionaryEntry> selectedValues)
    {
        var selectedList = selectedValues.ToList();
        if (selectedList.Count == 0)
        {
            return 1d;
        }

        var score = 1d;

        foreach (var selectedEntry in selectedList)
        {
            var sharedTags = entry.Tags.Intersect(selectedEntry.Tags, StringComparer.OrdinalIgnoreCase).Count();
            if (sharedTags > 0)
            {
                score += sharedTags * 0.18d;
            }
            else
            {
                score *= 0.92d;
            }

            score *= CalculateActionObjectCompatibility(entry, selectedEntry);
        }

        return Math.Max(0.55d, score);
    }

    private static double CalculateActionObjectCompatibility(DictionaryEntry entry, DictionaryEntry selectedEntry)
    {
        if (!IsActionObjectPair(entry, selectedEntry))
        {
            return 1d;
        }

        var entryCompatibilityKeys = GetCompatibilityKeys(entry);
        var selectedCompatibilityKeys = GetCompatibilityKeys(selectedEntry);

        if (entryCompatibilityKeys.Count == 0 || selectedCompatibilityKeys.Count == 0)
        {
            return 0.88d;
        }

        var sharedCompatibilityKeys = entryCompatibilityKeys.Intersect(selectedCompatibilityKeys, StringComparer.OrdinalIgnoreCase).Count();
        if (sharedCompatibilityKeys > 0)
        {
            return 1.8d + ((sharedCompatibilityKeys - 1) * 0.2d);
        }

        return 0.16d;
    }

    private static bool IsActionObjectPair(DictionaryEntry entry, DictionaryEntry selectedEntry)
    {
        return (string.Equals(entry.Category, "action", StringComparison.OrdinalIgnoreCase)
                && string.Equals(selectedEntry.Category, "object", StringComparison.OrdinalIgnoreCase))
               || (string.Equals(entry.Category, "object", StringComparison.OrdinalIgnoreCase)
                   && string.Equals(selectedEntry.Category, "action", StringComparison.OrdinalIgnoreCase));
    }

    private static HashSet<string> GetCompatibilityKeys(DictionaryEntry entry)
    {
        return entry.Tags
            .Where(tag => tag.StartsWith("compat:", StringComparison.OrdinalIgnoreCase))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
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
