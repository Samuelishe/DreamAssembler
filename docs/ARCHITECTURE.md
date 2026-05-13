# Архитектура DreamAssembler

## Архитектурный принцип

DreamAssembler - это не универсальный story engine и не система для "умной генерации" ради самой сложности.

Текущая архитектура должна оставаться:

- controllable;
- data-driven;
- rule-based;
- мягкой по связности;
- прозрачной для ручной curated-настройки.

Главная цель архитектуры - не максимальная формальная правильность, а управляемое создание атмосферных текстовых фрагментов.

Важно: текущий archive / transport / bureaucracy слой - это первый strongest coherent field, который уже хорошо собран архитектурно. Но архитектура не должна закреплять проект только в нем. Ее задача - позволить постепенно выращивать новые atmospheric manifolds без перехода к жанровым режимам и без тяжелых систем.

Следующее важное уточнение: старый `archive / bureaucracy / city / transport / rainy melancholy` слой больше нельзя считать нейтральным foundation. Он уже ведет себя как отдельный dominant manifold и поэтому должен постепенно перестать быть скрытым baseline всей системы.

## Как смотреть на генерацию

Базовая модель проекта уже не сводится к "шаблон плюс слова".

Практически она ближе к:

- template-guided atmosphere assembly;
- curated semantic selection;
- soft coherence system;
- procedural emotional space.

То есть шаблон остается опорой, но качество рождается из сочетания:

- slot-совместимости;
- тегов;
- compat-совместимости;
- весов;
- anti-repeat логики;
- narrative-композиции;
- curated словарного материала.

Следующий шаг понимания: генерация должна мыслиться как работа с несколькими emotional-symbolic spaces, которые могут удерживаться, смешиваться и мягко проникать друг в друга.

## Слои приложения

### DreamAssembler.Core

Слой бизнес-логики. Не зависит от WPF.

Основные задачи:

- описание моделей генерации;
- загрузка словарей и шаблонов из JSON;
- хранение fallback-данных;
- выбор шаблонов и элементов словаря с учетом весов, тегов, absurdity, slot и compat;
- сборка итогового текста;
- мягкое удержание композиции и атмосферы в пределах текущих правил.

### DreamAssembler

Слой интерфейса на WPF.

Основные задачи:

- отображение настроек генерации;
- организация reader-first сценария;
- выполнение команд пользователя;
- показ результатов и истории;
- управление визуальной подачей текста;
- показ статусов данных, настроек и действий.

### DreamAssembler.DataTools

Консольный инструмент для валидации, статистики и контроля качества наборов данных.

## Основные классы

### Core

- `GenerationMode` - режим генерации.
- `AbsurdityLevel` - уровень странности результата.
- `TextGenerationOptions` - настройки генерации.
- `TextGenerationResult` - результат одной генерации.
- `DictionaryEntry` - запись словаря.
- `TemplateDefinition` - шаблон предложения, короткого текста или идеи.
- `WeightedRandomSelector` - выбор элементов по весу.
- `DictionaryRepository` - загрузка словарей из `Data/Dictionaries`.
- `TemplateRepository` - загрузка шаблонов из `Data/Templates/templates.json`.
- `DataSetManifestRepository` - загрузка manifest набора данных из `Data/data-manifest.json`.
- `AssociationFragmentRepository` - загрузка и фильтрация больших CSV-словарей слов для режимов `Словосочетание` и `Несколько слов`.
- `TemplateEngine` - подстановка значений.
- `TextGeneratorService` - главная точка генерации текста, где сейчас уже живут soft coherence, narrative-role ограничения, anti-repeat и словесные режимы.

### App

- `MainWindow` - главное окно и текущая reader-facing структура интерфейса.
- `MainViewModel` - состояние экрана, команды, настройки, summary и presentation-параметры.
- `ResultItemViewModel` - элемент результата в списке.
- `ClipboardService` - копирование текста в буфер обмена.
- `UserSettingsService` - загрузка и сохранение пользовательских настроек.
- `AppearanceService` - применение тем и шрифтов чтения.

## Поток данных от кнопки "Сгенерировать" до результата

1. Пользователь нажимает кнопку "Сгенерировать".
2. `GenerateCommand` в `MainViewModel` собирает `TextGenerationOptions`.
3. `MainViewModel` вызывает `TextGeneratorService`.
4. `TextGeneratorService` выбирает путь генерации по режиму.
5. Для `Sentence`, `Idea` и `ShortText` сервис выбирает шаблон с учетом режима, absurdity, анти-повторов и роли в коротком тексте.
6. Для `ShortText` сервис дополнительно следит за narrative rhythm через `compositionRole`.
7. Для каждой категории сервис выбирает словарную запись с учетом slot, тегов, compat и недавних выборов.
8. Для `Словосочетание` и `Несколько слов` сервис использует отдельный CSV-лексикон и согласует формы через уже выделенные словарные типы.
9. `TemplateEngine` подставляет выбранные значения.
10. `MainViewModel` добавляет результат в историю и передает его в текущую reading-подачу UI.
11. Если возникает ошибка, UI показывает понятный статус.

## Где лежат словари, шаблоны и настройки

- словари: `DreamAssembler/Data/Dictionaries/*.json`
- manifest набора данных: `DreamAssembler/Data/data-manifest.json`
- шаблоны: `DreamAssembler/Data/Templates/templates.json`
- CSV-словари словесных режимов: `DreamAssembler/Data/AssociationWords/Sources/*.csv`
- пользовательские настройки: `%LocalAppData%/DreamAssembler/settings.json`

Важно: `data-manifest.json` теперь является не только списком файлов, но и реальным источником истины по составу corpus. Через него и через `DreamAssembler.DataTools` нужно понимать, какие atmospheric packs уже существуют, какие поля еще thin first-wave, а какие уже стали second-wave manifolds.

## Текущая generation architecture

Важно зафиксировать, что генератор уже работает как soft-coherence system.

Сейчас атмосферная связность появляется из комбинации:

- `slot` и `slotRequirements`, которые держат грамматически и позиционно безопасные места;
- тегов шаблонов и словарных записей;
- `compat:*` для `action` / `object` и других потенциально слабых пар;
- absurdity-weighting;
- anti-repeat по шаблонам, категориям и словесным фрагментам;
- композиционных ролей `ShortText`;
- curated подбора самих словарных записей.

То есть уже сейчас архитектура ближе к "мягкой атмосферной сборке", чем к случайному генератору.

Но следующий bottleneck уже не сводится к gross semantic defects. Он смещается в:

- rhythm sameness;
- isolated fragment-feel;
- слишком linear progression внутри серии;
- слабую symbolic recurrence;
- недостаток emotional drift;
- недостаточную глубину manifolds как отдельных emotional ecologies.
- `legacy atmospheric gravity`, где новые manifolds получают новые nouns, но не получают собственную emotional physics;
- слишком world-imagery-heavy foundation, который тянет новые spaces обратно в old urban/archive calibration.

## Guidance For Atmosphere Architecture

Следующий слой архитектуры стоит развивать мягко, поверх текущего ядра.

Полезные понятия:

- `narrative temperature` - преобладающее эмоциональное состояние фрагмента;
- `atmosphere inheritance` - выбранные элементы слегка тянут дальнейшие выборки в свой тон;
- `semantic gravity` - некоторые сильные образы имеют больший атмосферный вес;
- `weighted emotional drift` - короткий текст может постепенно смещаться по настроению;
- `atmosphere clusters` - curated поднаборы данных для устойчивых emotional spaces;
- `soft coherence instead of hard rules` - связность задается весами и дрейфом, а не тотальной запретительной логикой.
- `neutral atmospheric foundation` - базовый слой должен быть скорее procedural physics, чем скрытый urban manifold;
- `manifold-local cadence` - у manifold должны отличаться не только nouns, но и дыхание текста;
- `silence architecture` - часть фрагментов должна строиться вокруг residue, observation и implied procedural state, а не вокруг действия;
- `runtime surfacing audit` - lightweight способ смотреть, какие manifolds и cadence реально доминируют в живой выдаче.

Примеры narrative temperature:

- rainy;
- quiet;
- bureaucratic;
- transport;
- industrial;
- melancholic;
- archive-like;
- provincial;
- night-shift;
- urban decay;
- anxious;
- dreamlike.

Важно: это не жанры, а эмоционально-атмосферные пространства.

Но важно и другое: многие из этих temperature-tags исторически выросли из first-wave urban DreamAssembler. Поэтому следующий architectural step - не только добавлять новые tags, но и отделять universal atmospheric physics от old-world imagery.

Полезные дополнительные manifolds для дальнейшего роста:

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

На текущий момент уже выращены и зафиксированы в данных:

- `archive / transport / bureaucracy` base field;
- `nocturnal airports / fluorescent insomnia` second-wave field;
- `dead shopping malls / abandoned commerce / fluorescent afterhours` second-wave field;
- `impossible museums / echo exhibits / ceremonial curation` second-wave field;
- `recursive hospitality / nocturnal hotels / ceremonial check-in` second-wave field.

Это важно, потому что следующие сессии не должны воспринимать manifolds как чисто концептуальную идею. Они уже являются частью реальной generation architecture на data-слое.

Нужная архитектурная цель не "переключение жанров", а:

- atmosphere continuity;
- atmospheric mixing;
- weighted emotional drift;
- symbolic interference;
- atmosphere permeability.

Следующая цель поверх этого: generation должна начинать ощущаться не как пачка отдельных outputs, а как permeable field, внутри которого fragments мягко резонируют, помнят мотивы и дрейфуют по общему emotional pressure.

При этом dominant manifold должен удерживать не только совместимые `place` и `object`, но и совместимые:

- cadence;
- silence density;
- observation style;
- implication style;
- procedural logic.

## Guidance For Continuity Layer

Следующий слой не должен быть тяжелой memory-system.

Нужен very soft continuity layer поверх уже существующих правил:

- weak motif memory;
- symbolic resurfacing;
- emotional echoes;
- atmospheric resurfacing;
- dominant emotional pressure;
- fragment-to-fragment resonance без literal repetition.

Практически это значит:

- если в серии уже появились ключ, номерок, жетон, стекло, табличка, схема, витрина или аудиогид, более поздние fragments могут получать слабый bias к emotionally compatible objects и procedural details;
- continuity должна работать как atmospheric suggestion, а не как сюжетная обязанность;
- resurfacing должен быть редким и мягким, иначе charm быстро превратится в механический self-reference;
- batch-level pressure полезнее жесткой state machine.
- pressure-layer должен уметь удерживать и silence/pacing, иначе новый manifold останется только noun-overlay.

## Guidance For Rhythm Architecture

Следующая эволюция templates должна идти не только через комбинации категорий, а через cadence classes.

Нужные rhythm families:

- announcement;
- inventory;
- procedural note;
- ceremonial statement;
- museum label;
- quiet instruction;
- interrupted memory;
- static observation;
- delayed revelation;
- emotionally suspended statement;
- almost-poetry fragment.

Важно: rhythm здесь является архитектурным слоем, потому что от него зависит emotional pacing всей серии, а не только красота одной строки.

Следующий шаг - считать cadence таким же manifold-marker, как `place`, `object`, `condition` или strong field-tags.

Примеры manifold-local cadence:

- `weather_systems` - delayed bulletin, pressure report, calibration warning;
- `observatory` - sparse observation, distant notation, silence-heavy procedural loneliness;
- `hydroelectric` - heavy pulse, pressure accumulation, turbine-like reporting;
- `mall` - looping announcement, fluorescent waiting, retail drift;
- `hospitality` - delayed arrival, anonymous service, corridor insomnia;
- `museum` - archival annotation, conservation silence, static residue.

## Guidance For Manifold Ecology

Manifold growth больше нельзя мыслить как "еще один набор слов для темы".

Каждый mature manifold должен получать:

- recurring rituals;
- procedural habits;
- service behavior;
- maintenance logic;
- environmental memory;
- recurring anxieties;
- forms of silence;
- собственный rhythm bias;
- symbolic vocabulary;
- internal dream-rules.

Это особенно важно для уже выращенных `museum`, `mall`, `hospitality` и `airport` полей: их следующий рост должен идти не в breadth, а в emotional ecology depth.

Это же относится и к новым non-urban manifolds: у них уже достаточно dataset-mass, но им не хватает runtime-local cadence и собственной silence/pacing logic.

## Guidance For Foundation Neutralization

Следующий слой архитектуры должен постепенно разделить:

- old-core manifold imagery;
- neutral procedural atmosphere.

Foundation больше не стоит строить вокруг:

- `city`;
- `transport`;
- `bureaucracy`;
- `archive`;
- `paper`;
- `provincial`.

Эти теги не нужно удалять, но их нужно воспринимать как strong old-core field.

Более нейтральный foundation должен тяготеть к:

- `silence`;
- `waiting`;
- `maintenance`;
- `transition`;
- `repetition`;
- `residue`;
- `interval`;
- `distant_sound`;
- `dim_light`;
- `observation`;
- `drift`;
- `hum`;
- `procedural`;
- `routine`;
- `empty_space`.

Это foundation не мира, а atmospheric physics.

## Guidance For Silence Architecture

DreamAssembler уже достаточно силен в action-bearing atmospheric fragments, но следующий рост требует большего числа шаблонов и runtime-biases вокруг:

- static observation;
- unresolved state;
- residue;
- incomplete implication;
- found procedural line;
- impossible document fragment;
- note without explanation;
- atmospheric aftermath.

Иногда фрагмент должен звучать так, будто это строка из документа, найденного без контекста, а не мини-сцена с обязательным действием.

## Guidance For Atmospheric Audit

Следующий tooling-layer не должен превращаться в telemetry dashboard.

Нужен lightweight audit, который помогает смотреть:

- какие manifolds реально surfacing в `Sentence`, `Idea`, `ShortText`;
- какие cadence реально доминируют независимо от manifold;
- какие generic tags слишком универсальны;
- какие baseline-structures слишком часто возвращают old DreamAssembler gravity;
- где новый manifold уже вырос по corpus, но все еще не имеет собственной runtime identity.

## Guidance For Generation Quality

Архитектура не должна тащить проект к стерильной правильности.

Нужно помнить:

- atmosphere важнее grammar perfection;
- coherence важнее randomness, но не должна становиться полицейской логикой;
- dream-like ambiguity является частью продукта;
- слегка невозможные semantic combinations иногда полезнее слишком правильных;
- soft surreal drift лучше, чем полностью рационализированная сцена.

Практически это значит:

- не превращать compat-систему в жесткий валидатор всего;
- не строить тяжелую причинно-следственную модель для `ShortText`;
- не усложнять Core, если проблему можно решить curated-данными;
- не вводить абстракции ради "красивой архитектуры";
- не вводить world simulation, lore encyclopedia или deterministic storytelling engine.

## Guidance For Datasets

С архитектурной точки зрения datasets - это не топливо второго сорта, а половина продукта.

Поэтому:

- curated datasets важнее огромного словаря;
- atmosphere-oriented curation эффективнее слепого масштабирования;
- low-image и low-density элементы должны отсеиваться;
- recurring motifs полезнее случайного разнообразия;
- thematic subsets лучше, чем единый шумный пул.

Приоритетные пространства данных:

- bureaucracy;
- transport;
- industrial;
- rainy;
- archive;
- city-night;
- melancholic urban;
- quiet surreal;
- provincial mystery;
- infrastructural melancholy.

Но этим список не должен ограничиваться. Новые поля стоит выращивать как curated manifolds со своими:

- recurring objects;
- environmental feel;
- emotional logic;
- symbolic vocabulary;
- internal dream-rules.

Также постепенно стоит усиливать non-urban expansion через infrastructure-near-nature spaces:

- observatories;
- hydroelectric infrastructure;
- sanatorium systems;
- procedural weather infrastructure;
- botanical maintenance;
- sleeping ferries;
- reservoir machinery;
- coastal service zones;
- underground research facilities;
- distant transmission stations.

## Guidance For UI Architecture

Текущий WPF-слой уже движется в нужную сторону: кастомный chrome, темы, встроенные шрифты, более крупный текст и карточки результата.

Дальше UI-архитектуру стоит развивать так:

- typography-first;
- content-first;
- minimal chrome;
- fullscreen reading ready;
- calm interaction design;
- action-light presentation;
- softer controls;
- quieter dropdowns;
- less obvious chrome;
- phrase-centered reading surfaces.

Нужный критерий: интерфейс должен служить чтению, а не объяснению, что приложение умеет.

Дополнительный критерий: пользователь должен ощущать не панель управления генератором, а atmospheric reading instrument.

Это значит:

- пустота и breathing room являются допустимыми элементами композиции;
- isolated fragment может быть важнее плотного заполнения окна;
- controls не должны спорить с текстом за внимание;
- fullscreen reading mode должен развиваться как core presentation surface, а не как вторичная модальная функция.

## Чего архитектуре делать не надо

- не превращать проект в LLM frontend;
- не добавлять embeddings, vector database и online-generation;
- не тянуть database-first инфраструктуру;
- не строить plugin framework раньше реальной нужды;
- не создавать отдельные большие подсистемы, если достаточно тегов, весов и curated-наборов;
- не делать "идеально правильный русский" главной целью архитектуры.
