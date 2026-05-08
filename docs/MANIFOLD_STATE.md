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

Состояние на `data-manifest.json = 0.6.10`.

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
| `observatory` | 16 | 92 | action, atmosphere, character, concept, condition, object, place, twist | early second-wave | следующий шаг не срочный surfacing, а выборочный ecology growth позже |
| `sanatorium` | 16 | 92 | action, atmosphere, character, concept, condition, object, place, twist | early second-wave | следующий шаг не срочный surfacing, а выборочный ecology growth позже |
| `hydroelectric` | 16 | 92 | action, atmosphere, character, concept, condition, object, place, twist | early second-wave | следующий шаг не срочный surfacing, а выборочный ecology growth позже |
| `weather_systems` | 16 | 92 | action, atmosphere, character, concept, condition, object, place, twist | early second-wave | следующий шаг не срочный surfacing, а выборочный ecology growth позже |
| `coastal_fog` | 16 | 92 | action, atmosphere, character, concept, condition, object, place, twist | early second-wave | следующий шаг не срочный surfacing, а выборочный ecology growth позже |

## Balance Reading

### Что видно сразу

- `museum` сейчас самый тяжелый manifold в corpus: `161` tagged entries и `22` sets;
- `hospitality` уже догнал плотное mature-state;
- `airport` и `mall` остаются рабочими second-wave полями;
- `observatory` уже вышел из thin first-wave состояния: `92` entries и `16` sets.
- `sanatorium` тоже уже вышел из thin first-wave состояния: `92` entries и `16` sets.
- `hydroelectric` тоже уже вышел из thin first-wave состояния: `92` entries и `16` sets.
- `weather_systems` тоже уже вышел из thin first-wave состояния: `92` entries и `16` sets.
- `coastal_fog` тоже уже вышел из thin first-wave состояния: `92` entries и `16` sets.
- `procedural weather systems` входит в ту же first-wave группу: `46` entries и `8` sets.
- `coastal fog logistics` входит в ту же first-wave группу: `46` entries и `8` sets.

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

### `coastal_fog`

- action: 12
- atmosphere: 10
- character: 12
- concept: 12
- condition: 12
- object: 12
- place: 12
- twist: 10

Комментарий:
`coastal_fog` уже получил второй слой вокруг quay ledgers, lighthouse routine, wet rope storage, tide bulletins и harbor hush.

### `weather_systems`

- action: 12
- atmosphere: 10
- character: 12
- concept: 12
- condition: 12
- object: 12
- place: 12
- twist: 10

Комментарий:
`weather_systems` уже получил второй слой вокруг pressure charts, barograph paper, dawn bulletins, calibration desks и bureaucratic meteorology.

### `observatory`

- action: 12
- atmosphere: 10
- character: 12
- concept: 12
- condition: 12
- object: 12
- place: 12
- twist: 10

Комментарий:
`observatory` уже перестал быть просто first-wave sketch и получил второй слой вокруг calibration, signal bureaucracy, auxiliary mirrors и false-dawn procedure.

### `sanatorium`

- action: 12
- atmosphere: 10
- character: 12
- concept: 12
- condition: 12
- object: 12
- place: 12
- twist: 10

Комментарий:
`sanatorium` уже получил второй слой вокруг inhalation routine, balcony regime, temperature sheets, blanket storage и pre-morning care.

### `hydroelectric`

- action: 12
- atmosphere: 10
- character: 12
- concept: 12
- condition: 12
- object: 12
- place: 12
- twist: 10

Комментарий:
`hydroelectric` уже получил второй слой вокруг relay rooms, intake gates, lubrication routine, oil shift logs и machine-hum bureaucracy.

## Current Growth Guidance

Следующие решения должны приниматься так:

1. не расширять дальше `museum` просто потому, что он и так хорошо звучит;
2. не считать новые manifolds "слабыми" до surfacing-pass;
3. текущая non-urban expansion wave уже целиком переведена в early second-wave состояние;
4. следующий growth-step логичнее выбирать либо как third pack для strongest new field, либо как открытие следующего нового manifold;
4. отдельно держать в уме, что `mall` остается кандидатом на очередной deepening-pass раньше, чем `airport` или `hospitality`;
5. после каждого new second-wave шага снова проверять не только counts, но и реальное surfacing в `Sentence` и `ShortText`.

## Runtime Observations

Срез по live generation после `0.6.5`:

- `dotnet test DreamAssembler.Core.Tests/DreamAssembler.Core.Tests.csproj` проходит: `43/43`;
- structural breakage сейчас не является главным bottleneck;
- в `Sentence` samples новые manifolds уже поднимаются как anchor-fields заметно чаще, чем раньше: это видно не только по `observatory / sanatorium / hydroelectric`, но и по `weather_systems` и `coastal_fog`;
- в `ShortText` старые heavy fields и generic cross-field frames все еще слишком легко возвращаются в середине текста;
- сохраняются отдельные phrase-level шероховатости уровня `вернуть в расписание расписание коридорного отдыха` и `На месте были папку с мокрыми заявлениями`.
- после `foundation suppression / anti-repeat pass` старые generic packs стали заметно реже перехватывать foreground через `чайник / мокрые газеты / пригородные поезда`, но из-за этого еще яснее стал виден следующий bottleneck: внутри already-strong fields слишком часто побеждает именно `museum`.

### Что это значит

- следующий шаг нужен не как еще один cleanup-pass;
- `manifold surfacing balance pass` уже начат;
- цель: поднять runtime visibility новых manifolds без тяжелой смены architecture и без выпадения в random mode cycling.

### Что уже изменено

- `observatory`, `sanatorium` и `hydroelectric` добавлены в `StrongManifoldTags`, поэтому теперь участвуют в dominant-manifold memory и field-affinity на равных правах со старыми mature fields;
- добавлен early surfacing bias для тонких first-wave manifolds, чтобы новые non-urban fields легче становились anchor-field в начале batch, а не только secondary accent позже;
- после этого `Sentence` samples уже начали чаще стартовать из `observatory`-сцен и держать их не как случайную вставку, а как локальную atmospheric anchor.
- затем добавлен short-text retention layer: opening manifold теперь мягко удерживается еще на 2-й и 3-й фразе `ShortText`, чтобы новые поля реже растворялись обратно в старой museum/urban gravity сразу после удачного старта;
- затем `weather_systems` и `coastal_fog` вошли в ту же thin first-wave группу и тоже были подключены к strong-manifold layer, поэтому теперь new non-urban batch может удерживать уже не три, а пять distinct anchor-fields.

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
