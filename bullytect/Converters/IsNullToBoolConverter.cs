using System;
using System.Globalization;
using Xamarin.Forms;

namespace bullytect.Converters
{
	public class IsNullToBoolConverter : IValueConverter
	{
		public static IsNullToBoolConverter Instance = new IsNullToBoolConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null;
		}
	}
}
