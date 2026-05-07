using DreamAssembler.Core.Models;

namespace DreamAssembler.Core.Services;

/// <summary>
/// Загружает комплект данных генератора из файловой системы.
/// </summary>
public sealed class GeneratorDataLoader
{
    private readonly DictionaryRepository _dictionaryRepository;
    private readonly TemplateRepository _templateRepository;
    private readonly DataSetManifestRepository _dataSetManifestRepository;
    private readonly AssociationFragmentRepository _associationFragmentRepository;

    /// <summary>
    /// Создает загрузчик данных генератора.
    /// </summary>
    /// <param name="dictionaryRepository">Репозиторий словарей.</param>
    /// <param name="templateRepository">Репозиторий шаблонов.</param>
    /// <param name="dataSetManifestRepository">Репозиторий manifest-файла набора данных.</param>
    /// <param name="associationFragmentRepository">Репозиторий словарей ассоциативного режима.</param>
    public GeneratorDataLoader(
        DictionaryRepository dictionaryRepository,
        TemplateRepository templateRepository,
        DataSetManifestRepository dataSetManifestRepository,
        AssociationFragmentRepository associationFragmentRepository)
    {
        _dictionaryRepository = dictionaryRepository;
        _templateRepository = templateRepository;
        _dataSetManifestRepository = dataSetManifestRepository;
        _associationFragmentRepository = associationFragmentRepository;
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
        var associationFragmentsPath = Path.Combine(dataRootPath, "AssociationWords", "Sources");
        var manifestPath = Path.Combine(dataRootPath, "data-manifest.json");

        var dictionaryResult = _dictionaryRepository.Load(dictionariesPath);
        var templateResult = _templateRepository.Load(templatesPath);
        var associationFragmentResult = _associationFragmentRepository.Load(associationFragmentsPath);
        var manifest = _dataSetManifestRepository.Load(manifestPath);

        var messages = new List<string>();
        if (!string.IsNullOrWhiteSpace(dictionaryResult.Message))
        {
            messages.Add(dictionaryResult.Message);
        }

        if (!string.IsNullOrWhiteSpace(templateResult.Message))
        {
            messages.Add(templateResult.Message);
        }

        if (!string.IsNullOrWhiteSpace(associationFragmentResult.Message))
        {
            messages.Add(associationFragmentResult.Message);
        }

        if (manifest is not null)
        {
            messages.Add($"Подключен набор данных '{manifest.Id}' версии {manifest.Version}.");
        }

        return new GeneratorDataBundle
        {
            DictionaryEntries = dictionaryResult.Data,
            Templates = templateResult.Data,
            AssociationFragments = associationFragmentResult.Data,
            UsedFallback = dictionaryResult.UsedFallback || templateResult.UsedFallback || associationFragmentResult.UsedFallback,
            StatusMessage = string.Join(" ", messages),
            Manifest = manifest
        };
    }
}
