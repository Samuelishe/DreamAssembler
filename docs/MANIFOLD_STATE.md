# Manifold State

Этот файл хранит текущий баланс уже выращенных manifolds и нужен как рабочий snapshot перед любым `expansion`, `deepening` или `surfacing` pass.

Важно: это не wishlist и не концептуальная карта. Здесь фиксируются только те поля, которые уже реально присутствуют в `data-manifest.json` и в JSON-corpus.

## Как читать этот файл

- `sets` = сколько dictionary sets в `data-manifest.json` явно относятся к manifold;
- `entries` = сколько dictionary entries несут соответствующий strong manifold tag;
- `categories` = покрытие по основным runtime categories;
- `status` = текущая зрелость поля как emotional ecology;
- `priority` = что с этим полем делать дальше.

Важно: базовый `archive / transport / bureaucracy` слой здесь не считается как один strong-manifold tag, потому что он распределен по нескольким cross-cutting tags и исторически является foundation-field, а не одним isolated pack-family.

## Snapshot

Состояние на `data-manifest.json = 0.6.2`.

### Core Manifolds

| manifold | sets | entries | categories | status | priority |
|---|---:|---:|---|---|---|
| `museum` | 22 | 161 | action, atmosphere, character, concept, condition, object, place, twist | deepened second-wave-plus | не расширять вслепую; дальше только точечный ecology-deepening |
| `hospitality` | 16 | 114 | action, atmosphere, character, concept, condition, object, place, twist | strong second-wave | можно усиливать позже, но не срочно |
| `airport` | 14 | 98 | action, atmosphere, character, concept, condition, object, place, twist | stable second-wave | скорее удерживать, чем срочно расширять |
| `mall` | 11 | 98 | action, atmosphere, character, concept, condition, object, place, twist | thinner second-wave | кандидат на future deepening |

### New Non-Urban Manifolds

| manifold | sets | entries | categories | status | priority |
|---|---:|---:|---|---|---|
| `observatory` | 8 | 46 | action, atmosphere, character, concept, condition, object, place, twist | first-wave complete | нужно усиливать surfacing и затем second pack |
| `sanatorium` | 8 | 46 | action, atmosphere, character, concept, condition, object, place, twist | first-wave complete | нужно усиливать surfacing и затем second pack |
| `hydroelectric` | 8 | 46 | action, atmosphere, character, concept, condition, object, place, twist | first-wave complete | нужно усиливать surfacing и затем second pack |

## Balance Reading

### Что видно сразу

- `museum` сейчас самый тяжелый manifold в corpus: `161` tagged entries и `22` sets;
- `hospitality` уже догнал плотное mature-state;
- `airport` и `mall` остаются рабочими second-wave полями;
- `observatory`, `sanatorium` и `hydroelectric` пока симметрично thin: по `46` entries и `8` sets каждое.

### Практический вывод

- проблема уже не в том, что новых manifolds нет;
- проблема в том, что новые manifolds пока количественно и вероятностно проигрывают старым fields;
- следующий bottleneck находится в `surfacing balance`, а не в отсутствии идей для expansion.

## Category Balance

### `museum`

- action: 22
- atmosphere: 15
- character: 22
- concept: 18
- condition: 22
- object: 22
- place: 22
- twist: 18

Комментарий:
`museum` уже живет не как theme-pack, а как полноценная ecology с ощутимой внутренней плотностью.

### `hospitality`

- action: 16
- atmosphere: 10
- character: 16
- concept: 12
- condition: 16
- object: 16
- place: 16
- twist: 12

Комментарий:
`hospitality` уже достаточно плотный и устойчивый, но пока не так доминирует, как `museum`.

### `airport`

- action: 8
- atmosphere: 10
- character: 8
- concept: 12
- condition: 16
- object: 16
- place: 16
- twist: 12

Комментарий:
`airport` держится как coherent second-wave field, но по плотности уже заметно уступает `museum`.

### `mall`

- action: 8
- atmosphere: 10
- character: 8
- concept: 12
- condition: 16
- object: 16
- place: 16
- twist: 12

Комментарий:
`mall` structurally complete, но по ощущению и по corpus-mass это still thinner second-wave, чем `hospitality`.

### `observatory`

- action: 6
- atmosphere: 5
- character: 6
- concept: 6
- condition: 6
- object: 6
- place: 6
- twist: 5

Комментарий:
поле уже собрано как complete first-wave manifold, но пока слишком тонкое для заметного surfacing без дополнительной balance-work.

### `sanatorium`

- action: 6
- atmosphere: 5
- character: 6
- concept: 6
- condition: 6
- object: 6
- place: 6
- twist: 5

Комментарий:
состояние симметрично `observatory`: ecology уже есть, corpus-mass пока недостаточна.

### `hydroelectric`

- action: 6
- atmosphere: 5
- character: 6
- concept: 6
- condition: 6
- object: 6
- place: 6
- twist: 5

Комментарий:
новый industrial-water field уже complete по структуре, но пока еще не занял достаточную долю runtime-space.

## Current Growth Guidance

Следующие решения должны приниматься так:

1. не расширять дальше `museum` просто потому, что он и так хорошо звучит;
2. не считать новые manifolds "слабыми" до surfacing-pass;
3. сначала поднять surfacing visibility для `observatory`, `sanatorium`, `hydroelectric`;
4. затем выбрать, какой из них первым переводить из `first-wave complete` в `second-wave`;
5. отдельно держать в уме, что `mall` остается кандидатом на очередной deepening-pass раньше, чем `airport` или `hospitality`.

## Runtime Observations

Срез по live generation после `0.6.2`:

- `dotnet test DreamAssembler.Core.Tests/DreamAssembler.Core.Tests.csproj` проходит: `41/41`;
- structural breakage сейчас не является главным bottleneck;
- в `Sentence` и `ShortText` samples старые heavy fields, особенно `museum`, все еще доминируют по surfacing;
- `observatory`, `sanatorium` и `hydroelectric` уже появляются, но чаще как secondary accent, а не как anchor-field всей сцены.

### Что это значит

- следующий шаг нужен не как еще один cleanup-pass;
- `manifold surfacing balance pass` уже начат;
- цель: поднять runtime visibility новых manifolds без тяжелой смены architecture и без выпадения в random mode cycling.

### Что уже изменено

- `observatory`, `sanatorium` и `hydroelectric` добавлены в `StrongManifoldTags`, поэтому теперь участвуют в dominant-manifold memory и field-affinity на равных правах со старыми mature fields;
- добавлен early surfacing bias для тонких first-wave manifolds, чтобы новые non-urban fields легче становились anchor-field в начале batch, а не только secondary accent позже;
- после этого `Sentence` samples уже начали чаще стартовать из `observatory`-сцен и держать их не как случайную вставку, а как локальную atmospheric anchor.
- затем добавлен short-text retention layer: opening manifold теперь мягко удерживается еще на 2-й и 3-й фразе `ShortText`, чтобы новые поля реже растворялись обратно в старой museum/urban gravity сразу после удачного старта.

### Что все еще стоит держать в уме

- остаются отдельные phrase-level слабости вроде `вернуть в расписание расписание коридорного отдыха`;
- встречаются и slot/grammar шероховатости уровня `на месте были запотевшую панель управления`;
- но это уже secondary defects по сравнению с общей imbalance-проблемой.

## What To Update

После каждого заметного `manifold`-шага нужно обновлять:

1. этот файл;
2. `docs/PROJECT_STATE.md`;
3. `docs/SESSION_INDEX.md`;
4. `docs/WORKLOG.md`;
5. при изменении общей стратегии - `docs/ROADMAP.md` и `docs/IDEAS_BACKLOG.md`.
