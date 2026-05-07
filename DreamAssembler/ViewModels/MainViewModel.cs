using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
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
    private readonly AppearanceService _appearanceService;
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
        _appearanceService = new AppearanceService();
        Results = [];
        ResultCountOptions = Enumerable.Range(1, 10).ToArray();
        ThemeOptions =
        [
            new OptionItem<AppTheme> { DisplayName = "Теплая светлая", Value = AppTheme.WarmLight },
            new OptionItem<AppTheme> { DisplayName = "Туманная светлая", Value = AppTheme.MistLight },
            new OptionItem<AppTheme> { DisplayName = "Дождливый день", Value = AppTheme.RainyDay },
            new OptionItem<AppTheme> { DisplayName = "Графитовая темная", Value = AppTheme.GraphiteDark },
            new OptionItem<AppTheme> { DisplayName = "Сливовая ночь", Value = AppTheme.PlumNight },
            new OptionItem<AppTheme> { DisplayName = "Архивные сумерки", Value = AppTheme.ArchiveDusk },
            new OptionItem<AppTheme> { DisplayName = "Ночной маршрут", Value = AppTheme.TramNight }
        ];
        ReadingFontOptions =
        [
            new OptionItem<ReadingFontOption> { DisplayName = "Literata", Value = ReadingFontOption.Literata },
            new OptionItem<ReadingFontOption> { DisplayName = "Manrope", Value = ReadingFontOption.Manrope },
            new OptionItem<ReadingFontOption> { DisplayName = "Inter", Value = ReadingFontOption.Inter }
        ];

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
    /// Получает доступные темы оформления.
    /// </summary>
    public IReadOnlyList<OptionItem<AppTheme>> ThemeOptions { get; }

    /// <summary>
    /// Получает доступные встроенные шрифты чтения.
    /// </summary>
    public IReadOnlyList<OptionItem<ReadingFontOption>> ReadingFontOptions { get; }

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
    /// Получает или задает выбранную тему оформления.
    /// </summary>
    [ObservableProperty]
    private OptionItem<AppTheme> selectedTheme = null!;

    /// <summary>
    /// Получает или задает выбранный шрифт чтения.
    /// </summary>
    [ObservableProperty]
    private OptionItem<ReadingFontOption> selectedReadingFont = null!;

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
    /// Получает ширину панели управления.
    /// </summary>
    public GridLength ControlPanelWidth => new(IsLexicalMode ? 248d : 300d);

    /// <summary>
    /// Получает ширину промежутка между панелями.
    /// </summary>
    public GridLength ContentGapWidth => new(IsLexicalMode ? 26d : 18d);

    /// <summary>
    /// Получает максимальную ширину карточки результата для текущего режима.
    /// </summary>
    public double ResultCardMaxWidth => IsLexicalMode ? 760d : 840d;

    /// <summary>
    /// Получает минимальную ширину карточки результата для текущего режима.
    /// </summary>
    public double ResultCardMinWidth => IsLexicalMode ? 340d : 260d;

    /// <summary>
    /// Получает рекомендуемый размер шрифта основного текста результата.
    /// </summary>
    public double ResultTextFontSize => IsLexicalMode ? 32d : 21d;

    /// <summary>
    /// Получает рекомендуемую высоту строки для основного текста результата.
    /// </summary>
    public double ResultTextLineHeight => IsLexicalMode ? 40d : 30d;

    /// <summary>
    /// Получает заголовок секции результатов.
    /// </summary>
    public string ResultsSectionTitle => IsLexicalMode ? "Фрагменты" : "Результаты";

    /// <summary>
    /// Получает дополнительное пояснение для словесных режимов.
    /// </summary>
    public string ResultsSectionHint => IsLexicalMode
        ? "Короткие фразы лучше читать как отдельные найденные обрывки."
        : string.Empty;

    /// <summary>
    /// Получает подпись для выделенного фрагмента в словесных режимах.
    /// </summary>
    public string LexicalSpotlightTitle => "Выбранный фрагмент";

    /// <summary>
    /// Получает подпись для списка истории в словесных режимах.
    /// </summary>
    public string ResultsListCaption => IsLexicalMode ? "Остальные фрагменты" : string.Empty;

    /// <summary>
    /// Получает reader-facing подпись текущего lexical mood.
    /// </summary>
    public string SelectedLexicalAtmosphereLabel => IsLexicalMode ? SelectedResult?.AtmosphereLabel ?? string.Empty : string.Empty;

    /// <summary>
    /// Получает краткое пояснение текущего lexical mood.
    /// </summary>
    public string SelectedLexicalAtmosphereDescription => IsLexicalMode ? SelectedResult?.AtmosphereDescription ?? string.Empty : string.Empty;

    /// <summary>
    /// Получает признак наличия локального lexical mood у выбранного фрагмента.
    /// </summary>
    public bool HasSelectedLexicalAtmosphere => IsLexicalMode && !string.IsNullOrWhiteSpace(SelectedResult?.AtmosphereLabel);

    /// <summary>
    /// Получает заголовок для fullscreen reading mode.
    /// </summary>
    public string ReadingModeTitle => HasSelectedLexicalAtmosphere
        ? SelectedLexicalAtmosphereLabel
        : SelectedResult?.Header ?? "Фрагмент";

    /// <summary>
    /// Получает строку версии приложения.
    /// </summary>
    public string AppVersion => $"Версия {typeof(MainViewModel).Assembly.GetName().Version}";

    /// <summary>
    /// Получает короткое пояснение для выбранной темы.
    /// </summary>
    public string ThemeDescription => SelectedTheme?.Value switch
    {
        AppTheme.WarmLight => "Теплая светлая тема с мягким песочным акцентом.",
        AppTheme.MistLight => "Светлая прохладная тема с бумажно-мятным характером.",
        AppTheme.RainyDay => "Светлая прохладная тема с дождливым серо-синим настроением.",
        AppTheme.GraphiteDark => "Темная графитовая тема с янтарным акцентом.",
        AppTheme.PlumNight => "Темная сливовая тема с более выразительным цветовым контрастом.",
        AppTheme.ArchiveDusk => "Темная архивная тема с оливково-бумажным оттенком.",
        AppTheme.TramNight => "Темная ночная тема с бирюзовым маршрутным свечением.",
        _ => string.Empty
    };

    /// <summary>
    /// Получает живую preview-строку для шрифта чтения.
    /// </summary>
    public string ReadingFontPreviewText => "Сырая платформа помнит шаги ночной смены.";

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
        OnPropertyChanged(nameof(ControlPanelWidth));
        OnPropertyChanged(nameof(ContentGapWidth));
        OnPropertyChanged(nameof(ResultCardMaxWidth));
        OnPropertyChanged(nameof(ResultCardMinWidth));
        OnPropertyChanged(nameof(ResultTextFontSize));
        OnPropertyChanged(nameof(ResultTextLineHeight));
        OnPropertyChanged(nameof(ResultsSectionTitle));
        OnPropertyChanged(nameof(ResultsSectionHint));
        OnPropertyChanged(nameof(SelectedLexicalAtmosphereLabel));
        OnPropertyChanged(nameof(SelectedLexicalAtmosphereDescription));
        OnPropertyChanged(nameof(HasSelectedLexicalAtmosphere));
        OnPropertyChanged(nameof(ReadingModeTitle));
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

        SelectedTheme = ThemeOptions.FirstOrDefault(item => item.Value == loadResult.Settings.Theme) ?? ThemeOptions[0];
        SelectedReadingFont = ReadingFontOptions.FirstOrDefault(item => item.Value == loadResult.Settings.ReadingFont) ?? ReadingFontOptions[0];
        SelectedMode = Modes.FirstOrDefault(item => item.Value == loadResult.Settings.Mode) ?? Modes[0];
        SelectedAbsurdityLevel = AbsurdityLevels.FirstOrDefault(item => item.Value == loadResult.Settings.AbsurdityLevel) ?? AbsurdityLevels[1];
        ResultCount = Math.Clamp(loadResult.Settings.ResultCount, 1, 10);

        _isRestoringSettings = false;

        _appearanceService.Apply(SelectedTheme.Value, SelectedReadingFont.Value);
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
            Theme = SelectedTheme.Value,
            ReadingFont = SelectedReadingFont.Value,
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

    partial void OnSelectedThemeChanged(OptionItem<AppTheme> value)
    {
        OnPropertyChanged(nameof(ThemeDescription));
        ApplyAppearanceAndSave();
    }

    partial void OnSelectedReadingFontChanged(OptionItem<ReadingFontOption> value)
    {
        ApplyAppearanceAndSave();
    }

    partial void OnSelectedResultChanged(ResultItemViewModel? value)
    {
        OnPropertyChanged(nameof(SelectedLexicalAtmosphereLabel));
        OnPropertyChanged(nameof(SelectedLexicalAtmosphereDescription));
        OnPropertyChanged(nameof(HasSelectedLexicalAtmosphere));
        OnPropertyChanged(nameof(ReadingModeTitle));
    }

    private void ApplyAppearanceAndSave()
    {
        if (_isRestoringSettings)
        {
            return;
        }

        _appearanceService.Apply(SelectedTheme.Value, SelectedReadingFont.Value);
        SaveSettingsIfNeeded();
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
