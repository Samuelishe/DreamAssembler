using System.Windows;

namespace DreamAssembler.App.Services;

/// <summary>
/// Реализация сервиса работы с буфером обмена WPF.
/// </summary>
public sealed class ClipboardService : IClipboardService
{
    /// <summary>
    /// Копирует текст в буфер обмена.
    /// </summary>
    /// <param name="text">Текст для копирования.</param>
    public void SetText(string text)
    {
        Clipboard.SetText(text);
    }
}

