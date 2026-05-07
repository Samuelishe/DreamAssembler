using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DreamAssembler.App.Converters;

/// <summary>
/// Возвращает цвет текста в зависимости от булевого значения.
/// </summary>
public sealed class BooleanToBrushConverter : IValueConverter
{
    /// <summary>
    /// Преобразует булево значение в кисть.
    /// </summary>
    /// <param name="value">Исходное значение.</param>
    /// <param name="targetType">Целевой тип.</param>
    /// <param name="parameter">Дополнительный параметр.</param>
    /// <param name="culture">Культура преобразования.</param>
    /// <returns>Кисть для отображения статуса.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is true
            ? new SolidColorBrush(Color.FromRgb(164, 78, 46))
            : new SolidColorBrush(Color.FromRgb(72, 112, 76));
    }

    /// <summary>
    /// Обратное преобразование не поддерживается.
    /// </summary>
    /// <param name="value">Исходное значение.</param>
    /// <param name="targetType">Целевой тип.</param>
    /// <param name="parameter">Дополнительный параметр.</param>
    /// <param name="culture">Культура преобразования.</param>
    /// <returns>Ничего не возвращает.</returns>
    /// <exception cref="NotSupportedException">Выбрасывается всегда.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

