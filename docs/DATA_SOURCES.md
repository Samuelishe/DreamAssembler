# Источники данных

## Словесные режимы

На 2026-05-07 для режимов `Словосочетание` и `Несколько слов` в проект загружены внешние словари слов:

- `DreamAssembler/Data/AssociationWords/Sources/openrussian-nouns.csv`
- `DreamAssembler/Data/AssociationWords/Sources/openrussian-adjectives.csv`
- `DreamAssembler/Data/AssociationWords/Sources/openrussian-verbs.csv`
- `DreamAssembler/Data/AssociationWords/Sources/openrussian-others.csv`

Источник:

- репозиторий `Badestrand/russian-dictionary`
- URL: `https://github.com/Badestrand/russian-dictionary`
- лицензия репозитория: `CC-BY-SA-4.0`

Практическая причина выбора:

- в наборе уже есть большие CSV-словари;
- для существительных есть поле `gender`;
- для прилагательных есть отдельные формы по роду;
- для глаголов есть формы прошедшего времени по роду;
- этого достаточно, чтобы собирать 2 слова и короткие фразы в 3-4 слова с базовым согласованием по роду без полноценной морфологии.

## Что из этих данных используется сейчас

- из существительных берутся одиночные словоформы в единственном числе;
- из прилагательных берутся формы `decl_m_nom`, `decl_f_nom`, `decl_n_nom`;
- из глаголов берутся формы `past_m`, `past_f`, `past_n`;
- `others.csv` пока хранится в проекте как запас на будущее и загружается в общий лексический набор как `other_raw`, но в схемах генерации пока не используется;
- служебные слова и слишком плохие кандидаты отфильтровываются в коде репозитория.

## Что важно помнить

- это не curated художественный словарь, а большой сырой лексический источник;
- словесные режимы намеренно допускают случайность и слабую семантическую связность;
- для основного генератора `Sentence`, `ShortText` и `Idea` эти CSV-словари пока не используются.

## Шрифты интерфейса

На 2026-05-07 в проект добавлены бесплатные шрифты с кириллицей для UI:

- `DreamAssembler/Assets/Fonts/Unbounded-Black.ttf`
- `DreamAssembler/Assets/Fonts/Inter-Variable.ttf`
- `DreamAssembler/Assets/Fonts/Literata-Variable.ttf`
- `DreamAssembler/Assets/Fonts/Manrope-Variable.ttf`

Источники:

- `Unbounded`: `https://github.com/w3f/unbounded`
- `Inter`: `https://github.com/google/fonts/tree/main/ofl/inter`
- `Literata`: `https://github.com/google/fonts/tree/main/ofl/literata`
- `Manrope`: `https://github.com/google/fonts/tree/main/ofl/manrope`

Лицензии:

- `DreamAssembler/Assets/Fonts/Unbounded-OFL.txt`
- `DreamAssembler/Assets/Fonts/Inter-OFL.txt`
- `DreamAssembler/Assets/Fonts/Literata-OFL.txt`
- `DreamAssembler/Assets/Fonts/Manrope-OFL.txt`

Практическая причина выбора:

- `Unbounded` хорошо подходит для жирного выразительного заголовка приложения;
- `Inter` подходит для плотного интерфейса и длинного чтения;
- `Literata` подходит для более мягкого книжного чтения результатов;
- `Manrope` подходит для более чистого современного reading-mode без слишком системного ощущения;
- все шрифты поддерживают кириллицу и допускают хранение в проекте по OFL.

Техническая заметка для WPF:

- `Literata` внутри TTF определяется как family `Literata 12pt`;
- при загрузке шрифтов из файлов рядом с приложением нужно использовать именно это internal family name, иначе WPF может тихо откатиться к fallback-шрифту;
- для стабильного поведения проект сейчас использует file-based загрузку локальных `.ttf`, а не pack URI.
