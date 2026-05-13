namespace DreamAssembler.Core.Models;

/// <summary>
/// Содержит lightweight runtime-диагностику для одного сгенерированного результата.
/// </summary>
public sealed class GenerationDebugTrace
{
    /// <summary>
    /// Получает или задает идентификаторы шаблонов, использованных при сборке результата.
    /// </summary>
    public IReadOnlyList<string> TemplateIds { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Получает или задает cadence-классы, использованные при сборке результата.
    /// </summary>
    public IReadOnlyList<string> Cadences { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Получает или задает strong-manifold tags, встретившиеся в результате.
    /// </summary>
    public IReadOnlyList<string> StrongManifolds { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Получает или задает pressure-tags, встретившиеся в результате.
    /// </summary>
    public IReadOnlyList<string> PressureTags { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Получает или задает dominant atmospheric key на момент завершения результата.
    /// </summary>
    public string? DominantAtmosphereKey { get; set; }

    /// <summary>
    /// Получает или задает dominant strong manifold на момент завершения результата.
    /// </summary>
    public string? DominantStrongManifold { get; set; }

    /// <summary>
    /// Получает или задает dominant pressure tag на момент завершения результата.
    /// </summary>
    public string? DominantPressureTag { get; set; }

    /// <summary>
    /// Получает или задает число legacy-baseline тегов, использованных в результате.
    /// </summary>
    public int LegacyBaselineTagCount { get; set; }

    /// <summary>
    /// Получает или задает число neutral-foundation тегов, использованных в результате.
    /// </summary>
    public int NeutralFoundationTagCount { get; set; }
}
