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
            new DictionaryEntry { Id = "hide", Text = "спрятать", Category = "action", Slot = "action_infinitive", Tags = ["secret", "compat:hideable"], Absurdity = 0, Weight = 1.0 },
            new DictionaryEntry { Id = "open", Text = "открыть", Category = "action", Slot = "action_infinitive", Tags = ["business", "compat:openable"], Absurdity = 0, Weight = 1.0 },
            new DictionaryEntry { Id = "trace", Text = "разыскать", Category = "action", Slot = "action_infinitive", Tags = ["mystery", "compat:searchable"], Absurdity = 2, Weight = 0.9 },
            new DictionaryEntry { Id = "carry_through_city", Text = "провезти через весь город", Category = "action", Slot = "action_infinitive", Tags = ["city", "daily", "compat:portable"], Absurdity = 0, Weight = 1.0 },
            new DictionaryEntry { Id = "exchange_in_secret", Text = "тайно обменять", Category = "action", Slot = "action_infinitive", Tags = ["secret", "story", "compat:exchangeable"], Absurdity = 1, Weight = 0.9 },
            new DictionaryEntry { Id = "rusty_elevator", Text = "ржавый лифт", Category = "object", Slot = "object_direct", Tags = ["city", "decay", "industrial", "compat:repairable"], Absurdity = 1, Weight = 1.0 },
            new DictionaryEntry { Id = "forgotten_alarm_clocks", Text = "забытые будильники", Category = "object", Slot = "object_direct", Tags = ["home", "absurd", "compat:portable", "compat:repairable", "compat:wakeable", "compat:hideable"], Absurdity = 2, Weight = 1.0 },
            new DictionaryEntry { Id = "own_voice", Text = "собственный голос", Category = "object", Slot = "object_direct", Tags = ["inner", "absurd", "compat:whisperable", "compat:rewriteable"], Absurdity = 3, Weight = 0.8 },
            new DictionaryEntry { Id = "paper_moon_ticket", Text = "бумажный билет на луну", Category = "object", Slot = "object_direct", Tags = ["dream", "surreal", "compat:portable", "compat:hideable", "compat:exchangeable", "compat:rewriteable", "compat:searchable"], Absurdity = 2, Weight = 0.9 },
            new DictionaryEntry { Id = "rolled_city_map", Text = "свернутую карту города", Category = "object", Slot = "object_direct", Tags = ["city", "story", "compat:portable", "compat:searchable", "compat:hideable", "compat:protectable", "compat:rewriteable"], Absurdity = 0, Weight = 0.9 },
            new DictionaryEntry { Id = "night_pharmacy", Text = "круглосуточной аптеке", Category = "place", Slot = "place_in", Tags = ["city", "night"], Absurdity = 0, Weight = 1.1 },
            new DictionaryEntry { Id = "inward_windows_city", Text = "городе, где все окна смотрят внутрь людей,", Category = "place", Slot = "place_in_clause", Tags = ["city", "surreal"], Absurdity = 3, Weight = 0.9 },
            new DictionaryEntry { Id = "night_shop", Text = "ночном магазине", Category = "place", Slot = "place_in", Tags = ["city", "night"], Absurdity = 0, Weight = 1.0 },
            new DictionaryEntry { Id = "courtyard_with_radio", Text = "дворе, где старое радио знает все новости заранее,", Category = "place", Slot = "place_in_clause", Tags = ["city", "surreal"], Absurdity = 2, Weight = 0.9 },
            new DictionaryEntry { Id = "library_basement", Text = "подвале библиотеки", Category = "place", Slot = "place_in", Tags = ["quiet", "story"], Absurdity = 1, Weight = 0.9 },
            new DictionaryEntry { Id = "people_remember_childhood", Text = "каждый посетитель помнит его детство", Category = "twist", Slot = "twist_character_clause", Tags = ["memory", "absurd"], Absurdity = 2, Weight = 1.0 },
            new DictionaryEntry { Id = "truth_tuesday", Text = "по вторникам запрещено говорить правду", Category = "twist", Slot = "twist_general_clause", Tags = ["rule", "absurd"], Absurdity = 3, Weight = 1.0 },
            new DictionaryEntry { Id = "no_noise_after_midnight", Text = "ему запретили шуметь после полуночи", Category = "twist", Slot = "twist_character_clause", Tags = ["rule", "night"], Absurdity = 1, Weight = 1.0 },
            new DictionaryEntry { Id = "every_lamp_disagrees", Text = "каждый фонарь с ним не согласен", Category = "twist", Slot = "twist_character_clause", Tags = ["city", "surreal"], Absurdity = 2, Weight = 0.9 },
            new DictionaryEntry { Id = "building_remembers_name", Text = "само здание знает его по имени", Category = "twist", Slot = "twist_character_clause", Tags = ["city", "story"], Absurdity = 1, Weight = 0.8 },
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
            new DictionaryEntry { Id = "quiet_rebellion", Text = "тихий бунт предметов", Category = "concept", Slot = "concept_story_frame", Tags = ["story", "absurd"], Absurdity = 2, Weight = 0.9 },
            new DictionaryEntry { Id = "bureaucracy_of_memories", Text = "бюрократия воспоминаний", Category = "concept", Slot = "concept_reflection", Tags = ["bureaucracy", "memory"], Absurdity = 2, Weight = 0.8 },
            new DictionaryEntry { Id = "silent_rehearsal_of_city", Text = "тихая репетиция города", Category = "concept", Slot = "concept_reflection", Tags = ["city", "story"], Absurdity = 1, Weight = 0.8 },
            new DictionaryEntry { Id = "everybody_spoke_whisper", Text = "все говорили шепотом", Category = "condition", Slot = "condition_initial_state", Tags = ["quiet", "story"], Absurdity = 0, Weight = 1.0 },
            new DictionaryEntry { Id = "nobody_trusted_clocks", Text = "никто не доверял часам", Category = "condition", Slot = "condition_initial_state", Tags = ["time", "surreal"], Absurdity = 2, Weight = 0.9 },
            new DictionaryEntry { Id = "air_smelled_of_ozone", Text = "воздух пах озоном и архивной пылью", Category = "condition", Slot = "condition_scene_detail", Tags = ["city", "story"], Absurdity = 1, Weight = 0.9 },
            new DictionaryEntry { Id = "chairs_held_council", Text = "стулья вели молчаливое совещание", Category = "condition", Slot = "condition_reveal_state", Tags = ["bureaucracy", "surreal"], Absurdity = 3, Weight = 0.7 }
        ];
    }

    /// <summary>
    /// Возвращает минимальный набор фрагментов для ассоциативных фраз.
    /// </summary>
    public static IReadOnlyList<AssociationFragmentEntry> GetAssociationFragments()
    {
        return
        [
            new AssociationFragmentEntry { Id = "adjective_m_quiet", Text = "тихий", Kind = "adjective_m", Weight = 1.0 },
            new AssociationFragmentEntry { Id = "adjective_f_quiet", Text = "тихая", Kind = "adjective_f", Weight = 1.0 },
            new AssociationFragmentEntry { Id = "adjective_n_quiet", Text = "тихое", Kind = "adjective_n", Weight = 1.0 },
            new AssociationFragmentEntry { Id = "adjective_m_glass", Text = "стеклянный", Kind = "adjective_m", Weight = 0.9 },
            new AssociationFragmentEntry { Id = "adjective_f_glass", Text = "стеклянная", Kind = "adjective_f", Weight = 0.9 },
            new AssociationFragmentEntry { Id = "adjective_n_glass", Text = "стеклянное", Kind = "adjective_n", Weight = 0.9 },
            new AssociationFragmentEntry { Id = "adjective_m_paper", Text = "бумажный", Kind = "adjective_m", Weight = 0.9 },
            new AssociationFragmentEntry { Id = "adjective_f_paper", Text = "бумажная", Kind = "adjective_f", Weight = 0.9 },
            new AssociationFragmentEntry { Id = "adjective_n_paper", Text = "бумажное", Kind = "adjective_n", Weight = 0.9 },
            new AssociationFragmentEntry { Id = "adjective_m_night", Text = "ночной", Kind = "adjective_m", Weight = 0.8 },
            new AssociationFragmentEntry { Id = "adjective_f_night", Text = "ночная", Kind = "adjective_f", Weight = 0.8 },
            new AssociationFragmentEntry { Id = "adjective_n_night", Text = "ночное", Kind = "adjective_n", Weight = 0.8 },
            new AssociationFragmentEntry { Id = "noun_m_archive", Text = "архив", Kind = "noun_m", Weight = 1.0 },
            new AssociationFragmentEntry { Id = "noun_m_mechanism", Text = "механизм", Kind = "noun_m", Weight = 0.9 },
            new AssociationFragmentEntry { Id = "noun_f_staircase", Text = "лестница", Kind = "noun_f", Weight = 1.0 },
            new AssociationFragmentEntry { Id = "noun_f_memory", Text = "память", Kind = "noun_f", Weight = 0.9 },
            new AssociationFragmentEntry { Id = "noun_n_echo", Text = "эхо", Kind = "noun_n", Weight = 1.0 },
            new AssociationFragmentEntry { Id = "noun_n_schedule", Text = "расписание", Kind = "noun_n", Weight = 0.8 },
            new AssociationFragmentEntry { Id = "verb_past_m_trembled", Text = "дрожал", Kind = "verb_past_m", Weight = 1.0 },
            new AssociationFragmentEntry { Id = "verb_past_f_trembled", Text = "дрожала", Kind = "verb_past_f", Weight = 1.0 },
            new AssociationFragmentEntry { Id = "verb_past_n_trembled", Text = "дрожало", Kind = "verb_past_n", Weight = 1.0 },
            new AssociationFragmentEntry { Id = "verb_past_m_listened", Text = "слушал", Kind = "verb_past_m", Weight = 0.8 },
            new AssociationFragmentEntry { Id = "verb_past_f_listened", Text = "слушала", Kind = "verb_past_f", Weight = 0.8 },
            new AssociationFragmentEntry { Id = "verb_past_n_listened", Text = "слушало", Kind = "verb_past_n", Weight = 0.8 },
            new AssociationFragmentEntry { Id = "other_raw_softly", Text = "тихо", Kind = "other_raw", Weight = 1.0 },
            new AssociationFragmentEntry { Id = "other_raw_perhaps", Text = "будто", Kind = "other_raw", Weight = 0.8 }
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
                Text = "{character} получает странное поручение: {action} {object} в {place}, но {twist}.",
                Mode = GenerationMode.Sentence,
                RequiredCategories = ["character", "action", "object", "place", "twist"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["character"] = "character_subject",
                    ["action"] = "action_infinitive",
                    ["object"] = "object_direct",
                    ["place"] = "place_in",
                    ["twist"] = "twist_character_clause"
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
                Id = "sentence_place_clause_character_action_object",
                Text = "В {place} {character} пытается {action} {object}.",
                Mode = GenerationMode.Sentence,
                RequiredCategories = ["place", "character", "action", "object"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in_clause",
                    ["character"] = "character_subject",
                    ["action"] = "action_infinitive",
                    ["object"] = "object_direct"
                },
                Tags = ["scene", "daily", "surreal"],
                MinAbsurdity = 1,
                MaxAbsurdity = 3,
                Weight = 0.8
            },
            new TemplateDefinition
            {
                Id = "sentence_character_place_twist",
                Text = "{character} однажды обнаруживает одну странность: в {place} {twist}.",
                Mode = GenerationMode.Sentence,
                RequiredCategories = ["character", "place", "twist"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["character"] = "character_subject",
                    ["place"] = "place_in",
                    ["twist"] = "twist_character_clause"
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
                    ["condition"] = "condition_scene_detail"
                },
                Tags = ["scene", "story", "quiet"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.9
            },
            new TemplateDefinition
            {
                Id = "sentence_place_clause_condition",
                Text = "В {place} {condition}.",
                Mode = GenerationMode.Sentence,
                RequiredCategories = ["place", "condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in_clause",
                    ["condition"] = "condition_scene_detail"
                },
                Tags = ["scene", "story", "surreal"],
                MinAbsurdity = 1,
                MaxAbsurdity = 3,
                Weight = 0.8
            },
            new TemplateDefinition
            {
                Id = "sentence_character_object_condition",
                Text = "{character} получает {object}, и вокруг {condition}.",
                Mode = GenerationMode.Sentence,
                RequiredCategories = ["character", "object", "condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["character"] = "character_subject",
                    ["object"] = "object_direct",
                    ["condition"] = "condition_initial_state"
                },
                Tags = ["story", "city"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.8
            },
            new TemplateDefinition
            {
                Id = "sentence_character_condition_action_object",
                Text = "{character} видит, что {condition}, и тогда приходится {action} {object}.",
                Mode = GenerationMode.Sentence,
                RequiredCategories = ["character", "condition", "action", "object"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["character"] = "character_subject",
                    ["condition"] = "condition_scene_detail",
                    ["action"] = "action_infinitive",
                    ["object"] = "object_direct"
                },
                Tags = ["story", "scene", "city"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.9
            },
            new TemplateDefinition
            {
                Id = "sentence_character_action_object_concept",
                Text = "{character} снова и снова пробует {action} {object}, словно все это уже было почти как {concept}.",
                Mode = GenerationMode.Sentence,
                RequiredCategories = ["character", "action", "object", "concept"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["character"] = "character_subject",
                    ["action"] = "action_infinitive",
                    ["object"] = "object_direct",
                    ["concept"] = "concept_reflection"
                },
                Tags = ["story", "concept", "quiet"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.8
            },
            new TemplateDefinition
            {
                Id = "sentence_place_atmosphere_twist",
                Text = "В {place} обстановка оставалась {atmosphere}, пока {twist}.",
                Mode = GenerationMode.Sentence,
                RequiredCategories = ["place", "atmosphere", "twist"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in",
                    ["atmosphere"] = "atmosphere_state",
                    ["twist"] = "twist_general_clause"
                },
                Tags = ["mood", "scene", "story"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.9
            },
            new TemplateDefinition
            {
                Id = "sentence_place_clause_atmosphere_twist",
                Text = "В {place} обстановка оставалась {atmosphere}, пока {twist}.",
                Mode = GenerationMode.Sentence,
                RequiredCategories = ["place", "atmosphere", "twist"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in_clause",
                    ["atmosphere"] = "atmosphere_state",
                    ["twist"] = "twist_general_clause"
                },
                Tags = ["mood", "scene", "surreal"],
                MinAbsurdity = 1,
                MaxAbsurdity = 3,
                Weight = 0.8
            },
            new TemplateDefinition
            {
                Id = "shorttext_intro",
                Text = "В {place} все началось с того, что пришлось {action} {object}. Рядом уже стоял {character}.",
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
                Cadence = "procedural_report",
                Tags = ["story"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 1.0
            },
            new TemplateDefinition
            {
                Id = "shorttext_intro_clause_place",
                Text = "В {place} все началось с того, что пришлось {action} {object}. Рядом уже стоял {character}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["place", "character", "action", "object"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in_clause",
                    ["character"] = "character_subject",
                    ["action"] = "action_infinitive",
                    ["object"] = "object_direct"
                },
                CompositionRole = "setup",
                Cadence = "procedural_report",
                Tags = ["story", "surreal"],
                MinAbsurdity = 1,
                MaxAbsurdity = 3,
                Weight = 0.8
            },
            new TemplateDefinition
            {
                Id = "shorttext_twist",
                Text = "И тогда сама обстановка стала {atmosphere}, потому что {twist}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["atmosphere", "twist"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["atmosphere"] = "atmosphere_state",
                    ["twist"] = "twist_general_clause"
                },
                CompositionRole = "development",
                Cadence = "ceremonial",
                Tags = ["story", "mood"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.8
            },
            new TemplateDefinition
            {
                Id = "shorttext_reflection",
                Text = "Из-за этого всем стало {emotion}, и все это напоминало: {concept}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["emotion", "concept"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["emotion"] = "emotion_group_state",
                    ["concept"] = "concept_reflection"
                },
                CompositionRole = "reflection",
                Cadence = "suspended_statement",
                Tags = ["story", "mood", "concept"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.9
            },
            new TemplateDefinition
            {
                Id = "shorttext_interpretation",
                Text = "Со временем все это стало выглядеть как {concept}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["concept"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["concept"] = "concept_reflection"
                },
                CompositionRole = "interpretation",
                Cadence = "delayed_revelation",
                Tags = ["story", "concept", "mood"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.8
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
                    ["condition"] = "condition_reveal_state"
                },
                CompositionRole = "observation",
                Cadence = "static_observation",
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
                Cadence = "quiet_instruction",
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
                    ["condition"] = "condition_initial_state"
                },
                CompositionRole = "scene",
                Cadence = "suspended_statement",
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
                    ["condition"] = "condition_reveal_state"
                },
                CompositionRole = "development",
                Cadence = "announcement",
                Tags = ["story", "scene", "city"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.9
            },
            new TemplateDefinition
            {
                Id = "shorttext_reveal_clause_place",
                Text = "Позже выяснилось, что в {place} {condition}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["place", "condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in_clause",
                    ["condition"] = "condition_reveal_state"
                },
                CompositionRole = "development",
                Cadence = "announcement",
                Tags = ["story", "scene", "surreal"],
                MinAbsurdity = 1,
                MaxAbsurdity = 3,
                Weight = 0.8
            },
            new TemplateDefinition
            {
                Id = "shorttext_scene_detail",
                Text = "Сначала в {place} {condition}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["place", "condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in",
                    ["condition"] = "condition_scene_detail"
                },
                CompositionRole = "scene",
                Cadence = "static_observation",
                Tags = ["story", "scene", "city"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.8
            },
            new TemplateDefinition
            {
                Id = "shorttext_scene_detail_clause_place",
                Text = "Сначала в {place} {condition}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["place", "condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in_clause",
                    ["condition"] = "condition_scene_detail"
                },
                CompositionRole = "scene",
                Cadence = "static_observation",
                Tags = ["story", "scene", "surreal"],
                MinAbsurdity = 1,
                MaxAbsurdity = 3,
                Weight = 0.7
            },
            new TemplateDefinition
            {
                Id = "shorttext_character_twist",
                Text = "{character} долго не мог отделаться от мысли, что можно {action} {object}, но вскоре оказалось, что {twist}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["character", "action", "object", "twist"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["character"] = "character_subject",
                    ["action"] = "action_infinitive",
                    ["object"] = "object_direct",
                    ["twist"] = "twist_character_clause"
                },
                CompositionRole = "development",
                Cadence = "interrupted_memory",
                Tags = ["story", "surreal", "character"],
                MinAbsurdity = 1,
                MaxAbsurdity = 3,
                Weight = 0.9
            },
            new TemplateDefinition
            {
                Id = "shorttext_atmosphere_place",
                Text = "Даже в {place} обстановка оставалась {atmosphere}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["place", "atmosphere"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in",
                    ["atmosphere"] = "atmosphere_state"
                },
                CompositionRole = "scene",
                Cadence = "static_observation",
                Tags = ["mood", "scene", "quiet"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.8
            },
            new TemplateDefinition
            {
                Id = "shorttext_atmosphere_place_clause",
                Text = "Даже в {place} обстановка оставалась {atmosphere}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["place", "atmosphere"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in_clause",
                    ["atmosphere"] = "atmosphere_state"
                },
                CompositionRole = "scene",
                Cadence = "static_observation",
                Tags = ["mood", "scene", "surreal"],
                MinAbsurdity = 1,
                MaxAbsurdity = 3,
                Weight = 0.7
            },
            new TemplateDefinition
            {
                Id = "shorttext_genre_turn",
                Text = "Так постепенно получалось не событие, а {genre}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["genre"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["genre"] = "genre_label"
                },
                CompositionRole = "interpretation",
                Cadence = "delayed_revelation",
                Tags = ["tone", "concept", "story"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.8
            },
            new TemplateDefinition
            {
                Id = "shorttext_concept_name",
                Text = "Тогда все это зазвучало как {concept}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["concept"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["concept"] = "concept_reflection"
                },
                CompositionRole = "reflection",
                Cadence = "suspended_statement",
                Tags = ["concept", "mood", "story"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.8
            },
            new TemplateDefinition
            {
                Id = "shorttext_meta_style_turn",
                Text = "В другом изложении это можно было бы подать {style}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["style"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["style"] = "style_note"
                },
                CompositionRole = "meta",
                Cadence = "quiet_instruction",
                Tags = ["tone", "story", "quiet"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.7
            },
            new TemplateDefinition
            {
                Id = "shorttext_reaction",
                Text = "Никто особенно не спорил, когда стало ясно, что {twist}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["twist"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["twist"] = "twist_general_clause"
                },
                CompositionRole = "reaction",
                Cadence = "announcement",
                Tags = ["story", "quiet", "rule"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.8
            },
            new TemplateDefinition
            {
                Id = "shorttext_inventory_scene",
                Text = "На месте были {object} и {condition}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["object", "condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["object"] = "object_direct",
                    ["condition"] = "condition_scene_detail"
                },
                CompositionRole = "scene",
                Cadence = "inventory",
                Tags = ["story", "scene", "quiet"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.8
            },
            new TemplateDefinition
            {
                Id = "shorttext_notice",
                Text = "По внутреннему объявлению, в {place} {condition}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["place", "condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in",
                    ["condition"] = "condition_reveal_state"
                },
                CompositionRole = "development",
                Cadence = "announcement",
                Tags = ["story", "quiet", "rule"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.75
            },
            new TemplateDefinition
            {
                Id = "shorttext_museum_label",
                Text = "Подпись к витрине была короткой: {concept}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["concept"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["concept"] = "concept_reflection"
                },
                CompositionRole = "observation",
                Cadence = "museum_label",
                Tags = ["story", "concept", "quiet"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.75
            },
            new TemplateDefinition
            {
                Id = "shorttext_quiet_instruction",
                Text = "Просили только об одном: {action} {object} без лишних вопросов.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["action", "object"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["action"] = "action_infinitive",
                    ["object"] = "object_direct"
                },
                CompositionRole = "reaction",
                Cadence = "quiet_instruction",
                Tags = ["story", "quiet", "procedural"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.75
            },
            new TemplateDefinition
            {
                Id = "shorttext_procedural_residue",
                Text = "После этого в {place} оставались только {object} и {condition}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["place", "object", "condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in",
                    ["object"] = "object_direct",
                    ["condition"] = "condition_scene_detail"
                },
                CompositionRole = "observation",
                Cadence = "procedural_residue",
                Tags = ["story", "quiet", "procedural"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.72
            },
            new TemplateDefinition
            {
                Id = "shorttext_interrupted_note",
                Text = "В служебной записи позже осталось только: {condition}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["condition"] = "condition_reveal_state"
                },
                CompositionRole = "observation",
                Cadence = "interrupted_note",
                Tags = ["story", "quiet", "procedural"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.72
            },
            new TemplateDefinition
            {
                Id = "shorttext_object_pressure",
                Text = "{object} дольше всего удерживал на себе {atmosphere}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["object", "atmosphere"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["object"] = "object_direct",
                    ["atmosphere"] = "atmosphere_state"
                },
                CompositionRole = "scene",
                Cadence = "object_pressure",
                Tags = ["story", "quiet", "mood"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.72
            },
            new TemplateDefinition
            {
                Id = "shorttext_delayed_implication",
                Text = "После этого уже трудно было понять, почему {condition}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["condition"] = "condition_reveal_state"
                },
                CompositionRole = "interpretation",
                Cadence = "delayed_implication",
                Tags = ["story", "quiet", "concept"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.72
            },
            new TemplateDefinition
            {
                Id = "shorttext_weather_bulletin_residue",
                Text = "К утру в сводке осталось только одно: {condition}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["condition"] = "condition_reveal_state"
                },
                CompositionRole = "observation",
                Cadence = "interrupted_note",
                Tags = ["story", "quiet", "procedural", "weather_systems"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.78
            },
            new TemplateDefinition
            {
                Id = "shorttext_hydroelectric_pressure_memory",
                Text = "Дольше всего в машинной памяти держались {object} и {condition}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["object", "condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["object"] = "object_direct",
                    ["condition"] = "condition_scene_detail"
                },
                CompositionRole = "observation",
                Cadence = "object_pressure",
                Tags = ["story", "quiet", "procedural", "hydroelectric"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.78
            },
            new TemplateDefinition
            {
                Id = "shorttext_observatory_distant_note",
                Text = "В {place} потом долго существовало только одно: {condition}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["place", "condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in",
                    ["condition"] = "condition_reveal_state"
                },
                CompositionRole = "observation",
                Cadence = "delayed_implication",
                Tags = ["story", "quiet", "procedural", "observatory"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.78
            },
            new TemplateDefinition
            {
                Id = "idea_character_action_object_place_twist",
                Text = "Идея: {character} получает странное поручение: {action} {object} в {place}, и при этом {twist}.",
                Mode = GenerationMode.Idea,
                RequiredCategories = ["character", "action", "object", "place", "twist"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["character"] = "character_subject",
                    ["action"] = "action_infinitive",
                    ["object"] = "object_direct",
                    ["place"] = "place_in",
                    ["twist"] = "twist_character_clause"
                },
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
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in",
                    ["concept"] = "concept_story_frame",
                    ["genre"] = "genre_label"
                },
                Tags = ["story", "concept", "city"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 1.0
            },
            new TemplateDefinition
            {
                Id = "idea_place_clause_concept_genre",
                Text = "Идея: история о том, как в {place} возникает {concept}. По тону это {genre}.",
                Mode = GenerationMode.Idea,
                RequiredCategories = ["place", "concept", "genre"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in_clause",
                    ["concept"] = "concept_story_frame",
                    ["genre"] = "genre_label"
                },
                Tags = ["story", "concept", "city", "surreal"],
                MinAbsurdity = 1,
                MaxAbsurdity = 3,
                Weight = 0.8
            },
            new TemplateDefinition
            {
                Id = "idea_genre_concept_style",
                Text = "Идея: {genre} на тему: {concept}, поданная {style}.",
                Mode = GenerationMode.Idea,
                RequiredCategories = ["genre", "concept", "style"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["genre"] = "genre_label",
                    ["concept"] = "concept_story_frame",
                    ["style"] = "style_note"
                },
                Tags = ["concept", "tone"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.9
            },
            new TemplateDefinition
            {
                Id = "idea_character_place_condition_genre",
                Text = "Идея: в {place} {condition}, а в центре истории {character}. По жанру это {genre}.",
                Mode = GenerationMode.Idea,
                RequiredCategories = ["place", "condition", "character", "genre"],
                Tags = ["story", "scene", "concept"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in",
                    ["condition"] = "condition_scene_detail",
                    ["character"] = "character_subject",
                    ["genre"] = "genre_label"
                },
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.9
            },
            new TemplateDefinition
            {
                Id = "idea_genre_character_action_object_condition",
                Text = "Идея: {genre} о том, как {character} пытается {action} {object}, пока {condition}.",
                Mode = GenerationMode.Idea,
                RequiredCategories = ["genre", "character", "action", "object", "condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["genre"] = "genre_label",
                    ["character"] = "character_subject",
                    ["action"] = "action_infinitive",
                    ["object"] = "object_direct",
                    ["condition"] = "condition_initial_state"
                },
                Tags = ["story", "genre", "scene"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.9
            },
            new TemplateDefinition
            {
                Id = "idea_character_place_twist_style",
                Text = "Идея: {character} в {place}, где {twist}. Подать это {style}.",
                Mode = GenerationMode.Idea,
                RequiredCategories = ["character", "place", "twist", "style"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["character"] = "character_subject",
                    ["place"] = "place_in",
                    ["twist"] = "twist_character_clause",
                    ["style"] = "style_note"
                },
                Tags = ["story", "tone", "surreal"],
                MinAbsurdity = 1,
                MaxAbsurdity = 3,
                Weight = 0.8
            },
            new TemplateDefinition
            {
                Id = "idea_condition_twist_genre",
                Text = "Идея: сначала {condition}, потом {twist}. По жанру это {genre}.",
                Mode = GenerationMode.Idea,
                RequiredCategories = ["condition", "twist", "genre"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["condition"] = "condition_initial_state",
                    ["twist"] = "twist_general_clause",
                    ["genre"] = "genre_label"
                },
                Tags = ["story", "scene", "tone"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.8
            },
            new TemplateDefinition
            {
                Id = "idea_concept_place_style",
                Text = "Идея: {concept}, но развернутая в {place} и поданная {style}.",
                Mode = GenerationMode.Idea,
                RequiredCategories = ["concept", "place", "style"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["concept"] = "concept_story_frame",
                    ["place"] = "place_in",
                    ["style"] = "style_note"
                },
                Tags = ["concept", "tone", "city"],
                MinAbsurdity = 0,
                MaxAbsurdity = 3,
                Weight = 0.8
            }
        ];
    }
}
