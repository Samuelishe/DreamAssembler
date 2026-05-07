using DreamAssembler.Core.Enums;
using DreamAssembler.Core.Models;
using DreamAssembler.Core.Services;

namespace DreamAssembler.Core.Tests.Services;

/// <summary>
/// Содержит тесты для анализа набора данных.
/// </summary>
public sealed class DataSetAnalyzerTests
{
    /// <summary>
    /// Проверяет, что анализатор находит отсутствие покрытия по slot.
    /// </summary>
    [Fact]
    public void Analyze_ReturnsError_WhenTemplateRequiresMissingSlot()
    {
        var bundle = new GeneratorDataBundle
        {
            DictionaryEntries =
            [
                new DictionaryEntry
                {
                    Id = "character_1",
                    Text = "персонаж",
                    Category = "character",
                    Slot = "character_subject",
                    Weight = 1.0
                }
            ],
            Templates =
            [
                new TemplateDefinition
                {
                    Id = "template_1",
                    Text = "{character}",
                    Mode = GenerationMode.Sentence,
                    RequiredCategories = ["character"],
                    SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        ["character"] = "character_predicate"
                    },
                    Weight = 1.0
                }
            ]
        };

        var report = new DataSetAnalyzer().Analyze(bundle);

        Assert.True(report.HasErrors);
        Assert.Contains(report.Issues, issue => issue.Code == "missing-slot-coverage");
    }

    /// <summary>
    /// Проверяет, что анализатор находит отсутствие прилагательных для словесных режимов.
    /// </summary>
    [Fact]
    public void Analyze_ReturnsError_WhenAssociationAdjectivesAreMissing()
    {
        var bundle = new GeneratorDataBundle
        {
            AssociationFragments =
            [
                new AssociationFragmentEntry
                {
                    Id = "noun_m_archive",
                    Text = "архив",
                    Kind = "noun_m",
                    Weight = 1.0
                }
            ]
        };

        var report = new DataSetAnalyzer().Analyze(bundle);

        Assert.True(report.HasErrors);
        Assert.Contains(report.Issues, issue => issue.Code == "missing-association-adjectives");
    }

    /// <summary>
    /// Проверяет, что анализатор предупреждает об отсутствии глаголов для режима нескольких слов.
    /// </summary>
    [Fact]
    public void Analyze_ReturnsWarning_WhenAssociationVerbsAreMissing()
    {
        var bundle = new GeneratorDataBundle
        {
            AssociationFragments =
            [
                new AssociationFragmentEntry
                {
                    Id = "noun_m_archive",
                    Text = "архив",
                    Kind = "noun_m",
                    Weight = 1.0
                },
                new AssociationFragmentEntry
                {
                    Id = "adjective_m_tihii",
                    Text = "тихий",
                    Kind = "adjective_m",
                    Weight = 1.0
                }
            ]
        };

        var report = new DataSetAnalyzer().Analyze(bundle);

        Assert.Contains(report.Issues, issue => issue.Code == "missing-association-verbs");
    }
}
