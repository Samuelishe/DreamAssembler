using System.Text.RegularExpressions;
using DreamAssembler.Core.Models;

namespace DreamAssembler.Core.Services;

/// <summary>
/// Выполняет подстановку значений в текстовый шаблон.
/// </summary>
public sealed class TemplateEngine
{
    private static readonly Regex PlaceholderRegex = new(@"\{(?<name>[a-zA-Z0-9_]+)\}", RegexOptions.Compiled);

    /// <summary>
    /// Подставляет значения категорий в шаблон.
    /// </summary>
    /// <param name="template">Шаблон.</param>
    /// <param name="values">Значения по категориям.</param>
    /// <returns>Готовый текст.</returns>
    public string Render(TemplateDefinition template, IReadOnlyDictionary<string, DictionaryEntry> values)
    {
        ArgumentNullException.ThrowIfNull(template);
        ArgumentNullException.ThrowIfNull(values);

        return PlaceholderRegex.Replace(template.Text, match =>
        {
            var key = match.Groups["name"].Value;
            return values.TryGetValue(key, out var entry) ? entry.Text : match.Value;
        });
    }
}
