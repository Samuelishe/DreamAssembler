using System.Windows;
using DreamAssembler.App.Models;
using DreamAssembler.App.Services;

namespace DreamAssembler.App;

/// <summary>
/// Представляет точку входа WPF-приложения.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Вызывается при старте приложения и применяет базовое оформление до загрузки окна.
    /// </summary>
    /// <param name="e">Аргументы запуска.</param>
    protected override void OnStartup(StartupEventArgs e)
    {
        var settingsService = new UserSettingsService();
        var settings = settingsService.Load().Settings;
        var appearanceService = new AppearanceService();

        appearanceService.Apply(settings.Theme, settings.ReadingFont);

        base.OnStartup(e);
    }
}
