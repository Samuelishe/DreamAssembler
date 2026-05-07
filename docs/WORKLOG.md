# Журнал изменений

## 2026-05-07

### Что сделано

- создан план первого этапа;
- добавлена структура документации;
- подготовлена архитектура решения с разделением на `DreamAssembler.Core` и `DreamAssembler.App`;
- реализован MVP-генератор, загрузка JSON и fallback-данные;
- добавлен WPF-интерфейс на MVVM;
- добавлены стартовые словари и шаблоны.

### Что сделано после уточнения по ведению проекта

- добавлен быстрый индекс для новых сессий и агентов;
- добавлен отдельный документ с текущим состоянием проекта;
- добавлен backlog идей, которые еще не стали планом;
- добавлен корневой `README.md` для GitHub;
- расширен `.gitignore` под Rider, .NET и служебные артефакты.
- папка `docs` добавлена в `DreamAssembler.sln` как solution folder, чтобы она отображалась в панели Solution.
- исправлен тип solution folder для `docs`, чтобы IDE не пыталась загрузить ее как обычный проект.
- для стабильного отображения в Rider папка `docs` переведена в отдельный нулевой SDK-проект `docs/docs.csproj`, который показывает markdown-файлы без прикладного кода.
- добавлен тестовый проект `DreamAssembler.Core.Tests` с первыми unit-тестами для загрузчиков, шаблонного движка и генератора.
- исправлена загрузка enum-поля `mode` в шаблонах JSON: `TemplateRepository` теперь корректно читает строковые значения через `JsonStringEnumConverter`.
- расширены словари категорий `character`, `action`, `object`, `place`, `twist`, `atmosphere`, `genre`, `style`, `emotion`, `concept`;
- добавлены новые шаблоны для `Sentence`, `Idea` и `ShortText`;
- расширен fallback-набор данных, чтобы улучшения качества работали и при отсутствии JSON-файлов.
- добавлен JSON-сервис пользовательских настроек в `%LocalAppData%/DreamAssembler/settings.json`;
- сохранение режима генерации, уровня абсурдности и количества результатов теперь работает между запусками приложения.
- статусы UI разделены на три зоны: данные, настройки и действия пользователя;
- добавана понятная обработка ошибок генерации и копирования в буфер обмена.
- в `TextGeneratorService` добавлены штрафы за недавние шаблоны и записи, чтобы уменьшить повторы;
- из шаблонов и словоформ убраны несколько небезопасных сочетаний, которые давали заметные ошибки согласования.
- сильно расширены JSON-словари в основных категориях;
- добавлена новая категория `condition` и шаблоны, которые используют ее как безопасный слот для коротких состояний.
- словари перестроены из плоских файлов в дерево тематических наборов;
- добавлен `Data/data-manifest.json` и загрузка manifest в `Core`;
- `DictionaryRepository` теперь читает JSON рекурсивно, а `DictionaryEntry` поддерживает поле `slot`.

### Какие файлы изменены

- `docs/README.md`
- `docs/ARCHITECTURE.md`
- `docs/ROADMAP.md`
- `docs/WORKLOG.md`
- `docs/DICTIONARY_FORMAT.md`
- `docs/SESSION_INDEX.md`
- `docs/PROJECT_STATE.md`
- `docs/IDEAS_BACKLOG.md`
- `docs/PROMPTS_FOR_DATA.md`
- `README.md`
- `DreamAssembler.sln`
- `.gitignore`
- `DreamAssembler.Core.Tests/*`
- `DreamAssembler/DreamAssembler.csproj`
- `DreamAssembler/App.xaml`
- `DreamAssembler/App.xaml.cs`
- `DreamAssembler/Models/AppSettings.cs`
- `DreamAssembler/Models/SettingsLoadResult.cs`
- `DreamAssembler/MainWindow.xaml`
- `DreamAssembler/MainWindow.xaml.cs`
- `DreamAssembler/Services/IUserSettingsService.cs`
- `DreamAssembler/Services/UserSettingsService.cs`
- `DreamAssembler/Data/Dictionaries/*.json`
- `DreamAssembler/Data/Templates/templates.json`
- `DreamAssembler/ViewModels/*`
- `DreamAssembler/Services/*`
- `DreamAssembler/Converters/*`
- `DreamAssembler.Core/*`

### Что проверить

- проект собирается;
- при наличии JSON используются данные из файлов;
- при повреждении или отсутствии JSON отображается fallback-сообщение;
- генерация работает для всех трех режимов;
- копирование и очистка истории работают в UI.
- новые сессии могут быстро понять состояние проекта по `docs/SESSION_INDEX.md`.
- `dotnet test` проходит для первых unit-тестов ядра.
