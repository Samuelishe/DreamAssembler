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

    /// <summary>
    /// Проверяет, что анализатор собирает статистику по JSON-пакам и CSV-источникам из manifest.
    /// </summary>
    [Fact]
    public void Analyze_ReturnsPackStatistics_FromManifestFiles()
    {
        var dataRootPath = Path.Combine(Path.GetTempPath(), $"dreamassembler-analyzer-{Guid.NewGuid():N}");
        Directory.CreateDirectory(Path.Combine(dataRootPath, "Dictionaries", "place"));
        Directory.CreateDirectory(Path.Combine(dataRootPath, "AssociationWords", "Sources"));

        try
        {
            File.WriteAllText(
                Path.Combine(dataRootPath, "Dictionaries", "place", "sample_pack.json"),
                """
                {
                  "entries": [
                    { "id": "place_1", "text": "пустом зале", "category": "place", "slot": "place_in", "weight": 1.0 },
                    { "id": "place_2", "text": "коридоре, где лампы гудели,", "category": "place", "slot": "place_in_clause", "weight": 1.0 }
                  ]
                }
                """);

            File.WriteAllText(
                Path.Combine(dataRootPath, "AssociationWords", "Sources", "sample-nouns.csv"),
                """
                bare	sg_nom	sg_gen	sg_acc	gender	pl_only
                hall	зал	зала	зал	m	0
                lamp	лампа	лампы	лампу	f	0
                """);

            var bundle = new GeneratorDataBundle
            {
                Manifest = new DataSetManifest
                {
                    DictionarySets = ["place/sample_pack"],
                    AssociationSets = ["Sources/sample-nouns.csv"]
                },
                DataRootPath = dataRootPath
            };

            var report = new DataSetAnalyzer().Analyze(bundle);

            var dictionarySet = Assert.Single(report.DictionarySetStats);
            Assert.Equal("place/sample_pack", dictionarySet.SetName);
            Assert.Equal(2, dictionarySet.EntryCount);
            Assert.Equal(2, dictionarySet.CategoryCounts["place"]);
            Assert.Equal(1, dictionarySet.SlotCounts["place_in"]);
            Assert.Equal(1, dictionarySet.SlotCounts["place_in_clause"]);

            var associationSet = Assert.Single(report.AssociationSetStats);
            Assert.Equal("Sources/sample-nouns.csv", associationSet.SetName);
            Assert.Equal("nouns", associationSet.SourceKind);
            Assert.Equal(2, associationSet.RowCount);
        }
        finally
        {
            if (Directory.Exists(dataRootPath))
            {
                Directory.Delete(dataRootPath, true);
            }
        }
    }
}
