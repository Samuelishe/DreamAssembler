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
- `TemplateDefinition` теперь поддерживает `slotRequirements`;
- `TextGeneratorService` начал фильтровать записи по `slot` и учитывать совместимость по тегам между уже выбранными элементами фразы.
- добавлен `DreamAssembler.DataTools` для валидации и статистики наборов данных;
- в `Core` появился `DataSetAnalyzer`, который ищет дубли, пустые слоты, слабые категории и отсутствие покрытия шаблонов по slot.
- по визуальной проверке UI найден дефект: длинные результаты в карточках плохо переносятся по строкам при сужении окна;
- по визуальной проверке генерации зафиксированы оставшиеся проблемы качества: повторяющиеся служебные конструкции и семантически слабые сочетания в `Sentence` и `ShortText`.
- исправлен шаблон списка результатов в `MainWindow.xaml`: карточки теперь растягиваются по ширине списка, горизонтальный скролл отключен, длинный текст должен корректно уходить в multiline при ресайзе окна.
- по следующей визуальной проверке `ShortText` дополнительно зафиксированы новые классы проблем: чрезмерное повторение каркасных предложений, неестественные наречные и оценочные связки, а также местами слабые местоименные отсылки.
- в `TemplateDefinition` добавлено поле `CompositionRole`, а `TextGeneratorService` начал учитывать композиционные роли при сборке `ShortText`;
- `ShortText` больше не должен начинаться с meta-ремарки и не должен набиваться несколькими одинаковыми meta-шаблонами при наличии альтернатив;
- часть `emotion`-данных разведена по slot-подтипам `emotion_group_state` и `emotion_observer_state`;
- обновлены short-text шаблоны и fallback-данные, чтобы снизить количество повторов и убрать часть неестественных связок.
- `twist` разделен на `twist_character_clause` и `twist_general_clause`, чтобы местоименные конструкции выбирались только там, где в шаблоне есть явный персонаж;
- добавлен тест на соблюдение персонального twist-slot и синхронизированы fallback-данные с новой схемой.
- `concept` разделен на `concept_story_frame` и `concept_reflection`, чтобы сюжетные темы не смешивались с рефлексивными смысловыми хвостами;
- добавлен новый short-text шаблон интерпретации и тест на соблюдение reflection-slot для `concept`.
- `condition` разделен на `condition_initial_state`, `condition_scene_detail` и `condition_reveal_state`;
- `TextGeneratorService` получил более строгие правила порядка для narrative-ролей `ShortText`, чтобы late-роли не вылезали слишком рано;
- добавлен новый short-text шаблон сценической детали и тесты на `condition`-slot и порядок narrative-ролей.
- для сложных мест с придаточной частью введен slot `place_in_clause` и отдельные шаблоны, которые ожидают закрывающую запятую после такого места;
- добавлен тест на clause-slot для `place`, чтобы конструкции вида `в дворе, где ... ,` не откатывались назад к сломанной пунктуации.

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
- `DreamAssembler.Core/Models/TemplateDefinition.cs`
- `DreamAssembler.Core/Services/TextGeneratorService.cs`
- `DreamAssembler.Core/Services/FallbackDataProvider.cs`
- `DreamAssembler.Core.Tests/Services/TextGeneratorServiceTests.cs`
- `DreamAssembler/Data/Dictionaries/emotion/predicative_states.json`
- `DreamAssembler/Data/Dictionaries/atmosphere/scene_tones.json`
- `DreamAssembler/Data/Dictionaries/twist/rules_and_reactions.json`
- `DreamAssembler/Data/Dictionaries/concept/story_concepts.json`
- `DreamAssembler/Data/Dictionaries/condition/scene_conditions.json`
- `DreamAssembler/Data/Dictionaries/place/surreal_places.json`
- `DreamAssembler/Data/Templates/templates.json`
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
- длинные результаты корректно переносятся по строкам в узком окне и не обрезаются визуально.
- новые улучшения генерации уменьшают количество повторяющихся вводных фраз и слабых сочетаний.
- `ShortText` не начинается с meta-фразы и не повторяет ее несколько раз в одной генерации при наличии других шаблонов.
- местоименные twist-конструкции не попадают в шаблоны, которые ждут общий безличный twist без явной опоры на персонажа.
- рефлексивные short-text конструкции берут `concept` только из `concept_reflection`, а idea-шаблоны с сюжетной темой используют `concept_story_frame`.
- ранние short-text шаблоны берут только стартовые `condition`, а поздние reveal-шаблоны не используют начальные сценические состояния.
- сложные `place` с придаточной частью попадают только в clause-шаблоны и не ломают запятые в вынесенных позициях.
