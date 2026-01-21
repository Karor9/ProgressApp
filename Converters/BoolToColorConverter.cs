using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ProgressApp.Converters;

internal class BoolToColorConverter : IValueConverter
{
    Color unreaden = new Color(255, 127, 127);
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isCompleted)
            return isCompleted ? Colors.LightGreen : unreaden;
        return unreaden;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
