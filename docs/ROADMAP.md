# Roadmap

Этот roadmap фиксирует не только последовательность задач, но и порядок ценностей.

DreamAssembler не должен пытаться делать все сразу. Его развитие должно усиливать identity проекта: atmospheric reading, curated surrealism, quiet absurdity, semantic density, controlled weirdness и постепенное расширение в несколько coherent atmospheric spaces без ухода в жанровость.

## Зафиксированные приоритеты

Порядок приоритетов на ближайшее развитие:

1. reader-first UI;
2. atmospheric presentation;
3. curated datasets quality;
4. semantic coherence;
5. atmospheric thematic modes;
6. atmosphere system;
7. better narrative rhythm;
8. anti-generic filtering;
9. visual refinement;
10. optional morphology improvements later.

## Что не входит в ближайшие этапы

Следующие направления сознательно не считаются приоритетными:

- AI integration;
- database;
- plugin frameworks;
- cloud;
- online features;
- embeddings;
- overengineering ради "умного" вида архитектуры.

## Этап 1. Reader-First UI

Цель: сделать так, чтобы проект воспринимался как инструмент чтения атмосферных фрагментов, а не как control-heavy генератор.

Приоритеты:

- облегчить визуальный вес боковых панелей и служебных блоков;
- усилить центральность результата;
- увести действия над карточкой в более тихий режим;
- продумать fullscreen reading mode;
- развить phrase-centered presentation для коротких режимов;
- улучшить typography hierarchy, spacing и breathing room;
- постепенно ослабить оставшийся standard WPF / utility feel в controls и surfaces;
- сделать fullscreen reading mode одним из strongest ritual-reading сценариев проекта.

## Этап 2. Atmospheric Presentation

Цель: усилить подачу настроения без смены базовой архитектуры.

Приоритеты:

- мягкие visual reading modes;
- более зрелые atmospheric palettes;
- спокойные переходы между состояниями UI;
- более cinematic подача одиночной фразы;
- разные режимы визуального чтения для короткой фразы, предложения и короткого текста;
- distinct curated palettes вроде `archive-like`, `rainy`, `nocturnal`, `sterile`, `industrial`, `soft-light`, `fluorescent`, но без theme-sprawl;
- ослабление obvious cards, borders и visually loud interaction zones.

## Этап 3. Curated Datasets Quality

Цель: повысить качество выдачи через данные, а не через тяжелую новую логику.

Приоритеты:

- ручная curated-чистка слабых словарных элементов;
- anti-generic filtering;
- removal of low-image phrases;
- усиление recurring emotional motifs;
- тематические поднаборы для мест, состояний, объектов и концептов;
- чистка и отбор словесных CSV-лексиконов;
- развитие atmosphere-oriented curation вместо blind expansion.

## Этап 4. Semantic Coherence

Цель: сделать выдачу более собранной, не убивая dream-like ambiguity.

Приоритеты:

- дальнейшее развитие `slot` и `slotRequirements` там, где это реально помогает;
- расширение `compat:*` для слабых зон;
- мягкое усиление tag-based притяжения между элементами;
- снижение частоты пустых action/object/place сочетаний;
- защита от слишком generic и слишком шумных комбинаций.

Важно: coherence здесь значит не "полная логичность", а атмосферная убедительность.

## Этап 5. Atmospheric Thematic Modes

Цель: ввести управляемые tonal spaces без жанровой перегрузки.

Направления:

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

Следующий уровень после этого:

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

Их нужно выращивать медленно, как curated fields со своими motifs и emotional logic, а не как "новые режимы".

## Этап 6. Atmosphere System

Цель: начать мыслить генерацию как единое эмоциональное пространство.

Потенциальные механики:

- atmosphere tags;
- narrative temperature;
- atmosphere inheritance;
- semantic gravity;
- weighted emotional drift;
- atmosphere clusters;
- curated semantic layers.

Важно: это должен быть soft system поверх текущих тегов, слотов и весов, а не новая тяжеловесная подсистема.

Отдельно важно:

- atmosphere continuity важнее feature-count;
- atmospheric mixing важнее переключателя "режимов";
- symbolic interference важнее прямолинейной тематической маркировки;
- emotional drift важнее жесткой deterministic consistency.

## Этап 7. Better Narrative Rhythm

Цель: сделать `ShortText` более музыкальным и менее каркасным.

Приоритеты:

- снизить повторяемость одинаковых вводных конструкций;
- точнее управлять порядком narrative-ролей;
- вводить мягкие повторяющиеся мотивы;
- удерживать quiet progression вместо механической смены шаблонов;
- усиливать rhythm over correctness.

## Этап 8. Anti-Generic Filtering

Цель: убрать безликие и маловыразительные зоны выдачи.

Приоритеты:

- фильтрация low-image слов и фраз;
- сокращение слишком техничных и канцелярских слов вне осмысленного контекста;
- защита от шаблонной "литературности" без образа;
- curated blacklist и stop-lists для словесных режимов.

## Этап 9. Visual Refinement

Цель: довести UI до спокойной зрелости без превращения в дизайнерский аттракцион.

Приоритеты:

- мягче карточки и выделения;
- меньше визуальной агрессии;
- аккуратнее hover-поведение;
- лучшее соотношение текста и пустого пространства;
- export/share image cards как опциональный outward-facing слой.

## Этап 10. Optional Morphology Improvements Later

Цель: аккуратно улучшать форму языка только там, где это усиливает ритм и не убивает характер.

Правила:

- морфология не должна становиться центром архитектуры;
- не нужно лечить каждый сюрреалистический сдвиг;
- сначала данные, ритм и atmosphere, потом точечные грамматические обходы.

## Принципы отсечения

Если новая идея:

- делает проект умнее, но менее атмосферным;
- увеличивает количество настроек, но ухудшает чтение;
- повышает логическую связность, но убирает dream-like feeling;
- усложняет архитектуру, но не усиливает controllability и curated feel;
- делает UI "красивее", но разрушает visual silence и content dominance,

то это не приоритет, даже если идея технически "интересная".
