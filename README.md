# DreamAssembler

DreamAssembler is a Windows desktop application for generating Russian sentences, short texts, and absurd ideas with a rule-based engine.

It is intentionally not an AI text generator. The project combines JSON dictionaries, templates, tags, weights, and absurdity levels to produce results that can range from ordinary to surreal.

## Current state

- WPF application on .NET 10
- MVVM with CommunityToolkit.Mvvm
- Core generation logic separated from UI
- JSON-based dictionaries and templates
- Fallback data when JSON files are missing or broken
- Project documentation in [`docs`](docs)

## Repository guide

- Quick project index: [`docs/SESSION_INDEX.md`](docs/SESSION_INDEX.md)
- Architecture: [`docs/ARCHITECTURE.md`](docs/ARCHITECTURE.md)
- Current state and next steps: [`docs/PROJECT_STATE.md`](docs/PROJECT_STATE.md)
- Ideas backlog: [`docs/IDEAS_BACKLOG.md`](docs/IDEAS_BACKLOG.md)

## Run locally

```bash
dotnet build DreamAssembler.sln
dotnet run --project DreamAssembler/DreamAssembler.csproj
```

## Validate data

```bash
dotnet run --project DreamAssembler.DataTools/DreamAssembler.DataTools.csproj
```

## Stack

- C#
- .NET 10
- WPF
- MVVM
- CommunityToolkit.Mvvm
- JSON data files
