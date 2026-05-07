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

## Guidance For Atmosphere Architecture

Следующий слой архитектуры стоит развивать мягко, поверх текущего ядра.

Полезные понятия:

- `narrative temperature` - преобладающее эмоциональное состояние фрагмента;
- `atmosphere inheritance` - выбранные элементы слегка тянут дальнейшие выборки в свой тон;
- `semantic gravity` - некоторые сильные образы имеют больший атмосферный вес;
- `weighted emotional drift` - короткий текст может постепенно смещаться по настроению;
- `atmosphere clusters` - curated поднаборы данных для устойчивых emotional spaces;
- `soft coherence instead of hard rules` - связность задается весами и дрейфом, а не тотальной запретительной логикой.

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
- `recursive hospitality / nocturnal hotels / ceremonial check-in` first-wave field.

Это важно, потому что следующие сессии не должны воспринимать manifolds как чисто концептуальную идею. Они уже являются частью реальной generation architecture на data-слое.

Нужная архитектурная цель не "переключение жанров", а:

- atmosphere continuity;
- atmospheric mixing;
- weighted emotional drift;
- symbolic interference;
- atmosphere permeability.

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
