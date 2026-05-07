using DreamAssembler.Core.Models;

namespace DreamAssembler.Core.Services;

/// <summary>
/// Загружает словарные данные для словесных режимов из внешних CSV-источников.
/// </summary>
public sealed class AssociationFragmentRepository
{
    private const double CuratedEntryBoost = 3.2d;
    private const double NonCuratedPenalty = 0.32d;

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
            var sourceDirectoryPath = ResolveSourceDirectory(directoryPath);
            var curatedDirectoryPath = ResolveCuratedDirectory(directoryPath, sourceDirectoryPath);
            var curatedPreference = LoadCuratedPreference(curatedDirectoryPath);
            var files = Directory.GetFiles(sourceDirectoryPath, "*.csv", SearchOption.AllDirectories);
            var entries = new List<AssociationFragmentEntry>();

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);

                if (fileName.Contains("nouns", StringComparison.OrdinalIgnoreCase))
                {
                    entries.AddRange(ParseNouns(file, curatedPreference));
                }
                else if (fileName.Contains("adjectives", StringComparison.OrdinalIgnoreCase))
                {
                    entries.AddRange(ParseAdjectives(file, curatedPreference));
                }
                else if (fileName.Contains("verbs", StringComparison.OrdinalIgnoreCase))
                {
                    entries.AddRange(ParseVerbs(file, curatedPreference));
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

    private static IReadOnlyList<AssociationFragmentEntry> ParseNouns(string filePath, CuratedAssociationPreference curatedPreference)
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
            var nominative = NormalizeWord(GetColumnValue(columns, header, "sg_nom"));
            var genitive = NormalizeWord(GetColumnValue(columns, header, "sg_gen"));
            var accusative = NormalizeWord(GetColumnValue(columns, header, "sg_acc"));
            var gender = ResolveNounGender(
                GetColumnValue(columns, header, "gender"),
                nominative,
                genitive,
                accusative);
            if (!IsSupportedNounGender(gender))
            {
                continue;
            }

            if (GetColumnValue(columns, header, "pl_only") == "1")
            {
                continue;
            }

            if (!IsSingleRussianWord(nominative) || !IsAllowedNounLemma(nominative))
            {
                continue;
            }

            entries.Add(new AssociationFragmentEntry
            {
                Id = $"noun_{gender}_{nominative}",
                Text = nominative,
                Kind = $"noun_{gender}",
                Tags = curatedPreference.GetTagsForNoun(nominative),
                Weight = CalculateLexicalWeight(
                    $"noun_{gender}",
                    nominative,
                    curatedPreference.PreferredNouns.Contains(nominative),
                    curatedPreference.HasNounSubset)
            });
        }

        return entries;
    }

    private static IReadOnlyList<AssociationFragmentEntry> ParseAdjectives(string filePath, CuratedAssociationPreference curatedPreference)
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
                Tags = curatedPreference.GetTagsForAdjective(bare),
                Weight = CalculateLexicalWeight(
                    "adjective_m",
                    masculine,
                    curatedPreference.PreferredAdjectives.Contains(bare),
                    curatedPreference.HasAdjectiveSubset)
            });

            entries.Add(new AssociationFragmentEntry
            {
                Id = $"adjective_f_{feminine}",
                Text = feminine,
                Kind = "adjective_f",
                Tags = curatedPreference.GetTagsForAdjective(bare),
                Weight = CalculateLexicalWeight(
                    "adjective_f",
                    feminine,
                    curatedPreference.PreferredAdjectives.Contains(bare),
                    curatedPreference.HasAdjectiveSubset)
            });

            entries.Add(new AssociationFragmentEntry
            {
                Id = $"adjective_n_{neuter}",
                Text = neuter,
                Kind = "adjective_n",
                Tags = curatedPreference.GetTagsForAdjective(bare),
                Weight = CalculateLexicalWeight(
                    "adjective_n",
                    neuter,
                    curatedPreference.PreferredAdjectives.Contains(bare),
                    curatedPreference.HasAdjectiveSubset)
            });
        }

        return entries;
    }

    private static IReadOnlyList<AssociationFragmentEntry> ParseVerbs(string filePath, CuratedAssociationPreference curatedPreference)
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
                    Tags = curatedPreference.GetTagsForVerb(bare),
                    Weight = CalculateLexicalWeight(
                        "verb_past_m",
                        pastMasculine,
                        curatedPreference.PreferredVerbs.Contains(bare),
                        curatedPreference.HasVerbSubset)
                });
            }

            if (IsSingleRussianWord(pastFeminine))
            {
                entries.Add(new AssociationFragmentEntry
                {
                    Id = $"verb_past_f_{pastFeminine}",
                    Text = pastFeminine,
                    Kind = "verb_past_f",
                    Tags = curatedPreference.GetTagsForVerb(bare),
                    Weight = CalculateLexicalWeight(
                        "verb_past_f",
                        pastFeminine,
                        curatedPreference.PreferredVerbs.Contains(bare),
                        curatedPreference.HasVerbSubset)
                });
            }

            if (IsSingleRussianWord(pastNeuter))
            {
                entries.Add(new AssociationFragmentEntry
                {
                    Id = $"verb_past_n_{pastNeuter}",
                    Text = pastNeuter,
                    Kind = "verb_past_n",
                    Tags = curatedPreference.GetTagsForVerb(bare),
                    Weight = CalculateLexicalWeight(
                        "verb_past_n",
                        pastNeuter,
                        curatedPreference.PreferredVerbs.Contains(bare),
                        curatedPreference.HasVerbSubset)
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

    private static string ResolveNounGender(string sourceGender, string nominative, string genitive, string accusative)
    {
        if (string.IsNullOrWhiteSpace(nominative))
        {
            return sourceGender;
        }

        if (LooksLikeMasculineAnimateNoun(nominative, accusative))
        {
            return "m";
        }

        if (LooksLikeNeuterNoun(nominative, genitive))
        {
            return "n";
        }

        if (LooksLikeFeminineNoun(nominative, genitive))
        {
            return "f";
        }

        return sourceGender;
    }

    private static bool LooksLikeMasculineAnimateNoun(string nominative, string accusative)
    {
        if (!string.Equals(accusative, nominative, StringComparison.Ordinal))
        {
            if (nominative.EndsWith("ин", StringComparison.Ordinal)
                || nominative.EndsWith("нин", StringComparison.Ordinal)
                || nominative.EndsWith("тель", StringComparison.Ordinal)
                || nominative.EndsWith("арь", StringComparison.Ordinal)
                || nominative.EndsWith("ист", StringComparison.Ordinal)
                || nominative.EndsWith("лог", StringComparison.Ordinal)
                || nominative.EndsWith("ор", StringComparison.Ordinal)
                || nominative.EndsWith("ер", StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    private static bool LooksLikeNeuterNoun(string nominative, string genitive)
    {
        return nominative.EndsWith("о", StringComparison.Ordinal)
               || nominative.EndsWith("е", StringComparison.Ordinal)
               || nominative.EndsWith("ие", StringComparison.Ordinal)
               || nominative.EndsWith("мя", StringComparison.Ordinal)
               || (nominative.EndsWith("ание", StringComparison.Ordinal) && genitive.EndsWith("ания", StringComparison.Ordinal))
               || (nominative.EndsWith("ение", StringComparison.Ordinal) && genitive.EndsWith("ения", StringComparison.Ordinal));
    }

    private static bool LooksLikeFeminineNoun(string nominative, string genitive)
    {
        return nominative.EndsWith("а", StringComparison.Ordinal)
               || nominative.EndsWith("я", StringComparison.Ordinal)
               || (nominative.EndsWith("ь", StringComparison.Ordinal) && genitive.EndsWith("и", StringComparison.Ordinal));
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

    private static string ResolveSourceDirectory(string directoryPath)
    {
        var nestedSourcesPath = Path.Combine(directoryPath, "Sources");
        return Directory.Exists(nestedSourcesPath) ? nestedSourcesPath : directoryPath;
    }

    private static string? ResolveCuratedDirectory(string directoryPath, string sourceDirectoryPath)
    {
        var directCuratedPath = Path.Combine(directoryPath, "Curated");
        if (Directory.Exists(directCuratedPath))
        {
            return directCuratedPath;
        }

        var siblingCuratedPath = Path.Combine(Directory.GetParent(sourceDirectoryPath)?.FullName ?? directoryPath, "Curated");
        return Directory.Exists(siblingCuratedPath) ? siblingCuratedPath : null;
    }

    private static CuratedAssociationPreference LoadCuratedPreference(string? curatedDirectoryPath)
    {
        if (string.IsNullOrWhiteSpace(curatedDirectoryPath) || !Directory.Exists(curatedDirectoryPath))
        {
            return CuratedAssociationPreference.Empty;
        }

        return new CuratedAssociationPreference(
            LoadCuratedWordSet(Path.Combine(curatedDirectoryPath, "preferred-nouns.txt")),
            LoadCuratedWordSet(Path.Combine(curatedDirectoryPath, "preferred-adjectives.txt")),
            LoadCuratedWordSet(Path.Combine(curatedDirectoryPath, "preferred-verbs.txt")),
            LoadCuratedClusters(Path.Combine(curatedDirectoryPath, "Clusters")));
    }

    private static HashSet<string> LoadCuratedWordSet(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return [];
        }

        return File.ReadLines(filePath)
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith('#'))
            .Select(NormalizeWord)
            .Where(IsSingleRussianWord)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    private static IReadOnlyList<CuratedAssociationCluster> LoadCuratedClusters(string clustersDirectoryPath)
    {
        if (!Directory.Exists(clustersDirectoryPath))
        {
            return [];
        }

        var clusters = new List<CuratedAssociationCluster>();
        foreach (var clusterDirectoryPath in Directory.GetDirectories(clustersDirectoryPath))
        {
            var key = Path.GetFileName(clusterDirectoryPath).Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(key))
            {
                continue;
            }

            clusters.Add(new CuratedAssociationCluster(
                key,
                LoadCuratedWordSet(Path.Combine(clusterDirectoryPath, "preferred-nouns.txt")),
                LoadCuratedWordSet(Path.Combine(clusterDirectoryPath, "preferred-adjectives.txt")),
                LoadCuratedWordSet(Path.Combine(clusterDirectoryPath, "preferred-verbs.txt"))));
        }

        return clusters;
    }

    private static double CalculateLexicalWeight(string kind, string text, bool isCuratedPreferred, bool hasCuratedSubset)
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

        if (hasCuratedSubset)
        {
            weight *= isCuratedPreferred ? CuratedEntryBoost : NonCuratedPenalty;
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

    private sealed record CuratedAssociationPreference(
        HashSet<string> PreferredNouns,
        HashSet<string> PreferredAdjectives,
        HashSet<string> PreferredVerbs,
        IReadOnlyList<CuratedAssociationCluster> Clusters)
    {
        public static CuratedAssociationPreference Empty { get; } = new([], [], [], []);

        public bool HasNounSubset => PreferredNouns.Count > 0;

        public bool HasAdjectiveSubset => PreferredAdjectives.Count > 0;

        public bool HasVerbSubset => PreferredVerbs.Count > 0;

        public IReadOnlyList<string> GetTagsForNoun(string lemma)
        {
            return GetTags(lemma, cluster => cluster.PreferredNouns.Contains(lemma));
        }

        public IReadOnlyList<string> GetTagsForAdjective(string lemma)
        {
            return GetTags(lemma, cluster => cluster.PreferredAdjectives.Contains(lemma));
        }

        public IReadOnlyList<string> GetTagsForVerb(string lemma)
        {
            return GetTags(lemma, cluster => cluster.PreferredVerbs.Contains(lemma));
        }

        private IReadOnlyList<string> GetTags(string lemma, Func<CuratedAssociationCluster, bool> predicate)
        {
            return Clusters
                .Where(predicate)
                .Select(cluster => $"cluster:{cluster.Key}")
                .ToArray();
        }
    }

    private sealed record CuratedAssociationCluster(
        string Key,
        HashSet<string> PreferredNouns,
        HashSet<string> PreferredAdjectives,
        HashSet<string> PreferredVerbs);
}
