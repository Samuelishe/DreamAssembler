using DreamAssembler.Core.Enums;
using DreamAssembler.Core.Models;
using DreamAssembler.Core.Services;

namespace DreamAssembler.Core.Tests.Services;

/// <summary>
/// Содержит тесты для сервиса генерации текста.
/// </summary>
public sealed class TextGeneratorServiceTests
{
    /// <summary>
    /// Проверяет генерацию указанного количества результатов.
    /// </summary>
    [Fact]
    public void Generate_ReturnsRequestedNumberOfSentenceResults()
    {
        var service = CreateService();

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.Sentence,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 3
        });

        Assert.Equal(3, result.Count);
        Assert.All(result, item => Assert.False(string.IsNullOrWhiteSpace(item.Text)));
    }

    /// <summary>
    /// Проверяет генерацию короткого текста из нескольких предложений.
    /// </summary>
    [Fact]
    public void Generate_ReturnsShortText_WithSeveralSentences()
    {
        var service = CreateService();

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.ShortText,
            AbsurdityLevel = AbsurdityLevel.Absurd,
            ResultCount = 1
        });

        var text = Assert.Single(result).Text;
        Assert.Contains('.', text);
        Assert.True(text.Split('.', StringSplitOptions.RemoveEmptyEntries).Length >= 2);
    }

    /// <summary>
    /// Проверяет генерацию словосочетания ровно из двух слов.
    /// </summary>
    [Fact]
    public void Generate_ReturnsWordPair_WithTwoWords()
    {
        var service = CreateService();

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.WordPair,
            AbsurdityLevel = AbsurdityLevel.Insane,
            ResultCount = 1
        });

        var text = Assert.Single(result).Text;
        var wordCount = text.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length;

        Assert.Equal(2, wordCount);
    }

    /// <summary>
    /// Проверяет генерацию короткой фразы длиной от трех до четырех слов.
    /// </summary>
    [Fact]
    public void Generate_ReturnsWordCluster_WithThreeToFourWords()
    {
        var service = CreateService();

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.WordCluster,
            AbsurdityLevel = AbsurdityLevel.Insane,
            ResultCount = 1
        });

        var text = Assert.Single(result).Text;
        var wordCount = text.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length;

        Assert.InRange(wordCount, 3, 4);
    }

    /// <summary>
    /// Проверяет, что режим нескольких слов стабильно не выходит за пределы 3-4 слов при серии запусков.
    /// </summary>
    [Fact]
    public void Generate_ReturnsWordCluster_WithinExpectedRange_AcrossMultipleRuns()
    {
        var service = CreateService();

        for (var index = 0; index < 40; index++)
        {
            var result = service.Generate(new TextGenerationOptions
            {
                Mode = GenerationMode.WordCluster,
                AbsurdityLevel = AbsurdityLevel.Insane,
                ResultCount = 1
            });

            var text = Assert.Single(result).Text;
            var wordCount = text.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length;
            Assert.InRange(wordCount, 3, 4);
        }
    }

    /// <summary>
    /// Проверяет, что lexical batch получает общий atmospheric-cluster key.
    /// </summary>
    [Fact]
    public void Generate_AssignsSameAtmosphereKey_AcrossLexicalBatch()
    {
        var associationFragments = new List<AssociationFragmentEntry>
        {
            new() { Id = "noun_m_archive", Text = "архив", Kind = "noun_m", Tags = ["cluster:archive"], Weight = 1.0 },
            new() { Id = "noun_m_card", Text = "конверт", Kind = "noun_m", Tags = ["cluster:archive"], Weight = 1.0 },
            new() { Id = "adjective_m_archive", Text = "архивный", Kind = "adjective_m", Tags = ["cluster:archive"], Weight = 1.0 },
            new() { Id = "adjective_m_paper", Text = "бумажный", Kind = "adjective_m", Tags = ["cluster:archive"], Weight = 1.0 },
            new() { Id = "verb_past_m_waited", Text = "молчал", Kind = "verb_past_m", Tags = ["cluster:archive"], Weight = 1.0 }
        };

        var service = new TextGeneratorService(
            FallbackDataProvider.GetDictionaryEntries(),
            FallbackDataProvider.GetTemplates(),
            associationFragments,
            new WeightedRandomSelector(new Random(1)),
            new TemplateEngine(),
            new Random(1));

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.WordPair,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 3
        });

        Assert.Equal(3, result.Count);
        Assert.All(result, item => Assert.Equal("archive", item.AtmosphereKey));
    }

    /// <summary>
    /// Проверяет, что обычная batch-генерация получает мягкое atmospheric-притяжение к уже возникшему полю.
    /// </summary>
    [Fact]
    public void Generate_PrefersRecurringAtmosphericField_AcrossSentenceBatch()
    {
        var template = new TemplateDefinition
        {
            Id = "continuity_sentence_object",
            Text = "{object}.",
            Mode = GenerationMode.Sentence,
            RequiredCategories = ["object"],
            SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["object"] = "object_direct"
            },
            Weight = 1.0
        };

        var entries = new List<DictionaryEntry>
        {
            new() { Id = "archive_key", Category = "object", Slot = "object_direct", Text = "ключ", Tags = ["archive"], Weight = 1.0 },
            new() { Id = "archive_token", Category = "object", Slot = "object_direct", Text = "жетон", Tags = ["archive"], Weight = 1.0 },
            new() { Id = "archive_card", Category = "object", Slot = "object_direct", Text = "карточку", Tags = ["archive"], Weight = 1.0 },
            new() { Id = "fluorescent_sign", Category = "object", Slot = "object_direct", Text = "вывеску", Tags = ["fluorescent"], Weight = 0.9 },
            new() { Id = "fluorescent_glass", Category = "object", Slot = "object_direct", Text = "стекло", Tags = ["fluorescent"], Weight = 0.9 }
        };

        var service = new TextGeneratorService(
            entries,
            [template],
            [],
            new WeightedRandomSelector(new Random(1)),
            new TemplateEngine(),
            new Random(1));

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.Sentence,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 3
        });

        Assert.Equal(3, result.Count);
        Assert.All(result, item => Assert.Equal("archive", item.AtmosphereKey));
    }

    /// <summary>
    /// Проверяет, что batch старается не повторять один и тот же cadence подряд, если есть альтернатива.
    /// </summary>
    [Fact]
    public void Generate_PrefersDifferentCadence_WhenAlternativeExists()
    {
        var templates = new List<TemplateDefinition>
        {
            new()
            {
                Id = "sentence_announcement",
                Text = "Объявление: {condition}.",
                Mode = GenerationMode.Sentence,
                RequiredCategories = ["condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["condition"] = "condition_clause"
                },
                Cadence = "announcement",
                Weight = 10.0
            },
            new()
            {
                Id = "sentence_inventory",
                Text = "По списку: {condition}.",
                Mode = GenerationMode.Sentence,
                RequiredCategories = ["condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["condition"] = "condition_clause"
                },
                Cadence = "inventory",
                Weight = 1.0
            }
        };

        var entries = new List<DictionaryEntry>
        {
            new() { Id = "condition_ok", Category = "condition", Slot = "condition_clause", Text = "лампы продолжали гудеть", Weight = 1.0 }
        };

        var service = new TextGeneratorService(
            entries,
            templates,
            [],
            new WeightedRandomSelector(new Random(1)),
            new TemplateEngine(),
            new Random(1));

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.Sentence,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 2
        });

        Assert.Equal(2, result.Count);
        Assert.StartsWith("Объявление:", result[0].Text, StringComparison.Ordinal);
        Assert.StartsWith("По списку:", result[1].Text, StringComparison.Ordinal);
    }

    /// <summary>
    /// Проверяет, что после появления dominant manifold batch тянется к нему сильнее, чем к другому strong manifold.
    /// </summary>
    [Fact]
    public void Generate_PrefersDominantManifold_AcrossSentenceBatch()
    {
        var template = new TemplateDefinition
        {
            Id = "manifold_bias_sentence_object",
            Text = "{object}.",
            Mode = GenerationMode.Sentence,
            RequiredCategories = ["object"],
            SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["object"] = "object_direct"
            },
            Weight = 1.0
        };

        var entries = new List<DictionaryEntry>
        {
            new() { Id = "museum_card", Category = "object", Slot = "object_direct", Text = "каталожную карточку", Tags = ["museum", "paper"], Weight = 1.0 },
            new() { Id = "museum_cover", Category = "object", Slot = "object_direct", Text = "ночной чехол экспоната", Tags = ["museum", "quiet"], Weight = 1.0 },
            new() { Id = "museum_key", Category = "object", Slot = "object_direct", Text = "ключ от зала без посетителей", Tags = ["museum", "memory"], Weight = 1.0 },
            new() { Id = "airport_tag", Category = "object", Slot = "object_direct", Text = "багажную бирку", Tags = ["airport", "transit"], Weight = 1.0 },
            new() { Id = "airport_ticket", Category = "object", Slot = "object_direct", Text = "посадочный талон", Tags = ["airport", "night"], Weight = 1.0 }
        };

        var service = new TextGeneratorService(
            entries,
            [template],
            [],
            new WeightedRandomSelector(new Random(1)),
            new TemplateEngine(),
            new Random(1));

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.Sentence,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 3
        });

        Assert.Equal(3, result.Count);
        Assert.All(result, item => Assert.Equal("museum", item.AtmosphereKey));
    }

    /// <summary>
    /// Проверяет, что генератор использует slot-требования шаблона.
    /// </summary>
    [Fact]
    public void Generate_UsesSlotRequirements_WhenTemplateDefinesThem()
    {
        var template = new TemplateDefinition
        {
            Id = "slot_test",
            Text = "{condition}.",
            Mode = GenerationMode.Sentence,
            RequiredCategories = ["condition"],
            SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["condition"] = "condition_clause"
            },
            Weight = 1.0
        };

        var entries = new List<DictionaryEntry>
        {
            new() { Id = "condition_ok", Category = "condition", Slot = "condition_clause", Text = "все молчали", Weight = 1.0 },
            new() { Id = "condition_wrong", Category = "condition", Slot = "emotion_predicative", Text = "тихо тревожно", Weight = 100.0 }
        };

        var service = new TextGeneratorService(
            entries,
            [template],
            [],
            new WeightedRandomSelector(new Random(1)),
            new TemplateEngine());

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.Sentence,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 1
        });

        Assert.Equal("Все молчали.", Assert.Single(result).Text);
    }

    /// <summary>
    /// Проверяет, что шаблон с персонажем берет twist только из персонального slot.
    /// </summary>
    [Fact]
    public void Generate_UsesCharacterTwistSlot_WhenTemplateRequiresCharacterContext()
    {
        var template = new TemplateDefinition
        {
            Id = "twist_character_slot_test",
            Text = "{character} понял, что {twist}.",
            Mode = GenerationMode.Sentence,
            RequiredCategories = ["character", "twist"],
            SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["character"] = "character_subject",
                ["twist"] = "twist_character_clause"
            },
            Weight = 1.0
        };

        var entries = new List<DictionaryEntry>
        {
            new() { Id = "character_ok", Category = "character", Slot = "character_subject", Text = "кассир", Weight = 1.0 },
            new() { Id = "twist_character_ok", Category = "twist", Slot = "twist_character_clause", Text = "очередь начала ему аплодировать", Weight = 1.0 },
            new() { Id = "twist_general_wrong", Category = "twist", Slot = "twist_general_clause", Text = "по вторникам запрещено говорить правду", Weight = 100.0 }
        };

        var service = new TextGeneratorService(
            entries,
            [template],
            [],
            new WeightedRandomSelector(new Random(1)),
            new TemplateEngine(),
            new Random(1));

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.Sentence,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 1
        });

        Assert.Equal("Кассир понял, что очередь начала ему аплодировать.", Assert.Single(result).Text);
    }

    /// <summary>
    /// Проверяет, что рефлексивный short-text шаблон берет concept только из reflection-slot.
    /// </summary>
    [Fact]
    public void Generate_UsesReflectionConceptSlot_WhenTemplateRequiresIt()
    {
        var template = new TemplateDefinition
        {
            Id = "concept_reflection_slot_test",
            Text = "Это было {concept}.",
            Mode = GenerationMode.ShortText,
            RequiredCategories = ["concept"],
            SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["concept"] = "concept_reflection"
            },
            CompositionRole = "reflection",
            Weight = 1.0
        };

        var entries = new List<DictionaryEntry>
        {
            new() { Id = "concept_reflection_ok", Category = "concept", Slot = "concept_reflection", Text = "маленькая городская тайна", Weight = 1.0 },
            new() { Id = "concept_story_wrong", Category = "concept", Slot = "concept_story_frame", Text = "тихий бунт предметов", Weight = 100.0 }
        };

        var service = new TextGeneratorService(
            entries,
            [template],
            [],
            new WeightedRandomSelector(new Random(1)),
            new TemplateEngine(),
            new Random(1));

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.ShortText,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 1
        });

        Assert.Contains("маленькая городская тайна", Assert.Single(result).Text, StringComparison.Ordinal);
    }

    /// <summary>
    /// Проверяет, что шаблон ранней сцены берет condition только из initial-state slot.
    /// </summary>
    [Fact]
    public void Generate_UsesInitialConditionSlot_WhenTemplateRequiresOpeningState()
    {
        var template = new TemplateDefinition
        {
            Id = "condition_initial_slot_test",
            Text = "С самого начала было ясно одно: {condition}.",
            Mode = GenerationMode.ShortText,
            RequiredCategories = ["condition"],
            SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["condition"] = "condition_initial_state"
            },
            CompositionRole = "scene",
            Weight = 1.0
        };

        var entries = new List<DictionaryEntry>
        {
            new() { Id = "condition_initial_ok", Category = "condition", Slot = "condition_initial_state", Text = "никто никуда не спешил", Weight = 1.0 },
            new() { Id = "condition_reveal_wrong", Category = "condition", Slot = "condition_reveal_state", Text = "стулья вели молчаливое совещание", Weight = 100.0 }
        };

        var service = new TextGeneratorService(
            entries,
            [template],
            [],
            new WeightedRandomSelector(new Random(1)),
            new TemplateEngine(),
            new Random(1));

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.ShortText,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 1
        });

        Assert.Contains("никто никуда не спешил", Assert.Single(result).Text, StringComparison.Ordinal);
    }

    /// <summary>
    /// Проверяет, что при прочих равных генератор предпочитает запись из того же strong-manifold field.
    /// </summary>
    [Fact]
    public void Generate_PrefersSameStrongManifoldField_WhenCandidatesAreOtherwiseEqual()
    {
        var template = new TemplateDefinition
        {
            Id = "manifold_affinity_test",
            Text = "{place}. {character}.",
            Mode = GenerationMode.Sentence,
            RequiredCategories = ["place", "character"],
            Weight = 1.0
        };

        var entries = new List<DictionaryEntry>
        {
            new() { Id = "museum_place", Category = "place", Slot = "place_in", Text = "в закрытом крыле музея", Tags = ["museum"], Weight = 1.0 },
            new() { Id = "museum_character", Category = "character", Slot = "character_subject", Text = "куратор ночной экспозиции", Tags = ["museum"], Weight = 1.0 },
            new() { Id = "mall_character", Category = "character", Slot = "character_subject", Text = "администратор пустого этажа", Tags = ["mall"], Weight = 1.0 }
        };

        var service = new TextGeneratorService(
            entries,
            [template],
            [],
            new WeightedRandomSelector(new Random(1)),
            new TemplateEngine(),
            new Random(1));

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.Sentence,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 1
        });

        Assert.Contains("куратор ночной экспозиции", Assert.Single(result).Text, StringComparison.Ordinal);
    }

    /// <summary>
    /// Проверяет, что scene-anchor slot не уводит фразу в другой strong manifold после уже выбранного места.
    /// </summary>
    [Fact]
    public void Generate_PrefersMatchingAnchorTwist_WhenPlaceAlreadyAnchorsManifold()
    {
        var template = new TemplateDefinition
        {
            Id = "anchor_manifold_consistency_test",
            Text = "{place}. {twist}.",
            Mode = GenerationMode.Sentence,
            RequiredCategories = ["place", "twist"],
            SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["place"] = "place_in",
                ["twist"] = "twist_general_clause"
            },
            Weight = 1.0
        };

        var entries = new List<DictionaryEntry>
        {
            new() { Id = "museum_place", Category = "place", Slot = "place_in", Text = "в закрытом крыле музея", Tags = ["museum"], Weight = 1.0 },
            new() { Id = "museum_twist", Category = "twist", Slot = "twist_general_clause", Text = "этикетки продолжали переписывать друг друга", Tags = ["museum"], Weight = 1.0 },
            new() { Id = "airport_twist_heavy", Category = "twist", Slot = "twist_general_clause", Text = "диктор снова объявил посадку без пассажиров", Tags = ["airport"], Weight = 6.0 }
        };

        var service = new TextGeneratorService(
            entries,
            [template],
            [],
            new WeightedRandomSelector(new Random(1)),
            new TemplateEngine(),
            new Random(1));

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.Sentence,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 1
        });

        var text = Assert.Single(result).Text;
        Assert.Contains("этикетки продолжали переписывать друг друга", text, StringComparison.Ordinal);
        Assert.DoesNotContain("диктор снова объявил посадку без пассажиров", text, StringComparison.Ordinal);
    }

    /// <summary>
    /// Проверяет, что новые non-urban tags учитываются как strong manifolds и становятся atmosphere key серии.
    /// </summary>
    [Fact]
    public void Generate_AssignsObservatoryAsAtmosphereKey_WhenBatchUsesObservatoryEntries()
    {
        var template = new TemplateDefinition
        {
            Id = "observatory_surfacing_test",
            Text = "{place}.",
            Mode = GenerationMode.Sentence,
            RequiredCategories = ["place"],
            SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["place"] = "place_in"
            },
            Weight = 1.0
        };

        var entries = new List<DictionaryEntry>
        {
            new() { Id = "observatory_place_1", Category = "place", Slot = "place_in", Text = "в холодном наблюдательном куполе", Tags = ["observatory"], Weight = 1.0 },
            new() { Id = "observatory_place_2", Category = "place", Slot = "place_in", Text = "в башне погодных измерений", Tags = ["observatory"], Weight = 1.0 },
            new() { Id = "observatory_place_3", Category = "place", Slot = "place_in", Text = "в комнате красных ночных ламп", Tags = ["observatory"], Weight = 1.0 }
        };

        var service = new TextGeneratorService(
            entries,
            [template],
            [],
            new WeightedRandomSelector(new Random(1)),
            new TemplateEngine(),
            new Random(1));

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.Sentence,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 3
        });

        Assert.Equal(3, result.Count);
        Assert.All(result, item => Assert.Equal("observatory", item.AtmosphereKey));
    }

    /// <summary>
    /// Проверяет, что шаблон со сложным местом берет только clause-slot и сохраняет закрывающую запятую.
    /// </summary>
    [Fact]
    public void Generate_UsesClausePlaceSlot_WhenTemplateRequiresComplexPlace()
    {
        var template = new TemplateDefinition
        {
            Id = "place_clause_slot_test",
            Text = "В {place} все началось.",
            Mode = GenerationMode.Sentence,
            RequiredCategories = ["place"],
            SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["place"] = "place_in_clause"
            },
            Weight = 1.0
        };

        var entries = new List<DictionaryEntry>
        {
            new() { Id = "place_clause_ok", Category = "place", Slot = "place_in_clause", Text = "дворе, где старое радио знает все новости заранее,", Weight = 1.0 },
            new() { Id = "place_simple_wrong", Category = "place", Slot = "place_in", Text = "ночном магазине", Weight = 100.0 }
        };

        var service = new TextGeneratorService(
            entries,
            [template],
            [],
            new WeightedRandomSelector(new Random(1)),
            new TemplateEngine(),
            new Random(1));

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.Sentence,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 1
        });

        Assert.Equal("В дворе, где старое радио знает все новости заранее, все началось.", Assert.Single(result).Text);
    }

    /// <summary>
    /// Проверяет, что short-text шаблоны с персонажем не требуют гендерно жестких форм без морфологии.
    /// </summary>
    [Fact]
    public void Generate_ShortText_AllowsNeutralCharacterPhrasing()
    {
        var template = new TemplateDefinition
        {
            Id = "neutral_character_shorttext_test",
            Text = "В {place} все началось с того, что пришлось {action} {object}. Рядом уже можно было заметить {character}.",
            Mode = GenerationMode.ShortText,
            RequiredCategories = ["place", "action", "object", "character"],
            SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["place"] = "place_in",
                ["action"] = "action_infinitive",
                ["object"] = "object_direct",
                ["character"] = "character_subject"
            },
            CompositionRole = "setup",
            Weight = 1.0
        };

        var entries = new List<DictionaryEntry>
        {
            new() { Id = "place_ok", Category = "place", Slot = "place_in", Text = "дежурной комнате", Weight = 1.0 },
            new() { Id = "action_ok", Category = "action", Slot = "action_infinitive", Text = "проверить", Weight = 1.0 },
            new() { Id = "object_ok", Category = "object", Slot = "object_direct", Text = "журнал учета", Weight = 1.0 },
            new() { Id = "character_ok", Category = "character", Slot = "character_subject", Text = "уборщица музея эха", Weight = 1.0 }
        };

        var service = new TextGeneratorService(
            entries,
            [template],
            [],
            new WeightedRandomSelector(new Random(1)),
            new TemplateEngine(),
            new Random(1));

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.ShortText,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 1
        });

        var text = Assert.Single(result).Text;
        Assert.Contains("можно было заметить уборщица музея эха", text, StringComparison.Ordinal);
        Assert.DoesNotContain("был уборщица музея эха", text, StringComparison.Ordinal);
    }

    /// <summary>
    /// Проверяет, что action и object с общими compat-тегами получают приоритет над несовместимой парой.
    /// </summary>
    [Fact]
    public void Generate_PrefersCompatibleActionObjectPair_WhenCompatTagsExist()
    {
        var template = new TemplateDefinition
        {
            Id = "action_object_compat_test",
            Text = "{object} нужно {action}.",
            Mode = GenerationMode.Sentence,
            RequiredCategories = ["object", "action"],
            SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["object"] = "object_direct",
                ["action"] = "action_infinitive"
            },
            Weight = 1.0
        };

        var entries = new List<DictionaryEntry>
        {
            new()
            {
                Id = "object_repairable",
                Category = "object",
                Slot = "object_direct",
                Text = "сломанные часы",
                Tags = ["compat:repairable"],
                Weight = 1.0
            },
            new()
            {
                Id = "action_wrong",
                Category = "action",
                Slot = "action_infinitive",
                Text = "тайно обменять",
                Tags = ["compat:exchangeable"],
                Weight = 3.0
            },
            new()
            {
                Id = "action_right",
                Category = "action",
                Slot = "action_infinitive",
                Text = "чинить по ночам",
                Tags = ["compat:repairable"],
                Weight = 1.0
            }
        };

        var service = new TextGeneratorService(
            entries,
            [template],
            [],
            new WeightedRandomSelector(new Random(1)),
            new TemplateEngine(),
            new Random(1));

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.Sentence,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 1
        });

        Assert.Equal("Сломанные часы нужно чинить по ночам.", Assert.Single(result).Text);
    }

    /// <summary>
    /// Проверяет, что object без нужного compat-ключа резко проигрывает совместимому объекту.
    /// </summary>
    [Fact]
    public void Generate_PrefersOpenableObject_WhenActionRequiresOpenable()
    {
        var template = new TemplateDefinition
        {
            Id = "action_object_openable_test",
            Text = "{action} {object}.",
            Mode = GenerationMode.Sentence,
            RequiredCategories = ["action", "object"],
            SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["action"] = "action_infinitive",
                ["object"] = "object_direct"
            },
            Weight = 1.0
        };

        var entries = new List<DictionaryEntry>
        {
            new()
            {
                Id = "action_open",
                Category = "action",
                Slot = "action_infinitive",
                Text = "открыть мастер-ключом",
                Tags = ["compat:openable"],
                Weight = 1.0
            },
            new()
            {
                Id = "object_wrong",
                Category = "object",
                Slot = "object_direct",
                Text = "табличку с выцветшим номером маршрута",
                Tags = ["compat:hideable", "compat:protectable"],
                Weight = 2.0
            },
            new()
            {
                Id = "object_right",
                Category = "object",
                Slot = "object_direct",
                Text = "запечатанный таможенный конверт",
                Tags = ["compat:openable", "compat:hideable"],
                Weight = 1.0
            }
        };

        var service = new TextGeneratorService(
            entries,
            [template],
            [],
            new WeightedRandomSelector(new Random(1)),
            new TemplateEngine(),
            new Random(1));

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.Sentence,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 1
        });

        Assert.Equal("Открыть мастер-ключом запечатанный таможенный конверт.", Assert.Single(result).Text);
    }

    /// <summary>
    /// Проверяет, что при наличии совместимых object-кандидатов генератор не рассматривает явно несовместимые.
    /// </summary>
    [Fact]
    public void Generate_FiltersToCompatibleObjects_WhenCompatibleCandidatesExist()
    {
        var template = new TemplateDefinition
        {
            Id = "action_object_strong_filter_test",
            Text = "{action} {object}.",
            Mode = GenerationMode.Sentence,
            RequiredCategories = ["action", "object"],
            SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["action"] = "action_infinitive",
                ["object"] = "object_direct"
            },
            Weight = 1.0
        };

        var entries = new List<DictionaryEntry>
        {
            new()
            {
                Id = "action_open",
                Category = "action",
                Slot = "action_infinitive",
                Text = "открыть мастер-ключом",
                Tags = ["compat:openable"],
                Weight = 1.0
            },
            new()
            {
                Id = "object_wrong_heavy",
                Category = "object",
                Slot = "object_direct",
                Text = "табличку с выцветшим номером маршрута",
                Tags = ["compat:hideable", "compat:protectable"],
                Weight = 100.0
            },
            new()
            {
                Id = "object_right",
                Category = "object",
                Slot = "object_direct",
                Text = "запечатанный авиапочтовый пакет",
                Tags = ["compat:openable", "compat:hideable"],
                Weight = 0.1
            }
        };

        var service = new TextGeneratorService(
            entries,
            [template],
            [],
            new WeightedRandomSelector(new Random(1)),
            new TemplateEngine(),
            new Random(1));

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.Sentence,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 1
        });

        Assert.Equal("Открыть мастер-ключом запечатанный авиапочтовый пакет.", Assert.Single(result).Text);
    }

    /// <summary>
    /// Проверяет, что короткий текст не начинается с meta-шаблона и не повторяет его несколько раз при наличии альтернатив.
    /// </summary>
    [Fact]
    public void Generate_ShortText_LimitsMetaTemplates_WhenAlternativesExist()
    {
        var templates = new List<TemplateDefinition>
        {
            new()
            {
                Id = "setup",
                Text = "Начало: {condition}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["condition"] = "condition_clause"
                },
                CompositionRole = "setup",
                Weight = 1.0
            },
            new()
            {
                Id = "meta",
                Text = "Если пересказывать это {style}, все звучит почти убедительно.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["style"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["style"] = "style_note"
                },
                CompositionRole = "meta",
                Weight = 10.0
            },
            new()
            {
                Id = "development",
                Text = "Потом оказалось, что {condition}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["condition"] = "condition_clause"
                },
                CompositionRole = "development",
                Weight = 1.0
            }
        };

        var entries = new List<DictionaryEntry>
        {
            new() { Id = "condition_ok", Category = "condition", Slot = "condition_clause", Text = "все молчали", Weight = 1.0 },
            new() { Id = "style_ok", Category = "style", Slot = "style_note", Text = "с серьезной интонацией", Weight = 1.0 }
        };

        var service = new TextGeneratorService(
            entries,
            templates,
            [],
            new WeightedRandomSelector(new Random(1)),
            new TemplateEngine(),
            new Random(0));

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.ShortText,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 1
        });

        var text = Assert.Single(result).Text;
        Assert.False(text.StartsWith("Если пересказывать это", StringComparison.Ordinal));
        Assert.True(CountOccurrences(text, "Если пересказывать это") <= 1);
    }

    /// <summary>
    /// Проверяет, что короткий текст не выбирает late-роль второй фразой, если есть обычное развитие.
    /// </summary>
    [Fact]
    public void Generate_ShortText_DelaysLateRoles_WhenDevelopmentExists()
    {
        var templates = new List<TemplateDefinition>
        {
            new()
            {
                Id = "scene",
                Text = "С самого начала было ясно одно: {condition}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["condition"] = "condition_initial_state"
                },
                CompositionRole = "scene",
                Weight = 1.0
            },
            new()
            {
                Id = "reflection",
                Text = "Из-за этого всем стало {emotion}, и всем показалось, что это {concept}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["emotion", "concept"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["emotion"] = "emotion_group_state",
                    ["concept"] = "concept_reflection"
                },
                CompositionRole = "reflection",
                Weight = 10.0
            },
            new()
            {
                Id = "development",
                Text = "Позже выяснилось, что в {place} {condition}.",
                Mode = GenerationMode.ShortText,
                RequiredCategories = ["place", "condition"],
                SlotRequirements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["place"] = "place_in",
                    ["condition"] = "condition_reveal_state"
                },
                CompositionRole = "development",
                Weight = 1.0
            }
        };

        var entries = new List<DictionaryEntry>
        {
            new() { Id = "condition_initial_ok", Category = "condition", Slot = "condition_initial_state", Text = "никто никуда не спешил", Weight = 1.0 },
            new() { Id = "condition_reveal_ok", Category = "condition", Slot = "condition_reveal_state", Text = "стулья вели молчаливое совещание", Weight = 1.0 },
            new() { Id = "place_ok", Category = "place", Slot = "place_in", Text = "в подвале библиотеки", Weight = 1.0 },
            new() { Id = "emotion_ok", Category = "emotion", Slot = "emotion_group_state", Text = "не по себе", Weight = 1.0 },
            new() { Id = "concept_ok", Category = "concept", Slot = "concept_reflection", Text = "маленькая городская тайна", Weight = 1.0 }
        };

        var service = new TextGeneratorService(
            entries,
            templates,
            [],
            new WeightedRandomSelector(new Random(1)),
            new TemplateEngine(),
            new Random(3));

        var result = service.Generate(new TextGenerationOptions
        {
            Mode = GenerationMode.ShortText,
            AbsurdityLevel = AbsurdityLevel.Normal,
            ResultCount = 1
        });

        var sentences = Assert.Single(result).Text
            .Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        Assert.True(sentences.Length >= 3);
        Assert.StartsWith("С самого начала было ясно одно:", sentences[0], StringComparison.Ordinal);
        Assert.StartsWith("Позже выяснилось", sentences[1], StringComparison.Ordinal);
    }

    private static TextGeneratorService CreateService()
    {
        var random = new Random(12345);

        return new TextGeneratorService(
            FallbackDataProvider.GetDictionaryEntries(),
            FallbackDataProvider.GetTemplates(),
            FallbackDataProvider.GetAssociationFragments(),
            new WeightedRandomSelector(random),
            new TemplateEngine(),
            random);
    }

    private static int CountOccurrences(string text, string value)
    {
        var count = 0;
        var index = 0;

        while ((index = text.IndexOf(value, index, StringComparison.Ordinal)) >= 0)
        {
            count++;
            index += value.Length;
        }

        return count;
    }
}
