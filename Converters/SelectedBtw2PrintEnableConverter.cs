using System;
using System.IO;
using System.Windows.Data;

namespace FeedLabelPrint.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class SelectedBtw2PrintEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value is null)
                return false;
            if (string.IsNullOrWhiteSpace((string)value))
                return false;
            return File.Exists((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
