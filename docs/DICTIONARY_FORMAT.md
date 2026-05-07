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
