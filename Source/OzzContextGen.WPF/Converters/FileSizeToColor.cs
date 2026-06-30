using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace OzzContextGen.WPF.Converters
{
    public class FileSizeToColor : IValueConverter
    {
        public FileSizeToColor()
        {
            MinFileSize = 1024;
            MaxFileSize = 32768;
            MinLevel = 0;
            MaxLevel = 255;

            BlueLevel = 0;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long fileSize)
            {
                byte red, green, blue;

                if (fileSize <= MinFileSize)
                {
                    red = MinLevel;
                    green = MaxLevel;
                    blue = BlueLevel;
                }
                else if (fileSize >= MaxFileSize)
                {
                    red = MaxLevel;
                    green = MinLevel;
                    blue = (byte)(BlueLevel * 0.05);
                }
                else
                {
                    // Two-phase gradient: green→yellow (first half) then yellow→red (second half).
                    // This keeps one channel pinned at MaxLevel so the midpoint is always
                    // a bright yellow rather than a muddy olive.
                    float ratio = (float)(fileSize - MinFileSize) / (MaxFileSize - MinFileSize);
                    if (ratio <= 0.5f)
                    {
                        // green → yellow: red climbs 0→MaxLevel, green stays MaxLevel
                        red = (byte)(MinLevel + (MaxLevel - MinLevel) * (ratio * 2f));
                        green = MaxLevel;
                        blue = BlueLevel;
                    }
                    else
                    {
                        // yellow → red: red stays MaxLevel, green falls MaxLevel→0
                        red = MaxLevel;
                        green = (byte)(MinLevel + (MaxLevel - MinLevel) * ((1f - ratio) * 2f));
                        blue = (byte)(BlueLevel * (1f - ratio));

                    }
                }

                return new SolidColorBrush(System.Windows.Media.Color.FromRgb(red, green, blue));
            }

            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public byte MinLevel { get; set; }
        public byte MaxLevel { get; set; }
        public byte BlueLevel { get; set; }
        public long MinFileSize { get; set; }
        public long MaxFileSize { get; set; }
    }
}
