# Архитектура DreamAssembler

## Архитектурный принцип

DreamAssembler - это controllable, data-driven, rule-based atmospheric text engine.

Архитектура должна оставаться:

- прозрачной;
- мягкой по связности;
- удобной для curated data growth;
- пригодной для targeted runtime tuning;
- свободной от enterprise overengineering.

Главная цель архитектуры - не "умная генерация" ради сложности, а управляемое создание атмосферных фрагментов с manifold identity, silence, residue и procedural dream-logic.

## Что важно понимать о runtime

Система уже не сводится к "шаблон плюс слова".

Фактическая генерация строится из сочетания:

- templates;
- tags;
- `slot` / `slotRequirements`;
- `compat:*`;
- weights;
- anti-repeat;
- dominant manifold memory;
- cadence affinity;
- pressure handling;
- curated datasets.

Следующий уровень зрелости - не massive rewrite, а мягкая эволюция runtime ecology.

## Слои приложения

### `DreamAssembler.Core`

Бизнес-логика и runtime.

Основные задачи:

- модели генерации;
- загрузка словарей и шаблонов;
- template selection;
- dictionary selection;
- anti-repeat;
- manifold memory;
- cadence / pressure / continuity biases;
- generation debug trace.

### `DreamAssembler`

WPF UI.

Основные задачи:

- reader-first presentation;
- управление генерацией;
- показ результатов;
- user settings;
- visual reading surface.

### `DreamAssembler.DataTools`

Консольный diagnostics and validation layer.

Основные задачи:

- validation;
- dataset summary;
- samples;
- runtime audit;
- runtime report;
- snapshot export;
- compare.

## Важные классы и файлы

- `DreamAssembler.Core/Services/TextGeneratorService.cs`
- `DreamAssembler.Core/Models/TextGenerationResult.cs`
- `DreamAssembler.Core/Models/GenerationDebugTrace.cs`
- `DreamAssembler.Core/Models/GenerationTraceBuilder.cs`
- `DreamAssembler.Core/Services/FallbackDataProvider.cs`
- `DreamAssembler/Data/Templates/templates.json`
- `DreamAssembler.DataTools/Program.cs`

## Diagnostics Layer

### Что это такое

`DebugTrace` / `audit` / `report` / `compare` - это diagnostics-only слой.

Он нужен для observability runtime ecology:

- какие manifolds surface-ятся;
- какие cadence доминируют;
- какой pressure реально несет runtime;
- где растет monotony;
- где runtime теряет manifold-local autonomy.

### Чем он не является

Он не должен:

- влиять на UI;
- оценивать литературное качество;
- становиться quality score;
- превращаться в KPI-dashboard;
- навязывать hard balancing goals.

Нужная формулировка:

`DebugTrace` нужен для понимания runtime behavior, а не для оценки "хорошего текста".

## Runtime Workflow Architecture

Если меняется generation/runtime/data/template layer, нормальный порядок такой:

1. baseline `report` / `snapshot`;
2. human `samples`, если нужен reading check;
3. targeted code or data pass;
4. новый `report` / `snapshot`;
5. `compare`;
6. `dotnet test`;
7. `dotnet build`;
8. docs sync.

Это уже часть архитектуры процесса, а не факультатив.

## Generation Logic Today

Текущая generation architecture уже включает:

- soft manifold-local gravity;
- cadence preference by manifold;
- targeted damping of legacy baseline;
- contextual quiet differentiation;
- short-text autonomy bias;
- temporal competition against overdominant cadence skeletons.

То есть runtime evolution уже идет в сторону atmospheric regulation, но пока без hard control systems.

## Что важно сохранять

- soft coherence instead of hard routing;
- atmospheric identity over statistical neatness;
- old-core atmosphere as one strong field, not as hidden universal baseline;
- controlled diagnostics instead of enterprise analytics;
- curated data over algorithmic overcompensation.

## Что архитектуре делать не надо

- не строить scoring engine;
- не строить hard manifold mode system;
- не строить telemetry dashboard;
- не превращать diagnostics в product centerpiece;
- не вытеснять content growth endless tooling work;
- не тянуть AI / vector DB / cloud features;
- не уходить в database-first rewrite.
