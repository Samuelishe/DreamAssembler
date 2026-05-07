# Session Index

Этот файл нужен как быстрый вход в проект для новых сессий и новых агентов.

## Читать в таком порядке

1. `docs/SESSION_INDEX.md` - быстрый индекс и философия проекта.
2. `docs/PROJECT_STATE.md` - где проект сейчас и какая у него tonal identity.
3. `docs/IDEAS_BACKLOG.md` - зрелая карта направлений, рисков и запретов.
4. `docs/ARCHITECTURE.md` - структура приложения и guidance по atmosphere architecture.
5. `docs/ROADMAP.md` - актуальные продуктовые приоритеты.
6. `docs/DICTIONARY_FORMAT.md` - если работа связана с данными.
7. `docs/DATA_SOURCES.md` - если задача касается внешних словарей и их происхождения.
8. `docs/VERSIONING.md` - если меняется приложение и нужно понять, как правильно повышать версию.
9. `docs/WORKLOG.md` - только если нужен журнал изменений.

## Что нужно понять сразу

DreamAssembler нельзя воспринимать как:

- random generator;
- AI assistant;
- LLM frontend;
- "генератор бреда".

Его нужно воспринимать как:

- atmospheric procedural text engine;
- curated surrealism engine;
- quiet absurdity engine;
- reader-first creative tool;
- procedural atmosphere system;
- liminal / post-soviet mood machine.

## Ключевая философия

- controlled weirdness;
- atmospheric coherence;
- emotional texture;
- curated semantic space;
- procedural melancholy;
- quiet absurdity;
- dream-like logic;
- atmosphere over plot;
- reader-first presentation.

## Текущее состояние

- MVP-каркас уже рабочий и собирается без ошибок.
- Core построен вокруг шаблонов, JSON-словарей, тегов, `slot`, `compat:*` и anti-repeat логики.
- `ShortText` уже использует композиционные роли и базовый narrative rhythm.
- WPF UI уже движется в reader-first сторону: кастомный chrome, темы, встроенные шрифты, крупнее текст, спокойнее карточки.
- Основная сила проекта уже сейчас не в сюжетности, а в atmosphere, emotional consistency и symbolic language.

## Главные направления

- atmosphere over complexity;
- coherence over randomness;
- symbolic consistency over raw variety;
- semantic density over word count;
- emotional texture over "smartness".

## Что может уничтожить charm проекта

- стерилизация текстов;
- попытка сделать идеально правильный русский любой ценой;
- чрезмерная логизация dream-like связок;
- AI-слой поверх rule-based identity;
- enterprise overengineering;
- dashboard-style UI вместо reading tool.

## Важные точки входа в код

- `DreamAssembler.Core/Services/TextGeneratorService.cs`
- `DreamAssembler.Core/Services/DictionaryRepository.cs`
- `DreamAssembler.Core/Services/TemplateRepository.cs`
- `DreamAssembler.Core/Services/AssociationFragmentRepository.cs`
- `DreamAssembler.Core.Tests/Services/*`
- `DreamAssembler/ViewModels/MainViewModel.cs`
- `DreamAssembler/MainWindow.xaml`

## Правило обновления документации

После каждого значимого шага обновлять:

1. `docs/PROJECT_STATE.md`
2. `docs/WORKLOG.md`
3. при необходимости `docs/IDEAS_BACKLOG.md`
4. при изменении приоритетов `docs/ROADMAP.md`
5. при изменении версии следовать `docs/VERSIONING.md`

## Что не нужно читать сразу

- весь `WORKLOG` целиком;
- все JSON-словари;
- все документы в `docs`, если задача локальная и уже понятна по индексу.
