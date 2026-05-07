using DreamAssembler.Core.Models;

namespace DreamAssembler.Core.Services;

/// <summary>
/// Загружает словарные данные для ассоциативного режима из внешних CSV-источников.
/// </summary>
public sealed class AssociationFragmentRepository
{
    private static readonly HashSet<string> AdjectiveStopWords =
    [
        "тот",
        "этот",
        "который",
        "какой",
        "такой",
        "весь",
        "мой",
        "твой",
        "свой",
        "наш",
        "ваш",
        "чей",
        "любой",
        "другой",
        "самый",
        "каждый",
        "всякий",
        "иной",
        "данный"
    ];

    /// <summary>
    /// Загружает словарные данные для ассоциативного режима из указанной папки.
    /// </summary>
    /// <param name="directoryPath">Путь к папке с CSV-источниками слов.</param>
    /// <returns>Результат загрузки с данными и статусом fallback.</returns>
    public RepositoryLoadResult<IReadOnlyList<AssociationFragmentEntry>> Load(string directoryPath)
    {
        if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
        {
            return new RepositoryLoadResult<IReadOnlyList<AssociationFragmentEntry>>
            {
                Data = FallbackDataProvider.GetAssociationFragments(),
                UsedFallback = true,
                Message = "Папка ассоциативных фрагментов не найдена. Использованы встроенные fallback-данные."
            };
        }

        try
        {
            var files = Directory.GetFiles(directoryPath, "*.csv", SearchOption.AllDirectories);
            var entries = new List<AssociationFragmentEntry>();

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);

                if (fileName.Contains("nouns", StringComparison.OrdinalIgnoreCase))
                {
                    entries.AddRange(ParseNouns(file));
                }
                else if (fileName.Contains("adjectives", StringComparison.OrdinalIgnoreCase))
                {
                    entries.AddRange(ParseAdjectives(file));
                }
            }

            entries = entries
                .Where(IsValid)
                .GroupBy(entry => $"{entry.Kind}::{entry.Text}", StringComparer.OrdinalIgnoreCase)
                .Select(group => group.First())
                .ToList();

            if (entries.Count == 0)
            {
                return new RepositoryLoadResult<IReadOnlyList<AssociationFragmentEntry>>
                {
                    Data = FallbackDataProvider.GetAssociationFragments(),
                    UsedFallback = true,
                    Message = "CSV-источники ассоциативного режима пусты или не содержат корректных записей. Использованы fallback-данные."
                };
            }

            return new RepositoryLoadResult<IReadOnlyList<AssociationFragmentEntry>>
            {
                Data = entries,
                UsedFallback = false,
                Message = "Словари ассоциативного режима успешно загружены."
            };
        }
        catch (IOException)
        {
            return new RepositoryLoadResult<IReadOnlyList<AssociationFragmentEntry>>
            {
                Data = FallbackDataProvider.GetAssociationFragments(),
                UsedFallback = true,
                Message = "Не удалось прочитать словари ассоциативного режима. Использованы fallback-данные."
            };
        }
        catch (UnauthorizedAccessException)
        {
            return new RepositoryLoadResult<IReadOnlyList<AssociationFragmentEntry>>
            {
                Data = FallbackDataProvider.GetAssociationFragments(),
                UsedFallback = true,
                Message = "Нет доступа к словарям ассоциативного режима. Использованы fallback-данные."
            };
        }

        catch (InvalidDataException)
        {
            return new RepositoryLoadResult<IReadOnlyList<AssociationFragmentEntry>>
            {
                Data = FallbackDataProvider.GetAssociationFragments(),
                UsedFallback = true,
                Message = "CSV-источники ассоциативного режима повреждены или имеют неверный формат. Использованы fallback-данные."
            };
        }
    }

    private static IReadOnlyList<AssociationFragmentEntry> ParseNouns(string filePath)
    {
        var rows = File.ReadLines(filePath).ToList();
        if (rows.Count <= 1)
        {
            return [];
        }

        var header = BuildHeaderMap(rows[0]);
        var entries = new List<AssociationFragmentEntry>();

        foreach (var row in rows.Skip(1))
        {
            var columns = row.Split('\t');
            var gender = GetColumnValue(columns, header, "gender");
            if (!IsSupportedNounGender(gender))
            {
                continue;
            }

            if (GetColumnValue(columns, header, "pl_only") == "1")
            {
                continue;
            }

            var nominative = NormalizeWord(GetColumnValue(columns, header, "sg_nom"));
            if (!IsSingleRussianWord(nominative))
            {
                continue;
            }

            entries.Add(new AssociationFragmentEntry
            {
                Id = $"noun_{gender}_{nominative}",
                Text = nominative,
                Kind = $"noun_{gender}",
                Weight = 1.0
            });
        }

        return entries;
    }

    private static IReadOnlyList<AssociationFragmentEntry> ParseAdjectives(string filePath)
    {
        var rows = File.ReadLines(filePath).ToList();
        if (rows.Count <= 1)
        {
            return [];
        }

        var header = BuildHeaderMap(rows[0]);
        var entries = new List<AssociationFragmentEntry>();

        foreach (var row in rows.Skip(1))
        {
            var columns = row.Split('\t');
            var bare = NormalizeWord(GetColumnValue(columns, header, "bare"));
            if (!LooksLikeAdjectiveLemma(bare) || AdjectiveStopWords.Contains(bare))
            {
                continue;
            }

            var masculine = NormalizeWord(GetColumnValue(columns, header, "decl_m_nom"));
            var feminine = NormalizeWord(GetColumnValue(columns, header, "decl_f_nom"));
            var neuter = NormalizeWord(GetColumnValue(columns, header, "decl_n_nom"));

            if (!IsSingleRussianWord(masculine) || !IsSingleRussianWord(feminine) || !IsSingleRussianWord(neuter))
            {
                continue;
            }

            entries.Add(new AssociationFragmentEntry
            {
                Id = $"adjective_m_{masculine}",
                Text = masculine,
                Kind = "adjective_m",
                Weight = 1.0
            });

            entries.Add(new AssociationFragmentEntry
            {
                Id = $"adjective_f_{feminine}",
                Text = feminine,
                Kind = "adjective_f",
                Weight = 1.0
            });

            entries.Add(new AssociationFragmentEntry
            {
                Id = $"adjective_n_{neuter}",
                Text = neuter,
                Kind = "adjective_n",
                Weight = 1.0
            });
        }

        return entries;
    }

    private static bool IsValid(AssociationFragmentEntry entry)
    {
        return !string.IsNullOrWhiteSpace(entry.Id)
               && !string.IsNullOrWhiteSpace(entry.Text)
               && !string.IsNullOrWhiteSpace(entry.Kind)
               && entry.Weight > 0d;
    }

    private static Dictionary<string, int> BuildHeaderMap(string headerLine)
    {
        return headerLine
            .Split('\t')
            .Select((name, index) => new { Name = name, Index = index })
            .ToDictionary(item => item.Name, item => item.Index, StringComparer.OrdinalIgnoreCase);
    }

    private static string GetColumnValue(IReadOnlyList<string> columns, IReadOnlyDictionary<string, int> header, string columnName)
    {
        if (!header.TryGetValue(columnName, out var index) || index < 0 || index >= columns.Count)
        {
            return string.Empty;
        }

        return columns[index];
    }

    private static string NormalizeWord(string value)
    {
        return value
            .Replace("'", string.Empty, StringComparison.Ordinal)
            .Trim()
            .ToLowerInvariant();
    }

    private static bool IsSupportedNounGender(string gender)
    {
        return string.Equals(gender, "m", StringComparison.OrdinalIgnoreCase)
               || string.Equals(gender, "f", StringComparison.OrdinalIgnoreCase)
               || string.Equals(gender, "n", StringComparison.OrdinalIgnoreCase);
    }

    private static bool LooksLikeAdjectiveLemma(string value)
    {
        return value.EndsWith("ый", StringComparison.Ordinal)
               || value.EndsWith("ий", StringComparison.Ordinal)
               || value.EndsWith("ой", StringComparison.Ordinal);
    }

    private static bool IsSingleRussianWord(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        foreach (var character in value)
        {
            if (character is '-' or 'ё')
            {
                continue;
            }

            if (character < 'а' || character > 'я')
            {
                return false;
            }
        }

        return true;
    }
}
