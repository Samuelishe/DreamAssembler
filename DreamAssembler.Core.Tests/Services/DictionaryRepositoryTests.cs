using DreamAssembler.Core.Services;

namespace DreamAssembler.Core.Tests.Services;

/// <summary>
/// Содержит тесты для загрузки словарей.
/// </summary>
public sealed class DictionaryRepositoryTests
{
    /// <summary>
    /// Проверяет использование fallback-данных при отсутствии папки словарей.
    /// </summary>
    [Fact]
    public void Load_UsesFallback_WhenDirectoryDoesNotExist()
    {
        var repository = new DictionaryRepository();

        var result = repository.Load(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));

        Assert.True(result.UsedFallback);
        Assert.NotEmpty(result.Data);
        Assert.Contains("fallback", result.Message, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Проверяет загрузку валидного словаря из JSON-файла.
    /// </summary>
    [Fact]
    public void Load_ReturnsEntries_WhenJsonIsValid()
    {
        var repository = new DictionaryRepository();
        var directoryPath = CreateTempDirectory();

        try
        {
            File.WriteAllText(
                Path.Combine(directoryPath, "characters.json"),
                """
                {
                  "entries": [
                    {
                      "id": "test_character",
                      "text": "тестовый персонаж",
                      "category": "character",
                      "tags": ["test"],
                      "absurdity": 1,
                      "weight": 1.0
                    }
                  ]
                }
                """);

            var result = repository.Load(directoryPath);

            Assert.False(result.UsedFallback);
            Assert.Single(result.Data);
            Assert.Equal("test_character", result.Data[0].Id);
        }
        finally
        {
            Directory.Delete(directoryPath, true);
        }
    }

    /// <summary>
    /// Проверяет рекурсивную загрузку словарей из вложенных папок.
    /// </summary>
    [Fact]
    public void Load_ReadsNestedJsonFiles_WhenDirectoryTreeIsUsed()
    {
        var repository = new DictionaryRepository();
        var directoryPath = CreateTempDirectory();
        var nestedPath = Path.Combine(directoryPath, "character");
        Directory.CreateDirectory(nestedPath);

        try
        {
            File.WriteAllText(
                Path.Combine(nestedPath, "workers.json"),
                """
                {
                  "entries": [
                    {
                      "id": "nested_character",
                      "text": "вложенный персонаж",
                      "category": "character",
                      "slot": "character_subject",
                      "tags": ["test"],
                      "absurdity": 0,
                      "weight": 1.0
                    }
                  ]
                }
                """);

            var result = repository.Load(directoryPath);

            Assert.False(result.UsedFallback);
            Assert.Single(result.Data);
            Assert.Equal("character_subject", result.Data[0].Slot);
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
