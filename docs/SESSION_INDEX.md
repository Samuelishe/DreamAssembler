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
- atmospheric dream-core engine.

## Ключевая философия

- controlled weirdness;
- atmospheric coherence;
- emotional texture;
- curated semantic space;
- procedural melancholy;
- quiet absurdity;
- dream-like logic;
- atmosphere over plot;
- reader-first presentation;
- emotional coherence over logical coherence;
- symbolic motifs over raw randomness;
- semantic density over verbosity.

## Текущее состояние

- MVP-каркас уже рабочий и собирается без ошибок.
- Core построен вокруг шаблонов, JSON-словарей, тегов, `slot`, `compat:*` и anti-repeat логики.
- `ShortText` уже использует композиционные роли и базовый narrative rhythm.
- WPF UI уже движется в reader-first сторону: кастомный chrome, темы, встроенные шрифты, крупнее текст, спокойнее карточки.
- UI должен ощущаться не как панель управления генератором, а как atmospheric reading instrument: strange literary machine, quiet reading surface, procedural literature device.
- Основная сила проекта уже сейчас не в сюжетности, а в atmosphere, emotional consistency и symbolic language.
- Текущий archive / transport / bureaucracy слой - это первый strongest coherent atmospheric field проекта, а не его конечная форма.

## Какие manifolds уже реально есть

Это важно для будущих сессий: ниже не идеи, а уже существующие data-fields, которые реально лежат в JSON-наборах и попадают в corpus.

- archive / transport / bureaucracy - базовое strongest coherent field;
- nocturnal airports / fluorescent insomnia - уже second-wave manifold;
- dead shopping malls / abandoned commerce / fluorescent afterhours - уже second-wave manifold;
- impossible museums / echo exhibits / ceremonial curation - уже second-wave manifold;
- recursive hospitality / nocturnal hotels / ceremonial check-in - уже second-wave manifold.

Если нужно быстро понять баланс growth, не гадать по памяти, а запускать `DreamAssembler.DataTools` и смотреть pack-level stats из `data-manifest.json`.

Текущее состояние корпуса на конец этой сессии:

- `data-manifest.json`: `0.5.8`
- corpus: `721` entries
- базовый workflow контроля: `DreamAssembler.DataTools` + `samples`

## Главные направления

- atmosphere over complexity;
- coherence over randomness;
- symbolic consistency over raw variety;
- semantic density over word count;
- emotional texture over "smartness";
- deeper atmosphere over bigger feature-count.

## Как мыслить expansion

Не через жанровые режимы вроде `horror`, `sci-fi`, `fantasy` или `cyberpunk`.

А через atmospheric semantic spaces:

- endless transit;
- fluorescent insomnia;
- impossible domesticity;
- sacred machinery;
- procedural ritual;
- recursive hospitality;
- exhausted utopia;
- abandoned commerce;
- synthetic loneliness;
- ritualized infrastructure;
- biological infrastructure;
- impossible bureaucracy;
- ceremonial administration;
- archival divinity;
- quiet cosmic isolation;
- decaying luxury;
- provincial futurism;
- underwater transport systems;
- impossible museums;
- nocturnal airports;
- dead shopping malls;
- emotional automation;
- dreamlike logistics;
- silent gigantism.

Это не жанры, а emotional-atmospheric manifolds.

## Что может уничтожить charm проекта

- стерилизация текстов;
- попытка сделать идеально правильный русский любой ценой;
- чрезмерная логизация dream-like связок;
- AI-слой поверх rule-based identity;
- enterprise overengineering;
- dashboard-style UI вместо reading tool;
- standard WPF / enterprise-clean controls, если они начинают спорить с текстом и ломают visual silence.

## Важные точки входа в код

- `DreamAssembler.Core/Services/TextGeneratorService.cs`
- `DreamAssembler.Core/Services/DictionaryRepository.cs`
- `DreamAssembler.Core/Services/TemplateRepository.cs`
- `DreamAssembler.Core/Services/AssociationFragmentRepository.cs`
- `DreamAssembler.Core.Tests/Services/*`
- `DreamAssembler/ViewModels/MainViewModel.cs`
- `DreamAssembler/MainWindow.xaml`
- `DreamAssembler/Data/data-manifest.json`

## Где смотреть atmosphere / manifold guidance

- `docs/PROJECT_STATE.md` - product identity, current strengths, current growth direction;
- `docs/ARCHITECTURE.md` - atmosphere architecture, soft coherence, manifolds как architectural growth model;
- `docs/IDEAS_BACKLOG.md` - карта следующих manifolds, risky directions и dataset strategy;
- `DreamAssembler/Data/data-manifest.json` + `DreamAssembler.DataTools` - фактический список уже подключенных packs.

## Что планируется дальше

Если новая сессия продолжает эту же линию, следующий разумный порядок такой:

1. быстро переснять `samples`, если нужен свежий sanity-check после паузы;
2. решить, нужен ли еще один очень короткий semantic-cleanup pass, или gross-defects уже достаточно погашены;
3. если gross-defects не всплывают, возвращаться к expansion, а не застревать в локальной полировке;
4. после следующего expansion снова прогонять `Sentence / Idea / ShortText` через `samples`.

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
