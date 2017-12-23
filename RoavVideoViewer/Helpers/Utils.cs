using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace RoavVideoViewer.Helpers
{
    public class Utils
    {
        public static string GetFileNameWithoutExt(string videoFile)
        {
            return videoFile.Substring(0, videoFile.Length - 4);
        }

        internal static string GetNoThumbnailPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Resources\\no-thumbnail.png";
        }
    }

    public class ImageConverter : IValueConverter
    {
        public object Convert(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new BitmapImage(new Uri(value.ToString())) { DecodePixelWidth = 180 };
        }

        public object ConvertBack(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
