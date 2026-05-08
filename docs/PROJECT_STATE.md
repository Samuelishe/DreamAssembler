# Project State

## Где сейчас проект

Проект находится на рабочем MVP-этапе с уже выраженной tonal identity.

При этом проект уже вышел из фазы, где главным bottleneck были gross defects, broken UI feel или полностью случайная выдача. Следующий зрелый этап - сместить фокус с базового polish и простого corpus growth в сторону atmospheric continuity, temporal rhythm, manifold depth и dream-flow.

DreamAssembler - это не utility app, не dashboard и не "генератор бреда". Текущее ядро проекта ближе к atmospheric reading/generation tool: это rule-based система, которая собирает короткие русскоязычные фрагменты с controlled weirdness, emotional texture и curated surrealism.

Его направление:

- atmospheric procedural text engine;
- curated surrealism engine;
- quiet absurdity engine;
- reader-first creative tool;
- atmospheric dream-core engine;
- procedural atmosphere system.

Важно: текущий liminal / post-soviet / archive / transport слой - это первый strongest coherent atmospheric field проекта, а не его окончательная форма.

## Текущая tonal identity

Сильнее всего проект звучит там, где появляются:

- поздний городской быт;
- архивы, станции, депо, почта, подземные переходы, служебные пространства;
- тихая тревога и сдержанная меланхолия;
- infrastructural melancholy;
- dream-like logic без полной потери бытовой опоры;
- symbolic language вместо прямого сюжета;
- quiet surrealism вместо громкого абсурда.

Это не жанровая машина и не engine для "историй обо всем". Ее сила в curated emotional field.

При этом проект не должен зацементироваться только в одном поле. Его следующая зрелость - уметь выращивать и мягко смешивать разные atmospheric semantic spaces, не превращаясь при этом в набор жанровых режимов.

Примеры таких пространств:

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
- observatory loneliness;
- sanatorium bureaucracy;
- hydroelectric infrastructure;
- procedural weather systems;
- emotional automation;
- dreamlike logistics;
- silent gigantism.

## Что уже сделано

- создано решение с проектами:
  - `DreamAssembler.Core`
  - `DreamAssembler`
  - `DreamAssembler.Core.Tests`
  - `DreamAssembler.DataTools`;
- включены nullable reference types;
- добавлены XML-doc комментарии для публичных типов и методов;
- реализованы модели генерации;
- реализованы загрузчики словарей и шаблонов из JSON;
- реализованы fallback-данные;
- реализован генератор с весами и мягким учетом уровня абсурдности;
- реализован WPF UI с выбором режима, уровня абсурдности и количества результатов;
- добавлены команды генерации, копирования и очистки истории;
- добавлено сохранение пользовательских настроек между запусками;
- улучшены статусы UI: отдельно для данных, настроек и действий пользователя;
- исправлено поведение карточек результатов при узкой ширине окна;
- добавлены стартовые JSON-данные;
- добавлен базовый пакет документации;
- добавлены unit-тесты для ядра;
- расширены словари и шаблоны для более разнообразной генерации;
- добавлены анти-повторы по недавним шаблонам и записям;
- убраны несколько небезопасных шаблонных сочетаний, которые часто ломали фразу без морфологии;
- заметно расширены наборы данных по основным категориям;
- добавлена категория `condition` для безопасных коротких состояний и наблюдений;
- словари перестроены в дерево тематических JSON-файлов с manifest-файлом набора данных;
- в модель данных заложен `slot` для безопасных шаблонных позиций;
- шаблоны теперь могут задавать `slotRequirements`, а генератор умеет их соблюдать;
- генератор усиливает совместимость по тегам между уже выбранными элементами одной фразы;
- добавлен консольный инструмент `DreamAssembler.DataTools` для проверки и статистики данных;
- открыт новый non-urban manifold `observatory loneliness` с отдельной ecology: night procedure, weather logging, analog signals, distant light, dome mechanics и instrument silence;
- открыт новый manifold `sanatorium bureaucracy` с отдельной ecology: corridor rest, procedure cards, mineral-water routine, scheduled silence, recovery paperwork и institutional tenderness;
- открыт новый manifold `hydroelectric infrastructure` с отдельной ecology: turbine halls, pressure logs, spillway routine, maintenance tides, reservoir monitoring и industrial water-memory;
- открыт новый manifold `procedural weather systems` с отдельной ecology: night forecasts, warning bulletins, radar strips, forecast rooms, delayed fronts и бюрократия климата;
- текущее числовое состояние manifolds теперь отдельно фиксируется в `docs/MANIFOLD_STATE.md`: это источник истины по pack-balance, relative corpus mass и тому, какие поля пора deepening-усиливать, а какие - сначала просто вытаскивать на поверхность чаще;
- после runtime-среза на `0.6.2` стало ясно, что главный bottleneck сейчас уже не в structural correctness: `dotnet test` проходит полностью, а live generation страдает прежде всего от manifold surfacing imbalance, где `museum` и старые fields все еще слишком сильно доминируют над `observatory`, `sanatorium` и `hydroelectric`;
- затем начат `manifold surfacing balance pass` в Core: новые non-urban fields (`observatory`, `sanatorium`, `hydroelectric`) включены в слой strong-manifold memory и получили ранний surfacing bias, чтобы чаще становиться anchor-field сцены, а не только слабым вторичным акцентом;
- после этого добавлен `ShortText` mid-flow retention layer: opening manifold теперь мягко удерживается и на следующих фразах композиции, так что новые non-urban поля уже могут не только стартовать текст, но и дольше оставаться его dominant local field;
- для `ShortText` введены композиционные роли шаблонов, чтобы снижать повторяемость одинаковых каркасных фраз;
- часть `emotion`-данных разведена по более безопасным slot-подтипам;
- `twist`, `concept` и `condition` разделены на более безопасные slot-подтипы;
- для сложных мест с придаточной частью введен отдельный slot `place_in_clause`;
- для `action` и `object` введен слой смысловой совместимости через `compat:*`;
- начато расширение боевых JSON-словарей в текущей схеме `slot` / `compat` / `composition`;
- добавлены отдельные режимы `Словосочетание` и `Несколько слов`;
- для них подключены внешние CSV-лексиконы;
- главное окно уже частично переработано в сторону reader-first подачи;
- действия над результатом перенесены ближе к карточке и в контекстное меню;
- начат переход на собственный window chrome;
- в проект добавлены встроенные шрифты с кириллицей;
- тема и шрифт чтения стали реальными пользовательскими настройками;
- система тем расширена до нескольких светлых и темных палитр;
- добавлен fullscreen reading mode с фокусом на выбранном результате;
- действия на карточках результатов переведены в более тихий hover-режим;
- для `Словосочетание` и `Несколько слов` начат phrase-centered layout: карточки стали крупнее, центрированнее и спокойнее;
- control panel в словесных режимах стала компактнее и меньше конкурирует с reading surface;
- в lexical modes основной экран теперь тоже поддерживает selected-fragment spotlight: выбранная фраза вынесена вперед, а список ниже стал работать как тихая история;
- исправлено реальное подключение встроенных шрифтов: reading font теперь должен действительно меняться из настроек, а не тихо откатываться в системный fallback;
- в настройки добавлен живой preview-блок шрифта чтения, чтобы смена typography была видна сразу;
- набор тем расширен новыми distinct-палитрами: дождливой светлой, архивной темной и транспортно-ночной;
- смена выбранного lexical-фрагмента теперь подается мягче: spotlight-блок получает короткую fade/slide анимацию;
- в lexical modes история под spotlight сделана тише по opacity и типографике, а у самого spotlight появились быстрые reader-actions;
- начат первый реальный soft-controls pass: основные кнопки, dropdowns, spotlight-секции, карточки результатов и fullscreen reading presentation стали мягче и менее utility-oriented по визуальному тону;
- после этого начат и layout-rhythm pass: левая панель стала тише и компактнее, lexical history ослаблена как chrome-зона, а fullscreen reading mode приблизился к более самостоятельной ritual-reading surface;
- затем начат targeted shell/popup pass: верхний window chrome получил более тихую identity-подпись текущего режима, а settings popup стал меньше напоминать служебное WPF-меню и ближе к calm atmospheric tuning surface;
- затем начат content-facing pass для `Sentence / Idea / ShortText`: numbered result-feel ослаблен, history стала тише, а обычные текстовые режимы подаются ближе к reading fragments, чем к списку generated items;
- после этого выполнен targeted curated data pass по `action / object / place`: сужены слишком широкие `compat`-связки, исправлена проблемная формулировка `place_in_clause` для платформы и ослаблены несколько пар, которые давали пустоватые или неестественные действия на живой выборке;
- затем выполнен короткий output-driven pass по `character` и `ShortText` rhythm: исправлена форма `дежурный по бюро пропусков` и разведены два слишком доминирующих atmospheric-каркаса, чтобы short-text выдача чуть реже повторяла одну и ту же интонацию;
- затем начато controlled expansion вне archive/transport ядра: добавлен первый компактный curated field `nocturnal airports / fluorescent insomnia` по `place / object / condition / concept / atmosphere / twist`, чтобы проект рос не жанрами, а новыми atmospheric manifolds;
- затем добавлен еще один distinct manifold `dead shopping malls / abandoned commerce / fluorescent afterhours`: проект начал выходить и в пространство пустых галерей, закрытых витрин, усталой торговой вежливости и послезакрытной инфраструктуры;
- после этого airport-ось получила и второй curated pack: к первоначальному `nocturnal airports / fluorescent insomnia` слою добавлены `airport afterhours` characters, actions, places, objects, conditions, concepts, atmosphere и twists, чтобы airport-manifold держался уже не только на декорациях, но и на procedural ritual, late-night bureaucracy и transit-afterhours logic;
- проведен первый quality-pass по semantic coherence: из шаблонов убраны несколько явно gendered конструкций без морфологии, а lexical CSV-фильтр стал жестче к техническим, медицинским и слишком абстрактным словам;
- для lexical modes добавлен первый curated subset layer: поверх общего CSV-пула появились предпочтительные nouns / adjectives / verbs, которые усиливают атмосферный semantic field без жесткой тематической блокировки;
- поверх этого слоя добавлены первые atmospheric clusters для lexical batch coherence: `archive`, `rainy-city`, `night-route`;
- добавлена собственная иконка приложения: она встроена и в окно WPF, и в Windows `*.exe`, чтобы визуальная identity проекта начиналась еще до открытия генератора;
- lexical mood начал выходить в UI: spotlight и fullscreen reading mode теперь могут тихо подсказывать локальное atmospheric-пространство выбранной серии фрагментов;
- обнаружено, что внешний CSV иногда содержит сломанные gender-метки у существительных; генератор теперь должен чинить такие очевидные случаи мягкими morphology-free heuristics вместо слепого доверия источнику;
- lexical atmospheric header стал тише и аккуратнее: mood теперь лучше работает как reader-context, а не как лишний слой служебного текста;
- проведен второй lexical quality-pass по видимой выдаче: усилен curated bias и расширена фильтрация low-image / source-noise слов, которые звучали книжно, технически или просто разрушали атмосферу;
- исправлено maximize-поведение кастомного окна: теперь полноэкранное развертывание должно уважать рабочую область Windows и не уходить под панель задач;
- начато системное расширение боевых JSON-словарей для `Sentence`, `Idea` и `ShortText`: добавлен первый крупный curated pack по `character`, `place`, `object`, `action`, `condition`, `concept`, `twist`, `atmosphere`;
- добавлен второй curated pack в tonal-support категории `emotion`, `style`, `genre`, а также в дополнительные `place`, `condition`, `concept`, `twist`, чтобы core-режимы росли не только сценой, но и интерпретацией, mood-слоем и quieter genre/style variation;
- шаблонный слой тоже начал догонять выросший корпус: добавлен новый пакет `Sentence`, `Idea` и особенно `ShortText` templates, чтобы новые `condition / concept / style / genre / atmosphere / twist` использовались шире и музыкальнее;
- после контрольной выборки проведен первый output-driven pass: убраны оставшиеся gender-sensitive каркасы без морфологии и исправлены несколько реальных словарных шероховатостей в `genre` и `place`;
- output-driven quality pass теперь рассматривается как постоянный рабочий цикл: после каждого заметного расширения корпуса нужно снимать живую выборку и точечно чинить реальные слабые каркасы, а не спорить с абстрактным качеством на уровне теории;
- `DreamAssembler.DataTools` теперь умеет показывать не только общую сводку по категориям и слотам, но и pack-level статистику по каждому JSON-набору из `data-manifest.json`, а также обзор CSV-источников словесных режимов; это должно стать базовым инструментом контроля growth и перекосов между atmospheric fields;
- `DreamAssembler.DataTools` дополнительно получил режим живой выборки `samples`, чтобы быстро снимать реальные серии `Sentence / Idea / ShortText` и делать output-driven quality-pass не по интуиции, а по фактической выдаче;
- проект собирается без ошибок.

## Сильные стороны текущей генерации

Сейчас проект особенно силен в следующем:

- атмосферные места уже звучат узнаваемо и не сводятся к generic fantasy;
- `condition` и `concept` дают эмоциональный и символический слой, а не только событийность;
- `compat:*` уже начинает защищать выдачу от совсем пустых action/object-связок;
- narrative-роли в `ShortText` уже уменьшают грубые композиционные повторы;
- JSON-структура со `slot` удерживает controllability без чрезмерной сложности;
- данные уже содержат правильную tonal base: город, ночь, транспорт, бюрократия, архивность, тихая сюрреальность.
- новые поля уже начали расходиться в правильные стороны: помимо archive/transport base и airport-insomnia слоя появился mall/abandoned-commerce слой, что подтверждает работоспособность стратегии growth through curated manifolds.
- airport-field теперь уже нельзя считать тонкой первой пробой: после второго pack он стал самостоятельным second-wave manifold, который можно оценивать и наращивать по pack-level статистике, а не только по ощущениям от единичных фраз.
- затем expansion продолжен уже в следующий manifold: добавлен первый curated field `impossible museums / echo exhibits / ceremonial curation` по `character / action / place / object / condition / concept / atmosphere / twist`, чтобы growth шел не вокруг одной airport/mall оси, а серией distinct atmospheric spaces;
- live output-pass подтвердил, что основные грубые дефекты уже сместились из области ломаных template-склеек в область более тонкой semantic coherence: теперь заметнее не падежные поломки, а мягкое смешение разных atmospheric fields внутри одной сцены.
- lexical modes больше не полностью отданы сырому словарю: curated preference layer уже помогает удерживать городской, дождливый, архивный и инфраструктурный mood.
- lexical modes получили первый мягкий cluster-pass: внутри одной серии коротких фрагментов атмосфера чаще держится вокруг локального emotional space, а не распадается на полностью независимые лексические вспышки.
- lexical modes все еще должны развиваться прежде всего через curated dataset quality, а не через усложнение алгоритма: это подтверждено реальными пользовательскими примерами слабых слов.
- core JSON-режимам все еще нужна дальнейшая экспансия: нынешний pack улучшает вариативность заметно, но целевой масштаб действительно должен идти к сотням и затем тысячам curated фраз и словарных записей.
- текущий рост идет в правильной форме: не через random bulk, а через отдельные тематические packs, которые можно дальше наращивать сериями без потери identity.
- текущая pack-level статистика уже позволяет принимать решения по growth без догадок: airport-field сейчас достаточно плотный для паузы, mall-field остается рабочим first-wave слоем, а museum-field уже вышел за пределы первой декоративной museum-wave и начал расти как отдельная conservation / curation ecology.
- затем mall-field тоже переведен из first-wave в second-wave состояние: к исходному `dead shopping malls / abandoned commerce / fluorescent afterhours` слою добавлен второй pack с персонажами, действиями, backroom/service-пространствами и дополнительными tonal-support наборами, чтобы mall-ось держалась не только на пустых витринах, но и на afterhours service logic, fluorescent waiting и exhausted retail ritual.
- после этого открыт следующий manifold `recursive hospitality / nocturnal hotels / ceremonial check-in`: добавлен первый compact pack по `character`, `action`, `place`, `object`, `condition`, `concept`, `atmosphere` и `twist`, чтобы проект начал расти и в сторону ночного гостеприимства, задержанных приездов, коридорного терпения и service-insomnia, не дожидаясь финального output-pass.
- затем hospitality-field сразу усилен вторым pack вокруг `back office / room-service ritual / shifted departures / breakfast before morning`, чтобы этот manifold быстрее вышел из first-wave состояния и держался не только на лобби и коридорах, но и на service-memory, late checkout logic и внутренней ночной инфраструктуре гостеприимства.
- политика версионирования данных тоже уточнена: `data-manifest.json` больше не должен прыгать по `minor` на каждый новый pack; для обычного роста корпуса и новых atmospheric packs используется `PATCH`, чтобы версия отражала реальный масштаб изменения.
- затем выполнен промежуточный большой `samples`-pass по расширенному corpus, после которого последовательно усилены soft manifold-affinity, compat-scoring и прямой action/object candidate filtering, чтобы снизить случайные cross-field склейки и убрать оставшиеся пары уровня `открыть табличку мастер-ключом`.
- текущее состояние данных на конец этой сессии: `data-manifest.json = 0.5.9`, corpus = `768` entries, а `airport`, `mall`, `museum` и `hospitality` уже находятся в second-wave состоянии.
- теперь узким местом постепенно становится не только объем словарей, но и richness самих каркасов: это значит, что дальнейшее развитие должно идти параллельно по данным и по template rhythm.
- museum-layer теперь дополнительно углублен новым conservation-pack: humidity control, storage patience, catalog memory, absent visitors и procedural preservation начали работать уже не как отдельные слова, а как более плотная emotional ecology этого manifold.

Главное: проект уже умеет давать не только странность, но и настроение.

Теперь узкое место постепенно находится в другом:

- narrative cadence все еще слишком часто звучит одинаково;
- progression фраз в серии слишком linear;
- отдельные outputs временами ощущаются как isolated fragments без hidden continuity;
- symbolic resonance и motif resurfacing пока слишком слабы;
- внутри batch еще не хватает мягкого emotional drift;
- зрелые manifolds уже требуют не только расширения словаря, но и углубления собственной emotional ecology;
- lexical modes уже нельзя считать вторичным режимом: они становятся отдельным atmospheric instrument и требуют такого же внимания к continuity и resonance.
- первый реальный manifold deepening-pass уже подтверждает правильную форму роста: проекту важнее не только открывать новые fields, но и возвращаться в существующие пространства, чтобы наращивать их ritual behavior, maintenance logic и service anxiety.

## Philosophy Of Generation Quality

Качество генерации в DreamAssembler измеряется не только правильностью.

Приоритеты качества:

- atmosphere over complexity;
- coherence over randomness;
- symbolic consistency over raw variety;
- semantic density over word count;
- emotional texture over "smartness";
- rhythm over correctness;
- image density over verbosity;
- emotional coherence over plot coherence.

Практический смысл:

- не гнаться за полной логичностью каждой фразы;
- не убирать dream-like feeling в пользу объяснимости;
- не вычищать все невозможные сочетания, если они эмоционально работают;
- не пытаться строить сюжетность там, где проекту органичнее атмосферный фрагмент;
- считать мягкую surreal ambiguity частью identity, а не шумом.

## Philosophy Of Curated Surrealism

Curated surrealism в DreamAssembler значит:

- странность дозируется, а не льется без фильтра;
- бытовая и инфраструктурная опора сохраняется даже в сюрреалистических местах;
- текст должен звучать как "это почти возможно", а не как случайная нелепость;
- символические мотивы важнее разового вау-эффекта;
- проект должен оставаться узнаваемым по тону, а не только по набору категорий;
- новые atmospheric fields должны расти как curated manifolds со своими recurring objects, emotional logic, symbolic vocabulary и internal dream-rules, а не как пачка новых слов.

## Philosophy Of Quiet Absurdity

Quiet absurdity - это не комедийный хаос и не абстрактный nonsense.

Это:

- спокойная, почти служебная странность;
- тихая тревога;
- медленный сдвиг нормальности;
- ощущение, что мир слегка не совпадает сам с собой;
- фразы, которые не кричат абсурдом, а просачиваются в читателя.

Нужный эффект не "сюжет", а:

- resonance;
- emotional aftertaste;
- ощущение скрытой системы;
- чувство найденного фрагмента;
- ощущение "чужого сна, который почти логичен".

## Philosophy Of UI

DreamAssembler должен развиваться не как control-heavy utility, а как спокойный reader-first инструмент.

Принципы:

- контент важнее chrome;
- текст важнее панелей;
- типографика важнее насыщенности контролами;
- чтение важнее демонстрации функциональности;
- визуальная тишина важнее "богатого" интерфейса;
- cinematic calm лучше, чем dashboard density;
- пустота и воздух являются частью атмосферы, а не незаполненным местом;
- fullscreen reading mode является одним из core experiences проекта;
- пользователь должен ощущать не "генерацию текста", а ритуал чтения странного атмосферного фрагмента.

Нужное ощущение:

- strange literary machine;
- atmospheric terminal;
- найденная dream-machine;
- quiet reading surface;
- procedural literature device.

## Philosophy Of Reader-First Interaction

Reader-first interaction значит:

- основной опыт - смотреть и перечитывать фрагмент;
- результат должен чувствоваться центральным объектом окна;
- вторичные действия должны быть мягкими и не мешать восприятию;
- fullscreen reading mode логично продолжает уже начатое направление;
- шрифты, палитры, spacing и размер текста должны работать на атмосферу чтения;
- короткие режимы заслуживают почти phrase-centered presentation;
- кнопки и dropdowns не должны становиться главными объектами экрана;
- пользователь должен чувствовать, что он настраивает атмосферный приемник, а не крутит control surface сюрреализма.

Что важно ослаблять постепенно:

- standard WPF-like прямоугольность;
- механически очевидный chrome;
- агрессивные borders;
- visually loud cards;
- dashboard-like separation;
- utility-oriented control emphasis.

Что не нужно тащить в интерфейс:

- glassmorphism;
- RGB / glow;
- decorative cyberpunk;
- VHS overlays;
- excessive animations;
- visual gimmicks ради самих gimmicks.

DreamAssembler не должен быть дизайнерским аттракционом. Visual polish здесь нужен не для демонстрации красоты интерфейса, а для усиления атмосферы и reading pace.

## Guidance For UI Presentation

Presentation должна развиваться как:

- typography-first;
- calm;
- cinematic;
- fullscreen-reading-oriented;
- minimal chrome;
- content-dominant;
- phrase-centered;
- visually restrained.

Практически это значит:

- текст должен побеждать интерфейс;
- короткая фраза должна ощущаться не как list item, а как atmospheric object;
- spotlight presentation важнее обычной карточечной логики для коротких режимов;
- isolated fragments и большие пустые зоны допустимы и полезны;
- интерфейс должен "дышать", а не заполнять каждый участок окна.

Typography здесь не decoration, а часть generation experience:

- reading rhythm;
- line width;
- spacing;
- cinematic wrapping;
- phrase pacing;
- visual density;
- atmospheric hierarchy.

Иногда фраза должна ощущаться почти как стих.

## Guidance For Atmospheric / Emotional Direction

Если проект расширяется, нужно спрашивать не только "стало ли умнее?", но и:

- стало ли атмосфернее;
- стало ли узнаваемее;
- стало ли плотнее по образу;
- стало ли мягче по ритму;
- не исчез ли тихий dream-logic;
- не стала ли выдача слишком generic.

Дополнительно важно:

- удерживает ли новая зона atmosphere continuity;
- умеет ли одна атмосфера мягко просачиваться в другую;
- появился ли emotional drift без жанровой вывески;
- возникла ли symbolic interference между мотивами;
- осталась ли dream-like ambiguity живой, а не объясненной до конца.

DreamAssembler должен развиваться ближе к atmospheric software artifact, procedural dream instrument и quiet surreal reading environment, чем к storytelling engine.

Следующая стадия зрелости должна дать еще и другое ощущение: не просто генерацию отдельных удачных фрагментов, а пространство, в котором фразы эмоционально дрейфуют, перекликаются и иногда помнят друг друга.

## Что проверить при следующем шаге

- удерживает ли серия outputs хотя бы слабую atmosphere continuity, а не распадается на удачные, но atomized fragments;
- какие motifs стоит разрешить к very soft resurfacing внутри batch: объекты, служебные знаки, инфраструктурные детали, procedural artifacts;
- какие template classes слишком часто дают одинаковую длину дыхания и одинаковый emotional pacing;
- какие rhythm-формы нужно усиливать в первую очередь: `announcement`, `inventory`, `procedural note`, `quiet instruction`, `museum label`, `interrupted memory`, `static observation`, `delayed revelation`;
- появляются ли внутри `ShortText` и серий мягкие emotional echoes, или каждое предложение живет слишком отдельно;
- какой слой atmospheric gravity уже можно ввести без перехода к rigid mode-system;
- какие manifolds готовы не к еще одному random pack, а к углублению собственной emotional ecology;
- какой existing manifold логичнее выбрать для следующего deepening-pass: `museum`, `mall`, `hospitality` или `airport`;
- какой новый manifold лучше открыть следующим, если смотреть не на тему, а на distinct emotional pressure;
- достаточно ли lexical modes удерживают batch coherence, spotlight resonance и phrase-centered reading flow;
- не пора ли добавлять session-level fragment resurfacing, remembered fragments или atmospheric trails как very soft continuity layer;
- после каждого rhythm/continuity/manifold-pass обязательно снова снимать `DreamAssembler.DataTools -- samples` и смотреть уже не только на defects, но и на drift, cadence и recurrence.

## Следующие рекомендуемые шаги

1. Ввести `rhythm diversification pass` в templates: не просто новые каркасы, а новые способы дыхания текста и разные cadence classes.
2. Добавить very soft layer для `atmospheric continuity`: motif resurfacing, symbolic recurrence, emotional echoes, weak fragment memory.
3. Ввести мягкий `dominant emotional pressure` для batch или короткой серии, не превращая его в жесткий режим или жанровый переключатель.
4. Продолжать растить существующие manifolds как emotional ecologies: ritual behavior, maintenance logic, service patterns, institutional memory, recurring anxieties.
5. Открывать новые manifolds не по теме, а по distinct atmospheric pressure и internal dream-logic, особенно в non-urban / infra-near-nature направлении.
6. Усиливать lexical modes как отдельный atmospheric instrument: batch coherence, spotlight continuity, fragment trails, isolated resonance.
7. Дальше развивать atmospheric presentation только там, где оно реально поддерживает drift-reading experience.
8. Оставить morphology, heavy systems и architecture expansion в отложенном статусе.

## Технические ограничения текущей версии

- нет полноценной морфологии русского языка;
- нет базы данных;
- нет редактора словарей;
- нет плагинной системы;
- `ShortText` пока собирается как 2-5 независимых предложений без глубокой внутренней сцепки;
- atmosphere-система пока существует не как отдельная сущность, а как эффект сочетания тегов, слотов, весов и curated данных.

## Что важно помнить новым сессиям

- не добавлять нейросетевую генерацию;
- не тащить базу данных в MVP;
- не превращать проект в enterprise architecture exercise;
- не строить abstraction ради abstraction;
- не усложнять систему без реальной пользы для атмосферы и controllability;
- не хардкодить большие словари в коде;
- документация и комментарии должны оставаться на русском языке;
- atmosphere важнее "идеальной логики";
- charm проекта важнее демонстрации технической сложности.
