using DreamAssembler.Core.Services;

namespace DreamAssembler.Core.Tests.Services;

/// <summary>
/// Содержит тесты для загрузки manifest набора данных.
/// </summary>
public sealed class DataSetManifestRepositoryTests
{
    /// <summary>
    /// Проверяет загрузку валидного manifest-файла.
    /// </summary>
    [Fact]
    public void Load_ReturnsManifest_WhenJsonIsValid()
    {
        var repository = new DataSetManifestRepository();
        var directoryPath = CreateTempDirectory();
        var filePath = Path.Combine(directoryPath, "data-manifest.json");

        try
        {
            File.WriteAllText(
                filePath,
                """
                {
                  "id": "test-pack",
                  "version": "1.2.3",
                  "description": "Тестовый набор.",
                  "dictionarySets": ["character/city_workers"]
                }
                """);

            var manifest = repository.Load(filePath);

            Assert.NotNull(manifest);
            Assert.Equal("test-pack", manifest.Id);
            Assert.Equal("1.2.3", manifest.Version);
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

