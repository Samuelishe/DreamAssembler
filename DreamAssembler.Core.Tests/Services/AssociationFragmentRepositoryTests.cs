using DreamAssembler.Core.Services;

namespace DreamAssembler.Core.Tests.Services;

/// <summary>
/// Содержит тесты для загрузки словарей словесных режимов.
/// </summary>
public sealed class AssociationFragmentRepositoryTests
{
    /// <summary>
    /// Проверяет использование fallback-данных при отсутствии папки словарей словесных режимов.
    /// </summary>
    [Fact]
    public void Load_UsesFallback_WhenDirectoryDoesNotExist()
    {
        var repository = new AssociationFragmentRepository();

        var result = repository.Load(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));

        Assert.True(result.UsedFallback);
        Assert.NotEmpty(result.Data);
        Assert.Contains("fallback", result.Message, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Проверяет загрузку валидных существительных и прилагательных из CSV-файлов.
    /// </summary>
    [Fact]
    public void Load_ReturnsEntries_WhenCsvIsValid()
    {
        var repository = new AssociationFragmentRepository();
        var directoryPath = CreateTempDirectory();

        try
        {
            File.WriteAllText(
                Path.Combine(directoryPath, "sample-nouns.csv"),
                """
                bare	accented	translations_en	translations_de	gender	partner	animate	indeclinable	sg_only	pl_only	sg_nom	sg_gen	sg_dat	sg_acc	sg_inst	sg_prep	pl_nom	pl_gen	pl_dat	pl_acc	pl_inst	pl_prep
                архив	архи'в	archive	Archiv	m		0	0	0	0	архи'в	архи'ва	архи'ву	архив	архи'вом	архиве'	архи'вы	архивов	архивам	архивы	архивами	архивах
                """);

            File.WriteAllText(
                Path.Combine(directoryPath, "sample-adjectives.csv"),
                """
                bare	accented	translations_en	translations_de	comparative	superlative	short_m	short_f	short_n	short_pl	decl_m_nom	decl_m_gen	decl_m_dat	decl_m_acc	decl_m_inst	decl_m_prep	decl_f_nom	decl_f_gen	decl_f_dat	decl_f_acc	decl_f_inst	decl_f_prep	decl_n_nom	decl_n_gen	decl_n_dat	decl_n_acc	decl_n_inst	decl_n_prep	decl_pl_nom	decl_pl_gen	decl_pl_dat	decl_pl_acc	decl_pl_inst	decl_pl_prep
                бумажный	бума'жный	paper	papieren							бума'жный	бума'жного	бума'жному	бума'жный	бума'жным	бума'жном	бума'жная	бума'жной	бума'жной	бума'жную	бума'жной	бума'жной	бума'жное	бума'жного	бума'жному	бума'жное	бума'жным	бума'жном	бума'жные	бума'жных	бума'жным	бума'жные	бума'жными	бума'жных
                """);

            var result = repository.Load(directoryPath);

            Assert.False(result.UsedFallback);
            Assert.Contains(result.Data, entry => entry.Kind == "noun_m" && entry.Text == "архив");
            Assert.Contains(result.Data, entry => entry.Kind == "adjective_f" && entry.Text == "бумажная");
        }
        finally
        {
            Directory.Delete(directoryPath, true);
        }
    }

    /// <summary>
    /// Проверяет рекурсивную загрузку CSV-источников из вложенных папок.
    /// </summary>
    [Fact]
    public void Load_ReadsNestedCsvFiles_WhenDirectoryTreeIsUsed()
    {
        var repository = new AssociationFragmentRepository();
        var directoryPath = CreateTempDirectory();
        var nestedPath = Path.Combine(directoryPath, "sources");
        Directory.CreateDirectory(nestedPath);

        try
        {
            File.WriteAllText(
                Path.Combine(nestedPath, "nested-nouns.csv"),
                """
                bare	accented	translations_en	translations_de	gender	partner	animate	indeclinable	sg_only	pl_only	sg_nom	sg_gen	sg_dat	sg_acc	sg_inst	sg_prep	pl_nom	pl_gen	pl_dat	pl_acc	pl_inst	pl_prep
                память	па'мять	memory	Erinnerung	f		0	0	0	0	па'мять	па'мяти	па'мяти	па'мять	па'мятью	па'мяти	па'мяти	памятей	памятям	па'мяти	памятями	памятях
                """);

            var result = repository.Load(directoryPath);

            Assert.False(result.UsedFallback);
            Assert.Contains(result.Data, entry => entry.Kind == "noun_f" && entry.Text == "память");
        }
        finally
        {
            Directory.Delete(directoryPath, true);
        }
    }

    private static string CreateTempDirectory()
    {
        var directoryPath = Path.Combine(Path.GetTempPath(), $"dreamassembler-association-tests-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directoryPath);
        return directoryPath;
    }
}
