# Project State

## Где сейчас проект

DreamAssembler находится в фазе `audit-driven runtime tuning + controlled manifold expansion`.

Это уже не ранний MVP, где главным bottleneck были broken outputs, gross defects или UI polish. Базовый каркас работает, runtime diagnostics существует, а новые manifolds уже достаточно выросли в данных.

Главный вопрос теперь:

- не "хватает ли еще слов";
- а "как реально ведет себя runtime ecology под серией генераций".

## Что это значит practically

Проект сейчас развивается по связке:

- human samples;
- runtime diagnostics (`audit` / `report` / `snapshot` / `compare`);
- unit tests / build;
- targeted runtime or data passes;
- controlled corpus expansion.

Новые решения больше не должны приниматься по одиночному ощущению от нескольких outputs. Нужна связка:

1. читать живые samples;
2. смотреть `report` и `compare`;
3. вносить targeted pass;
4. снова проверять runtime ecology;
5. только потом фиксировать вывод.

## Текущая identity проекта

DreamAssembler - это:

- atmospheric procedural text engine;
- curated surrealism engine;
- quiet absurdity engine;
- reader-first creative tool;
- atmospheric reading instrument.

Это не:

- AI assistant;
- storytelling engine;
- dashboard with text output;
- "генератор бреда".

Его сила - не в сюжетности и не в полном semantic correctness, а в:

- atmospheric coherence;
- emotional texture;
- symbolic language;
- silence;
- residue;
- procedural dream logic.

## Текущий phase-shift

Раньше главным вопросом были:

- dataset depth;
- surface-level defects;
- UI basic polish.

Сейчас главный bottleneck сменился на:

- `legacy atmospheric gravity`;
- universal quiet fallback;
- cadence fatigue;
- weak manifold autonomy;
- hidden monotony;
- imbalance между corpus depth и runtime surfacing.

Это важно: новые manifolds уже существуют и не являются фикцией.

У проекта реально есть:

- airport;
- mall;
- museum;
- hospitality;
- observatory;
- sanatorium;
- hydroelectric;
- weather_systems;
- coastal_fog.

Проблема больше не в lack of manifolds, а в том, как runtime удерживает их cadence, pressure и silence identity.

При этом controlled corpus growth снова становится актуальным: после documentation sync и runtime stabilization новый content-pass по `mall` уже выполнен, так что bottleneck начинает смещаться от pure runtime tuning обратно к точечному manifold deepening.

## Три уровня проверки

### 1. Technical correctness

- `dotnet test DreamAssembler.Core.Tests/DreamAssembler.Core.Tests.csproj`
- `dotnet build DreamAssembler.sln`

Это отвечает за:

- regression safety;
- structural correctness;
- build integrity.

### 2. Human reading

- `DreamAssembler.DataTools -- samples`
- ручное чтение `Sentence`, `Idea`, `ShortText`

Это отвечает за:

- phrase feel;
- silence;
- drift;
- emotional plausibility;
- atmospheric identity.

### 3. Runtime ecology diagnostics

- `audit`
- `report`
- `snapshot`
- `compare`

Это отвечает за:

- manifold surfacing;
- cadence distribution;
- primary cadence;
- pressure dominance;
- legacy gravity;
- repetition;
- template dominance;
- manifold-local cadence activation;
- monotony symptoms.

Важно: diagnostics - это не quality score. Это observability layer.

## Что уже есть в runtime diagnostics layer

Система уже умеет:

- собирать `DebugTrace` внутри generation results;
- запускать mass-generation audit;
- печатать concise report;
- сохранять snapshots;
- сравнивать ecology drift между двумя snapshots.

Это lightweight diagnostics-only слой.

Он не должен:

- влиять на UI-поведение;
- становиться scoring engine;
- превращаться в KPI-dashboard;
- диктовать равномерное распределение manifolds.

## Текущее runtime reading

Последние targeted passes дали такой сдвиг:

- `static_observation` уже не доминирует как раньше;
- `weather_systems` и `hydroelectric` заметно усилили cadence autonomy;
- contextual quiet differentiation убрала generic `quiet` из роли universal pressure leader;
- long `500` audit показывает, что `observatory` surface-ится лучше, чем казалось по коротким `120` snapshots;
- `museum` остается самым тяжелым corpus-field и теперь выглядит как главный кандидат на следующий targeted runtime/deepening pass.
- `mall` получил controlled deepening-pass на `0.6.11`: afterhours maintenance, fluorescent residue, decorative exhaustion, service-corridor silence и closed-arcade memory теперь входят в corpus как отдельная ecology.
- короткий `audit-mall-g.json` после этого шага оказался sanatorium-heavy и не дает надежного вывода о runtime uplift самого `mall`, так что rebalance по одному этому окну делать не нужно.
- первый длинный `1000` audit после mall deepening показал, что `mall` уже не lexical-only: в `ShortText` он surface-ится на `10%` и держит `94%` preferred cadence activation, а в `Sentence` тоже стабильно появляется на `10%`.
- тот же длинный `1000` audit показал, что следующий runtime weakness смещается не в `mall`, а скорее в `hospitality cadence autonomy` и в `museum` как heavier sentence-field.
- затем выполнен hospitality autonomy pass, и он сработал без broad rebalance: `hospitality` поднялся с примерно `2%` surfacing и `45%` cadence activation до `22%` surfacing и `92.3%` cadence activation в длинном `ShortText` окне.
- при этом `quiet` не вернулся к universal solvent, `legacy-heavy` drift не вырос, cadence repetition немного снизилась, а `static_observation` не откатился обратно к старым `~40%`.

То есть проект уже двигается не вслепую: runtime symptoms видны, измеримы и сопоставимы before/after.

## Что важно не перепутать

### Runtime tuning не отменяет expansion

Проекту все еще нужны:

- новые manifolds;
- deepening existing manifolds;
- curated content growth.

Но expansion нельзя продолжать blindly. Сначала нужно понять, как новое content actually behaves в runtime.

### Diagnostics не равны цели проекта

Нельзя подменять atmospheric judgment метриками.

Цель не в том, чтобы:

- выровнять все manifolds;
- избавиться от доминирования вообще;
- оптимизировать цифры ради цифр.

Цель в том, чтобы:

- видеть symptoms;
- делать targeted runtime changes;
- сохранять identity;
- поддерживать живую manifold ecology.

## Что сейчас считать успехом

Успех текущей фазы - это не "идеальная статистика", а:

- меньше universal fallback layers;
- больше manifold-local cadence and pressure;
- меньше monotonous breathing;
- больше contextual silence;
- больше emotional autonomy новых fields;
- сохранение old-core atmosphere без возврата к hidden baseline domination.

## Controlled Expansion Direction

Когда текущий runtime cycle будет стабилизирован, следующий content-step должен быть controlled, а не bulk.

Сейчас самый разумный порядок такой:

1. удержать diagnostics discipline;
2. сделать еще один targeted pass там, где long audit показывает weak autonomy;
3. обновить docs и snapshots;
4. выбрать следующий deepening/expansion candidate;
5. после content-step снова делать report/compare.

На текущем состоянии самым разумным candidate выглядит:

- runtime-side: `museum cadence autonomy` и, возможно, `airport cadence autonomy`;
- content-side after that: следующий controlled candidate уже не обязан быть `mall`, потому что mall pass подтвердил corpus-value и базовую runtime recognizability;

а не открытие случайной новой волны из множества manifolds сразу.

## Что важно помнить новой сессии

- не принимать runtime decisions по одному sample batch;
- не превращать diagnostics в KPI;
- не зависать в endless diagnostics вместо corpus growth;
- не делать hard mode routing;
- не убивать old-core archive/transport/bureaucracy atmosphere;
- не открывать random bulk expansion без нового audit-backed checkpoint.
