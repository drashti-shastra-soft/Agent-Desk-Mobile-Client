using AgentDesk.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.Converters;


public class EnumDisplayConverter : IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        if (value == null)
            return string.Empty;

        var enumType = value.GetType();

        if (!enumType.IsEnum)
            return value.ToString();

        var field = enumType.GetField(value.ToString());

        var displayAttribute = field?
            .GetCustomAttributes(typeof(DisplayAttribute), false)
            .FirstOrDefault() as DisplayAttribute;

        return displayAttribute?.Name ?? value.ToString();
    }

    public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        return value;
    }
}