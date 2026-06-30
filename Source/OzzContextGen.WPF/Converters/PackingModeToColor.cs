using OzzContextGen.Core.Models;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace OzzContextGen.WPF.Converters
{
    public class PackingModeToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PackingMode packingMode)
            {
                return packingMode switch
                {
                    PackingMode.Excluded => new SolidColorBrush(Colors.LightPink),
                    PackingMode.MetadataOnly => new SolidColorBrush(Colors.Yellow),
                    PackingMode.FullPack => new SolidColorBrush(Colors.LightGreen),
                    _ => new SolidColorBrush(Colors.Gray)
                };
            }

            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
