# Архитектура DreamAssembler

## Слои приложения

### DreamAssembler.Core

Слой бизнес-логики. Не зависит от WPF.

Основные задачи:

- описание моделей генерации;
- загрузка словарей и шаблонов из JSON;
- хранение fallback-данных;
- выбор шаблонов и элементов словаря с учетом весов и уровня абсурдности;
- сборка итогового текста.

### DreamAssembler.App

Слой интерфейса на WPF.

Основные задачи:

- отображение настроек генерации;
- выполнение команд пользователя;
- показ результатов и истории;
- показ понятного статуса, если данные загружены из fallback.

## Основные классы

### Core

- `GenerationMode` - режим генерации.
- `AbsurdityLevel` - уровень странности результата.
- `TextGenerationOptions` - настройки генерации.
- `TextGenerationResult` - результат одной генерации.
- `DictionaryEntry` - запись словаря.
- `TemplateDefinition` - шаблон предложения или идеи.
- `WeightedRandomSelector` - выбор элементов по весу.
- `DictionaryRepository` - загрузка словарей из `Data/Dictionaries`.
- `TemplateRepository` - загрузка шаблонов из `Data/Templates/templates.json`.
- `DataSetManifestRepository` - загрузка manifest набора данных из `Data/data-manifest.json`.
- `TemplateEngine` - подстановка значений в шаблон.
- `TextGeneratorService` - главная точка генерации текста.

### App

- `MainWindow` - главное окно.
- `MainViewModel` - состояние экрана и команды.
- `ResultItemViewModel` - элемент результата в списке.
- `ClipboardService` - копирование текста в буфер обмена.
- `UserSettingsService` - загрузка и сохранение пользовательских настроек.

## Поток данных от кнопки "Сгенерировать" до результата

1. Пользователь нажимает кнопку "Сгенерировать".
2. `GenerateCommand` в `MainViewModel` собирает `TextGenerationOptions`.
3. `MainViewModel` вызывает `TextGeneratorService`.
4. `TextGeneratorService` выбирает подходящий шаблон по режиму и уровню абсурдности.
5. Для каждой категории из шаблона выбирается запись словаря с учетом веса, тегов и близости к выбранному уровню.
6. `TemplateEngine` подставляет найденные значения в шаблон.
7. `MainViewModel` добавляет результат в список выдачи и в историю текущего запуска.
8. Если при генерации или копировании возникает ошибка, `MainViewModel` показывает понятный статус в UI.

## Где лежат словари, шаблоны и настройки

- Словари: `DreamAssembler/Data/Dictionaries/*.json`
- Manifest набора данных: `DreamAssembler/Data/data-manifest.json`
- Шаблоны: `DreamAssembler/Data/Templates/templates.json`
- Пользовательские настройки: `%LocalAppData%/DreamAssembler/settings.json`

## Как расширять генератор

1. Добавить новые JSON-файлы словарей с нужной категорией.
2. Добавить новые шаблоны в `templates.json`.
3. При необходимости добавить новый режим генерации в `GenerationMode`.
4. Для безопасных шаблонных позиций использовать `slot`, а не только общую `category`.
5. Расширить логику весов или тегов в `TextGeneratorService`, не меняя UI-слой.
6. Позже можно добавить редактор словарей как отдельный модуль, не меняя базовую структуру `Core`.
