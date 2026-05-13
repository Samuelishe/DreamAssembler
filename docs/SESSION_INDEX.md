# Session Index

Этот файл - обязательный entry-point для любой новой сессии и любого нового агента.

Цель: быстро понять identity проекта, текущую фазу, обязательный workflow проверки и только потом идти в код или данные.

## Читать в таком порядке

1. `docs/SESSION_INDEX.md` - быстрый вход, identity и workflow.
2. `docs/PROJECT_STATE.md` - текущая фаза проекта и product direction.
3. `docs/ROADMAP.md` - приоритеты ближайшего развития.
4. `docs/ARCHITECTURE.md` - как устроен runtime и где проходит граница diagnostics.
5. `docs/MANIFOLD_STATE.md` - corpus balance, runtime observations и кандидаты на deepening/expansion.
6. `docs/IDEAS_BACKLOG.md` - рабочие направления, риски и запреты.
7. `docs/VERSIONING.md` - правила версии приложения и данных.
8. `docs/WORKLOG.md` - журнал последних решений, если нужен handoff по шагам.
9. `docs/DICTIONARY_FORMAT.md` и `docs/DATA_SOURCES.md` - только если задача касается JSON-наборов и их структуры.

## Что нужно понять сразу

DreamAssembler нельзя воспринимать как:

- random generator;
- AI assistant;
- LLM frontend;
- dashboard with text output;
- "генератор бреда".

Его нужно воспринимать как:

- atmospheric procedural text engine;
- curated surrealism engine;
- quiet absurdity engine;
- reader-first creative tool;
- procedural atmosphere system;
- atmospheric reading instrument.

## Ключевая философия

- atmosphere over plot;
- emotional coherence over logical coherence;
- symbolic motifs over raw randomness;
- semantic density over verbosity;
- controlled weirdness over noise;
- soft coherence over hard routing;
- quiet absurdity over loud surrealism;
- manifold identity over generic variety.

## Текущая фаза проекта

Проект находится в фазе `audit-driven runtime tuning + controlled manifold expansion`.

Это значит:

- gross structural defects уже не являются главным bottleneck;
- новые manifolds уже существуют и достаточно выросли по datasets;
- главный runtime-bottleneck сейчас не в отсутствии packs, а в том, как runtime реально ведет себя под нагрузкой;
- ключевые проблемы теперь формулируются как `legacy atmospheric gravity`, `quiet as universal solvent`, cadence fatigue, weak manifold autonomy и hidden monotony.

Важно: controlled expansion не отменена. Проекту все еще нужны новые manifolds и deepening existing fields. Но новые content-steps больше нельзя делать вслепую, без runtime-check.

## Три обязательных уровня проверки

В проекте есть не один вид проверки, а три.

### 1. Unit tests + build

Нужны для:

- structural correctness;
- regression safety;
- compile-time integrity.

Они не отвечают на вопрос, хорошо ли runtime дышит атмосферно.

### 2. Human samples

Нужны для:

- человеческой проверки текста;
- проверки atmospheric identity;
- оценки phrase feel, silence, cadence и residue.

`samples` полезны там, где нужно читать реальные outputs, а не только distribution.

### 3. Runtime diagnostics / audit

Нужны для:

- manifold surfacing;
- dominant manifolds;
- cadence distribution;
- primary cadence;
- pressure dominance;
- legacy gravity;
- cadence repetition;
- template dominance;
- manifold-local cadence activation;
- repeated outputs;
- monotony signals.

Это не quality score.

Это не KPI.

Это не система для hard quotas, равномерного manifold distribution или автоматического решения, какой текст "лучше".

Это atmospheric X-ray: инструмент наблюдения за runtime ecology.

## Runtime Diagnostics / Audit Workflow

Любой runtime / cadence / manifold / template / data pass должен проходить через этот workflow:

1. снять baseline `report` / `snapshot`;
2. внести targeted changes;
3. снять новый `report` / `snapshot`;
4. сравнить `before/after`;
5. только после этого делать выводы;
6. затем прогнать tests и build.

Базовые команды:

```powershell
dotnet run --project DreamAssembler.DataTools/DreamAssembler.DataTools.csproj -- report ShortText 120 Absurd --snapshot audit-shorttext-current.json
dotnet run --project DreamAssembler.DataTools/DreamAssembler.DataTools.csproj -- report Sentence 120 Absurd --snapshot audit-sentence-current.json
dotnet run --project DreamAssembler.DataTools/DreamAssembler.DataTools.csproj -- compare audit-shorttext-before.json audit-shorttext-after.json
dotnet test DreamAssembler.Core.Tests/DreamAssembler.Core.Tests.csproj
dotnet build DreamAssembler.sln
```

Default snapshot behavior:

- if `--snapshot` gets only a filename, it is saved to `artifacts/audit/`;
- if `--snapshot` gets an explicit path, that path is used as-is;
- `compare` can read bare filenames from the current directory or from `artifacts/audit/`.

Практический смысл инструментов:

- `samples` - human reading sanity-check;
- `report` / `audit` - distribution ecology;
- `compare` - drift после изменения;
- `dotnet test` - regression safety;
- `dotnet build` - финальная сборочная проверка.

Важно:

- не превращать diagnostics в optimization target;
- не подгонять runtime под "идеальную статистику";
- не считать равномерное распределение manifolds целью проекта;
- использовать diagnostics, чтобы видеть симптомы, а не диктовать output.

Типовые симптомы, за которыми нужно следить:

- legacy gravity;
- cadence fatigue;
- overdominant template;
- universal quiet fallback;
- weak manifold autonomy;
- excessive `static_observation`;
- monotony;
- repeated local skeletons.

## Какие manifolds уже реально есть

Это не wishlist, а реальные поля в corpus:

- archive / transport / bureaucracy - old-core field;
- nocturnal airports / fluorescent insomnia;
- dead shopping malls / abandoned commerce / fluorescent afterhours;
- impossible museums / echo exhibits / ceremonial curation;
- recursive hospitality / nocturnal hotels / ceremonial check-in;
- observatory loneliness;
- sanatorium bureaucracy;
- hydroelectric infrastructure;
- procedural weather systems;
- coastal fog logistics;
- radar stations / distant transmission infrastructure.

Если нужен баланс роста, сначала смотреть `docs/MANIFOLD_STATE.md`, затем подтверждать через `DreamAssembler.DataTools` и `DreamAssembler/Data/data-manifest.json`.

## Текущее factual state

- app version: `0.2.1.0`
- data version: `0.6.12`
- corpus: `1316` entries
- templates: `58`
- unit tests: `49/49` green на текущем runtime этапе
- базовый workflow контроля: `samples` + `report/snapshot/compare` + `dotnet test` + `dotnet build`

## Главные направления текущей линии

- runtime atmospheric rebalance;
- cadence diversification;
- silence architecture;
- manifold-local emotional autonomy;
- contextual silence instead of universal quiet;
- targeted runtime tuning before blind expansion;
- controlled manifold expansion after diagnostics-backed decisions.
- controlled manifold deepening of existing fields, with `mall` now moved from thin second-wave toward exhausted-commercial deepened state.
- compact manifold growth is active again, with `radar_stations` opened as a new quiet signal-infrastructure field.

## Что может уничтожить charm проекта

- sterilization;
- metrics-driven tuning instead of atmospheric judgment;
- hard quotas or equal-distribution logic;
- endless diagnostics вместо content growth;
- превращение diagnostics в scoring engine;
- genre-mode system;
- deterministic routing;
- dashboard-style UI вместо reading surface.

## Важные точки входа в код

- `DreamAssembler.Core/Services/TextGeneratorService.cs`
- `DreamAssembler.Core/Models/GenerationDebugTrace.cs`
- `DreamAssembler.Core/Models/GenerationTraceBuilder.cs`
- `DreamAssembler.Core/Models/TextGenerationResult.cs`
- `DreamAssembler.DataTools/Program.cs`
- `DreamAssembler.Core.Tests/Services/*`
- `DreamAssembler/Data/Templates/templates.json`
- `DreamAssembler.Core/Services/FallbackDataProvider.cs`
- `DreamAssembler/Data/data-manifest.json`

## Как мыслить следующий шаг

Нормальный порядок для новой сессии:

1. прочитать этот файл и `PROJECT_STATE.md`;
2. понять, о каком слое идет задача: correctness, content, runtime, UI или docs;
3. если задача касается generation/runtime/data/templates, снять baseline `report` и при необходимости `samples`;
4. сделать targeted pass;
5. снять новый `report`, сделать `compare`, прогнать tests/build;
6. обновить docs, если изменился runtime steering, workflow или factual state.

## Правило обновления документации

После каждого значимого шага обновлять:

1. `docs/PROJECT_STATE.md`
2. `docs/WORKLOG.md`
3. `docs/SESSION_INDEX.md`, если изменился workflow входа или обязательные проверки
4. `docs/MANIFOLD_STATE.md`, если изменился corpus/runtime reading или expansion priorities
5. `docs/ROADMAP.md` и `docs/IDEAS_BACKLOG.md`, если изменились продуктовые приоритеты или риски
6. `docs/VERSIONING.md`, если изменилась фактическая текущая версия или правила версии

## Что не нужно читать сразу

- весь `WORKLOG` подряд;
- все JSON-словари;
- весь repo без понятной задачи;
- diagnostics как KPI-таблицу.
