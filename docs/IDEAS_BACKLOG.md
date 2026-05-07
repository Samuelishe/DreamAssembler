# Ideas Backlog

Этот файл хранит не просто список пожеланий, а карту направлений развития проекта с учетом его identity.

DreamAssembler не должен эволюционировать в случайный генератор, utility dashboard или AI-обвязку. Его ценность в другом: curated surrealism, quiet absurdity, reader-first presentation и атмосферная процедурность.

## Core Direction

Это направления, которые считаются ядром проекта и усиливают его характер.

- развивать DreamAssembler как atmospheric procedural text engine, а не как "генератор чего угодно";
- усиливать controlled weirdness вместо голой случайности;
- строить генерацию вокруг atmospheric coherence, emotional texture и symbolic consistency;
- рассматривать выход не как "результат генерации", а как фрагмент эмоционального пространства;
- усиливать quiet surrealism, liminal mood, infrastructural melancholy, bureaucratic dream-logic;
- делать reader-first UI, где текст доминирует над chrome;
- повышать semantic density: меньше пустых слов, больше образной и атмосферной нагрузки;
- развивать curated semantic space вместо простого наращивания количества слов;
- наращивать recurring emotional motifs и symbolic motifs, чтобы проект звучал узнаваемо;
- поддерживать dream-like logic: текст может быть слегка невозможным, если он атмосферно убедителен.

## Short-Term

Это ближайшие зрелые шаги, которые можно делать без ломки архитектуры.

- ввести fullscreen reading mode для фокусного чтения одной фразы или короткого текста;
- усилить typography-first presentation: крупнее текст, спокойнее вторичный UI, мягче иерархия;
- увести карточки результатов в более reader-first вид: меньше визуального шума, больше воздуха;
- скрывать часть действий в hover и context menu, чтобы короткие фразы не выглядели перегруженными;
- добавить мягкие fade-переходы и более спокойную смену состояний выдачи;
- развивать atmospheric palettes и visual reading modes без превращения UI в декоративный арт-объект;
- усилить anti-generic filtering для данных: вычищать слабые, безобразные и канцелярские фразы без образа;
- продолжать curated-cleanup JSON-словарей по местам, состояниям, объектам и концептам;
- расширять тихие и liminal-наборы данных: архив, транспорт, дождь, поздний город, провинциальная инфраструктура;
- усиливать template rhythm и narrative rhythm в `ShortText`, чтобы тексты лучше "дышали";
- уменьшать долю слишком прямолинейных meta-фраз и каркасных конструкций;
- точнее разводить словари по slot-подтипам там, где это добавляет атмосферную связность;
- дополнять `compat:*` не ради логической строгости, а ради мягкой эмоционально-смысловой совместимости.
- постепенно расширять curated free font set для чтения и заголовков, но только через редкий отбор по кириллице, mood fit и читаемости в WPF.

## Medium-Term

Это следующий слой развития после стабилизации ближайших UI и data-улучшений.

- ввести atmospheric thematic modes как curated emotional spaces, а не как жанры;
- добавить atmosphere tags поверх текущих тегов: `rainy`, `archive`, `bureaucratic`, `provincial`, `night_shift`, `urban_decay`, `transport`, `anxious`, `dreamlike`;
- развить narrative temperature как мягкий вектор состояния текста;
- ввести atmosphere inheritance между шаблоном, местом, состоянием и концептом;
- ввести semantic gravity: некоторые выбранные элементы должны слегка притягивать совместимые по тону выборки;
- развивать weighted emotional drift вместо жестких правил связности;
- ввести atmosphere clusters для curated-наборов и фильтрации выдачи;
- развивать emotional topology: где текст начинается, в какую сторону дрейфует и где эмоционально оседает;
- добавить экспорт/share image cards для красивой публикации одной фразы или короткого текста;
- развивать отдельные визуальные режимы чтения: дневной, туманный, ночной, архивный;
- подготовить curated thematic subsets для словесных режимов вместо опоры на сырой большой CSV-пул;
- добавить более тонкое разделение на safe/charged/rarely surreal шаблоны.
- постепенно расширять набор тем оформления еще на несколько спокойных, distinct-палитр без превращения настроек в свалку похожих вариантов.

## Experimental

Это направления, которые могут сильно усилить charm проекта, но требуют аккуратной проверки на реальных выдачах.

- soft atmosphere system поверх текущих тегов и слотов без полной смены архитектуры;
- atmospheric inheritance через легкие веса, а не через жесткие графы зависимостей;
- semantic drift внутри `ShortText`, когда поздние предложения слегка смещаются в сторону уже возникшего настроения;
- symbolic recurrence: редкое повторное появление мотивов вроде архивов, дождя, расписаний, чеков, платформ, почты;
- narrative temperature presets как пользовательский выбор атмосферы, а не жанра;
- extremely minimal reading UI, где служебные элементы почти исчезают во время чтения;
- phrase-centered layout для сверхкоротких режимов;
- отдельные curated output modes наподобие `тихо`, `промышленно`, `дождливо`, `служебно`, `провинциально`, `ночная смена`;
- мягкие морфологические обходы позже, только если они помогают ритму и не стерилизуют текст.

## Risky Directions

Это допустимые, но опасные направления. Их можно трогать только после проверки, что charm не падает.

- слишком активное усиление логической совместимости между всеми категориями;
- агрессивная чистка сюрреалистических сочетаний, которая делает текст "правильнее", но беднее;
- расширение шаблонной системы до слишком строгой narrative machine;
- попытка сделать `ShortText` слишком сюжетным и причинно-объяснимым;
- перегрузка UI декоративными эффектами, которые мешают чтению;
- избыточное количество тем и шрифтов без curated-отбора;
- слишком широкий импорт внешних слов без ручной атмосферной фильтрации;
- жесткие atmosphere-профили, которые убивают живой дрейф текста;
- форсированная морфологическая корректность любой ценой.

## Dangerous Overengineering Ideas

Это направления, которые сейчас противоречат identity проекта и должны рассматриваться как нежелательные.

- LLM, embeddings, vector database, online-generation, AI storytelling;
- облачные функции, collaborative cloud, пользовательские аккаунты;
- database-first архитектура для MVP;
- plugin framework раньше реальной необходимости;
- попытка строить "универсальный движок историй" вместо atmospheric engine;
- enterprise-grade configuration matrix для каждого аспекта генерации;
- сложные rule graphs и state machines, которые делают систему хрупкой и непрозрачной;
- полная формальная модель русского языка внутри проекта;
- metrics-driven optimization на количество вариативности в ущерб тону;
- превращение интерфейса в settings-heavy dashboard.

## Guidance For Atmosphere Engine

DreamAssembler стоит постепенно мыслить не как "шаблон плюс слова", а как мягко удерживаемое эмоциональное пространство.

Полезные рабочие идеи:

- `narrative temperature`:
  rainy, quiet, bureaucratic, transport, industrial, melancholic, archive-like, provincial, night-shift, urban decay, anxious, dreamlike;
- `atmosphere inheritance`:
  место, состояние, объект и концепт могут мягко тянуть выдачу в близкую тональность;
- `semantic gravity`:
  некоторые элементы должны иметь больший атмосферный вес и притягивать совместимые фразы;
- `weighted emotional drift`:
  короткий текст может сдвигаться по тону, не теряя общей температуры;
- `atmosphere clusters`:
  curated поднаборы слов и шаблонов по эмоционально-пространственным зонам;
- `soft coherence instead of hard rules`:
  проекту нужна не железная логика, а управляемая атмосферная убедительность.

Важно: это не жанры. Это эмоционально-атмосферные пространства.

## Guidance For Generation Quality

Что считать качеством в DreamAssembler:

- rhythm over correctness;
- image density over verbosity;
- emotional coherence over plot coherence;
- symbolic consistency over raw variety;
- atmosphere over perfect logic;
- soft surreal ambiguity as identity, not as bug.

Что не нужно делать:

- не гнаться за полной логичностью каждой связки;
- не убирать все слегка невозможные semantic combinations;
- не лечить каждую странность как дефект;
- не делать "идеально правильный русский" ценой ритма и настроения;
- не превращать сонную логику в сухую причинность.

Часть charm проекта создается именно тем, что текст звучит как сон, который пытается быть логичным, но не обязан им стать до конца.

## Guidance For Datasets

Большие наборы слов сами по себе не улучшают DreamAssembler. Важнее curated datasets.

Нужно усиливать:

- atmosphere-oriented curation;
- thematic subsets;
- emotional filtering;
- semantic density;
- anti-generic filtering;
- removal of low-image phrases;
- symbolic motifs;
- recurring emotional motifs;
- повторяемые мягкие топосы и мотивы вместо случайного разброса.

Приоритетные atmospheric categories:

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

## Guidance For UI/UX

UI должен ощущаться не как "генератор", а как машина атмосферных фрагментов.

Направления:

- typography-first;
- atmospheric;
- calm;
- cinematic;
- reader-focused;
- minimal chrome;
- focus on phrase;
- softer cards;
- atmospheric spacing;
- visual breathing room;
- phrase-centered layout.

Практический вывод:

- контент должен доминировать над панелями и служебными статусами;
- действия должны становиться тише и реже конкурировать с текстом;
- fullscreen reading mode является естественным следующим шагом;
- hover-only actions и контекстные действия лучше подходят характеру проекта;
- оформление должно поддерживать чтение, а не продавать приложение.

## What Can Kill The Charm

- стерилизация словаря и шаблонов;
- чрезмерная грамматическая полировка;
- логическая сверхнормализация каждой сцены;
- generic writing без образной плотности;
- слишком много "умных" систем поверх слабых данных;
- перегруженный control-heavy интерфейс;
- попытка сделать проект универсальным вместо узнаваемого.

## Why Atmosphere Matters More Than Perfect Logic

DreamAssembler силен не сюжетом и не интеллектом композиции как таковым. Он силен состоянием.

Пользователь запоминает:

- ощущение мокрого архива;
- служебную тревогу пустой станции;
- тихую бессонницу вещей;
- медленную, странно убедительную бюрократию сна.

Если логика становится слишком строгой, проект теряет дрожание, а вместе с ним и identity.
