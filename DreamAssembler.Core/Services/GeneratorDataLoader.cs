using DreamAssembler.Core.Models;

namespace DreamAssembler.Core.Services;

/// <summary>
/// Загружает комплект данных генератора из файловой системы.
/// </summary>
public sealed class GeneratorDataLoader
{
    private readonly DictionaryRepository _dictionaryRepository;
    private readonly TemplateRepository _templateRepository;

    /// <summary>
    /// Создает загрузчик данных генератора.
    /// </summary>
    /// <param name="dictionaryRepository">Репозиторий словарей.</param>
    /// <param name="templateRepository">Репозиторий шаблонов.</param>
    public GeneratorDataLoader(DictionaryRepository dictionaryRepository, TemplateRepository templateRepository)
    {
        _dictionaryRepository = dictionaryRepository;
        _templateRepository = templateRepository;
    }

    /// <summary>
    /// Загружает словари и шаблоны по указанному корневому пути данных.
    /// </summary>
    /// <param name="dataRootPath">Корневая папка `Data`.</param>
    /// <returns>Комплект данных генератора.</returns>
    public GeneratorDataBundle Load(string dataRootPath)
    {
        var dictionariesPath = Path.Combine(dataRootPath, "Dictionaries");
        var templatesPath = Path.Combine(dataRootPath, "Templates", "templates.json");

        var dictionaryResult = _dictionaryRepository.Load(dictionariesPath);
        var templateResult = _templateRepository.Load(templatesPath);

        var messages = new List<string>();
        if (!string.IsNullOrWhiteSpace(dictionaryResult.Message))
        {
            messages.Add(dictionaryResult.Message);
        }

        if (!string.IsNullOrWhiteSpace(templateResult.Message))
        {
            messages.Add(templateResult.Message);
        }

        return new GeneratorDataBundle
        {
            DictionaryEntries = dictionaryResult.Data,
            Templates = templateResult.Data,
            UsedFallback = dictionaryResult.UsedFallback || templateResult.UsedFallback,
            StatusMessage = string.Join(" ", messages)
        };
    }
}

