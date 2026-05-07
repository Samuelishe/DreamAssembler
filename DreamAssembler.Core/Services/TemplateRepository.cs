using System.Text.Json;
using System.Text.Json.Serialization;
using DreamAssembler.Core.Models;

namespace DreamAssembler.Core.Services;

/// <summary>
/// Загружает шаблоны генерации из JSON-файла.
/// </summary>
public sealed class TemplateRepository
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    /// <summary>
    /// Загружает шаблоны из указанного JSON-файла.
    /// </summary>
    /// <param name="filePath">Путь к файлу шаблонов.</param>
    /// <returns>Результат загрузки с данными и статусом fallback.</returns>
    public RepositoryLoadResult<IReadOnlyList<TemplateDefinition>> Load(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            return new RepositoryLoadResult<IReadOnlyList<TemplateDefinition>>
            {
                Data = FallbackDataProvider.GetTemplates(),
                UsedFallback = true,
                Message = "Файл шаблонов не найден. Использованы встроенные fallback-данные."
            };
        }

        try
        {
            var content = File.ReadAllText(filePath);
            var dataFile = JsonSerializer.Deserialize<TemplatesFile>(content, _jsonOptions);
            var templates = dataFile?.Templates?.Where(IsValid).ToList() ?? [];

            if (templates.Count == 0)
            {
                return new RepositoryLoadResult<IReadOnlyList<TemplateDefinition>>
                {
                    Data = FallbackDataProvider.GetTemplates(),
                    UsedFallback = true,
                    Message = "Файл шаблонов пуст или не содержит корректных записей. Использованы fallback-данные."
                };
            }

            return new RepositoryLoadResult<IReadOnlyList<TemplateDefinition>>
            {
                Data = templates,
                UsedFallback = false,
                Message = "Шаблоны успешно загружены."
            };
        }
        catch (IOException)
        {
            return new RepositoryLoadResult<IReadOnlyList<TemplateDefinition>>
            {
                Data = FallbackDataProvider.GetTemplates(),
                UsedFallback = true,
                Message = "Не удалось прочитать файл шаблонов. Использованы fallback-данные."
            };
        }
        catch (UnauthorizedAccessException)
        {
            return new RepositoryLoadResult<IReadOnlyList<TemplateDefinition>>
            {
                Data = FallbackDataProvider.GetTemplates(),
                UsedFallback = true,
                Message = "Нет доступа к файлу шаблонов. Использованы fallback-данные."
            };
        }
        catch (JsonException)
        {
            return new RepositoryLoadResult<IReadOnlyList<TemplateDefinition>>
            {
                Data = FallbackDataProvider.GetTemplates(),
                UsedFallback = true,
                Message = "Файл шаблонов поврежден или имеет неверный JSON-формат. Использованы fallback-данные."
            };
        }
    }

    private static bool IsValid(TemplateDefinition template)
    {
        return !string.IsNullOrWhiteSpace(template.Id)
               && !string.IsNullOrWhiteSpace(template.Text)
               && template.RequiredCategories.Count > 0
               && template.Weight > 0d;
    }

    private sealed class TemplatesFile
    {
        public List<TemplateDefinition> Templates { get; set; } = [];
    }
}
