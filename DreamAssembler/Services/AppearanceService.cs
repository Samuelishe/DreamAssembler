using System.Windows;
using System.Windows.Media;
using DreamAssembler.App.Models;

namespace DreamAssembler.App.Services;

/// <summary>
/// Применяет тему и встроенные шрифты к ресурсам приложения.
/// </summary>
public sealed class AppearanceService
{
    /// <summary>
    /// Применяет тему оформления и шрифт чтения к текущему приложению.
    /// </summary>
    /// <param name="theme">Выбранная тема оформления.</param>
    /// <param name="readingFont">Выбранный шрифт чтения.</param>
    public void Apply(AppTheme theme, ReadingFontOption readingFont)
    {
        var resources = Application.Current.Resources;
        var palette = CreatePalette(theme);

        resources["WindowBackgroundBrush"] = CreateBrush(palette.WindowBackground);
        resources["PanelBackgroundBrush"] = CreateBrush(palette.PanelBackground);
        resources["PanelAltBackgroundBrush"] = CreateBrush(palette.PanelAltBackground);
        resources["PanelBorderBrush"] = CreateBrush(palette.PanelBorder);
        resources["PrimaryTextBrush"] = CreateBrush(palette.PrimaryText);
        resources["SecondaryTextBrush"] = CreateBrush(palette.SecondaryText);
        resources["MutedTextBrush"] = CreateBrush(palette.MutedText);
        resources["AccentBrush"] = CreateBrush(palette.Accent);
        resources["AccentSoftBrush"] = CreateBrush(palette.AccentSoft);
        resources["ResultCardBackgroundBrush"] = CreateBrush(palette.ResultCardBackground);
        resources["TitleGradientBrush"] = CreateGradientBrush(palette.TitleGradientStart, palette.TitleGradientMiddle, palette.TitleGradientEnd);
        resources["ReadingFontFamily"] = GetReadingFontFamily(readingFont);
    }

    private static ThemePalette CreatePalette(AppTheme theme)
    {
        return theme switch
        {
            AppTheme.GraphiteDark => new ThemePalette
            {
                WindowBackground = Color.FromRgb(0x19, 0x17, 0x1B),
                PanelBackground = Color.FromRgb(0x22, 0x20, 0x25),
                PanelAltBackground = Color.FromRgb(0x2A, 0x27, 0x2E),
                PanelBorder = Color.FromRgb(0x3F, 0x39, 0x46),
                PrimaryText = Color.FromRgb(0xF2, 0xEE, 0xE8),
                SecondaryText = Color.FromRgb(0xC8, 0xBF, 0xB5),
                MutedText = Color.FromRgb(0x9E, 0x94, 0x89),
                Accent = Color.FromRgb(0xD9, 0xAE, 0x7B),
                AccentSoft = Color.FromRgb(0x36, 0x32, 0x39),
                ResultCardBackground = Color.FromRgb(0x27, 0x24, 0x2A),
                TitleGradientStart = Color.FromRgb(0xF4, 0xD4, 0xAF),
                TitleGradientMiddle = Color.FromRgb(0xD8, 0x98, 0x79),
                TitleGradientEnd = Color.FromRgb(0x9F, 0x6D, 0x64)
            },
            _ => new ThemePalette
            {
                WindowBackground = Color.FromRgb(0xF4, 0xF0, 0xE8),
                PanelBackground = Color.FromRgb(0xFB, 0xF9, 0xF5),
                PanelAltBackground = Color.FromRgb(0xF5, 0xEC, 0xDD),
                PanelBorder = Color.FromRgb(0xE1, 0xD7, 0xC9),
                PrimaryText = Color.FromRgb(0x23, 0x1F, 0x1A),
                SecondaryText = Color.FromRgb(0x63, 0x5B, 0x51),
                MutedText = Color.FromRgb(0x87, 0x7D, 0x71),
                Accent = Color.FromRgb(0x3A, 0x31, 0x28),
                AccentSoft = Color.FromRgb(0xEA, 0xDB, 0xC8),
                ResultCardBackground = Color.FromRgb(0xF9, 0xF6, 0xF0),
                TitleGradientStart = Color.FromRgb(0xF7, 0xE6, 0xD0),
                TitleGradientMiddle = Color.FromRgb(0xF3, 0xD2, 0xB1),
                TitleGradientEnd = Color.FromRgb(0xE8, 0xC2, 0xA8)
            }
        };
    }

    private static SolidColorBrush CreateBrush(Color color)
    {
        var brush = new SolidColorBrush(color);
        brush.Freeze();
        return brush;
    }

    private static LinearGradientBrush CreateGradientBrush(Color start, Color middle, Color end)
    {
        var brush = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 1)
        };

        brush.GradientStops.Add(new GradientStop(start, 0));
        brush.GradientStops.Add(new GradientStop(middle, 0.55));
        brush.GradientStops.Add(new GradientStop(end, 1));
        brush.Freeze();
        return brush;
    }

    private static FontFamily GetReadingFontFamily(ReadingFontOption readingFont)
    {
        return readingFont switch
        {
            ReadingFontOption.Inter => new FontFamily("pack://application:,,,/DreamAssembler.App;component/Assets/Fonts/#Inter"),
            _ => new FontFamily("pack://application:,,,/DreamAssembler.App;component/Assets/Fonts/#Literata")
        };
    }

    private sealed class ThemePalette
    {
        public Color WindowBackground { get; init; }

        public Color PanelBackground { get; init; }

        public Color PanelAltBackground { get; init; }

        public Color PanelBorder { get; init; }

        public Color PrimaryText { get; init; }

        public Color SecondaryText { get; init; }

        public Color MutedText { get; init; }

        public Color Accent { get; init; }

        public Color AccentSoft { get; init; }

        public Color ResultCardBackground { get; init; }

        public Color TitleGradientStart { get; init; }

        public Color TitleGradientMiddle { get; init; }

        public Color TitleGradientEnd { get; init; }
    }
}
