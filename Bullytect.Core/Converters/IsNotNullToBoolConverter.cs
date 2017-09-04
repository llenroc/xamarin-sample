using System;
using System.Globalization;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
	public class IsNotNullToBoolConverter : IValueConverter
	{
		public static IsNotNullToBoolConverter Instance = new IsNotNullToBoolConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value != null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value != null;
		}
	}
}
