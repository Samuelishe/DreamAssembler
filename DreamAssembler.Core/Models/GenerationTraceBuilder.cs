namespace DreamAssembler.Core.Models;

/// <summary>
/// Накапливает lightweight runtime-след по мере сборки одного результата.
/// </summary>
public sealed class GenerationTraceBuilder
{
    private readonly HashSet<string> _templateIds = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<string> _cadences = [];
    private readonly HashSet<string> _strongManifolds = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _pressureTags = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Получает или задает число legacy-baseline тегов, замеченных в результате.
    /// </summary>
    public int LegacyBaselineTagCount { get; set; }

    /// <summary>
    /// Получает или задает число neutral-foundation тегов, замеченных в результате.
    /// </summary>
    public int NeutralFoundationTagCount { get; set; }

    /// <summary>
    /// Запоминает шаблон и его cadence.
    /// </summary>
    public void RememberTemplate(string templateId, string cadence)
    {
        if (!string.IsNullOrWhiteSpace(templateId))
        {
            _templateIds.Add(templateId);
        }

        if (!string.IsNullOrWhiteSpace(cadence) && !string.Equals(cadence, "default", StringComparison.OrdinalIgnoreCase))
        {
            _cadences.Add(cadence);
        }
    }

    /// <summary>
    /// Запоминает strong manifold tag.
    /// </summary>
    public void RememberStrongManifold(string tag)
    {
        if (!string.IsNullOrWhiteSpace(tag))
        {
            _strongManifolds.Add(tag);
        }
    }

    /// <summary>
    /// Запоминает pressure tag.
    /// </summary>
    public void RememberPressureTag(string tag)
    {
        if (!string.IsNullOrWhiteSpace(tag))
        {
            _pressureTags.Add(tag);
        }
    }

    /// <summary>
    /// Собирает итоговый trace.
    /// </summary>
    public GenerationDebugTrace Build(string? dominantAtmosphereKey, string? dominantStrongManifold, string? dominantPressureTag)
    {
        return new GenerationDebugTrace
        {
            TemplateIds = _templateIds.OrderBy(value => value, StringComparer.OrdinalIgnoreCase).ToArray(),
            Cadences = _cadences.ToArray(),
            StrongManifolds = _strongManifolds.OrderBy(value => value, StringComparer.OrdinalIgnoreCase).ToArray(),
            PressureTags = _pressureTags.OrderBy(value => value, StringComparer.OrdinalIgnoreCase).ToArray(),
            DominantAtmosphereKey = dominantAtmosphereKey,
            DominantStrongManifold = dominantStrongManifold,
            DominantPressureTag = dominantPressureTag,
            LegacyBaselineTagCount = LegacyBaselineTagCount,
            NeutralFoundationTagCount = NeutralFoundationTagCount
        };
    }
}
