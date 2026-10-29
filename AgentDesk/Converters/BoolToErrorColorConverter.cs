using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.Converters;

//public class BoolToErrorColorConverter : IValueConverter
//{
//    public object Convert(
//        object value,
//        Type targetType,
//        object parameter,
//        CultureInfo culture)
//    {
//        return value is true
//            ? Color.FromArgb("#E51B3E")
//            : Color.FromArgb("#D3D3D3");
//    }

//    public object ConvertBack(
//        object value,
//        Type targetType,
//        object parameter,
//        CultureInfo culture)
//    {
//        throw new NotImplementedException();
//    }
//}

public class BoolToErrorColorConverter : IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        return value is true
            ? Color.FromArgb("#E51B3E")
            : Color.FromArgb("#D3D3D3");
    }

    public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
