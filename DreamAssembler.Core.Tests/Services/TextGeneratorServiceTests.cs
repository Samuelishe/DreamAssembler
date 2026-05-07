using DreamAssembler.Core.Enums;
using DreamAssembler.Core.Models;
using DreamAssembler.Core.Services;

namespace DreamAssembler.Core.Tests.Services;

/// <summary>
/// Содержит тесты для сервиса генерации текста.
/// </summary>
public sealed class TextGeneratorServiceTests
{
    /// <summary>
    /// Проверяет генерацию указанного количества результатов.
    /// </summary>
    [Fact]
    public void Generate_ReturnsRequestedNumberOfSentenceResults()
    {
        var service = CreateService();

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.Sentence,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 3
        });

        Assert.Equal(3, result.Count);
        Assert.All(result, item => Assert.False(string.IsNullOrWhiteSpace(item.Text)));
    }

    /// <summary>
    /// Проверяет генерацию короткого текста из нескольких предложений.
    /// </summary>
    [Fact]
    public void Generate_ReturnsShortText_WithSeveralSentences()
    {
        var service = CreateService();

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.ShortText,
            AbsurdityLevel = AbsurdityLevel.Absurd,
            ResultCount = 1
        });

        var text = Assert.Single(result).Text;
        Assert.Contains('.', text);
        Assert.True(text.Split('.', StringSplitOptions.RemoveEmptyEntries).Length >= 2);
    }

    /// <summary>
    /// Проверяет, что генератор использует slot-требования шаблона.
    /// </summary>
    [Fact]
    public void Generate_UsesSlotRequirements_WhenTemplateDefinesThem()
    {
        var template = new TemplateDefinition
        {
            Id = "slot_test",
            Text = "{condition}.",
            Mode = GenerationMode.Sentence,
            RequiredCategories = ["condition"],
            SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["condition"] = "condition_clause"
            },
            Weight = 1.0
        };

        var entries = new List<DictionaryEntry>
        {
            new() { Id = "condition_ok", Category = "condition", Slot = "condition_clause", Text = "все молчали", Weight = 1.0 },
            new() { Id = "condition_wrong", Category = "condition", Slot = "emotion_predicative", Text = "тихо тревожно", Weight = 100.0 }
        };

        var service = new TextGeneratorService(
            entries,
            [template],
            new WeightedRandomSelector(new Random(1)),
            new TemplateEngine());

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.Sentence,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 1
        });

        Assert.Equal("все молчали.", Assert.Single(result).Text);
    }

    /// <summary>
    /// Проверяет, что короткий текст не начинается с meta-шаблона и не повторяет его несколько раз при наличии альтернатив.
    /// </summary>
    [Fact]
    public void Generate_ShortText_LimitsMetaTemplates_WhenAlternativesExist()
    {
        var templates = new List<TemplateDefinition>
        {
            new()
            {
                Id = "setup",
                Text = "Начало: {condition}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["condition"] = "condition_clause"
                },
                CompositionRole = "setup",
                Weight = 1.0
            },
            new()
            {
                Id = "meta",
                Text = "Если пересказывать это {style}, все звучит почти убедительно.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["style"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["style"] = "style_note"
                },
                CompositionRole = "meta",
                Weight = 10.0
            },
            new()
            {
                Id = "development",
                Text = "Потом оказалось, что {condition}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["condition"] = "condition_clause"
                },
                CompositionRole = "development",
                Weight = 1.0
            }
        };

        var entries = new List<DictionaryEntry>
        {
            new() { Id = "condition_ok", Category = "condition", Slot = "condition_clause", Text = "все молчали", Weight = 1.0 },
            new() { Id = "style_ok", Category = "style", Slot = "style_note", Text = "с серьезной интонацией", Weight = 1.0 }
        };

        var service = new TextGeneratorService(
            entries,
            templates,
            new WeightedRandomSelector(new Random(1)),
            new TemplateEngine(),
            new Random(0));

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.ShortText,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 1
        });

        var text = Assert.Single(result).Text;
        Assert.False(text.StartsWith("Если пересказывать это", StringComparison.Ordinal));
        Assert.True(CountOccurrences(text, "Если пересказывать это") <= 1);
    }

    private static TextGeneratorService CreateService()
    {
        var random = new Random(12345);

        return new TextGeneratorService(
            FallbackDataProvider.GetDictionaryEntries(),
            FallbackDataProvider.GetTemplates(),
            new WeightedRandomSelector(random),
            new TemplateEngine(),
            random);
    }

    private static int CountOccurrences(string text, string value)
    {
        var count = 0;
        var index = 0;

        while ((index = text.IndexOf(value, index, StringComparison.Ordinal)) >= 0)
        {
            count++;
            index += value.Length;
        }

        return count;
    }
}
