using DreamAssembler.Core.Enums;
using DreamAssembler.Core.Models;

namespace DreamAssembler.Core.Services;

/// <summary>
/// Предоставляет минимальные встроенные данные на случай отсутствия JSON-файлов.
/// </summary>
public static class FallbackDataProvider
{
    /// <summary>
    /// Возвращает минимальный набор словарных записей.
    /// </summary>
    public static IReadOnlyList<DictionaryEntry> GetDictionaryEntries()
    {
        return
        [
            new DictionaryEntry { Id = "tired_librarian", Text = "уставший библиотекарь", Category = "character", Slot = "character_subject", Tags = ["city", "quiet"], Absurdity = 0, Weight = 1.2 },
            new DictionaryEntry { Id = "pigeon_engineer", Text = "голубь с дипломом инженера", Category = "character", Slot = "character_subject", Tags = ["city", "absurd"], Absurdity = 2, Weight = 1.0 },
            new DictionaryEntry { Id = "accounting_brownie", Text = "домовой из бухгалтерии", Category = "character", Slot = "character_subject", Tags = ["office", "absurd"], Absurdity = 3, Weight = 0.8 },
            new DictionaryEntry { Id = "retired_projectionist", Text = "пенсионер-киномеханик", Category = "character", Slot = "character_subject", Tags = ["city", "nostalgia"], Absurdity = 0, Weight = 1.0 },
            new DictionaryEntry { Id = "hide", Text = "спрятать", Category = "action", Slot = "action_infinitive", Tags = ["secret"], Absurdity = 0, Weight = 1.0 },
            new DictionaryEntry { Id = "open", Text = "открыть", Category = "action", Slot = "action_infinitive", Tags = ["business"], Absurdity = 0, Weight = 1.0 },
            new DictionaryEntry { Id = "trace", Text = "разыскать", Category = "action", Slot = "action_infinitive", Tags = ["mystery"], Absurdity = 2, Weight = 0.9 },
            new DictionaryEntry { Id = "carry_through_city", Text = "провезти через весь город", Category = "action", Slot = "action_infinitive", Tags = ["city", "daily"], Absurdity = 0, Weight = 1.0 },
            new DictionaryEntry { Id = "exchange_in_secret", Text = "тайно обменять", Category = "action", Slot = "action_infinitive", Tags = ["secret", "story"], Absurdity = 1, Weight = 0.9 },
            new DictionaryEntry { Id = "rusty_elevator", Text = "ржавый лифт", Category = "object", Slot = "object_direct", Tags = ["city", "decay", "industrial"], Absurdity = 1, Weight = 1.0 },
            new DictionaryEntry { Id = "forgotten_alarm_clocks", Text = "забытые будильники", Category = "object", Slot = "object_direct", Tags = ["home", "absurd"], Absurdity = 2, Weight = 1.0 },
            new DictionaryEntry { Id = "own_voice", Text = "собственный голос", Category = "object", Slot = "object_direct", Tags = ["inner", "absurd"], Absurdity = 3, Weight = 0.8 },
            new DictionaryEntry { Id = "paper_moon_ticket", Text = "бумажный билет на луну", Category = "object", Slot = "object_direct", Tags = ["dream", "surreal"], Absurdity = 2, Weight = 0.9 },
            new DictionaryEntry { Id = "rolled_city_map", Text = "свернутую карту города", Category = "object", Slot = "object_direct", Tags = ["city", "story"], Absurdity = 0, Weight = 0.9 },
            new DictionaryEntry { Id = "night_pharmacy", Text = "круглосуточной аптеке", Category = "place", Slot = "place_in", Tags = ["city", "night"], Absurdity = 0, Weight = 1.1 },
            new DictionaryEntry { Id = "inward_windows_city", Text = "городе, где все окна смотрят внутрь людей", Category = "place", Slot = "place_in", Tags = ["city", "surreal"], Absurdity = 3, Weight = 0.9 },
            new DictionaryEntry { Id = "night_shop", Text = "ночном магазине", Category = "place", Slot = "place_in", Tags = ["city", "night"], Absurdity = 0, Weight = 1.0 },
            new DictionaryEntry { Id = "courtyard_with_radio", Text = "дворе, где старое радио знает все новости заранее", Category = "place", Slot = "place_in", Tags = ["city", "surreal"], Absurdity = 2, Weight = 0.9 },
            new DictionaryEntry { Id = "library_basement", Text = "подвале библиотеки", Category = "place", Slot = "place_in", Tags = ["quiet", "story"], Absurdity = 1, Weight = 0.9 },
            new DictionaryEntry { Id = "people_remember_childhood", Text = "каждый посетитель помнит его детство", Category = "twist", Slot = "twist_clause", Tags = ["memory", "absurd"], Absurdity = 2, Weight = 1.0 },
            new DictionaryEntry { Id = "truth_tuesday", Text = "по вторникам запрещено говорить правду", Category = "twist", Slot = "twist_clause", Tags = ["rule", "absurd"], Absurdity = 3, Weight = 1.0 },
            new DictionaryEntry { Id = "no_noise_after_midnight", Text = "ему запретили шуметь после полуночи", Category = "twist", Slot = "twist_clause", Tags = ["rule", "night"], Absurdity = 1, Weight = 1.0 },
            new DictionaryEntry { Id = "every_lamp_disagrees", Text = "каждый фонарь с ним не согласен", Category = "twist", Slot = "twist_clause", Tags = ["city", "surreal"], Absurdity = 2, Weight = 0.9 },
            new DictionaryEntry { Id = "building_remembers_name", Text = "само здание знает его по имени", Category = "twist", Slot = "twist_clause", Tags = ["city", "story"], Absurdity = 1, Weight = 0.8 },
            new DictionaryEntry { Id = "foggy", Text = "туманной", Category = "atmosphere", Slot = "atmosphere_state", Tags = ["mood"], Absurdity = 0, Weight = 1.0 },
            new DictionaryEntry { Id = "whispering", Text = "шепчущей", Category = "atmosphere", Slot = "atmosphere_state", Tags = ["mood", "surreal"], Absurdity = 2, Weight = 0.9 },
            new DictionaryEntry { Id = "ceremonially_quiet", Text = "церемонно тихой", Category = "atmosphere", Slot = "atmosphere_state", Tags = ["quiet", "story"], Absurdity = 1, Weight = 0.9 },
            new DictionaryEntry { Id = "rainy_and_muted", Text = "дождливо притихшей", Category = "atmosphere", Slot = "atmosphere_state", Tags = ["weather", "mood"], Absurdity = 0, Weight = 0.9 },
            new DictionaryEntry { Id = "uneasy", Text = "тревожно", Category = "emotion", Slot = "emotion_group_state", Tags = ["mood"], Absurdity = 0, Weight = 1.0 },
            new DictionaryEntry { Id = "quietly_anxious", Text = "тихо тревожно", Category = "emotion", Slot = "emotion_observer_state", Tags = ["mood", "story"], Absurdity = 0, Weight = 0.9 },
            new DictionaryEntry { Id = "slightly_embarrassing", Text = "слегка неловко", Category = "emotion", Slot = "emotion_group_state", Tags = ["comic", "daily"], Absurdity = 0, Weight = 0.9 },
            new DictionaryEntry { Id = "not_themselves", Text = "не по себе", Category = "emotion", Slot = "emotion_group_state", Tags = ["mood", "story"], Absurdity = 0, Weight = 0.9 },
            new DictionaryEntry { Id = "bureaucratic_fantasy", Text = "бюрократическое фэнтези", Category = "genre", Slot = "genre_label", Tags = ["story"], Absurdity = 2, Weight = 0.8 },
            new DictionaryEntry { Id = "city_parable", Text = "городская притча", Category = "genre", Slot = "genre_label", Tags = ["story", "daily"], Absurdity = 0, Weight = 1.0 },
            new DictionaryEntry { Id = "night_urban_fable", Text = "ночная городская басня", Category = "genre", Slot = "genre_label", Tags = ["night", "city", "story"], Absurdity = 1, Weight = 0.9 },
            new DictionaryEntry { Id = "deadpan_style", Text = "с серьезной интонацией", Category = "style", Slot = "style_note", Tags = ["tone"], Absurdity = 0, Weight = 1.0 },
            new DictionaryEntry { Id = "late_night_radio_style", Text = "как ночной радиомонолог", Category = "style", Slot = "style_note", Tags = ["night", "tone"], Absurdity = 1, Weight = 0.9 },
            new DictionaryEntry { Id = "with_tender_irony", Text = "с мягкой иронией", Category = "style", Slot = "style_note", Tags = ["tone", "comic"], Absurdity = 0, Weight = 0.9 },
            new DictionaryEntry { Id = "quiet_rebellion", Text = "тихий бунт предметов", Category = "concept", Slot = "concept_label", Tags = ["story", "absurd"], Absurdity = 2, Weight = 0.9 },
            new DictionaryEntry { Id = "bureaucracy_of_memories", Text = "бюрократия воспоминаний", Category = "concept", Slot = "concept_label", Tags = ["bureaucracy", "memory"], Absurdity = 2, Weight = 0.8 },
            new DictionaryEntry { Id = "silent_rehearsal_of_city", Text = "тихая репетиция города", Category = "concept", Slot = "concept_label", Tags = ["city", "story"], Absurdity = 1, Weight = 0.8 },
            new DictionaryEntry { Id = "everybody_spoke_whisper", Text = "все говорили шепотом", Category = "condition", Slot = "condition_clause", Tags = ["quiet", "story"], Absurdity = 0, Weight = 1.0 },
            new DictionaryEntry { Id = "nobody_trusted_clocks", Text = "никто не доверял часам", Category = "condition", Slot = "condition_clause", Tags = ["time", "surreal"], Absurdity = 2, Weight = 0.9 }
        ];
    }

    /// <summary>
    /// Возвращает минимальный набор шаблонов.
    /// </summary>
    public static IReadOnlyList<TemplateDefinition> GetTemplates()
    {
        return
        [
            new TemplateDefinition
            {
                Id = "sentence_character_action_object_place_twist",
                Text = "{character} должен {action} {object} в {place}, но {twist}.",
                Mode = GenerationMode.Sentence,
                RequiredCategories = ["character", "action", "object", "place", "twist"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["character"] = "character_subject",
                    ["action"] = "action_infinitive",
                    ["object"] = "object_direct",
                    ["place"] = "place_in",
                    ["twist"] = "twist_clause"
                },
                Tags = ["story"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 1.0
            },
            new TemplateDefinition
            {
                Id = "sentence_place_character_action_object",
                Text = "В {place} {character} пытается {action} {object}.",
                Mode = GenerationMode.Sentence,
                RequiredCategories = ["place", "character", "action", "object"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in",
                    ["character"] = "character_subject",
                    ["action"] = "action_infinitive",
                    ["object"] = "object_direct"
                },
                Tags = ["scene"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 1.0
            },
            new TemplateDefinition
            {
                Id = "sentence_character_place_twist",
                Text = "{character} однажды понял, что в {place} {twist}.",
                Mode = GenerationMode.Sentence,
                RequiredCategories = ["character", "place", "twist"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["character"] = "character_subject",
                    ["place"] = "place_in",
                    ["twist"] = "twist_clause"
                },
                Tags = ["story", "surreal"],
                MinAbsurdity = 1,
                MaxAbsurdity = 3,
                Weight = 0.9
            },
            new TemplateDefinition
            {
                Id = "sentence_place_condition",
                Text = "В {place} {condition}.",
                Mode = GenerationMode.Sentence,
                RequiredCategories = ["place", "condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in",
                    ["condition"] = "condition_clause"
                },
                Tags = ["scene", "story", "quiet"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.9
            },
            new TemplateDefinition
            {
                Id = "shorttext_intro",
                Text = "В {place} все началось с того, что {character} решил {action} {object}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["place", "character", "action", "object"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in",
                    ["character"] = "character_subject",
                    ["action"] = "action_infinitive",
                    ["object"] = "object_direct"
                },
                CompositionRole = "setup",
                Tags = ["story"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 1.0
            },
            new TemplateDefinition
            {
                Id = "shorttext_twist",
                Text = "Обстановка стала {atmosphere}, потому что {twist}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["atmosphere", "twist"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["atmosphere"] = "atmosphere_state",
                    ["twist"] = "twist_clause"
                },
                CompositionRole = "development",
                Tags = ["story", "mood"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 1.0
            },
            new TemplateDefinition
            {
                Id = "shorttext_reflection",
                Text = "Из-за этого всем стало {emotion}, и всем показалось, что это {concept}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["emotion", "concept"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["emotion"] = "emotion_group_state",
                    ["concept"] = "concept_label"
                },
                CompositionRole = "reflection",
                Tags = ["story", "mood", "concept"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.9
            },
            new TemplateDefinition
            {
                Id = "shorttext_observer",
                Text = "Со стороны все звучало {emotion}, хотя на деле {condition}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["emotion", "condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["emotion"] = "emotion_observer_state",
                    ["condition"] = "condition_clause"
                },
                CompositionRole = "observation",
                Tags = ["story", "mood", "surreal"],
                MinAbsurdity = 1,
                MaxAbsurdity = 3,
                Weight = 0.8
            },
            new TemplateDefinition
            {
                Id = "shorttext_style_note",
                Text = "Если пересказывать это {style}, все звучит почти убедительно.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["style"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["style"] = "style_note"
                },
                CompositionRole = "meta",
                Tags = ["tone", "story"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.8
            },
            new TemplateDefinition
            {
                Id = "shorttext_condition",
                Text = "С самого начала было ясно одно: {condition}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["condition"] = "condition_clause"
                },
                CompositionRole = "scene",
                Tags = ["story", "scene", "quiet"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.9
            },
            new TemplateDefinition
            {
                Id = "shorttext_reveal",
                Text = "Позже выяснилось, что в {place} {condition}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["place", "condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in",
                    ["condition"] = "condition_clause"
                },
                CompositionRole = "development",
                Tags = ["story", "scene", "city"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.9
            },
            new TemplateDefinition
            {
                Id = "shorttext_reaction",
                Text = "Никто особенно не спорил, когда стало ясно, что {twist}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["twist"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["twist"] = "twist_clause"
                },
                CompositionRole = "reaction",
                Tags = ["story", "scene", "quiet"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.8
            },
            new TemplateDefinition
            {
                Id = "idea_character_action_object_place_twist",
                Text = "Идея: {character} должен {action} {object} в {place}, и при этом {twist}.",
                Mode = GenerationMode.Idea,
                RequiredCategories = ["character", "action", "object", "place", "twist"],
                Tags = ["story", "concept"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 1.1
            },
            new TemplateDefinition
            {
                Id = "idea_place_concept_genre",
                Text = "Идея: история о том, как в {place} возникает {concept}. По тону это {genre}.",
                Mode = GenerationMode.Idea,
                RequiredCategories = ["place", "concept", "genre"],
                Tags = ["story", "concept", "city"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 1.0
            },
            new TemplateDefinition
            {
                Id = "idea_character_place_condition_genre",
                Text = "Идея: в {place} {condition}, а в центре истории {character}. По жанру это {genre}.",
                Mode = GenerationMode.Idea,
                RequiredCategories = ["place", "condition", "character", "genre"],
                Tags = ["story", "scene", "concept"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.9
            }
        ];
    }
}
