using DreamAssembler.Core.Enums;
using DreamAssembler.Core.Models;
using DreamAssembler.Core.Services;

var isSamplesCommand = args.Length > 0 && string.Equals(args[0], "samples", StringComparison.OrdinalIgnoreCase);
var isAuditCommand = args.Length > 0 && string.Equals(args[0], "audit", StringComparison.OrdinalIgnoreCase);
var dataPathArgument = args.Length > 0 && !isSamplesCommand && !isAuditCommand ? args[0] : null;
var dataPath = !string.IsNullOrWhiteSpace(dataPathArgument)
    ? Path.GetFullPath(dataPathArgument)
    : Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "DreamAssembler", "Data"));

Console.WriteLine($"DreamAssembler DataTools");
Console.WriteLine($"Data path: {dataPath}");
Console.WriteLine();

var loader = new GeneratorDataLoader(
    new DictionaryRepository(),
    new TemplateRepository(),
    new DataSetManifestRepository(),
    new AssociationFragmentRepository());

var bundle = loader.Load(dataPath);
var analyzer = new DataSetAnalyzer();
var report = analyzer.Analyze(bundle);

if (isSamplesCommand)
{
    PrintSamples(bundle, args.Skip(1).ToArray());
}
else if (isAuditCommand)
{
    PrintAudit(bundle, args.Skip(1).ToArray());
}
else
{
    PrintSummary(report);
    PrintIssues(report.Issues);
}

Environment.ExitCode = report.HasErrors ? 1 : 0;

static void PrintSummary(DataValidationReport report)
{
    Console.WriteLine($"Data set: {report.DataSetId} ({report.Version})");
    Console.WriteLine($"Entries: {report.EntryCount}");
    Console.WriteLine($"Templates: {report.TemplateCount}");
    Console.WriteLine($"Word lexicon entries: {report.AssociationFragmentCount}");
    Console.WriteLine();

    Console.WriteLine("Categories:");
    foreach (var pair in report.CategoryCounts.OrderBy(pair => pair.Key, StringComparer.OrdinalIgnoreCase))
    {
        Console.WriteLine($"  - {pair.Key}: {pair.Value}");
    }

    Console.WriteLine();
    Console.WriteLine("Dictionary sets:");
    foreach (var set in report.DictionarySetStats.OrderByDescending(set => set.EntryCount).ThenBy(set => set.SetName, StringComparer.OrdinalIgnoreCase))
    {
        var categories = string.Join(", ", set.CategoryCounts.OrderBy(pair => pair.Key, StringComparer.OrdinalIgnoreCase).Select(pair => $"{pair.Key}:{pair.Value}"));
        var slots = string.Join(", ", set.SlotCounts.OrderBy(pair => pair.Key, StringComparer.OrdinalIgnoreCase).Select(pair => $"{pair.Key}:{pair.Value}"));
        Console.WriteLine($"  - {set.SetName}: {set.EntryCount}");
        Console.WriteLine($"    categories: {categories}");
        Console.WriteLine($"    slots: {(string.IsNullOrWhiteSpace(slots) ? "-" : slots)}");
    }

    Console.WriteLine();
    Console.WriteLine("Slots:");
    foreach (var pair in report.SlotCounts.OrderBy(pair => pair.Key, StringComparer.OrdinalIgnoreCase))
    {
        Console.WriteLine($"  - {pair.Key}: {pair.Value}");
    }

    Console.WriteLine();
    Console.WriteLine("Word lexicon kinds:");
    foreach (var pair in report.AssociationKindCounts.OrderBy(pair => pair.Key, StringComparer.OrdinalIgnoreCase))
    {
        Console.WriteLine($"  - {pair.Key}: {pair.Value}");
    }

    Console.WriteLine();
    Console.WriteLine("Association sources:");
    foreach (var set in report.AssociationSetStats.OrderByDescending(set => set.RowCount).ThenBy(set => set.SetName, StringComparer.OrdinalIgnoreCase))
    {
        Console.WriteLine($"  - {set.SetName}: {set.RowCount} rows ({set.SourceKind})");
    }

    Console.WriteLine();
}

static void PrintIssues(IReadOnlyList<DataValidationIssue> issues)
{
    if (issues.Count == 0)
    {
        Console.WriteLine("No issues found.");
        return;
    }

    Console.WriteLine("Issues:");
    foreach (var issue in issues.OrderByDescending(issue => issue.Severity).ThenBy(issue => issue.Code, StringComparer.OrdinalIgnoreCase))
    {
        Console.WriteLine($"  [{issue.Severity}] {issue.Code}: {issue.Message}");
    }
}

static void PrintSamples(GeneratorDataBundle bundle, IReadOnlyList<string> args)
{
    var mode = ParseMode(args.FirstOrDefault()) ?? GenerationMode.Sentence;
    var count = args.Count > 1 && int.TryParse(args[1], out var parsedCount)
        ? Math.Clamp(parsedCount, 1, 50)
        : 12;
    var absurdity = ParseAbsurdity(args.Count > 2 ? args[2] : null) ?? AbsurdityLevel.Absurd;

    var service = new TextGeneratorService(
        bundle.DictionaryEntries,
        bundle.Templates,
        bundle.AssociationFragments,
        new WeightedRandomSelector(new Random(17)),
        new TemplateEngine(),
        new Random(17));

    var results = GenerateAuditResults(service, mode, absurdity, count);

    Console.WriteLine($"Samples: mode={mode}, absurdity={absurdity}, count={count}");
    Console.WriteLine();

    for (var index = 0; index < results.Count; index++)
    {
        Console.WriteLine($"[{index + 1}] {results[index].Text}");
        Console.WriteLine();
    }
}

static void PrintAudit(GeneratorDataBundle bundle, IReadOnlyList<string> args)
{
    var mode = ParseMode(args.FirstOrDefault()) ?? GenerationMode.ShortText;
    var count = args.Count > 1 && int.TryParse(args[1], out var parsedCount)
        ? Math.Clamp(parsedCount, 10, 5000)
        : 400;
    var absurdity = ParseAbsurdity(args.Count > 2 ? args[2] : null) ?? AbsurdityLevel.Absurd;

    var service = new TextGeneratorService(
        bundle.DictionaryEntries,
        bundle.Templates,
        bundle.AssociationFragments,
        new WeightedRandomSelector(new Random(17)),
        new TemplateEngine(),
        new Random(17));

    var results = GenerateAuditResults(service, mode, absurdity, count);
    var actualCount = results.Count;

    var atmosphereCounts = CountBy(results.Select(result => result.AtmosphereKey ?? "none"));
    var dominantManifoldCounts = CountBy(results.Select(result => result.DebugTrace?.DominantStrongManifold ?? "none"));
    var pressureCounts = CountBy(results.Select(result => result.DebugTrace?.DominantPressureTag ?? "none"));
    var cadenceCounts = CountBy(results.SelectMany(result => result.DebugTrace?.Cadences ?? []));
    var templateCounts = CountBy(results.SelectMany(result => result.DebugTrace?.TemplateIds ?? []));
    var primaryCadenceCounts = CountBy(results.Select(result => result.DebugTrace?.Cadences.FirstOrDefault() ?? "none"));
    var repeatedTexts = results
        .GroupBy(result => result.Text, StringComparer.Ordinal)
        .Where(group => group.Count() > 1)
        .OrderByDescending(group => group.Count())
        .ThenBy(group => group.Key, StringComparer.Ordinal)
        .ToList();

    var legacyHeavyCount = results.Count(result =>
        result.DebugTrace is not null
        && result.DebugTrace.LegacyBaselineTagCount > result.DebugTrace.NeutralFoundationTagCount);

    var neutralHeavyCount = results.Count(result =>
        result.DebugTrace is not null
        && result.DebugTrace.NeutralFoundationTagCount > result.DebugTrace.LegacyBaselineTagCount);

    var cadenceRepetitionCount = CountAdjacentRepetitions(results
        .Select(result => result.DebugTrace?.Cadences.FirstOrDefault())
        .ToList());

    var manifoldCadenceActivation = results
        .Where(result => result.DebugTrace is not null
                         && !string.IsNullOrWhiteSpace(result.DebugTrace.DominantStrongManifold)
                         && result.DebugTrace.Cadences.Count > 0)
        .GroupBy(result => result.DebugTrace!.DominantStrongManifold!, StringComparer.OrdinalIgnoreCase)
        .Select(group => new
        {
            Manifold = group.Key,
            Total = group.Count(),
            Preferred = group.Count(result => IsPreferredCadenceForManifold(
                result.DebugTrace!.DominantStrongManifold!,
                result.DebugTrace.Cadences))
        })
        .OrderByDescending(item => item.Total)
        .ThenBy(item => item.Manifold, StringComparer.OrdinalIgnoreCase)
        .ToList();

    Console.WriteLine($"Audit: mode={mode}, absurdity={absurdity}, requested={count}, generated={actualCount}");
    Console.WriteLine();

    Console.WriteLine("Surfacing frequency:");
    PrintDistribution(atmosphereCounts, actualCount);

    Console.WriteLine();
    Console.WriteLine("Dominant manifold frequency:");
    PrintDistribution(dominantManifoldCounts, actualCount);

    Console.WriteLine();
    Console.WriteLine("Cadence family distribution:");
    PrintDistribution(cadenceCounts, Math.Max(1, cadenceCounts.Values.Sum()));

    Console.WriteLine();
    Console.WriteLine("Primary cadence distribution:");
    PrintDistribution(primaryCadenceCounts, actualCount);

    Console.WriteLine();
    Console.WriteLine("Emotional pressure dominance:");
    PrintDistribution(pressureCounts, actualCount);

    Console.WriteLine();
    Console.WriteLine("Legacy gravity:");
    Console.WriteLine($"  - legacy-heavier results: {legacyHeavyCount} ({ToPercent(legacyHeavyCount, actualCount)})");
    Console.WriteLine($"  - neutral-heavier results: {neutralHeavyCount} ({ToPercent(neutralHeavyCount, actualCount)})");
    Console.WriteLine($"  - balanced/undecided results: {actualCount - legacyHeavyCount - neutralHeavyCount} ({ToPercent(actualCount - legacyHeavyCount - neutralHeavyCount, actualCount)})");

    Console.WriteLine();
    Console.WriteLine("Cadence repetition:");
    Console.WriteLine($"  - adjacent primary-cadence repeats: {cadenceRepetitionCount} ({ToPercent(cadenceRepetitionCount, Math.Max(1, actualCount - 1))})");

    Console.WriteLine();
    Console.WriteLine("Manifold-local cadence activation:");
    foreach (var item in manifoldCadenceActivation)
    {
        Console.WriteLine($"  - {item.Manifold}: preferred cadence active in {item.Preferred}/{item.Total} results ({ToPercent(item.Preferred, item.Total)})");
    }

    Console.WriteLine();
    Console.WriteLine("Overdominant templates:");
    PrintDistribution(templateCounts, Math.Max(1, templateCounts.Values.Sum()), 12);

    Console.WriteLine();
    Console.WriteLine("Atmospheric monotony signals:");
    PrintMonotonySignal("top atmosphere", atmosphereCounts, actualCount);
    PrintMonotonySignal("top dominant manifold", dominantManifoldCounts, actualCount);
    PrintMonotonySignal("top primary cadence", primaryCadenceCounts, actualCount);
    PrintMonotonySignal("top pressure", pressureCounts, actualCount);

    Console.WriteLine();
    Console.WriteLine("Repeated outputs:");
    if (repeatedTexts.Count == 0)
    {
        Console.WriteLine("  - none");
    }
    else
    {
        foreach (var group in repeatedTexts.Take(8))
        {
            Console.WriteLine($"  - {group.Count()}x {group.Key}");
        }
    }
}

static GenerationMode? ParseMode(string? value)
{
    if (string.IsNullOrWhiteSpace(value))
    {
        return null;
    }

    return Enum.TryParse<GenerationMode>(value, true, out var mode) ? mode : null;
}

static AbsurdityLevel? ParseAbsurdity(string? value)
{
    if (string.IsNullOrWhiteSpace(value))
    {
        return null;
    }

    return Enum.TryParse<AbsurdityLevel>(value, true, out var absurdity) ? absurdity : null;
}

static IReadOnlyList<TextGenerationResult> GenerateAuditResults(
    TextGeneratorService service,
    GenerationMode mode,
    AbsurdityLevel absurdity,
    int requestedCount)
{
    const int maxBatchSize = 20;

    var results = new List<TextGenerationResult>(requestedCount);
    var remaining = requestedCount;

    while (remaining > 0)
    {
        var batchSize = Math.Min(maxBatchSize, remaining);
        var batch = service.Generate(new TextGenerationOptions
        {
            Mode = mode,
            AbsurdityLevel = absurdity,
            ResultCount = batchSize
        });

        results.AddRange(batch);
        remaining -= batch.Count;
    }

    return results;
}

static Dictionary<string, int> CountBy(IEnumerable<string> values)
{
    return values
        .Where(value => !string.IsNullOrWhiteSpace(value))
        .GroupBy(value => value, StringComparer.OrdinalIgnoreCase)
        .ToDictionary(group => group.Key, group => group.Count(), StringComparer.OrdinalIgnoreCase);
}

static void PrintDistribution(IReadOnlyDictionary<string, int> counts, int total, int limit = 10)
{
    if (counts.Count == 0)
    {
        Console.WriteLine("  - none");
        return;
    }

    foreach (var pair in counts
                 .OrderByDescending(pair => pair.Value)
                 .ThenBy(pair => pair.Key, StringComparer.OrdinalIgnoreCase)
                 .Take(limit))
    {
        Console.WriteLine($"  - {pair.Key}: {pair.Value} ({ToPercent(pair.Value, total)})");
    }
}

static string ToPercent(int part, int total)
{
    if (total <= 0)
    {
        return "0.0%";
    }

    return $"{(part * 100d / total):0.0}%";
}

static int CountAdjacentRepetitions(IReadOnlyList<string?> values)
{
    var count = 0;
    for (var index = 1; index < values.Count; index++)
    {
        if (!string.IsNullOrWhiteSpace(values[index])
            && string.Equals(values[index], values[index - 1], StringComparison.OrdinalIgnoreCase))
        {
            count++;
        }
    }

    return count;
}

static bool IsPreferredCadenceForManifold(string manifold, IReadOnlyList<string> cadences)
{
    if (cadences.Count == 0)
    {
        return false;
    }

    var preferredByManifold = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase)
    {
        ["weather_systems"] = new(["announcement", "procedural_report", "inventory"], StringComparer.OrdinalIgnoreCase),
        ["observatory"] = new(["static_observation", "suspended_statement", "interrupted_memory"], StringComparer.OrdinalIgnoreCase),
        ["hydroelectric"] = new(["procedural_report", "inventory", "suspended_statement"], StringComparer.OrdinalIgnoreCase),
        ["coastal_fog"] = new(["static_observation", "announcement", "suspended_statement"], StringComparer.OrdinalIgnoreCase),
        ["sanatorium"] = new(["quiet_instruction", "suspended_statement", "static_observation"], StringComparer.OrdinalIgnoreCase),
        ["mall"] = new(["announcement", "inventory", "static_observation"], StringComparer.OrdinalIgnoreCase),
        ["hospitality"] = new(["quiet_instruction", "ceremonial", "suspended_statement"], StringComparer.OrdinalIgnoreCase),
        ["museum"] = new(["museum_label", "static_observation", "inventory"], StringComparer.OrdinalIgnoreCase),
        ["airport"] = new(["announcement", "procedural_report", "quiet_instruction"], StringComparer.OrdinalIgnoreCase)
    };

    return preferredByManifold.TryGetValue(manifold, out var preferred)
           && cadences.Any(preferred.Contains);
}

static void PrintMonotonySignal(string label, IReadOnlyDictionary<string, int> counts, int total)
{
    if (counts.Count == 0 || total <= 0)
    {
        Console.WriteLine($"  - {label}: none");
        return;
    }

    var top = counts
        .OrderByDescending(pair => pair.Value)
        .ThenBy(pair => pair.Key, StringComparer.OrdinalIgnoreCase)
        .First();

    Console.WriteLine($"  - {label}: {top.Key} at {ToPercent(top.Value, total)}");
}
