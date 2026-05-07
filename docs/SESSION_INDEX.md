# Session Index

Этот файл нужен как быстрый вход в проект для новых сессий и новых агентов.

## Читать в таком порядке

1. `docs/SESSION_INDEX.md` - быстрый индекс.
2. `docs/PROJECT_STATE.md` - где остановились, что уже сделано, что делать дальше.
3. `docs/IDEAS_BACKLOG.md` - идеи, которые еще не стали планом.
4. `docs/ARCHITECTURE.md` - если нужна структура приложения.
5. `docs/DICTIONARY_FORMAT.md` - если работа связана с данными.
6. `docs/WORKLOG.md` - только если нужен журнал изменений.

## Текущее состояние

- MVP-каркас создан.
- Решение разделено на `DreamAssembler.Core` и `DreamAssembler.App`.
- Для ядра добавлен отдельный тестовый проект `DreamAssembler.Core.Tests`.
- Генерация работает через JSON-словари, шаблоны и fallback-данные.
- Наборы словарей и шаблонов уже расширены относительно стартового MVP.
- WPF UI уже умеет генерировать, копировать и очищать историю.
- Проект собирается без ошибок.

## Важные точки входа в код

- `DreamAssembler.Core/Services/TextGeneratorService.cs`
- `DreamAssembler.Core/Services/DictionaryRepository.cs`
- `DreamAssembler.Core/Services/TemplateRepository.cs`
- `DreamAssembler.Core.Tests/Services/*`
- `DreamAssembler/ViewModels/MainViewModel.cs`
- `DreamAssembler/MainWindow.xaml`

## Правило обновления документации

После каждого значимого шага обновлять:

1. `docs/PROJECT_STATE.md`
2. `docs/WORKLOG.md`
3. при необходимости `docs/IDEAS_BACKLOG.md`

## Что не нужно читать сразу

- весь `WORKLOG` целиком;
- все JSON-словари;
- все документы в `docs`, если задача локальная и уже понятна по индексу.
