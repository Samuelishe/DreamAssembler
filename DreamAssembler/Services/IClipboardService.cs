namespace DreamAssembler.App.Services;

/// <summary>
/// Определяет сервис копирования текста в буфер обмена.
/// </summary>
public interface IClipboardService
{
    /// <summary>
    /// Копирует текст в буфер обмена.
    /// </summary>
    /// <param name="text">Текст для копирования.</param>
    void SetText(string text);
}

