using System;
using System.Globalization;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class IsValidStringConverter : IValueConverter
	{
        public static IsValidStringConverter Instance = new IsValidStringConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            return !string.IsNullOrWhiteSpace((string)value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{

            return !string.IsNullOrWhiteSpace((string)value);
		}
	}
}
