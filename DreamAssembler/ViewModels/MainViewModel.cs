using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DreamAssembler.App.Models;
using DreamAssembler.App.Services;
using DreamAssembler.Core.Enums;
using DreamAssembler.Core.Models;
using DreamAssembler.Core.Services;

namespace DreamAssembler.App.ViewModels;

/// <summary>
/// Представляет основную модель представления главного окна.
/// </summary>
public partial class MainViewModel : ObservableObject
{
    private readonly TextGeneratorService _textGeneratorService;
    private readonly IClipboardService _clipboardService;

    /// <summary>
    /// Инициализирует модель представления главного окна.
    /// </summary>
    public MainViewModel()
    {
        Modes =
        [
            new OptionItem<GenerationMode> { DisplayName = "Предложение", Value = GenerationMode.Sentence },
            new OptionItem<GenerationMode> { DisplayName = "Короткий текст", Value = GenerationMode.ShortText },
            new OptionItem<GenerationMode> { DisplayName = "Идея", Value = GenerationMode.Idea }
        ];

        AbsurdityLevels =
        [
            new OptionItem<AbsurdityLevel> { DisplayName = "Нормально", Value = AbsurdityLevel.Normal },
            new OptionItem<AbsurdityLevel> { DisplayName = "Слегка странно", Value = AbsurdityLevel.SlightlyStrange },
            new OptionItem<AbsurdityLevel> { DisplayName = "Абсурдно", Value = AbsurdityLevel.Absurd },
            new OptionItem<AbsurdityLevel> { DisplayName = "Безумно", Value = AbsurdityLevel.Insane }
        ];

        SelectedMode = Modes[0];
        SelectedAbsurdityLevel = AbsurdityLevels[1];
        ResultCount = 3;

        _clipboardService = new ClipboardService();
        Results = [];

        var dataPath = Path.Combine(AppContext.BaseDirectory, "Data");
        var dataLoader = new GeneratorDataLoader(new DictionaryRepository(), new TemplateRepository());
        var dataBundle = dataLoader.Load(dataPath);

        IsFallbackActive = dataBundle.UsedFallback;
        StatusMessage = dataBundle.StatusMessage;

        _textGeneratorService = new TextGeneratorService(
            dataBundle.DictionaryEntries,
            dataBundle.Templates,
            new WeightedRandomSelector(Random.Shared),
            new TemplateEngine());

        SummaryText = "Результаты еще не сгенерированы.";
        LastGeneratedAtText = "История пуста";
    }

    /// <summary>
    /// Получает список режимов генерации.
    /// </summary>
    public IReadOnlyList<OptionItem<GenerationMode>> Modes { get; }

    /// <summary>
    /// Получает список уровней абсурдности.
    /// </summary>
    public IReadOnlyList<OptionItem<AbsurdityLevel>> AbsurdityLevels { get; }

    /// <summary>
    /// Получает коллекцию результатов и истории текущего запуска.
    /// </summary>
    public ObservableCollection<ResultItemViewModel> Results { get; }

    /// <summary>
    /// Получает или задает выбранный режим генерации.
    /// </summary>
    [ObservableProperty]
    private OptionItem<GenerationMode> selectedMode;

    /// <summary>
    /// Получает или задает выбранный уровень абсурдности.
    /// </summary>
    [ObservableProperty]
    private OptionItem<AbsurdityLevel> selectedAbsurdityLevel;

    /// <summary>
    /// Получает или задает количество генерируемых результатов.
    /// </summary>
    [ObservableProperty]
    private int resultCount;

    /// <summary>
    /// Получает или задает выбранный результат.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CopyCommand))]
    private ResultItemViewModel? selectedResult;

    /// <summary>
    /// Получает или задает текст статуса данных.
    /// </summary>
    [ObservableProperty]
    private string statusMessage = string.Empty;

    /// <summary>
    /// Получает или задает сводку по текущей выдаче.
    /// </summary>
    [ObservableProperty]
    private string summaryText = string.Empty;

    /// <summary>
    /// Получает или задает строку времени последней генерации.
    /// </summary>
    [ObservableProperty]
    private string lastGeneratedAtText = string.Empty;

    /// <summary>
    /// Получает или задает признак использования fallback-данных.
    /// </summary>
    [ObservableProperty]
    private bool isFallbackActive;

    /// <summary>
    /// Выполняет генерацию результатов по текущим настройкам.
    /// </summary>
    [RelayCommand]
    private void Generate()
    {
        var options = new TextGenerationOptions
        {
            Mode = SelectedMode.Value,
            AbsurdityLevel = SelectedAbsurdityLevel.Value,
            ResultCount = ResultCount
        };

        var generatedResults = _textGeneratorService.Generate(options);

        var headerStartIndex = Results.Count + 1;
        for (var index = 0; index < generatedResults.Count; index++)
        {
            Results.Insert(0, new ResultItemViewModel(generatedResults[index], headerStartIndex + index));
        }

        SelectedResult = Results.FirstOrDefault();
        SummaryText = $"В истории: {Results.Count}. Последняя выдача: {generatedResults.Count}.";
        LastGeneratedAtText = $"Обновлено {DateTime.Now:HH:mm:ss}";
    }

    /// <summary>
    /// Копирует выбранный результат в буфер обмена.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanCopy))]
    private void Copy()
    {
        if (SelectedResult is null)
        {
            return;
        }

        _clipboardService.SetText(SelectedResult.Text);
        StatusMessage = "Выбранный результат скопирован в буфер обмена.";
    }

    /// <summary>
    /// Очищает историю результатов текущего запуска.
    /// </summary>
    [RelayCommand]
    private void ClearHistory()
    {
        Results.Clear();
        SelectedResult = null;
        SummaryText = "История очищена.";
        LastGeneratedAtText = "История пуста";
    }

    private bool CanCopy()
    {
        return SelectedResult is not null;
    }
}
