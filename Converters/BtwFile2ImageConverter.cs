using Seagull.BarTender.Print;
using System;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace FeedLabelPrint.Converters
{
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class BtwFile2ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            string btwFileName = (string)value;
            System.Drawing.Image btwImage = LabelFormatThumbnail.Create(btwFileName, System.Drawing.Color.Gray,
   800, 800);
            using (var ms = new MemoryStream())
            {
                btwImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;

                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = ms;
                bi.EndInit();
                btwImage.Dispose(); //if bmp is not used further.
                return bi;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

