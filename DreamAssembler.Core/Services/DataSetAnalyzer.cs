using DreamAssembler.Core.Models;

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

        var categoryCounts = entries
            .GroupBy(entry => entry.Category, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.Count(), StringComparer.OrdinalIgnoreCase);

        var slotCounts = entries
            .Where(entry => !string.IsNullOrWhiteSpace(entry.Slot))
            .GroupBy(entry => entry.Slot!, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.Count(), StringComparer.OrdinalIgnoreCase);

        AddDuplicateIdIssues(entries, issues);
        AddDuplicateTextIssues(entries, issues);
        AddMissingSlotIssues(entries, issues);
        AddCategoryDensityWarnings(categoryCounts, issues);
        AddTemplateCoverageIssues(templates, categoryCounts, slotCounts, issues);

        var manifest = dataBundle.Manifest;
        return new DataValidationReport
        {
            DataSetId = manifest?.Id ?? "fallback",
            Version = manifest?.Version ?? "fallback",
            EntryCount = entries.Count,
            TemplateCount = templates.Count,
            CategoryCounts = categoryCounts,
            SlotCounts = slotCounts,
            Issues = issues
        };
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
}

