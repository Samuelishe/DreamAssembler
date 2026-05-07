using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
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
    private readonly IUserSettingsService _userSettingsService;
    private bool _isRestoringSettings;

    /// <summary>
    /// Инициализирует модель представления главного окна.
    /// </summary>
    public MainViewModel()
    {
        Modes =
        [
            new OptionItem<GenerationMode> { DisplayName = "Предложение", Value = GenerationMode.Sentence },
            new OptionItem<GenerationMode> { DisplayName = "Короткий текст", Value = GenerationMode.ShortText },
            new OptionItem<GenerationMode> { DisplayName = "Идея", Value = GenerationMode.Idea },
            new OptionItem<GenerationMode> { DisplayName = "Словосочетание", Value = GenerationMode.WordPair },
            new OptionItem<GenerationMode> { DisplayName = "Несколько слов", Value = GenerationMode.WordCluster }
        ];

        AbsurdityLevels =
        [
            new OptionItem<AbsurdityLevel> { DisplayName = "Нормально", Value = AbsurdityLevel.Normal },
            new OptionItem<AbsurdityLevel> { DisplayName = "Слегка странно", Value = AbsurdityLevel.SlightlyStrange },
            new OptionItem<AbsurdityLevel> { DisplayName = "Абсурдно", Value = AbsurdityLevel.Absurd },
            new OptionItem<AbsurdityLevel> { DisplayName = "Безумно", Value = AbsurdityLevel.Insane }
        ];

        _clipboardService = new ClipboardService();
        _userSettingsService = new UserSettingsService();
        Results = [];
        ResultCountOptions = Enumerable.Range(1, 10).ToArray();

        var dataPath = Path.Combine(AppContext.BaseDirectory, "Data");
        var dataLoader = new GeneratorDataLoader(
            new DictionaryRepository(),
            new TemplateRepository(),
            new DataSetManifestRepository(),
            new AssociationFragmentRepository());
        var dataBundle = dataLoader.Load(dataPath);

        IsFallbackActive = dataBundle.UsedFallback;
        DataStatusMessage = dataBundle.StatusMessage;

        _textGeneratorService = new TextGeneratorService(
            dataBundle.DictionaryEntries,
            dataBundle.Templates,
            dataBundle.AssociationFragments,
            new WeightedRandomSelector(Random.Shared),
            new TemplateEngine());

        ApplySettings(_userSettingsService.Load());

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
    /// Получает допустимые значения количества результатов.
    /// </summary>
    public IReadOnlyList<int> ResultCountOptions { get; }

    /// <summary>
    /// Получает или задает выбранный режим генерации.
    /// </summary>
    [ObservableProperty]
    private OptionItem<GenerationMode> selectedMode = null!;

    /// <summary>
    /// Получает или задает выбранный уровень абсурдности.
    /// </summary>
    [ObservableProperty]
    private OptionItem<AbsurdityLevel> selectedAbsurdityLevel = null!;

    /// <summary>
    /// Получает или задает количество генерируемых результатов.
    /// </summary>
    [ObservableProperty]
    private int resultCount;

    /// <summary>
    /// Получает или задает выбранный результат.
    /// </summary>
    [ObservableProperty]
    private ResultItemViewModel? selectedResult;

    /// <summary>
    /// Получает или задает текст статуса данных.
    /// </summary>
    [ObservableProperty]
    private string dataStatusMessage = string.Empty;

    /// <summary>
    /// Получает или задает текст статуса пользовательских настроек.
    /// </summary>
    [ObservableProperty]
    private string settingsStatusMessage = string.Empty;

    /// <summary>
    /// Получает или задает текст статуса последнего действия пользователя.
    /// </summary>
    [ObservableProperty]
    private string actionStatusMessage = "Действий еще не было.";

    /// <summary>
    /// Получает или задает признак ошибки в последнем действии.
    /// </summary>
    [ObservableProperty]
    private bool isActionError;

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
    /// Получает признак доступности выбора уровня абсурдности для текущего режима.
    /// </summary>
    public bool IsAbsurditySelectionEnabled
        => SelectedMode?.Value != GenerationMode.WordPair
           && SelectedMode?.Value != GenerationMode.WordCluster;

    /// <summary>
    /// Получает пояснение для режима, который не использует уровень абсурдности.
    /// </summary>
    public string AbsurdityModeNote => SelectedMode?.Value is GenerationMode.WordPair or GenerationMode.WordCluster
        ? "В этом режиме уровень абсурдности не влияет на выдачу."
        : string.Empty;

    /// <summary>
    /// Получает признак словесного режима с короткими фразами.
    /// </summary>
    public bool IsLexicalMode => SelectedMode?.Value is GenerationMode.WordPair or GenerationMode.WordCluster;

    /// <summary>
    /// Получает максимальную ширину карточки результата для текущего режима.
    /// </summary>
    public double ResultCardMaxWidth => IsLexicalMode ? 560d : 840d;

    /// <summary>
    /// Получает рекомендуемый размер шрифта основного текста результата.
    /// </summary>
    public double ResultTextFontSize => IsLexicalMode ? 30d : 21d;

    /// <summary>
    /// Получает рекомендуемую высоту строки для основного текста результата.
    /// </summary>
    public double ResultTextLineHeight => IsLexicalMode ? 38d : 30d;

    /// <summary>
    /// Выполняет генерацию результатов по текущим настройкам.
    /// </summary>
    [RelayCommand]
    private void Generate()
    {
        try
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
            ActionStatusMessage = "Генерация завершена успешно.";
            IsActionError = false;
        }
        catch (InvalidOperationException exception)
        {
            ActionStatusMessage = $"Генерация не выполнена: {exception.Message}";
            IsActionError = true;
        }
    }

    /// <summary>
    /// Копирует указанный результат в буфер обмена.
    /// </summary>
    /// <param name="result">Результат для копирования.</param>
    [RelayCommand]
    private void CopyResult(ResultItemViewModel? result)
    {
        if (result is null)
        {
            return;
        }

        try
        {
            _clipboardService.SetText(result.Text);
            ActionStatusMessage = "Результат скопирован в буфер обмена.";
            IsActionError = false;
        }
        catch (COMException)
        {
            ActionStatusMessage = "Не удалось скопировать текст в буфер обмена. Попробуйте еще раз.";
            IsActionError = true;
        }
        catch (ExternalException)
        {
            ActionStatusMessage = "Буфер обмена сейчас недоступен. Попробуйте еще раз.";
            IsActionError = true;
        }
    }

    /// <summary>
    /// Удаляет один результат из текущей истории.
    /// </summary>
    /// <param name="result">Результат для удаления.</param>
    [RelayCommand]
    private void RemoveResult(ResultItemViewModel? result)
    {
        if (result is null)
        {
            return;
        }

        var wasSelected = ReferenceEquals(SelectedResult, result);
        Results.Remove(result);

        if (wasSelected)
        {
            SelectedResult = Results.FirstOrDefault();
        }

        UpdateHistorySummary();
        ActionStatusMessage = "Результат удален из истории.";
        IsActionError = false;
    }

    /// <summary>
    /// Очищает историю результатов текущего запуска.
    /// </summary>
    [RelayCommand]
    private void ClearHistory()
    {
        Results.Clear();
        SelectedResult = null;
        UpdateHistorySummary();
        ActionStatusMessage = "История генераций очищена.";
        IsActionError = false;
    }

    partial void OnSelectedModeChanged(OptionItem<GenerationMode> value)
    {
        OnPropertyChanged(nameof(IsAbsurditySelectionEnabled));
        OnPropertyChanged(nameof(AbsurdityModeNote));
        OnPropertyChanged(nameof(IsLexicalMode));
        OnPropertyChanged(nameof(ResultCardMaxWidth));
        OnPropertyChanged(nameof(ResultTextFontSize));
        OnPropertyChanged(nameof(ResultTextLineHeight));
        SaveSettingsIfNeeded();
    }

    partial void OnSelectedAbsurdityLevelChanged(OptionItem<AbsurdityLevel> value)
    {
        SaveSettingsIfNeeded();
    }

    partial void OnResultCountChanged(int value)
    {
        SaveSettingsIfNeeded();
    }

    private void ApplySettings(SettingsLoadResult loadResult)
    {
        _isRestoringSettings = true;

        SelectedMode = Modes.FirstOrDefault(item => item.Value == loadResult.Settings.Mode) ?? Modes[0];
        SelectedAbsurdityLevel = AbsurdityLevels.FirstOrDefault(item => item.Value == loadResult.Settings.AbsurdityLevel) ?? AbsurdityLevels[1];
        ResultCount = Math.Clamp(loadResult.Settings.ResultCount, 1, 10);

        _isRestoringSettings = false;

        SettingsStatusMessage = loadResult.Message;
    }

    private void SaveSettingsIfNeeded()
    {
        if (_isRestoringSettings)
        {
            return;
        }

        var settings = new AppSettings
        {
            Mode = SelectedMode.Value,
            AbsurdityLevel = SelectedAbsurdityLevel.Value,
            ResultCount = ResultCount
        };

        if (!_userSettingsService.Save(settings))
        {
            SettingsStatusMessage = "Не удалось сохранить пользовательские настройки.";
            return;
        }

        SettingsStatusMessage = "Пользовательские настройки сохранены.";
    }

    private void UpdateHistorySummary()
    {
        if (Results.Count == 0)
        {
            SummaryText = "История пуста.";
            LastGeneratedAtText = "История пуста";
            return;
        }

        SummaryText = $"В истории: {Results.Count}.";
        LastGeneratedAtText = $"Последняя выдача {Results[0].GeneratedAt:HH:mm:ss}";
    }
}
