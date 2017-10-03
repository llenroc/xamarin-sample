using System;
using System.Globalization;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
	public class TailStringConverter : IValueConverter
	{
		public static TailStringConverter Instance = new TailStringConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            return ((string)value).Substring((int)parameter);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}
