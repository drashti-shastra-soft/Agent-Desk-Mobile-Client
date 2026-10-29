using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.Converters;

//public class NullOrEmptyToDashConverter : IValueConverter
//{
//    public object Convert(object value, Type targetType,
//        object parameter, CultureInfo culture)
//    {
//        return string.IsNullOrEmpty(value as string) ? "--" : value;
//    }

//    public object ConvertBack(object value, Type targetType,
//        object parameter, CultureInfo culture)
//    {
//        return value;
//    }
//}


public class NullOrEmptyToDashConverter : IValueConverter
{
    public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
        if (value == null)
            return "--";

        if (value is string str)
            return string.IsNullOrWhiteSpace(str) ? "--" : str;

        if (value is DateTime date)
            return date.ToString("dd-MM-yyyy", culture);

        if (value is decimal amount)
            return "₹" + amount.ToString("N2", culture);

        return value;
    }

    public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
        return value;
    }
}

