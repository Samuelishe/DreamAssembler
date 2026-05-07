using System.Text.Json;
using DreamAssembler.Core.Models;

namespace DreamAssembler.Core.Services;

/// <summary>
/// Загружает manifest набора данных из JSON-файла.
/// </summary>
public sealed class DataSetManifestRepository
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Загружает manifest из указанного файла.
    /// </summary>
    /// <param name="filePath">Путь к manifest-файлу.</param>
    /// <returns>Загруженный manifest или <see langword="null"/>, если файл отсутствует или некорректен.</returns>
    public DataSetManifest? Load(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            return null;
        }

        try
        {
            var content = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<DataSetManifest>(content, _jsonOptions);
        }
        catch (IOException)
        {
            return null;
        }
        catch (UnauthorizedAccessException)
        {
            return null;
        }
        catch (JsonException)
        {
            return null;
        }
    }
}

