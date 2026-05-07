using DreamAssembler.Core.Models;
using System.Text.Json;

namespace DreamAssembler.Core.Services;

/// <summary>
/// Выполняет валидацию и сбор статистики по набору данных генератора.
/// </summary>
public sealed class DataSetAnalyzer
{
    /// <summary>
    /// Анализирует набор данных генератора.
    /// </summary>
    /// <param name="dataBundle">Набор данных генератора.</param>
    /// <returns>Отчет по валидации и статистике.</returns>
    public DataValidationReport Analyze(GeneratorDataBundle dataBundle)
    {
        ArgumentNullException.ThrowIfNull(dataBundle);

        var issues = new List<DataValidationIssue>();
        var entries = dataBundle.DictionaryEntries;
        var templates = dataBundle.Templates;
        var associationFragments = dataBundle.AssociationFragments;

        var categoryCounts = entries
            .GroupBy(entry => entry.Category, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.Count(), StringComparer.OrdinalIgnoreCase);

        var slotCounts = entries
            .Where(entry => !string.IsNullOrWhiteSpace(entry.Slot))
            .GroupBy(entry => entry.Slot!, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.Count(), StringComparer.OrdinalIgnoreCase);

        var associationKindCounts = associationFragments
            .GroupBy(entry => entry.Kind, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.Count(), StringComparer.OrdinalIgnoreCase);

        var dictionarySetStats = new List<DictionarySetStatistics>();
        var associationSetStats = new List<AssociationSetStatistics>();

        AddDuplicateIdIssues(entries, issues);
        AddDuplicateTextIssues(entries, issues);
        AddMissingSlotIssues(entries, issues);
        AddCategoryDensityWarnings(categoryCounts, issues);
        AddTemplateCoverageIssues(templates, categoryCounts, slotCounts, issues);
        AddAssociationFragmentIssues(associationFragments, associationKindCounts, issues);

        var manifest = dataBundle.Manifest;
        AddManifestSetStatistics(dataBundle.DataRootPath, manifest, dictionarySetStats, associationSetStats, issues);

        return new DataValidationReport
        {
            DataSetId = manifest?.Id ?? "fallback",
            Version = manifest?.Version ?? "fallback",
            EntryCount = entries.Count,
            TemplateCount = templates.Count,
            AssociationFragmentCount = associationFragments.Count,
            CategoryCounts = categoryCounts,
            SlotCounts = slotCounts,
            AssociationKindCounts = associationKindCounts,
            DictionarySetStats = dictionarySetStats,
            AssociationSetStats = associationSetStats,
            Issues = issues
        };
    }

    private static void AddManifestSetStatistics(
        string dataRootPath,
        DataSetManifest? manifest,
        ICollection<DictionarySetStatistics> dictionarySetStats,
        ICollection<AssociationSetStatistics> associationSetStats,
        ICollection<DataValidationIssue> issues)
    {
        if (manifest is null || string.IsNullOrWhiteSpace(dataRootPath) || !Directory.Exists(dataRootPath))
        {
            return;
        }

        foreach (var setName in manifest.DictionarySets)
        {
            var relativePath = $"{setName.Replace('/', Path.DirectorySeparatorChar)}.json";
            var filePath = Path.Combine(dataRootPath, "Dictionaries", relativePath);
            if (!File.Exists(filePath))
            {
                issues.Add(new DataValidationIssue
                {
                    Severity = DataValidationSeverity.Warning,
                    Code = "missing-dictionary-set-file",
                    Message = $"Файл JSON-пака '{setName}' не найден по пути '{filePath}'."
                });
                continue;
            }

            try
            {
                using var document = JsonDocument.Parse(File.ReadAllText(filePath));
                if (!document.RootElement.TryGetProperty("entries", out var entriesElement) || entriesElement.ValueKind != JsonValueKind.Array)
                {
                    issues.Add(new DataValidationIssue
                    {
                        Severity = DataValidationSeverity.Warning,
                        Code = "invalid-dictionary-set-file",
                        Message = $"Файл JSON-пака '{setName}' не содержит массива 'entries'."
                    });
                    continue;
                }

                var categoryCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                var slotCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                var entryCount = 0;

                foreach (var entryElement in entriesElement.EnumerateArray())
                {
                    if (!TryReadDictionaryEntry(entryElement, out var category, out var slot))
                    {
                        continue;
                    }

                    entryCount++;
                    categoryCounts[category] = categoryCounts.GetValueOrDefault(category) + 1;

                    if (!string.IsNullOrWhiteSpace(slot))
                    {
                        slotCounts[slot] = slotCounts.GetValueOrDefault(slot) + 1;
                    }
                }

                dictionarySetStats.Add(new DictionarySetStatistics
                {
                    SetName = setName,
                    EntryCount = entryCount,
                    CategoryCounts = categoryCounts,
                    SlotCounts = slotCounts
                });
            }
            catch (IOException)
            {
                issues.Add(new DataValidationIssue
                {
                    Severity = DataValidationSeverity.Warning,
                    Code = "unreadable-dictionary-set-file",
                    Message = $"Не удалось прочитать JSON-пак '{setName}'."
                });
            }
            catch (UnauthorizedAccessException)
            {
                issues.Add(new DataValidationIssue
                {
                    Severity = DataValidationSeverity.Warning,
                    Code = "unreadable-dictionary-set-file",
                    Message = $"Нет доступа к JSON-паку '{setName}'."
                });
            }
            catch (JsonException)
            {
                issues.Add(new DataValidationIssue
                {
                    Severity = DataValidationSeverity.Warning,
                    Code = "invalid-dictionary-set-file",
                    Message = $"Файл JSON-пака '{setName}' поврежден или имеет неверный JSON-формат."
                });
            }
        }

        foreach (var setName in manifest.AssociationSets)
        {
            var relativePath = setName.Replace('/', Path.DirectorySeparatorChar);
            var filePath = Path.Combine(dataRootPath, "AssociationWords", relativePath);
            if (!File.Exists(filePath))
            {
                issues.Add(new DataValidationIssue
                {
                    Severity = DataValidationSeverity.Warning,
                    Code = "missing-association-set-file",
                    Message = $"CSV-источник '{setName}' не найден по пути '{filePath}'."
                });
                continue;
            }

            try
            {
                var rowCount = File.ReadLines(filePath)
                    .Skip(1)
                    .Count(line => !string.IsNullOrWhiteSpace(line));

                associationSetStats.Add(new AssociationSetStatistics
                {
                    SetName = setName,
                    SourceKind = InferAssociationSourceKind(filePath),
                    RowCount = rowCount
                });
            }
            catch (IOException)
            {
                issues.Add(new DataValidationIssue
                {
                    Severity = DataValidationSeverity.Warning,
                    Code = "unreadable-association-set-file",
                    Message = $"Не удалось прочитать CSV-источник '{setName}'."
                });
            }
            catch (UnauthorizedAccessException)
            {
                issues.Add(new DataValidationIssue
                {
                    Severity = DataValidationSeverity.Warning,
                    Code = "unreadable-association-set-file",
                    Message = $"Нет доступа к CSV-источнику '{setName}'."
                });
            }
        }
    }

    private static bool TryReadDictionaryEntry(JsonElement entryElement, out string category, out string? slot)
    {
        category = string.Empty;
        slot = null;

        if (entryElement.ValueKind != JsonValueKind.Object)
        {
            return false;
        }

        var id = ReadString(entryElement, "id");
        var text = ReadString(entryElement, "text");
        category = ReadString(entryElement, "category");
        slot = ReadString(entryElement, "slot");
        var weight = ReadDouble(entryElement, "weight");

        return !string.IsNullOrWhiteSpace(id)
               && !string.IsNullOrWhiteSpace(text)
               && !string.IsNullOrWhiteSpace(category)
               && weight > 0d;
    }

    private static string ReadString(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property) || property.ValueKind != JsonValueKind.String)
        {
            return string.Empty;
        }

        return property.GetString() ?? string.Empty;
    }

    private static double ReadDouble(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property))
        {
            return 0d;
        }

        if (property.ValueKind == JsonValueKind.Number && property.TryGetDouble(out var number))
        {
            return number;
        }

        return 0d;
    }

    private static string InferAssociationSourceKind(string filePath)
    {
        var fileName = Path.GetFileName(filePath);
        if (fileName.Contains("nouns", StringComparison.OrdinalIgnoreCase))
        {
            return "nouns";
        }

        if (fileName.Contains("adjectives", StringComparison.OrdinalIgnoreCase))
        {
            return "adjectives";
        }

        if (fileName.Contains("verbs", StringComparison.OrdinalIgnoreCase))
        {
            return "verbs";
        }

        if (fileName.Contains("others", StringComparison.OrdinalIgnoreCase))
        {
            return "others";
        }

        return "unknown";
    }

    private static void AddDuplicateIdIssues(IReadOnlyList<DictionaryEntry> entries, ICollection<DataValidationIssue> issues)
    {
        foreach (var group in entries.GroupBy(entry => entry.Id, StringComparer.OrdinalIgnoreCase).Where(group => group.Count() > 1))
        {
            issues.Add(new DataValidationIssue
            {
                Severity = DataValidationSeverity.Error,
                Code = "duplicate-id",
                Message = $"Повторяющийся id словарной записи: '{group.Key}'."
            });
        }
    }

    private static void AddDuplicateTextIssues(IReadOnlyList<DictionaryEntry> entries, ICollection<DataValidationIssue> issues)
    {
        foreach (var group in entries
                     .GroupBy(entry => $"{entry.Category}::{entry.Text}", StringComparer.OrdinalIgnoreCase)
                     .Where(group => group.Count() > 1))
        {
            var sample = group.First();
            issues.Add(new DataValidationIssue
            {
                Severity = DataValidationSeverity.Warning,
                Code = "duplicate-text",
                Message = $"Повторяющийся text в категории '{sample.Category}': '{sample.Text}'."
            });
        }
    }

    private static void AddMissingSlotIssues(IReadOnlyList<DictionaryEntry> entries, ICollection<DataValidationIssue> issues)
    {
        foreach (var entry in entries.Where(entry => string.IsNullOrWhiteSpace(entry.Slot)))
        {
            issues.Add(new DataValidationIssue
            {
                Severity = DataValidationSeverity.Warning,
                Code = "missing-slot",
                Message = $"У записи '{entry.Id}' не указан slot."
            });
        }
    }

    private static void AddCategoryDensityWarnings(
        IReadOnlyDictionary<string, int> categoryCounts,
        ICollection<DataValidationIssue> issues)
    {
        foreach (var pair in categoryCounts.OrderBy(pair => pair.Key, StringComparer.OrdinalIgnoreCase))
        {
            if (pair.Value < 5)
            {
                issues.Add(new DataValidationIssue
                {
                    Severity = DataValidationSeverity.Warning,
                    Code = "low-category-density",
                    Message = $"В категории '{pair.Key}' пока мало записей: {pair.Value}."
                });
            }
        }
    }

    private static void AddTemplateCoverageIssues(
        IReadOnlyList<TemplateDefinition> templates,
        IReadOnlyDictionary<string, int> categoryCounts,
        IReadOnlyDictionary<string, int> slotCounts,
        ICollection<DataValidationIssue> issues)
    {
        foreach (var template in templates)
        {
            foreach (var category in template.RequiredCategories)
            {
                if (!categoryCounts.TryGetValue(category, out var count) || count == 0)
                {
                    issues.Add(new DataValidationIssue
                    {
                        Severity = DataValidationSeverity.Error,
                        Code = "missing-category-coverage",
                        Message = $"Шаблон '{template.Id}' требует категорию '{category}', но в данных нет подходящих записей."
                    });
                }
            }

            foreach (var slotRequirement in template.SlotRequirements)
            {
                if (!template.RequiredCategories.Contains(slotRequirement.Key, StringComparer.OrdinalIgnoreCase))
                {
                    issues.Add(new DataValidationIssue
                    {
                        Severity = DataValidationSeverity.Warning,
                        Code = "unused-slot-requirement",
                        Message = $"Шаблон '{template.Id}' содержит slotRequirement для '{slotRequirement.Key}', но эта категория не входит в requiredCategories."
                    });
                }

                if (!slotCounts.TryGetValue(slotRequirement.Value, out var slotCount) || slotCount == 0)
                {
                    issues.Add(new DataValidationIssue
                    {
                        Severity = DataValidationSeverity.Error,
                        Code = "missing-slot-coverage",
                        Message = $"Шаблон '{template.Id}' требует slot '{slotRequirement.Value}', но в словарях нет записей с таким slot."
                    });
                }
            }
        }
    }

    private static void AddAssociationFragmentIssues(
        IReadOnlyList<AssociationFragmentEntry> associationFragments,
        IReadOnlyDictionary<string, int> associationKindCounts,
        ICollection<DataValidationIssue> issues)
    {
        foreach (var group in associationFragments
                     .GroupBy(entry => entry.Id, StringComparer.OrdinalIgnoreCase)
                     .Where(group => group.Count() > 1))
        {
            issues.Add(new DataValidationIssue
            {
                Severity = DataValidationSeverity.Error,
                Code = "duplicate-association-id",
                Message = $"Повторяющийся id ассоциативной записи: '{group.Key}'."
            });
        }

        foreach (var group in associationFragments
                     .GroupBy(entry => $"{entry.Kind}::{entry.Text}", StringComparer.OrdinalIgnoreCase)
                     .Where(group => group.Count() > 1))
        {
            var sample = group.First();
            issues.Add(new DataValidationIssue
            {
                Severity = DataValidationSeverity.Warning,
                Code = "duplicate-association-text",
                Message = $"Повторяющийся text у ассоциативных слов типа '{sample.Kind}': '{sample.Text}'."
            });
        }

        var hasNouns = associationKindCounts.Keys.Any(kind => kind.StartsWith("noun_", StringComparison.OrdinalIgnoreCase));
        var hasAdjectives = associationKindCounts.Keys.Any(kind => kind.StartsWith("adjective_", StringComparison.OrdinalIgnoreCase));

        if (!hasNouns)
        {
            issues.Add(new DataValidationIssue
            {
                Severity = DataValidationSeverity.Error,
                Code = "missing-association-nouns",
                Message = "Для словесных режимов не найдены существительные."
            });
        }

        if (!hasAdjectives)
        {
            issues.Add(new DataValidationIssue
            {
                Severity = DataValidationSeverity.Error,
                Code = "missing-association-adjectives",
                Message = "Для словесных режимов не найдены прилагательные."
            });
        }

        var hasVerbs = associationKindCounts.Keys.Any(kind => kind.StartsWith("verb_past_", StringComparison.OrdinalIgnoreCase));
        if (!hasVerbs)
        {
            issues.Add(new DataValidationIssue
            {
                Severity = DataValidationSeverity.Warning,
                Code = "missing-association-verbs",
                Message = "Для режима нескольких слов пока не найдены глаголы прошедшего времени."
            });
        }

        foreach (var gender in new[] { "m", "f", "n" })
        {
            var nounKind = $"noun_{gender}";
            var adjectiveKind = $"adjective_{gender}";
            var verbKind = $"verb_past_{gender}";

            if (!associationKindCounts.ContainsKey(nounKind))
            {
                issues.Add(new DataValidationIssue
                {
                    Severity = DataValidationSeverity.Warning,
                    Code = "missing-association-gender-nouns",
                    Message = $"Для словесных режимов пока нет существительных типа '{nounKind}'."
                });
            }

            if (!associationKindCounts.ContainsKey(adjectiveKind))
            {
                issues.Add(new DataValidationIssue
                {
                    Severity = DataValidationSeverity.Warning,
                    Code = "missing-association-gender-adjectives",
                    Message = $"Для словесных режимов пока нет прилагательных типа '{adjectiveKind}'."
                });
            }

            if (!associationKindCounts.ContainsKey(verbKind))
            {
                issues.Add(new DataValidationIssue
                {
                    Severity = DataValidationSeverity.Warning,
                    Code = "missing-association-gender-verbs",
                    Message = $"Для режима нескольких слов пока нет глаголов типа '{verbKind}'."
                });
            }
        }
    }
}
