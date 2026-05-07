using System.Windows;
using System.Windows.Media;
using DreamAssembler.App.Models;
using System.IO;

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

        resources["UiFontFamily"] = GetUiFontFamily();
        resources["DisplayFontFamily"] = GetDisplayFontFamily();
        resources["WindowBackgroundBrush"] = CreateWindowBackgroundBrush(palette.WindowBackgroundStart, palette.WindowBackgroundEnd);
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

    private static FontFamily GetUiFontFamily()
    {
        return GetBundledFontFamily("Inter");
    }

    private static FontFamily GetDisplayFontFamily()
    {
        return GetBundledFontFamily("Unbounded");
    }

    private static ThemePalette CreatePalette(AppTheme theme)
    {
        return theme switch
        {
            AppTheme.MistLight => new ThemePalette
            {
                WindowBackgroundStart = Color.FromRgb(0xEF, 0xF4, 0xF2),
                WindowBackgroundEnd = Color.FromRgb(0xE7, 0xEC, 0xEE),
                PanelBackground = Color.FromRgb(0xFA, 0xFC, 0xFB),
                PanelAltBackground = Color.FromRgb(0xE8, 0xF0, 0xEC),
                PanelBorder = Color.FromRgb(0xCF, 0xDA, 0xD7),
                PrimaryText = Color.FromRgb(0x1F, 0x26, 0x26),
                SecondaryText = Color.FromRgb(0x56, 0x63, 0x63),
                MutedText = Color.FromRgb(0x7A, 0x88, 0x88),
                Accent = Color.FromRgb(0x25, 0x54, 0x57),
                AccentSoft = Color.FromRgb(0xD9, 0xE7, 0xE4),
                ResultCardBackground = Color.FromRgb(0xF8, 0xFB, 0xFA),
                TitleGradientStart = Color.FromRgb(0x95, 0xC9, 0xC0),
                TitleGradientMiddle = Color.FromRgb(0x68, 0xA7, 0xA9),
                TitleGradientEnd = Color.FromRgb(0x4D, 0x79, 0x9F)
            },
            AppTheme.RainyDay => new ThemePalette
            {
                WindowBackgroundStart = Color.FromRgb(0xE8, 0xED, 0xF0),
                WindowBackgroundEnd = Color.FromRgb(0xD9, 0xE0, 0xE7),
                PanelBackground = Color.FromRgb(0xF7, 0xFA, 0xFC),
                PanelAltBackground = Color.FromRgb(0xE6, 0xEC, 0xF1),
                PanelBorder = Color.FromRgb(0xC7, 0xD1, 0xD9),
                PrimaryText = Color.FromRgb(0x21, 0x28, 0x30),
                SecondaryText = Color.FromRgb(0x55, 0x63, 0x70),
                MutedText = Color.FromRgb(0x7A, 0x88, 0x94),
                Accent = Color.FromRgb(0x4B, 0x67, 0x7B),
                AccentSoft = Color.FromRgb(0xD9, 0xE4, 0xEC),
                ResultCardBackground = Color.FromRgb(0xF9, 0xFB, 0xFD),
                TitleGradientStart = Color.FromRgb(0xA4, 0xB6, 0xC8),
                TitleGradientMiddle = Color.FromRgb(0x7A, 0x93, 0xA8),
                TitleGradientEnd = Color.FromRgb(0x5A, 0x74, 0x8B)
            },
            AppTheme.GraphiteDark => new ThemePalette
            {
                WindowBackgroundStart = Color.FromRgb(0x19, 0x17, 0x1B),
                WindowBackgroundEnd = Color.FromRgb(0x12, 0x12, 0x16),
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
            AppTheme.PlumNight => new ThemePalette
            {
                WindowBackgroundStart = Color.FromRgb(0x1A, 0x14, 0x1F),
                WindowBackgroundEnd = Color.FromRgb(0x11, 0x10, 0x18),
                PanelBackground = Color.FromRgb(0x24, 0x1F, 0x2A),
                PanelAltBackground = Color.FromRgb(0x2E, 0x26, 0x36),
                PanelBorder = Color.FromRgb(0x49, 0x40, 0x56),
                PrimaryText = Color.FromRgb(0xF4, 0xEE, 0xF7),
                SecondaryText = Color.FromRgb(0xCF, 0xC0, 0xD8),
                MutedText = Color.FromRgb(0xA8, 0x97, 0xB4),
                Accent = Color.FromRgb(0xD7, 0x87, 0xA8),
                AccentSoft = Color.FromRgb(0x38, 0x2F, 0x43),
                ResultCardBackground = Color.FromRgb(0x2B, 0x23, 0x31),
                TitleGradientStart = Color.FromRgb(0xF0, 0xA1, 0xC3),
                TitleGradientMiddle = Color.FromRgb(0xC9, 0x78, 0xB6),
                TitleGradientEnd = Color.FromRgb(0x7A, 0x6D, 0xD3)
            },
            AppTheme.ArchiveDusk => new ThemePalette
            {
                WindowBackgroundStart = Color.FromRgb(0x1D, 0x1D, 0x18),
                WindowBackgroundEnd = Color.FromRgb(0x14, 0x15, 0x11),
                PanelBackground = Color.FromRgb(0x27, 0x27, 0x20),
                PanelAltBackground = Color.FromRgb(0x31, 0x31, 0x28),
                PanelBorder = Color.FromRgb(0x46, 0x45, 0x39),
                PrimaryText = Color.FromRgb(0xF0, 0xEC, 0xDF),
                SecondaryText = Color.FromRgb(0xC9, 0xC1, 0xAA),
                MutedText = Color.FromRgb(0x9E, 0x97, 0x83),
                Accent = Color.FromRgb(0xB6, 0xA3, 0x6A),
                AccentSoft = Color.FromRgb(0x39, 0x39, 0x30),
                ResultCardBackground = Color.FromRgb(0x2B, 0x2B, 0x24),
                TitleGradientStart = Color.FromRgb(0xD9, 0xCB, 0x9E),
                TitleGradientMiddle = Color.FromRgb(0xAB, 0x9A, 0x6E),
                TitleGradientEnd = Color.FromRgb(0x74, 0x6E, 0x4E)
            },
            AppTheme.TramNight => new ThemePalette
            {
                WindowBackgroundStart = Color.FromRgb(0x10, 0x19, 0x1C),
                WindowBackgroundEnd = Color.FromRgb(0x0C, 0x10, 0x14),
                PanelBackground = Color.FromRgb(0x18, 0x22, 0x27),
                PanelAltBackground = Color.FromRgb(0x20, 0x2D, 0x32),
                PanelBorder = Color.FromRgb(0x2F, 0x44, 0x49),
                PrimaryText = Color.FromRgb(0xE9, 0xF2, 0xF2),
                SecondaryText = Color.FromRgb(0xB6, 0xCB, 0xCC),
                MutedText = Color.FromRgb(0x87, 0xA2, 0xA5),
                Accent = Color.FromRgb(0x4D, 0xB2, 0xB0),
                AccentSoft = Color.FromRgb(0x1F, 0x31, 0x35),
                ResultCardBackground = Color.FromRgb(0x1C, 0x27, 0x2C),
                TitleGradientStart = Color.FromRgb(0x7E, 0xE1, 0xDC),
                TitleGradientMiddle = Color.FromRgb(0x43, 0xAF, 0xB2),
                TitleGradientEnd = Color.FromRgb(0x2B, 0x72, 0x8A)
            },
            _ => new ThemePalette
            {
                WindowBackgroundStart = Color.FromRgb(0xF4, 0xF0, 0xE8),
                WindowBackgroundEnd = Color.FromRgb(0xEE, 0xE4, 0xD8),
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

    private static LinearGradientBrush CreateWindowBackgroundBrush(Color start, Color end)
    {
        var brush = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 1)
        };

        brush.GradientStops.Add(new GradientStop(start, 0));
        brush.GradientStops.Add(new GradientStop(end, 1));
        brush.Freeze();
        return brush;
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
            ReadingFontOption.Literata => GetBundledFontFamily("Literata 12pt"),
            ReadingFontOption.Manrope => GetBundledFontFamily("Manrope"),
            ReadingFontOption.Inter => GetBundledFontFamily("Inter"),
            _ => GetBundledFontFamily("Literata 12pt")
        };
    }

    private static FontFamily GetBundledFontFamily(string familyName)
    {
        var fontDirectory = Path.Combine(AppContext.BaseDirectory, "Assets", "Fonts");
        var directoryUri = new Uri($"{fontDirectory.TrimEnd(Path.DirectorySeparatorChar)}{Path.DirectorySeparatorChar}", UriKind.Absolute);
        return new FontFamily(directoryUri, $"./#{familyName}");
    }

    private sealed class ThemePalette
    {
        public Color WindowBackgroundStart { get; init; }

        public Color WindowBackgroundEnd { get; init; }

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
