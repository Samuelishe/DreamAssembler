using System.Text;
using DreamAssembler.Core.Enums;
using DreamAssembler.Core.Models;

namespace DreamAssembler.Core.Services;

/// <summary>
/// Выполняет генерацию предложений, коротких текстов, идей, словосочетаний и коротких фраз.
/// </summary>
public sealed class TextGeneratorService
{
    private const int RecentTemplateLimit = 8;
    private const int RecentEntryLimitPerCategory = 6;
    private const int RecentAssociationFragmentLimit = 10;
    private static readonly string[] PreferredOpeningRoles = ["setup", "scene"];
    private static readonly string[] OpeningRoles = ["setup", "scene"];
    private static readonly string[] LateRoles = ["reflection", "interpretation", "meta"];
    private static readonly string[] RevealRoles = ["observation", "reaction"];
    private static readonly HashSet<string> StrongManifoldTags =
    [
        "airport",
        "museum",
        "mall",
        "hospitality"
    ];

    private readonly IReadOnlyList<DictionaryEntry> _dictionaryEntries;
    private readonly IReadOnlyList<TemplateDefinition> _templates;
    private readonly IReadOnlyList<AssociationFragmentEntry> _associationFragments;
    private readonly WeightedRandomSelector _selector;
    private readonly TemplateEngine _templateEngine;
    private readonly Random _random;
    private readonly Queue<string> _recentTemplateIds = new();
    private readonly Dictionary<string, Queue<string>> _recentEntryIdsByCategory = new(StringComparer.OrdinalIgnoreCase);
    private readonly Queue<string> _recentAssociationFragmentIds = new();

    /// <summary>
    /// Создает сервис генерации текста.
    /// </summary>
    /// <param name="dictionaryEntries">Список словарных записей.</param>
    /// <param name="templates">Список шаблонов.</param>
    /// <param name="associationFragments">Список словарных записей для словесных режимов.</param>
    /// <param name="selector">Селектор случайного выбора по весам.</param>
    /// <param name="templateEngine">Движок подстановки значений.</param>
    /// <param name="random">Источник случайности для композиции коротких текстов.</param>
    public TextGeneratorService(
        IReadOnlyList<DictionaryEntry> dictionaryEntries,
        IReadOnlyList<TemplateDefinition> templates,
        IReadOnlyList<AssociationFragmentEntry> associationFragments,
        WeightedRandomSelector selector,
        TemplateEngine templateEngine,
        Random? random = null)
    {
        _dictionaryEntries = dictionaryEntries;
        _templates = templates;
        _associationFragments = associationFragments;
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
        var lexicalClusterKey = options.Mode is GenerationMode.WordPair or GenerationMode.WordCluster
            ? SelectLexicalClusterKey()
            : null;

        for (var index = 0; index < resultCount; index++)
        {
            var text = options.Mode switch
            {
                GenerationMode.ShortText => GenerateShortText(options, context),
                GenerationMode.WordPair => GenerateWordPair(lexicalClusterKey),
                GenerationMode.WordCluster => GenerateWordCluster(lexicalClusterKey),
                _ => GenerateSingleTemplateText(options.Mode, options.AbsurdityLevel, context)
            };

            results.Add(new TextGenerationResult
            {
                Text = text,
                Mode = options.Mode,
                AbsurdityLevel = options.AbsurdityLevel,
                CreatedAt = DateTimeOffset.Now,
                AtmosphereKey = lexicalClusterKey
            });
        }

        return results;
    }

    private string GenerateWordPair(string? lexicalClusterKey)
    {
        if (_associationFragments.Count == 0)
        {
            throw new InvalidOperationException("Не найдены словари для режима словосочетаний.");
        }

        var supportedGenders = new List<string>();
        foreach (var gender in new[] { "m", "f", "n" })
        {
            if (GetAssociationFragmentsByKind($"noun_{gender}").Count > 0
                && GetAssociationFragmentsByKind($"adjective_{gender}").Count > 0)
            {
                supportedGenders.Add(gender);
            }
        }

        if (supportedGenders.Count == 0)
        {
            throw new InvalidOperationException("Словари словосочетаний загружены, но не содержат совместимых прилагательных и существительных.");
        }

        var genderKey = _selector.Select(supportedGenders, _ => 1.0);
        return RenderAssociationNominalPhrase(genderKey, 1, lexicalClusterKey);
    }

    private string GenerateWordCluster(string? lexicalClusterKey)
    {
        if (_associationFragments.Count == 0)
        {
            throw new InvalidOperationException("Не найдены словари для режима нескольких слов.");
        }

        var supportedGenders = new List<string>();
        foreach (var gender in new[] { "m", "f", "n" })
        {
            if (GetAssociationFragmentsByKind($"noun_{gender}").Count > 0
                && GetAssociationFragmentsByKind($"adjective_{gender}").Count > 0
                && GetAssociationFragmentsByKind($"verb_past_{gender}").Count > 0)
            {
                supportedGenders.Add(gender);
            }
        }

        if (supportedGenders.Count == 0)
        {
            throw new InvalidOperationException("Словари нескольких слов загружены, но не содержат согласованных существительных, прилагательных и глаголов.");
        }

        var genderKey = _selector.Select(supportedGenders, _ => 1.0);
        var patterns = new[] { 1, 2 };
        var adjectiveCount = _selector.Select(patterns, GetWordClusterPatternWeight);

        return RenderAssociationPredicatePhrase(genderKey, adjectiveCount, lexicalClusterKey);
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

        return NormalizeSentenceStart(_templateEngine.Render(template, values));
    }

    private static string NormalizeSentenceStart(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return text;
        }

        var firstLetterIndex = -1;
        for (var index = 0; index < text.Length; index++)
        {
            if (char.IsLetter(text[index]))
            {
                firstLetterIndex = index;
                break;
            }
        }

        if (firstLetterIndex < 0 || char.IsUpper(text[firstLetterIndex]))
        {
            return text;
        }

        var chars = text.ToCharArray();
        chars[firstLetterIndex] = char.ToUpper(chars[firstLetterIndex]);
        return new string(chars);
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

    private IReadOnlyList<AssociationFragmentEntry> GetAssociationFragmentsByKind(string kind)
    {
        return _associationFragments
            .Where(entry => string.Equals(entry.Kind, kind, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    private double GetWordClusterPatternWeight(int adjectiveCount)
    {
        return adjectiveCount switch
        {
            1 => 1.0,
            2 => 0.85,
            _ => 0.5
        };
    }

    private string RenderAssociationNominalPhrase(string genderKey, int adjectiveCount, string? lexicalClusterKey)
    {
        var nouns = GetAssociationFragmentsByKind($"noun_{genderKey}");
        var adjectives = GetAssociationFragmentsByKind($"adjective_{genderKey}");

        if (nouns.Count == 0 || adjectives.Count == 0)
        {
            throw new InvalidOperationException($"Не найдены данные для режима словосочетаний по роду '{genderKey}'.");
        }

        var noun = SelectAssociationFragment(nouns, lexicalClusterKey);
        RememberAssociationFragment(noun.Id);
        var phraseAdjectives = SelectUniqueAssociationAdjectives(adjectives, noun, adjectiveCount, lexicalClusterKey);

        var words = phraseAdjectives
            .Select(entry => entry.Text)
            .Append(noun.Text)
            .ToArray();

        return ValidateAssociationWordCount(words, 2, 2);
    }

    private string RenderAssociationPredicatePhrase(string genderKey, int adjectiveCount, string? lexicalClusterKey)
    {
        var nouns = GetAssociationFragmentsByKind($"noun_{genderKey}");
        var adjectives = GetAssociationFragmentsByKind($"adjective_{genderKey}");
        var verbs = GetAssociationFragmentsByKind($"verb_past_{genderKey}");

        if (nouns.Count == 0 || adjectives.Count == 0 || verbs.Count == 0)
        {
            throw new InvalidOperationException($"Не найдены данные для режима нескольких слов по роду '{genderKey}'.");
        }

        var noun = SelectAssociationFragment(nouns, lexicalClusterKey);
        RememberAssociationFragment(noun.Id);
        var phraseAdjectives = SelectUniqueAssociationAdjectives(adjectives, noun, adjectiveCount, lexicalClusterKey);
        var verb = SelectAssociationFragment(verbs, lexicalClusterKey);
        RememberAssociationFragment(verb.Id);

        var words = phraseAdjectives
            .Select(entry => entry.Text)
            .Append(noun.Text)
            .Append(verb.Text)
            .ToArray();

        return ValidateAssociationWordCount(words, 3, 4);
    }

    private List<AssociationFragmentEntry> SelectUniqueAssociationAdjectives(
        IReadOnlyList<AssociationFragmentEntry> adjectives,
        AssociationFragmentEntry noun,
        int adjectiveCount,
        string? lexicalClusterKey)
    {
        var phraseAdjectives = new List<AssociationFragmentEntry>(adjectiveCount);
        var usedIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var availableAdjectives = adjectives
            .Where(entry => !string.Equals(entry.Text, noun.Text, StringComparison.OrdinalIgnoreCase))
            .ToList();

        while (phraseAdjectives.Count < adjectiveCount && availableAdjectives.Count > 0)
        {
            var eligibleAdjectives = availableAdjectives
                .Where(entry => !usedIds.Contains(entry.Id))
                .ToList();

            if (eligibleAdjectives.Count == 0)
            {
                break;
            }

            var nextAdjective = SelectAssociationFragment(eligibleAdjectives, lexicalClusterKey);

            phraseAdjectives.Add(nextAdjective);
            usedIds.Add(nextAdjective.Id);
            RememberAssociationFragment(nextAdjective.Id);
        }

        if (phraseAdjectives.Count == 0)
        {
            throw new InvalidOperationException("Не удалось подобрать прилагательные для словесного режима.");
        }

        return phraseAdjectives;
    }

    private static string ValidateAssociationWordCount(string[] words, int minCount, int maxCount)
    {
        if (words.Length < minCount || words.Length > maxCount)
        {
            throw new InvalidOperationException($"Режим словесной генерации собрал фразу за пределами {minCount}-{maxCount} слов.");
        }

        return string.Join(' ', words);
    }

    private AssociationFragmentEntry SelectAssociationFragment(IReadOnlyList<AssociationFragmentEntry> candidates, string? lexicalClusterKey = null)
    {
        if (candidates.Count == 0)
        {
            throw new InvalidOperationException("Не найдены подходящие ассоциативные фрагменты.");
        }

        return _selector.Select(candidates, entry => CalculateAssociationFragmentWeight(entry, lexicalClusterKey));
    }

    private string? SelectLexicalClusterKey()
    {
        var clusterKeys = _associationFragments
            .SelectMany(entry => entry.Tags)
            .Where(tag => tag.StartsWith("cluster:", StringComparison.OrdinalIgnoreCase))
            .Select(tag => tag["cluster:".Length..])
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (clusterKeys.Count == 0)
        {
            return null;
        }

        return _selector.Select(clusterKeys, clusterKey => 1d);
    }

    private double CalculateAssociationFragmentWeight(AssociationFragmentEntry entry, string? lexicalClusterKey)
    {
        var baseWeight = Math.Max(0.1d, entry.Weight);
        var recentPenalty = _recentAssociationFragmentIds.Contains(entry.Id) ? 0.35d : 1d;
        var clusterWeight = CalculateLexicalClusterWeight(entry, lexicalClusterKey);
        return baseWeight * recentPenalty * clusterWeight;
    }

    private static double CalculateLexicalClusterWeight(AssociationFragmentEntry entry, string? lexicalClusterKey)
    {
        if (string.IsNullOrWhiteSpace(lexicalClusterKey))
        {
            return 1d;
        }

        var targetTag = $"cluster:{lexicalClusterKey}";
        if (entry.Tags.Contains(targetTag, StringComparer.OrdinalIgnoreCase))
        {
            return 1.9d;
        }

        return entry.Tags.Any(tag => tag.StartsWith("cluster:", StringComparison.OrdinalIgnoreCase))
            ? 0.72d
            : 0.94d;
    }

    private void RememberAssociationFragment(string fragmentId)
    {
        EnqueueWithLimit(_recentAssociationFragmentIds, fragmentId, RecentAssociationFragmentLimit);
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

        candidates = FilterStrongActionObjectCandidates(category, candidates, selectedValues);

        if (candidates.Count == 0)
        {
            var slotSuffix = string.IsNullOrWhiteSpace(requiredSlot) ? string.Empty : $" со слотом '{requiredSlot}'";
            throw new InvalidOperationException($"Не найдены словарные записи для категории '{category}'{slotSuffix}.");
        }

        var selectedEntry = _selector.Select(candidates, item => CalculateEntryWeight(item, template, targetAbsurdity, context, selectedValues));
        RememberEntry(selectedEntry, context);
        return selectedEntry;
    }

    private static List<DictionaryEntry> FilterStrongActionObjectCandidates(
        string category,
        List<DictionaryEntry> candidates,
        IReadOnlyDictionary<string, DictionaryEntry> selectedValues)
    {
        if (candidates.Count == 0)
        {
            return candidates;
        }

        DictionaryEntry? pairedEntry = null;

        if (string.Equals(category, "action", StringComparison.OrdinalIgnoreCase))
        {
            selectedValues.TryGetValue("object", out pairedEntry);
        }
        else if (string.Equals(category, "object", StringComparison.OrdinalIgnoreCase))
        {
            selectedValues.TryGetValue("action", out pairedEntry);
        }

        if (pairedEntry is null)
        {
            return candidates;
        }

        var pairedCompatibilityKeys = GetCompatibilityKeys(pairedEntry);
        if (pairedCompatibilityKeys.Count == 0)
        {
            return candidates;
        }

        var strongCandidates = candidates
            .Where(candidate => GetCompatibilityKeys(candidate).Intersect(pairedCompatibilityKeys, StringComparer.OrdinalIgnoreCase).Any())
            .ToList();

        return strongCandidates.Count > 0 ? strongCandidates : candidates;
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

            score *= CalculateManifoldAffinityBoost(entry, selectedEntry);
            score *= CalculateActionObjectCompatibility(entry, selectedEntry);
        }

        return Math.Max(0.55d, score);
    }

    private static double CalculateManifoldAffinityBoost(DictionaryEntry entry, DictionaryEntry selectedEntry)
    {
        var entryFieldTags = GetStrongManifoldTags(entry);
        var selectedFieldTags = GetStrongManifoldTags(selectedEntry);

        if (entryFieldTags.Count == 0 || selectedFieldTags.Count == 0)
        {
            return 1d;
        }

        var sharedFieldTags = entryFieldTags.Intersect(selectedFieldTags, StringComparer.OrdinalIgnoreCase).Count();
        if (sharedFieldTags > 0)
        {
            return 1d + (sharedFieldTags * 0.28d);
        }

        return 0.78d;
    }

    private static double CalculateActionObjectCompatibility(DictionaryEntry entry, DictionaryEntry selectedEntry)
    {
        if (!IsActionObjectPair(entry, selectedEntry))
        {
            return 1d;
        }

        var actionEntry = string.Equals(entry.Category, "action", StringComparison.OrdinalIgnoreCase) ? entry : selectedEntry;
        var objectEntry = string.Equals(entry.Category, "object", StringComparison.OrdinalIgnoreCase) ? entry : selectedEntry;

        var actionCompatibilityKeys = GetCompatibilityKeys(actionEntry);
        var objectCompatibilityKeys = GetCompatibilityKeys(objectEntry);

        if (actionCompatibilityKeys.Count == 0 && objectCompatibilityKeys.Count == 0)
        {
            return 0.88d;
        }

        if (actionCompatibilityKeys.Count == 0 || objectCompatibilityKeys.Count == 0)
        {
            return 0.42d;
        }

        var sharedCompatibilityKeys = actionCompatibilityKeys.Intersect(objectCompatibilityKeys, StringComparer.OrdinalIgnoreCase).Count();
        if (sharedCompatibilityKeys > 0)
        {
            return 1.8d + ((sharedCompatibilityKeys - 1) * 0.2d);
        }

        return 0.05d;
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

    private static HashSet<string> GetStrongManifoldTags(DictionaryEntry entry)
    {
        return entry.Tags
            .Where(tag => StrongManifoldTags.Contains(tag))
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
