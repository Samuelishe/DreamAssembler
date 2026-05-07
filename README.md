# DreamAssembler

DreamAssembler is a Windows desktop application for generating Russian sentences, short texts, absurd ideas, word pairs, and short 3-4 word phrases with a rule-based engine.

It is intentionally not an AI text generator. The project combines JSON dictionaries, templates, tags, weights, and absurdity levels to produce results that can range from ordinary to surreal.

## Current state

- WPF application on .NET 10
- MVVM with CommunityToolkit.Mvvm
- Core generation logic separated from UI
- JSON-based dictionaries and templates
- Separate large CSV lexicons for word-pair and multi-word modes
- Fallback data when JSON files are missing or broken
- Reader-first UI with fullscreen reading mode, theme switching, and bundled reading fonts
- Bundled desktop icon embedded into the WPF window and the built Windows executable
- Growth direction centered on curated atmospheric fields rather than genre presets or AI storytelling
- Project documentation in [`docs`](docs)

## Product direction

DreamAssembler is not positioned as an AI storyteller, prompt shell, or random-surreal generator. It is moving toward an atmospheric reading instrument built on a rule-based engine and curated datasets.

The project grows through emotional-semantic manifolds rather than genre modes. The current corpus already contains several real atmospheric fields:

- archive / transport / bureaucracy
- nocturnal airports / fluorescent insomnia
- dead shopping malls / abandoned commerce / fluorescent afterhours
- impossible museums / echo exhibits / ceremonial curation
- recursive hospitality / nocturnal hotels / ceremonial check-in

These are not separate apps or hard modes. They are curated manifold layers that can later mix, drift, and interfere inside the same soft-coherence generation architecture.

## Repository guide

- Quick project index: [`docs/SESSION_INDEX.md`](docs/SESSION_INDEX.md)
- Architecture: [`docs/ARCHITECTURE.md`](docs/ARCHITECTURE.md)
- Current state and next steps: [`docs/PROJECT_STATE.md`](docs/PROJECT_STATE.md)
- Ideas backlog: [`docs/IDEAS_BACKLOG.md`](docs/IDEAS_BACKLOG.md)
- External data sources: [`docs/DATA_SOURCES.md`](docs/DATA_SOURCES.md)

## External data attribution

The project currently reuses lexical data from [`Badestrand/russian-dictionary`](https://github.com/Badestrand/russian-dictionary), especially the `nouns.csv`, `adjectives.csv`, `verbs.csv`, and `others.csv` files used for the word-pair and multi-word modes.

The current UI also vendors open-source fonts with Cyrillic support:

- [`Unbounded`](https://github.com/w3f/unbounded) for the display title font
- [`Inter`](https://github.com/google/fonts/tree/main/ofl/inter) for the main interface font
- [`Literata`](https://github.com/google/fonts/tree/main/ofl/literata) for the reading-focused result font
- [`Manrope`](https://github.com/google/fonts/tree/main/ofl/manrope) as an alternative reading font with a cleaner geometric tone

The font files and their OFL license texts are stored under [`DreamAssembler/Assets/Fonts`](DreamAssembler/Assets/Fonts). At runtime the app loads them from local `.ttf` files copied next to the build output, because this is more reliable for WPF than the pack URI approach that silently fell back to system fonts.

## UI direction

The application is moving toward a calmer reading surface rather than a control-heavy generator UI:

- typography-first result presentation
- phrase-centered layout for lexical modes
- selected-fragment spotlight for short phrase reading
- multiple curated light and dark themes instead of unbounded theme sprawl
- a small bundled font set chosen for Cyrillic support and mood fit
- a custom app icon that carries the same quiet surreal / infrastructural mood into Windows shell surfaces
- fullscreen reading as a core experience rather than a secondary preview mode

## Run locally

```bash
dotnet build DreamAssembler.sln
dotnet run --project DreamAssembler/DreamAssembler.csproj
```

## Validate data

```bash
dotnet run --project DreamAssembler.DataTools/DreamAssembler.DataTools.csproj
```

`DreamAssembler.DataTools` is also the main way to inspect corpus growth between sessions. It reports:

- total entry counts by category
- pack-level statistics for each dictionary set from `data-manifest.json`
- lexical CSV source sizes
- structural issues such as duplicate ids or duplicate texts

For live output inspection there is also a sample mode:

```bash
dotnet run --project DreamAssembler.DataTools/DreamAssembler.DataTools.csproj -- samples Sentence 8 Absurd
dotnet run --project DreamAssembler.DataTools/DreamAssembler.DataTools.csproj -- samples ShortText 6 Absurd
```

This is the intended workflow for future sessions: expand curated packs, check pack-level stats, then inspect real generated output rather than reasoning only from raw dictionary size.

## Stack

- C#
- .NET 10
- WPF
- MVVM
- CommunityToolkit.Mvvm
- JSON data files
