using System;
using System.Globalization;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class IsNullOrWhiteSpaceConverter : IValueConverter
	{
        public static IsNullOrWhiteSpaceConverter Instance = new IsNullOrWhiteSpaceConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            return string.IsNullOrWhiteSpace((string)value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{

            return string.IsNullOrWhiteSpace((string)value);
		}
	}
}
