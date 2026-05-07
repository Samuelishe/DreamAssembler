using DreamAssembler.Core.Enums;
using DreamAssembler.Core.Models;
using DreamAssembler.Core.Services;

namespace DreamAssembler.Core.Tests.Services;

/// <summary>
/// Содержит тесты для подстановки данных в шаблон.
/// </summary>
public sealed class TemplateEngineTests
{
    /// <summary>
    /// Проверяет корректную подстановку значений в плейсхолдеры шаблона.
    /// </summary>
    [Fact]
    public void Render_ReplacesTemplatePlaceholders()
    {
        var engine = new TemplateEngine();
        var template = new TemplateDefinition
        {
            Id = "test",
            Text = "{character} должен {action} {object}.",
            Mode = GenerationMode.Sentence,
            RequiredCategories = ["character", "action", "object"]
        };

        var values = new Dictionary<string, DictionaryEntry>
        {
            ["character"] = new() { Id = "character_1", Text = "библиотекарь", Category = "character" },
            ["action"] = new() { Id = "action_1", Text = "спрятать", Category = "action" },
            ["object"] = new() { Id = "object_1", Text = "чайник", Category = "object" }
        };

        var result = engine.Render(template, values);

        Assert.Equal("библиотекарь должен спрятать чайник.", result);
    }
}

