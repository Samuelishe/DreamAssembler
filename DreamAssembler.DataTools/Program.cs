using System.Text.Json;
using DreamAssembler.Core.Enums;
using DreamAssembler.Core.Models;
using DreamAssembler.Core.Services;

var command = args.FirstOrDefault();
var isSamplesCommand = string.Equals(command, "samples", StringComparison.OrdinalIgnoreCase);
var isAuditCommand = string.Equals(command, "audit", StringComparison.OrdinalIgnoreCase);
var isReportCommand = string.Equals(command, "report", StringComparison.OrdinalIgnoreCase);
var isCompareCommand = string.Equals(command, "compare", StringComparison.OrdinalIgnoreCase);
var dataPathArgument = args.Length > 0 && !isSamplesCommand && !isAuditCommand && !isReportCommand && !isCompareCommand ? args[0] : null;
var dataPath = !string.IsNullOrWhiteSpace(dataPathArgument)
    ? Path.GetFullPath(dataPathArgument)
    : Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "DreamAssembler", "Data"));
var projectRoot = Path.GetFullPath(Path.Combine(dataPath, "..", ".."));

Console.WriteLine("DreamAssembler DataTools");
Console.WriteLine($"Data path: {dataPath}");
Console.WriteLine();

if (isCompareCommand)
{
    PrintSnapshotComparison(args.Skip(1).ToArray(), projectRoot);
    return;
}

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
    PrintAudit(bundle, args.Skip(1).ToArray(), reportOnly: false, projectRoot);
}
else if (isReportCommand)
{
    PrintAudit(bundle, args.Skip(1).ToArray(), reportOnly: true, projectRoot);
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
    var positional = GetPositionalArgs(args);
    var mode = ParseMode(positional.FirstOrDefault()) ?? GenerationMode.Sentence;
    var count = positional.Count > 1 && int.TryParse(positional[1], out var parsedCount)
        ? Math.Clamp(parsedCount, 1, 50)
        : 12;
    var absurdity = ParseAbsurdity(positional.Count > 2 ? positional[2] : null) ?? AbsurdityLevel.Absurd;

    var service = CreateService(bundle);
    var results = GenerateAuditResults(service, mode, absurdity, count);

    Console.WriteLine($"Samples: mode={mode}, absurdity={absurdity}, count={count}");
    Console.WriteLine();

    for (var index = 0; index < results.Count; index++)
    {
        Console.WriteLine($"[{index + 1}] {results[index].Text}");
        Console.WriteLine();
    }
}

static void PrintAudit(GeneratorDataBundle bundle, IReadOnlyList<string> args, bool reportOnly, string projectRoot)
{
    var positional = GetPositionalArgs(args);
    var mode = ParseMode(positional.FirstOrDefault()) ?? GenerationMode.ShortText;
    var count = positional.Count > 1 && int.TryParse(positional[1], out var parsedCount)
        ? Math.Clamp(parsedCount, 10, 5000)
        : 400;
    var absurdity = ParseAbsurdity(positional.Count > 2 ? positional[2] : null) ?? AbsurdityLevel.Absurd;
    var snapshotPath = GetOptionValue(args, "--snapshot");

    var service = CreateService(bundle);
    var results = GenerateAuditResults(service, mode, absurdity, count);
    var snapshot = BuildSnapshot(mode, absurdity, count, results);

    if (!reportOnly)
    {
        PrintRawAuditSnapshot(snapshot);
        Console.WriteLine();
    }

    PrintDiagnosisReport(snapshot);

    if (!string.IsNullOrWhiteSpace(snapshotPath))
    {
        var resolvedSnapshotPath = ResolveSnapshotOutputPath(snapshotPath, projectRoot);
        SaveSnapshot(snapshot, resolvedSnapshotPath);
        Console.WriteLine();
        Console.WriteLine($"Snapshot saved: {resolvedSnapshotPath}");
    }
}

static void PrintSnapshotComparison(IReadOnlyList<string> args, string projectRoot)
{
    var positional = GetPositionalArgs(args);
    if (positional.Count < 2)
    {
        Console.WriteLine("Compare usage: compare <snapshot-a.json> <snapshot-b.json>");
        return;
    }

    var firstPath = ResolveSnapshotInputPath(positional[0], projectRoot);
    var secondPath = ResolveSnapshotInputPath(positional[1], projectRoot);
    var first = LoadSnapshot(firstPath);
    var second = LoadSnapshot(secondPath);

    Console.WriteLine($"Compare: {Path.GetFileName(firstPath)} -> {Path.GetFileName(secondPath)}");
    Console.WriteLine();

    var topAtmosphereA = GetTopKey(first.AtmosphereCounts);
    var topAtmosphereB = GetTopKey(second.AtmosphereCounts);
    var topCadenceA = GetTopKey(first.PrimaryCadenceCounts);
    var topCadenceB = GetTopKey(second.PrimaryCadenceCounts);
    var topPressureA = GetTopKey(first.PressureCounts);
    var topPressureB = GetTopKey(second.PressureCounts);

    Console.WriteLine("Ecology drift:");
    Console.WriteLine($"  - atmosphere lead: {FormatTop(first.AtmosphereCounts, first.GeneratedCount)} -> {FormatTop(second.AtmosphereCounts, second.GeneratedCount)}");
    Console.WriteLine($"  - primary cadence lead: {FormatTop(first.PrimaryCadenceCounts, first.GeneratedCount)} -> {FormatTop(second.PrimaryCadenceCounts, second.GeneratedCount)}");
    Console.WriteLine($"  - pressure lead: {FormatTop(first.PressureCounts, first.GeneratedCount)} -> {FormatTop(second.PressureCounts, second.GeneratedCount)}");
    Console.WriteLine($"  - legacy-heavier results: {ToPercent(first.LegacyHeavyCount, first.GeneratedCount)} -> {ToPercent(second.LegacyHeavyCount, second.GeneratedCount)}");
    Console.WriteLine($"  - cadence repetition: {ToPercent(first.CadenceRepetitionCount, Math.Max(1, first.GeneratedCount - 1))} -> {ToPercent(second.CadenceRepetitionCount, Math.Max(1, second.GeneratedCount - 1))}");

    Console.WriteLine();
    Console.WriteLine("Targeted changes:");
    PrintComparisonLine("weather cadence activation", GetActivationPercent(first, "weather_systems"), GetActivationPercent(second, "weather_systems"));
    PrintComparisonLine("observatory cadence activation", GetActivationPercent(first, "observatory"), GetActivationPercent(second, "observatory"));
    PrintComparisonLine("hydroelectric cadence activation", GetActivationPercent(first, "hydroelectric"), GetActivationPercent(second, "hydroelectric"));
    PrintComparisonLine("museum cadence activation", GetActivationPercent(first, "museum"), GetActivationPercent(second, "museum"));

    Console.WriteLine();
    Console.WriteLine("Concise diagnosis:");
    foreach (var line in BuildComparisonDiagnosis(first, second, topAtmosphereA, topAtmosphereB, topCadenceA, topCadenceB, topPressureA, topPressureB))
    {
        Console.WriteLine($"  - {line}");
    }
}

static TextGeneratorService CreateService(GeneratorDataBundle bundle)
{
    return new TextGeneratorService(
        bundle.DictionaryEntries,
        bundle.Templates,
        bundle.AssociationFragments,
        new WeightedRandomSelector(new Random(17)),
        new TemplateEngine(),
        new Random(17));
}

static RuntimeEcologySnapshot BuildSnapshot(
    GenerationMode mode,
    AbsurdityLevel absurdity,
    int requestedCount,
    IReadOnlyList<TextGenerationResult> results)
{
    var actualCount = results.Count;
    var atmosphereCounts = CountBy(results.Select(result => result.AtmosphereKey ?? "none"));
    var dominantManifoldCounts = CountBy(results.Select(result => result.DebugTrace?.DominantStrongManifold ?? "none"));
    var pressureCounts = CountBy(results.Select(result => result.DebugTrace?.DominantPressureTag ?? "none"));
    var cadenceCounts = CountBy(results.SelectMany(result => result.DebugTrace?.Cadences ?? []));
    var templateCounts = CountBy(results.SelectMany(result => result.DebugTrace?.TemplateIds ?? []));
    var primaryCadenceCounts = CountBy(results.Select(result => result.DebugTrace?.Cadences.FirstOrDefault() ?? "none"));
    var repeatedOutputs = results
        .GroupBy(result => result.Text, StringComparer.Ordinal)
        .Where(group => group.Count() > 1)
        .OrderByDescending(group => group.Count())
        .ThenBy(group => group.Key, StringComparer.Ordinal)
        .Take(20)
        .Select(group => new RepeatedOutput(group.Key, group.Count()))
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
        .Select(group => new ManifoldCadenceActivation(
            group.Key,
            group.Count(),
            group.Count(result => IsPreferredCadenceForManifold(
                result.DebugTrace!.DominantStrongManifold!,
                result.DebugTrace.Cadences))))
        .OrderByDescending(item => item.Total)
        .ThenBy(item => item.Manifold, StringComparer.OrdinalIgnoreCase)
        .ToList();

    return new RuntimeEcologySnapshot
    {
        Mode = mode.ToString(),
        Absurdity = absurdity.ToString(),
        RequestedCount = requestedCount,
        GeneratedCount = actualCount,
        CreatedAt = DateTimeOffset.Now,
        AtmosphereCounts = atmosphereCounts,
        DominantManifoldCounts = dominantManifoldCounts,
        PressureCounts = pressureCounts,
        CadenceCounts = cadenceCounts,
        PrimaryCadenceCounts = primaryCadenceCounts,
        TemplateCounts = templateCounts,
        LegacyHeavyCount = legacyHeavyCount,
        NeutralHeavyCount = neutralHeavyCount,
        CadenceRepetitionCount = cadenceRepetitionCount,
        RepeatedOutputs = repeatedOutputs,
        ManifoldCadenceActivation = manifoldCadenceActivation
    };
}

static void PrintRawAuditSnapshot(RuntimeEcologySnapshot snapshot)
{
    Console.WriteLine($"Audit: mode={snapshot.Mode}, absurdity={snapshot.Absurdity}, requested={snapshot.RequestedCount}, generated={snapshot.GeneratedCount}");
    Console.WriteLine();

    Console.WriteLine("Surfacing frequency:");
    PrintDistribution(snapshot.AtmosphereCounts, snapshot.GeneratedCount);

    Console.WriteLine();
    Console.WriteLine("Dominant manifold frequency:");
    PrintDistribution(snapshot.DominantManifoldCounts, snapshot.GeneratedCount);

    Console.WriteLine();
    Console.WriteLine("Cadence family distribution:");
    PrintDistribution(snapshot.CadenceCounts, Math.Max(1, snapshot.CadenceCounts.Values.Sum()));

    Console.WriteLine();
    Console.WriteLine("Primary cadence distribution:");
    PrintDistribution(snapshot.PrimaryCadenceCounts, snapshot.GeneratedCount);

    Console.WriteLine();
    Console.WriteLine("Emotional pressure dominance:");
    PrintDistribution(snapshot.PressureCounts, snapshot.GeneratedCount);

    Console.WriteLine();
    Console.WriteLine("Legacy gravity:");
    Console.WriteLine($"  - legacy-heavier results: {snapshot.LegacyHeavyCount} ({ToPercent(snapshot.LegacyHeavyCount, snapshot.GeneratedCount)})");
    Console.WriteLine($"  - neutral-heavier results: {snapshot.NeutralHeavyCount} ({ToPercent(snapshot.NeutralHeavyCount, snapshot.GeneratedCount)})");
    Console.WriteLine($"  - balanced/undecided results: {snapshot.GeneratedCount - snapshot.LegacyHeavyCount - snapshot.NeutralHeavyCount} ({ToPercent(snapshot.GeneratedCount - snapshot.LegacyHeavyCount - snapshot.NeutralHeavyCount, snapshot.GeneratedCount)})");

    Console.WriteLine();
    Console.WriteLine("Cadence repetition:");
    Console.WriteLine($"  - adjacent primary-cadence repeats: {snapshot.CadenceRepetitionCount} ({ToPercent(snapshot.CadenceRepetitionCount, Math.Max(1, snapshot.GeneratedCount - 1))})");

    Console.WriteLine();
    Console.WriteLine("Manifold-local cadence activation:");
    foreach (var item in snapshot.ManifoldCadenceActivation)
    {
        Console.WriteLine($"  - {item.Manifold}: preferred cadence active in {item.Preferred}/{item.Total} results ({ToPercent(item.Preferred, item.Total)})");
    }

    Console.WriteLine();
    Console.WriteLine("Overdominant templates:");
    PrintDistribution(snapshot.TemplateCounts, Math.Max(1, snapshot.TemplateCounts.Values.Sum()), 12);

    Console.WriteLine();
    Console.WriteLine("Atmospheric monotony signals:");
    PrintMonotonySignal("top atmosphere", snapshot.AtmosphereCounts, snapshot.GeneratedCount);
    PrintMonotonySignal("top dominant manifold", snapshot.DominantManifoldCounts, snapshot.GeneratedCount);
    PrintMonotonySignal("top primary cadence", snapshot.PrimaryCadenceCounts, snapshot.GeneratedCount);
    PrintMonotonySignal("top pressure", snapshot.PressureCounts, snapshot.GeneratedCount);

    Console.WriteLine();
    Console.WriteLine("Repeated outputs:");
    if (snapshot.RepeatedOutputs.Count == 0)
    {
        Console.WriteLine("  - none");
    }
    else
    {
        foreach (var group in snapshot.RepeatedOutputs.Take(8))
        {
            Console.WriteLine($"  - {group.Count}x {group.Text}");
        }
    }
}

static void PrintDiagnosisReport(RuntimeEcologySnapshot snapshot)
{
    Console.WriteLine("Atmospheric Report");
    Console.WriteLine($"  Mode: {snapshot.Mode}, absurdity={snapshot.Absurdity}, generated={snapshot.GeneratedCount}");
    Console.WriteLine();

    Console.WriteLine("Concise summary:");
    foreach (var line in BuildDiagnosisLines(snapshot))
    {
        Console.WriteLine($"  - {line}");
    }

    Console.WriteLine();
    Console.WriteLine("Warnings:");
    var warnings = BuildWarningLines(snapshot);
    if (warnings.Count == 0)
    {
        Console.WriteLine("  - no strong monotony warnings detected in this snapshot.");
    }
    else
    {
        foreach (var line in warnings)
        {
            Console.WriteLine($"  - {line}");
        }
    }

    Console.WriteLine();
    Console.WriteLine("Recommended next direction:");
    foreach (var line in BuildRecommendationLines(snapshot))
    {
        Console.WriteLine($"  - {line}");
    }
}

static List<string> BuildDiagnosisLines(RuntimeEcologySnapshot snapshot)
{
    var lines = new List<string>();
    var topPressure = GetTopPair(snapshot.PressureCounts);
    var topCadence = GetTopPair(snapshot.CadenceCounts);
    var topManifold = GetTopPair(snapshot.DominantManifoldCounts);
    var strongestActivation = snapshot.ManifoldCadenceActivation.OrderByDescending(item => item.ActivationRate).FirstOrDefault();
    var weakestActivation = snapshot.ManifoldCadenceActivation.Where(item => item.Total >= 10).OrderBy(item => item.ActivationRate).FirstOrDefault();

    if (topPressure is not null)
    {
        lines.Add($"{topPressure.Value.Key} pressure dominates runtime at {ToPercent(topPressure.Value.Value, snapshot.GeneratedCount)}.");
    }

    if (topCadence is not null)
    {
        lines.Add($"{topCadence.Value.Key} remains the heaviest cadence family at {ToPercent(topCadence.Value.Value, Math.Max(1, snapshot.CadenceCounts.Values.Sum()))}.");
    }

    if (topManifold is not null)
    {
        lines.Add($"{topManifold.Value.Key} currently leads manifold surfacing at {ToPercent(topManifold.Value.Value, snapshot.GeneratedCount)}.");
    }

    if (strongestActivation is not null)
    {
        lines.Add($"{strongestActivation.Manifold} shows the strongest cadence autonomy at {ToPercent(strongestActivation.Preferred, strongestActivation.Total)} preferred activation.");
    }

    if (weakestActivation is not null && weakestActivation.ActivationRate < 0.7d)
    {
        lines.Add($"{weakestActivation.Manifold} still surfaces with weak cadence autonomy at {ToPercent(weakestActivation.Preferred, weakestActivation.Total)}.");
    }

    lines.Add($"legacy-heavier outputs sit at {ToPercent(snapshot.LegacyHeavyCount, snapshot.GeneratedCount)}, so old gravity is present but not fully dominant.");
    return lines;
}

static List<string> BuildWarningLines(RuntimeEcologySnapshot snapshot)
{
    var warnings = new List<string>();
    var topPressure = GetTopPair(snapshot.PressureCounts);
    var topPrimaryCadence = GetTopPair(snapshot.PrimaryCadenceCounts);
    var topTemplate = GetTopPair(snapshot.TemplateCounts);

    if (topPressure is not null && topPressure.Value.Value * 100d / snapshot.GeneratedCount >= 50d)
    {
        warnings.Add($"{topPressure.Value.Key} pressure dominates runtime and may suppress manifold-local emotional differentiation.");
    }

    if (topPrimaryCadence is not null
        && !string.Equals(topPrimaryCadence.Value.Key, "none", StringComparison.OrdinalIgnoreCase)
        && topPrimaryCadence.Value.Value * 100d / snapshot.GeneratedCount >= 35d)
    {
        warnings.Add($"{topPrimaryCadence.Value.Key} leads primary cadence too often and risks temporal sameness.");
    }

    if (snapshot.CadenceRepetitionCount * 100d / Math.Max(1, snapshot.GeneratedCount - 1) >= 20d)
    {
        warnings.Add("adjacent primary-cadence repetition is elevated and may produce breathing fatigue.");
    }

    if (topTemplate is not null && topTemplate.Value.Value * 100d / Math.Max(1, snapshot.TemplateCounts.Values.Sum()) >= 8d)
    {
        warnings.Add($"{topTemplate.Value.Key} risks overdominant template fatigue.");
    }

    foreach (var activation in snapshot.ManifoldCadenceActivation.Where(item => item.Total >= 10 && item.ActivationRate < 0.7d))
    {
        warnings.Add($"{activation.Manifold} surfaces lexically but still under-activates manifold-local cadence.");
    }

    return warnings;
}

static List<string> BuildRecommendationLines(RuntimeEcologySnapshot snapshot)
{
    var lines = new List<string>();
    var weakestActivation = snapshot.ManifoldCadenceActivation
        .Where(item => item.Total >= 10 && item.ActivationRate < 0.85d)
        .OrderBy(item => item.ActivationRate)
        .FirstOrDefault();
    var topPrimaryCadence = GetTopPair(snapshot.PrimaryCadenceCounts);
    var topTemplate = GetTopPair(snapshot.TemplateCounts);

    if (weakestActivation is not null)
    {
        lines.Add($"strengthen cadence identity for `{weakestActivation.Manifold}` before opening another manifold wave.");
    }

    if (topPrimaryCadence is not null && !string.Equals(topPrimaryCadence.Value.Key, "none", StringComparison.OrdinalIgnoreCase))
    {
        lines.Add($"add silence and residue variants that compete specifically with `{topPrimaryCadence.Value.Key}` as a primary cadence.");
    }

    if (topTemplate is not null)
    {
        lines.Add($"review `{topTemplate.Value.Key}` for cadence fatigue and consider splitting it into softer local rhythm variants.");
    }

    if (snapshot.LegacyHeavyCount > snapshot.NeutralHeavyCount)
    {
        lines.Add("continue neutral foundation tuning carefully; old gravity still outweighs procedural neutrality in this snapshot.");
    }
    else
    {
        lines.Add("keep old-core atmosphere intact and focus next on manifold-local autonomy rather than stronger legacy suppression.");
    }

    return lines;
}

static List<string> BuildComparisonDiagnosis(
    RuntimeEcologySnapshot first,
    RuntimeEcologySnapshot second,
    string? topAtmosphereA,
    string? topAtmosphereB,
    string? topCadenceA,
    string? topCadenceB,
    string? topPressureA,
    string? topPressureB)
{
    var lines = new List<string>();
    var atmosphereA = string.IsNullOrWhiteSpace(topAtmosphereA) ? "none" : topAtmosphereA;
    var atmosphereB = string.IsNullOrWhiteSpace(topAtmosphereB) ? "none" : topAtmosphereB;
    var cadenceA = string.IsNullOrWhiteSpace(topCadenceA) ? "none" : topCadenceA;
    var cadenceB = string.IsNullOrWhiteSpace(topCadenceB) ? "none" : topCadenceB;
    var pressureA = string.IsNullOrWhiteSpace(topPressureA) ? "none" : topPressureA;
    var pressureB = string.IsNullOrWhiteSpace(topPressureB) ? "none" : topPressureB;

    if (!string.Equals(atmosphereA, atmosphereB, StringComparison.OrdinalIgnoreCase))
    {
        lines.Add($"runtime lead shifted from `{atmosphereA}` to `{atmosphereB}`.");
    }

    if (!string.Equals(cadenceA, cadenceB, StringComparison.OrdinalIgnoreCase))
    {
        lines.Add($"primary temporal skeleton moved from `{cadenceA}` to `{cadenceB}`.");
    }

    if (!string.Equals(pressureA, pressureB, StringComparison.OrdinalIgnoreCase))
    {
        lines.Add($"dominant emotional pressure changed from `{pressureA}` to `{pressureB}`.");
    }

    var legacyDelta = GetPercent(second.LegacyHeavyCount, second.GeneratedCount) - GetPercent(first.LegacyHeavyCount, first.GeneratedCount);
    if (Math.Abs(legacyDelta) >= 3d)
    {
        lines.Add(legacyDelta < 0
            ? $"legacy gravity fell by {Math.Abs(legacyDelta):0.0} percentage points."
            : $"legacy gravity rose by {legacyDelta:0.0} percentage points.");
    }

    var cadenceRepeatDelta = GetPercent(second.CadenceRepetitionCount, Math.Max(1, second.GeneratedCount - 1)) - GetPercent(first.CadenceRepetitionCount, Math.Max(1, first.GeneratedCount - 1));
    if (Math.Abs(cadenceRepeatDelta) >= 3d)
    {
        lines.Add(cadenceRepeatDelta < 0
            ? $"cadence repetition relaxed by {Math.Abs(cadenceRepeatDelta):0.0} percentage points."
            : $"cadence repetition increased by {cadenceRepeatDelta:0.0} percentage points.");
    }

    if (lines.Count == 0)
    {
        lines.Add("no major ecology drift detected between the two snapshots.");
    }

    return lines;
}

static void SaveSnapshot(RuntimeEcologySnapshot snapshot, string snapshotPath)
{
    var json = JsonSerializer.Serialize(snapshot, new JsonSerializerOptions
    {
        WriteIndented = true
    });

    var directory = Path.GetDirectoryName(snapshotPath);
    if (!string.IsNullOrWhiteSpace(directory))
    {
        Directory.CreateDirectory(directory);
    }

    File.WriteAllText(snapshotPath, json);
}

static RuntimeEcologySnapshot LoadSnapshot(string snapshotPath)
{
    var json = File.ReadAllText(snapshotPath);
    return JsonSerializer.Deserialize<RuntimeEcologySnapshot>(json)
           ?? throw new InvalidOperationException($"Cannot deserialize snapshot: {snapshotPath}");
}

static string ResolveSnapshotOutputPath(string snapshotPath, string projectRoot)
{
    if (Path.IsPathRooted(snapshotPath) || !string.IsNullOrWhiteSpace(Path.GetDirectoryName(snapshotPath)))
    {
        return Path.GetFullPath(snapshotPath);
    }

    return Path.GetFullPath(Path.Combine(projectRoot, "artifacts", "audit", snapshotPath));
}

static string ResolveSnapshotInputPath(string snapshotPath, string projectRoot)
{
    if (Path.IsPathRooted(snapshotPath) || !string.IsNullOrWhiteSpace(Path.GetDirectoryName(snapshotPath)))
    {
        return Path.GetFullPath(snapshotPath);
    }

    var currentDirectoryPath = Path.GetFullPath(snapshotPath);
    if (File.Exists(currentDirectoryPath))
    {
        return currentDirectoryPath;
    }

    var artifactsPath = Path.GetFullPath(Path.Combine(projectRoot, "artifacts", "audit", snapshotPath));
    if (File.Exists(artifactsPath))
    {
        return artifactsPath;
    }

    return currentDirectoryPath;
}

static IReadOnlyList<string> GetPositionalArgs(IReadOnlyList<string> args)
{
    var result = new List<string>();
    for (var index = 0; index < args.Count; index++)
    {
        var current = args[index];
        if (string.Equals(current, "--snapshot", StringComparison.OrdinalIgnoreCase))
        {
            index++;
            continue;
        }

        if (current.StartsWith("--", StringComparison.OrdinalIgnoreCase))
        {
            continue;
        }

        result.Add(current);
    }

    return result;
}

static string? GetOptionValue(IReadOnlyList<string> args, string optionName)
{
    for (var index = 0; index < args.Count - 1; index++)
    {
        if (string.Equals(args[index], optionName, StringComparison.OrdinalIgnoreCase))
        {
            return args[index + 1];
        }
    }

    return null;
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

    return $"{GetPercent(part, total):0.0}%";
}

static double GetPercent(int part, int total)
{
    if (total <= 0)
    {
        return 0d;
    }

    return part * 100d / total;
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
        ["weather_systems"] = new(["announcement", "procedural_report", "inventory", "interrupted_note", "delayed_implication", "bulletin_fragment"], StringComparer.OrdinalIgnoreCase),
        ["observatory"] = new(["suspended_statement", "interrupted_memory", "interrupted_note", "delayed_implication", "threshold_state"], StringComparer.OrdinalIgnoreCase),
        ["hydroelectric"] = new(["procedural_report", "inventory", "suspended_statement", "procedural_residue", "object_pressure", "maintenance_note"], StringComparer.OrdinalIgnoreCase),
        ["coastal_fog"] = new(["announcement", "suspended_statement", "procedural_residue", "interrupted_note", "incomplete_instruction", "bulletin_fragment"], StringComparer.OrdinalIgnoreCase),
        ["radar_stations"] = new(["procedural_report", "interrupted_note", "delayed_implication", "threshold_state", "procedural_residue"], StringComparer.OrdinalIgnoreCase),
        ["sanatorium"] = new(["quiet_instruction", "suspended_statement", "procedural_residue", "threshold_state", "incomplete_instruction"], StringComparer.OrdinalIgnoreCase),
        ["mall"] = new(["announcement", "inventory", "static_observation", "procedural_residue", "object_pressure"], StringComparer.OrdinalIgnoreCase),
        ["hospitality"] = new(["quiet_instruction", "ceremonial", "suspended_statement", "delayed_implication", "procedural_residue", "threshold_state"], StringComparer.OrdinalIgnoreCase),
        ["museum"] = new(["museum_label", "inventory", "object_pressure", "procedural_residue", "maintenance_note"], StringComparer.OrdinalIgnoreCase),
        ["airport"] = new(["announcement", "procedural_report", "quiet_instruction"], StringComparer.OrdinalIgnoreCase)
    };

    return preferredByManifold.TryGetValue(manifold, out var preferred)
           && cadences.Any(preferred.Contains);
}

static void PrintMonotonySignal(string label, IReadOnlyDictionary<string, int> counts, int total)
{
    if (total <= 0)
    {
        Console.WriteLine($"  - {label}: none");
        return;
    }

    var top = GetTopPair(counts);
    if (top is null)
    {
        Console.WriteLine($"  - {label}: none");
        return;
    }

    Console.WriteLine($"  - {label}: {top.Value.Key} at {ToPercent(top.Value.Value, total)}");
}

static KeyValuePair<string, int>? GetTopPair(IReadOnlyDictionary<string, int> counts)
{
    var filtered = counts
        .Where(pair => !string.Equals(pair.Key, "none", StringComparison.OrdinalIgnoreCase))
        .ToList();

    if (filtered.Count == 0)
    {
        return null;
    }

    return filtered
        .OrderByDescending(pair => pair.Value)
        .ThenBy(pair => pair.Key, StringComparer.OrdinalIgnoreCase)
        .First();
}

static string? GetTopKey(IReadOnlyDictionary<string, int> counts)
{
    return GetTopPair(counts)?.Key;
}

static string FormatTop(IReadOnlyDictionary<string, int> counts, int total)
{
    var top = GetTopPair(counts);
    return top is null ? "none" : $"{top.Value.Key} ({ToPercent(top.Value.Value, total)})";
}

static double GetActivationPercent(RuntimeEcologySnapshot snapshot, string manifold)
{
    var activation = snapshot.ManifoldCadenceActivation.FirstOrDefault(item => string.Equals(item.Manifold, manifold, StringComparison.OrdinalIgnoreCase));
    return activation is null ? 0d : GetPercent(activation.Preferred, activation.Total);
}

static void PrintComparisonLine(string label, double before, double after)
{
    Console.WriteLine($"  - {label}: {before:0.0}% -> {after:0.0}%");
}

file sealed class RuntimeEcologySnapshot
{
    public string Mode { get; set; } = string.Empty;
    public string Absurdity { get; set; } = string.Empty;
    public int RequestedCount { get; set; }
    public int GeneratedCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Dictionary<string, int> AtmosphereCounts { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, int> DominantManifoldCounts { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, int> PressureCounts { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, int> CadenceCounts { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, int> PrimaryCadenceCounts { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, int> TemplateCounts { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public int LegacyHeavyCount { get; set; }
    public int NeutralHeavyCount { get; set; }
    public int CadenceRepetitionCount { get; set; }
    public List<RepeatedOutput> RepeatedOutputs { get; set; } = [];
    public List<ManifoldCadenceActivation> ManifoldCadenceActivation { get; set; } = [];
}

file sealed record RepeatedOutput(string Text, int Count);

file sealed record ManifoldCadenceActivation(string Manifold, int Total, int Preferred)
{
    public double ActivationRate => Total <= 0 ? 0d : Preferred / (double)Total;
}
