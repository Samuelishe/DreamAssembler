using System.Text.Json;
using DreamAssembler.Core.Models;

namespace DreamAssembler.Core.Services;

/// <summary>
/// Загружает словарные записи из JSON-файлов.
/// </summary>
public sealed class DictionaryRepository
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Загружает все словарные записи из указанной папки.
    /// </summary>
    /// <param name="directoryPath">Путь к папке со словарями.</param>
    /// <returns>Результат загрузки с данными и статусом fallback.</returns>
    public RepositoryLoadResult<IReadOnlyList<DictionaryEntry>> Load(string directoryPath)
    {
        if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
        {
            return new RepositoryLoadResult<IReadOnlyList<DictionaryEntry>>
            {
                Data = FallbackDataProvider.GetDictionaryEntries(),
                UsedFallback = true,
                Message = "Папка словарей не найдена. Использованы встроенные fallback-данные."
            };
        }

        try
        {
            var files = Directory.GetFiles(directoryPath, "*.json", SearchOption.AllDirectories);
            var entries = new List<DictionaryEntry>();

            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                var dataFile = JsonSerializer.Deserialize<DictionaryEntriesFile>(content, _jsonOptions);
                if (dataFile?.Entries is { Count: > 0 })
                {
                    entries.AddRange(dataFile.Entries.Where(IsValid));
                }
            }

            if (entries.Count == 0)
            {
                return new RepositoryLoadResult<IReadOnlyList<DictionaryEntry>>
                {
                    Data = FallbackDataProvider.GetDictionaryEntries(),
                    UsedFallback = true,
                    Message = "Словари пусты или не содержат корректных записей. Использованы fallback-данные."
                };
            }

            return new RepositoryLoadResult<IReadOnlyList<DictionaryEntry>>
            {
                Data = entries,
                UsedFallback = false,
                Message = "Словари успешно загружены."
            };
        }
        catch (IOException)
        {
            return new RepositoryLoadResult<IReadOnlyList<DictionaryEntry>>
            {
                Data = FallbackDataProvider.GetDictionaryEntries(),
                UsedFallback = true,
                Message = "Не удалось прочитать файлы словарей. Использованы fallback-данные."
            };
        }
        catch (UnauthorizedAccessException)
        {
            return new RepositoryLoadResult<IReadOnlyList<DictionaryEntry>>
            {
                Data = FallbackDataProvider.GetDictionaryEntries(),
                UsedFallback = true,
                Message = "Нет доступа к файлам словарей. Использованы fallback-данные."
            };
        }
        catch (JsonException)
        {
            return new RepositoryLoadResult<IReadOnlyList<DictionaryEntry>>
            {
                Data = FallbackDataProvider.GetDictionaryEntries(),
                UsedFallback = true,
                Message = "Файлы словарей повреждены или имеют неверный JSON-формат. Использованы fallback-данные."
            };
        }
    }

    private static bool IsValid(DictionaryEntry entry)
    {
        return !string.IsNullOrWhiteSpace(entry.Id)
               && !string.IsNullOrWhiteSpace(entry.Text)
               && !string.IsNullOrWhiteSpace(entry.Category)
               && entry.Weight > 0d;
    }

    private sealed class DictionaryEntriesFile
    {
        public List<DictionaryEntry> Entries { get; set; } = [];
    }
}
