# Manifold State

Этот файл фиксирует не только corpus balance, но и runtime reading того, как manifolds реально всплывают в генерации.

Важно: corpus balance и runtime balance - не одно и то же.

Проект уже дошел до стадии, где manifold может быть хорошо выращен в данных, но все еще звучать слабо или неавтономно в runtime.

## Как читать этот файл

- `sets` - сколько dictionary sets в `data-manifest.json` относятся к manifold;
- `entries` - сколько dictionary entries несут strong manifold tag;
- `categories` - покрытие по ключевым runtime categories;
- `corpus status` - зрелость поля как data ecology;
- `runtime status` - как поле ведет себя по текущим audit/report срезам;
- `next priority` - что делать дальше: rebalance, deepening, expansion или hold.

Важно:

- `archive / transport / bureaucracy` остается old-core field, но не считается здесь отдельным isolated manifold по тем же правилам, что новые strong-manifold packs;
- текущая задача проекта не в том, чтобы выровнять manifolds по статистике, а в том, чтобы каждый выросший manifold мог звучать как самостоятельное emotional-atmospheric space.

## Snapshot

Состояние на:

- app version: `0.2.1.0`
- data version: `0.6.11`
- corpus: `1268` entries
- templates: `55`

Подтверждено через:

- `DreamAssembler/Data/data-manifest.json`
- `dotnet run --project DreamAssembler.DataTools/DreamAssembler.DataTools.csproj`
- runtime reports `audit-shorttext-e.json`, `audit-shorttext-f.json`, `audit-shorttext-long-f.json`, `audit-mall-g.json`

## Corpus Balance

### Mature / heavy fields

| manifold | sets | entries | categories | corpus status | next priority |
|---|---:|---:|---|---|---|
| `museum` | 22 | 161 | action, atmosphere, character, concept, condition, object, place, twist | deepened second-wave-plus | не расширять вслепую; нужен cadence-autonomy deepening |
| `hospitality` | 16 | 114 | action, atmosphere, character, concept, condition, object, place, twist | strong second-wave | hold, затем выборочный deepening |
| `mall` | 18 | 138 | action, atmosphere, concept, condition, object, place, twist + existing character layer | deepened strong second-wave | проверить recognizability на следующем long audit; не трогать runtime вслепую |

### Stable second-wave fields

| manifold | sets | entries | categories | corpus status | next priority |
|---|---:|---:|---|---|---|
| `airport` | 14 | 98 | action, atmosphere, character, concept, condition, object, place, twist | stable second-wave | hold |

### Early second-wave non-urban fields

| manifold | sets | entries | categories | corpus status | next priority |
|---|---:|---:|---|---|---|
| `observatory` | 16 | 92 | action, atmosphere, character, concept, condition, object, place, twist | early second-wave | не форсировать вслепую; следить за cadence autonomy в long audit |
| `sanatorium` | 16 | 92 | action, atmosphere, character, concept, condition, object, place, twist | early second-wave | hold; возможен later ecology deepening |
| `hydroelectric` | 16 | 92 | action, atmosphere, character, concept, condition, object, place, twist | early second-wave | runtime уже ожил; later deepening возможен |
| `weather_systems` | 16 | 92 | action, atmosphere, character, concept, condition, object, place, twist | early second-wave | runtime уже ожил; later deepening возможен |
| `coastal_fog` | 16 | 92 | action, atmosphere, character, concept, condition, object, place, twist | early second-wave | hold; следить за cadence identity |

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
Самый тяжелый manifold в corpus. Проблема уже не в объеме, а в том, что он часто тянет на себя runtime gravity и при этом не всегда удерживает собственную cadence autonomy.

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
Плотный strong second-wave manifold. Сейчас не главный bottleneck и не первый кандидат на expansion.

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
Стабильный second-wave слой. Достаточно зрелый для паузы.

### `mall`

- action: 14
- atmosphere: 15
- character: 8
- concept: 18
- condition: 22
- object: 22
- place: 22
- twist: 17

Комментарий:
Mall больше не выглядит тонким second-wave слоем. Последний deepening-pass добавил afterhours maintenance, fluorescent residue, decorative exhaustion, closed arcade memory, service-corridor silence и failed-commercial stillness. Следующий вопрос уже не в corpus mass, а в runtime recognizability.

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
По corpus уже нормальный early second-wave manifold. По short `120` snapshot он может выпадать, но `500`-report показывает, что это не обязательно systemic failure. Отдельный observatory-pass пока не нужен.

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
Сильный quiet/institutional manifold. Короткие snapshots могут дрейфовать в его сторону, поэтому оценивать лучше по длинным series.

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
Корпус уже достаточный, а после cadence/autonomy passes runtime показал заметное восстановление manifold-local rhythm.

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
Хорошо держится как procedural climatology ecology. Один из главных бенефициаров runtime cadence work.

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
Данные уже достаточны для hold-состояния. Следующий смысловой рост нужен только после очередного runtime-check.

## Runtime Reading

### Что показывают последние diagnostics

Нужно различать короткие и длинные срезы:

- `120` outputs полезны для быстрых before/after сравнений;
- `500` outputs полезнее для оценки устойчивой ecology;
- короткие snapshots могут сильно дрейфовать в сторону одного поля.

### Последний важный срез

`audit-shorttext-long-f.json` (`ShortText`, `500`, `Absurd`) показал:

- dominant pressure lead: `museum:quiet` на `19.8%`, а не универсальный `quiet`;
- lead manifold surfacing: `museum` на `20.0%`;
- primary cadence lead: `static_observation` на `29.8%`, то есть он уже ослаблен по сравнению с более ранними `~40%`, но все еще тяжелый;
- `hydroelectric` preferred cadence activation: `97.5%`;
- `weather_systems` preferred cadence activation: `94.0%`;
- `observatory` preferred cadence activation: `95.0%`;
- `museum` preferred cadence activation: `59.0%`;
- legacy-heavier outputs: `26.2%`.

### Практический вывод

- проблема сейчас не в нехватке non-urban manifolds;
- `weather_systems`, `hydroelectric` и `observatory` уже показывают сильную runtime cadence autonomy;
- contextual quiet differentiation сработала: universal `quiet` перестал быть единственным carrying pressure;
- новый bottleneck смещается в `museum`: он все еще surface-ится тяжело, но cadence autonomy у него заметно слабее;
- old-core gravity остается присутствующей, но уже не выглядит тотально доминирующей.
- новый `mall` pack увеличил corpus depth, но короткий `120` report после него оказался sanatorium-heavy и не дает надежного сигнала по mall runtime; это пока не повод к rebalance.

## Distinctions That Matter

### Corpus balance

Показывает, сколько manifold выращен в данных.

Сейчас здесь основные проблемы уже не у `weather_systems` или `hydroelectric`, а скорее у необходимости выбирать осмысленный следующий deepening candidate, а не blindly adding packs.

### Runtime surfacing balance

Показывает, какие поля реально всплывают в outputs.

Сейчас это нужно оценивать не по одному `samples`-запуску, а по `report` и `compare`.

### Cadence autonomy

Показывает, удерживает ли manifold не только nouns, но и собственный temporal skeleton.

Сейчас именно cadence autonomy отличает зрелый runtime pass от простого noun-overlay.

### Pressure dominance

Показывает, какой emotional pressure реально держит runtime.

Сейчас главный прогресс в том, что pressure уже может быть `museum:quiet`, `sanatorium:quiet` и другими contextual forms, а не только generic `quiet`.

## Current Bottlenecks

- `museum` остается самым тяжелым corpus-field и still under-activates own cadence relative to its weight;
- `mall` уже deepened в данных, но его runtime identity после нового pack еще не подтверждена длинным dedicated audit;
- `static_observation` больше не монополист, но все еще самый тяжелый primary cadence;
- cadence repetition нужно продолжать отслеживать после каждого targeted pass;
- short `120` snapshots все еще могут переоценивать локальный drift;
- old-core legacy gravity не исчезла полностью и требует осторожного контроля;
- diagnostics нельзя превращать в KPI, иначе runtime начнет стерилизоваться.

## Next Expansion / Deepening Priorities

Порядок на ближайшее время:

1. не открывать random bulk wave;
2. сначала закончить текущий `runtime tuning` цикл вокруг `museum cadence autonomy` и contextual silence quality;
3. затем отдельно проверить новый `mall` corpus-layer на более длинном mall-focused audit, не путая short-batch drift с реальным regression;
4. только после этого выбирать следующий content-side candidate;
5. `museum` deepening делать только точечно и только если он помогает cadence identity, а не просто увеличивает corpus mass;
6. новые manifolds открывать только после очередного audit-backed runtime checkpoint.

## Working Rule

После каждого заметного manifold/runtime шага обновлять:

1. `docs/MANIFOLD_STATE.md`
2. `docs/PROJECT_STATE.md`
3. `docs/SESSION_INDEX.md`
4. `docs/WORKLOG.md`
5. при смене общих приоритетов - `docs/ROADMAP.md` и `docs/IDEAS_BACKLOG.md`
