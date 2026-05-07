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
- `tags` - смысловые теги шаблона.
- `minAbsurdity` - нижняя граница применимости.
- `maxAbsurdity` - верхняя граница применимости.
- `weight` - базовый вес выбора шаблона.

## Правила тегов и весов

- теги не являются жестким фильтром в MVP;
- если у записи и шаблона есть совпадающие теги, вероятность выбора повышается;
- `weight` задает базовую вероятность выбора;
- близость `absurdity` записи к выбранному уровню также повышает вероятность;
- на высоких уровнях абсурдности допускаются более конфликтные сочетания.

