using DreamAssembler.Core.Enums;
using DreamAssembler.Core.Services;

namespace DreamAssembler.Core.Tests.Services;

/// <summary>
/// Содержит тесты для загрузки шаблонов.
/// </summary>
public sealed class TemplateRepositoryTests
{
    /// <summary>
    /// Проверяет использование fallback-данных при отсутствии файла шаблонов.
    /// </summary>
    [Fact]
    public void Load_UsesFallback_WhenFileDoesNotExist()
    {
        var repository = new TemplateRepository();

        var result = repository.Load(Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.json"));

        Assert.True(result.UsedFallback);
        Assert.NotEmpty(result.Data);
        Assert.Contains("fallback", result.Message, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Проверяет загрузку валидного файла шаблонов.
    /// </summary>
    [Fact]
    public void Load_ReturnsTemplates_WhenJsonIsValid()
    {
        var repository = new TemplateRepository();
        var directoryPath = CreateTempDirectory();
        var filePath = Path.Combine(directoryPath, "templates.json");

        try
        {
            File.WriteAllText(
                filePath,
                """
                {
                  "templates": [
                    {
                      "id": "test_template",
                      "text": "{character} идет домой.",
                      "mode": "Sentence",
                      "requiredCategories": ["character"],
                      "tags": ["test"],
                      "minAbsurdity": 0,
                      "maxAbsurdity": 3,
                      "weight": 1.0
                    }
                  ]
                }
                """);

            var result = repository.Load(filePath);

            Assert.False(result.UsedFallback);
            Assert.Single(result.Data);
            Assert.Equal("test_template", result.Data[0].Id);
            Assert.Equal(GenerationMode.Sentence, result.Data[0].Mode);
        }
        finally
        {
            Directory.Delete(directoryPath, true);
        }
    }

    private static string CreateTempDirectory()
    {
        var directoryPath = Path.Combine(Path.GetTempPath(), $"dreamassembler-tests-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directoryPath);
        return directoryPath;
    }
}

