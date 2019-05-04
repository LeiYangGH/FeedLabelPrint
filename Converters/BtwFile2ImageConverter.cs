using System;
using System.IO;
using System.Windows.Data;

namespace FeedLabelPrint.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class BtwFile2ImageConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return Path.GetFileNameWithoutExtension((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

