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

    private static TextGeneratorService CreateService()
    {
        var random = new Random(12345);

        return new TextGeneratorService(
            FallbackDataProvider.GetDictionaryEntries(),
            FallbackDataProvider.GetTemplates(),
            new WeightedRandomSelector(random),
            new TemplateEngine());
    }
}

