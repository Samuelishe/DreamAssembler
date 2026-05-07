# Формат словарей и шаблонов

## Формат JSON-словарей

Каждый файл в `Data/Dictionaries` содержит массив `entries`.

Пример:

```json
{
  "entries": [
    {
      "id": "rusty_elevator",
      "text": "ржавый лифт",
      "category": "object",
      "slot": "object_direct",
      "tags": ["city", "decay", "industrial"],
      "absurdity": 2,
      "weight": 1.0
    }
  ]
}
```

## Поля записи словаря

- `id` - уникальный строковый идентификатор.
- `text` - текст, который попадет в шаблон.
- `category` - категория записи.
- `slot` - безопасная шаблонная позиция внутри категории.
- `tags` - список тегов для смысловой группировки.
- `absurdity` - уровень абсурдности от 0 до 3.
- `weight` - базовый вес выбора.

## Поддерживаемые категории

- `character`
- `action`
- `object`
- `place`
- `condition`
- `twist`
- `atmosphere`
- `genre`
- `style`
- `emotion`
- `concept`

## Формат шаблонов

Файл `Data/Templates/templates.json` содержит массив `templates`.

Пример:

```json
{
  "templates": [
    {
      "id": "character_action_object_place_twist",
      "text": "{character} должен {action} {object} в месте: {place}, но {twist}.",
      "mode": "idea",
      "requiredCategories": ["character", "action", "object", "place", "twist"],
      "slotRequirements": {
        "action": "action_infinitive",
        "object": "object_direct"
      },
      "compositionRole": "setup",
      "tags": ["story", "absurd"],
      "minAbsurdity": 1,
      "maxAbsurdity": 3,
      "weight": 1.0
    }
  ]
}
```

## Поля шаблона

- `id` - уникальный идентификатор шаблона.
- `text` - шаблон с плейсхолдерами вида `{category}`.
- `mode` - режим генерации: `sentence`, `shortText`, `idea`.
- `requiredCategories` - список обязательных категорий.
- `slotRequirements` - необязательная карта "категория -> нужный slot".
- `compositionRole` - необязательная роль шаблона внутри `ShortText`, например `setup`, `development`, `reflection`, `meta`.
- `tags` - смысловые теги шаблона.
- `minAbsurdity` - нижняя граница применимости.
- `maxAbsurdity` - верхняя граница применимости.
- `weight` - базовый вес выбора шаблона.

## Правила тегов и весов

- теги не являются жестким фильтром в MVP;
- если у записи и шаблона есть совпадающие теги, вероятность выбора повышается;
- теги вида `compat:*` используются для смысловой совместимости между `action` и `object`;
- `weight` задает базовую вероятность выбора;
- близость `absurdity` записи к выбранному уровню также повышает вероятность;
- для `ShortText` шаблоны с одинаковой `compositionRole` не должны доминировать в одном тексте;
- на высоких уровнях абсурдности допускаются более конфликтные сочетания.

## Примечание по безопасным словоформам

Для русского языка на MVP лучше хранить данные не в "словарной" форме, а в форме, безопасной для конкретных шаблонных позиций.

Примеры:

- `object` может храниться как прямое дополнение: `свернутую карту города`;
- `condition` лучше хранить как короткое законченное состояние: `никто не доверял часам`;
- `emotion` лучше разводить по подтипам, например `emotion_group_state` для фраз вида `всем стало тревожно` и `emotion_observer_state` для фраз вида `все звучало тихо тревожно`.
- `twist` тоже лучше разводить по подтипам, например `twist_character_clause` для фраз с явной опорой на персонажа и `twist_general_clause` для общих правил и событий без местоименной привязки.
- `concept` лучше разводить по подтипам, например `concept_story_frame` для сюжетной темы и `concept_reflection` для рефлексивных хвостов вроде `это {concept}`.
- `condition` лучше разводить по подтипам контекста, например `condition_initial_state` для раннего состояния сцены, `condition_scene_detail` для наблюдаемой детали места и `condition_reveal_state` для более позднего странного раскрытия.
- `place` тоже можно делить по пунктуационному контексту, например `place_in` для обычной вставки и `place_in_clause` для мест с придаточной частью и закрывающей запятой.
- `action` и `object` лучше связывать через совместимые `compat:*`-теги, например `compat:repairable`, `compat:portable`, `compat:hideable`, чтобы действие не применялось к случайному объекту только из-за общей странности.

## Отдельный формат ассоциативного режима

Режим `До 4 слов пойдет` не использует JSON-шаблоны и не зависит от `absurdity`.

Он берет данные из больших CSV-словарей в:

- `DreamAssembler/Data/AssociationWords/Sources/openrussian-nouns.csv`
- `DreamAssembler/Data/AssociationWords/Sources/openrussian-adjectives.csv`

Сейчас генератор использует такую схему:

- выбирается существительное;
- определяется его род;
- выбирается от 1 до 3 прилагательных в той же форме рода;
- итоговая фраза имеет длину 2-4 слова.

Это отдельный поток данных, он не обязан укладываться в схему `category` / `slot` / `absurdity` основного генератора.
