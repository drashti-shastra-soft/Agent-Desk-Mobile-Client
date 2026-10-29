using System.Globalization;

namespace AgentDesk.Converters;

public class RememberMeIconConverter : IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        return value is bool isChecked && isChecked
            ? "icon_check"
            : "icon_uncheck";
    }

    public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
