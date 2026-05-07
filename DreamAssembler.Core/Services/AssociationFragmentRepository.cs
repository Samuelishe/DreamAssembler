using DreamAssembler.Core.Models;

namespace DreamAssembler.Core.Services;

/// <summary>
/// Загружает словарные данные для словесных режимов из внешних CSV-источников.
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
        "данный",
        "венерический",
        "оргазмический",
        "водородистый",
        "синкретический",
        "прямокрылый",
        "перегрузочный",
        "вышеуказанный",
        "неблагопристойный",
        "маниакальный",
        "читательский",
        "псиный",
        "выборный",
        "козлиный",
        "дынный",
        "переимчивый",
        "можжевёловый",
        "малометражный",
        "опустошённый",
        "зажаренный",
        "печёночный",
        "пепсиновый",
        "наторелый",
        "сальный",
        "окислительный",
        "дражайший",
        "кукушечий",
        "вожделенный",
        "мавританский",
        "муниципальный",
        "кумулятивный",
        "льнопрядильный",
        "многоступенчатый",
        "ацетоновый",
        "вирулентный",
        "сексуальный"
    ];

    private static readonly HashSet<string> NounStopWords =
    [
        "заболеваемость",
        "идолопоклонничество",
        "фабула",
        "гладь",
        "жалостливость",
        "епископат",
        "дифирамб",
        "старение",
        "обзаведение",
        "поправка",
        "причастие",
        "абитуриент",
        "корь",
        "бенуар",
        "либерал",
        "нищенство",
        "отстранение",
        "буксование",
        "огневик",
        "бензол",
        "молодечество",
        "разрушение",
        "обжалование",
        "ухудшение",
        "трансляция",
        "сверление",
        "бескультурье"
    ];

    private static readonly HashSet<string> VerbStopWords =
    [
        "быть",
        "мочь",
        "стать",
        "являться",
        "выщелачивать",
        "приживлять",
        "перемахивать",
        "вперяться"
    ];

    /// <summary>
    /// Загружает словарные данные для словесных режимов из указанной папки.
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
                Message = "Папка словесных словарей не найдена. Использованы встроенные fallback-данные."
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
                else if (fileName.Contains("verbs", StringComparison.OrdinalIgnoreCase))
                {
                    entries.AddRange(ParseVerbs(file));
                }
                else if (fileName.Contains("others", StringComparison.OrdinalIgnoreCase))
                {
                    entries.AddRange(ParseOthers(file));
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
                    Message = "CSV-источники словесных режимов пусты или не содержат корректных записей. Использованы fallback-данные."
                };
            }

            return new RepositoryLoadResult<IReadOnlyList<AssociationFragmentEntry>>
            {
                Data = entries,
                UsedFallback = false,
                Message = "Словари словесных режимов успешно загружены."
            };
        }
        catch (IOException)
        {
            return new RepositoryLoadResult<IReadOnlyList<AssociationFragmentEntry>>
            {
                Data = FallbackDataProvider.GetAssociationFragments(),
                UsedFallback = true,
                Message = "Не удалось прочитать словари словесных режимов. Использованы fallback-данные."
            };
        }
        catch (UnauthorizedAccessException)
        {
            return new RepositoryLoadResult<IReadOnlyList<AssociationFragmentEntry>>
            {
                Data = FallbackDataProvider.GetAssociationFragments(),
                UsedFallback = true,
                Message = "Нет доступа к словарям словесных режимов. Использованы fallback-данные."
            };
        }

        catch (InvalidDataException)
        {
            return new RepositoryLoadResult<IReadOnlyList<AssociationFragmentEntry>>
            {
                Data = FallbackDataProvider.GetAssociationFragments(),
                UsedFallback = true,
                Message = "CSV-источники словесных режимов повреждены или имеют неверный формат. Использованы fallback-данные."
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
            if (!IsSingleRussianWord(nominative) || !IsAllowedNounLemma(nominative))
            {
                continue;
            }

            entries.Add(new AssociationFragmentEntry
            {
                Id = $"noun_{gender}_{nominative}",
                Text = nominative,
                Kind = $"noun_{gender}",
                Weight = CalculateLexicalWeight($"noun_{gender}", nominative)
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
            if (!LooksLikeAdjectiveLemma(bare) || AdjectiveStopWords.Contains(bare) || !IsAllowedAdjectiveLemma(bare))
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
                Weight = CalculateLexicalWeight("adjective_m", masculine)
            });

            entries.Add(new AssociationFragmentEntry
            {
                Id = $"adjective_f_{feminine}",
                Text = feminine,
                Kind = "adjective_f",
                Weight = CalculateLexicalWeight("adjective_f", feminine)
            });

            entries.Add(new AssociationFragmentEntry
            {
                Id = $"adjective_n_{neuter}",
                Text = neuter,
                Kind = "adjective_n",
                Weight = CalculateLexicalWeight("adjective_n", neuter)
            });
        }

        return entries;
    }

    private static IReadOnlyList<AssociationFragmentEntry> ParseVerbs(string filePath)
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
            if (!IsSingleRussianWord(bare) || VerbStopWords.Contains(bare) || !IsAllowedVerbLemma(bare))
            {
                continue;
            }

            var pastMasculine = NormalizeWord(GetColumnValue(columns, header, "past_m"));
            var pastFeminine = NormalizeWord(GetColumnValue(columns, header, "past_f"));
            var pastNeuter = NormalizeWord(GetColumnValue(columns, header, "past_n"));

            if (IsSingleRussianWord(pastMasculine))
            {
                entries.Add(new AssociationFragmentEntry
                {
                    Id = $"verb_past_m_{pastMasculine}",
                    Text = pastMasculine,
                    Kind = "verb_past_m",
                    Weight = CalculateLexicalWeight("verb_past_m", pastMasculine)
                });
            }

            if (IsSingleRussianWord(pastFeminine))
            {
                entries.Add(new AssociationFragmentEntry
                {
                    Id = $"verb_past_f_{pastFeminine}",
                    Text = pastFeminine,
                    Kind = "verb_past_f",
                    Weight = CalculateLexicalWeight("verb_past_f", pastFeminine)
                });
            }

            if (IsSingleRussianWord(pastNeuter))
            {
                entries.Add(new AssociationFragmentEntry
                {
                    Id = $"verb_past_n_{pastNeuter}",
                    Text = pastNeuter,
                    Kind = "verb_past_n",
                    Weight = CalculateLexicalWeight("verb_past_n", pastNeuter)
                });
            }
        }

        return entries;
    }

    private static IReadOnlyList<AssociationFragmentEntry> ParseOthers(string filePath)
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
            if (!IsSingleRussianWord(bare))
            {
                continue;
            }

            entries.Add(new AssociationFragmentEntry
            {
                Id = $"other_raw_{bare}",
                Text = bare,
                Kind = "other_raw",
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

    private static bool IsAllowedNounLemma(string value)
    {
        if (NounStopWords.Contains(value))
        {
            return false;
        }

        if (value.Length >= 11
            && (value.EndsWith("ость", StringComparison.Ordinal)
                || value.EndsWith("ение", StringComparison.Ordinal)
                || value.EndsWith("ание", StringComparison.Ordinal)
                || value.EndsWith("чество", StringComparison.Ordinal)
                || value.EndsWith("ичество", StringComparison.Ordinal)))
        {
            return false;
        }

        return true;
    }

    private static bool IsAllowedAdjectiveLemma(string value)
    {
        if (value.Length >= 12
            && (value.EndsWith("ический", StringComparison.Ordinal)
                || value.EndsWith("ическая", StringComparison.Ordinal)
                || value.EndsWith("ическое", StringComparison.Ordinal)
                || value.EndsWith("истический", StringComparison.Ordinal)))
        {
            return false;
        }

        return true;
    }

    private static bool IsAllowedVerbLemma(string value)
    {
        if (value.Length >= 12
            && (value.EndsWith("ировать", StringComparison.Ordinal)
                || value.EndsWith("изировать", StringComparison.Ordinal)))
        {
            return false;
        }

        return true;
    }

    private static double CalculateLexicalWeight(string kind, string text)
    {
        var weight = 1.0d;

        if (kind.StartsWith("noun_", StringComparison.OrdinalIgnoreCase))
        {
            if (text.Length >= 10)
            {
                weight *= 0.82d;
            }

            if (text.Length >= 13)
            {
                weight *= 0.7d;
            }

            if (text.EndsWith("ость", StringComparison.Ordinal)
                || text.EndsWith("ение", StringComparison.Ordinal)
                || text.EndsWith("ание", StringComparison.Ordinal)
                || text.EndsWith("чество", StringComparison.Ordinal))
            {
                weight *= 0.55d;
            }
        }
        else if (kind.StartsWith("adjective_", StringComparison.OrdinalIgnoreCase))
        {
            if (text.Length >= 10)
            {
                weight *= 0.8d;
            }

            if (text.Length >= 13)
            {
                weight *= 0.68d;
            }
        }
        else if (kind.StartsWith("verb_past_", StringComparison.OrdinalIgnoreCase))
        {
            if (text.Length >= 9)
            {
                weight *= 0.84d;
            }

            if (text.Length >= 12)
            {
                weight *= 0.72d;
            }
        }

        return Math.Max(0.2d, weight);
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
