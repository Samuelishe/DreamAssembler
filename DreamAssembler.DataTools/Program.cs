using DreamAssembler.Core.Models;
using DreamAssembler.Core.Services;

var dataPath = args.Length > 0
    ? Path.GetFullPath(args[0])
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

PrintSummary(report);
PrintIssues(report.Issues);

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
