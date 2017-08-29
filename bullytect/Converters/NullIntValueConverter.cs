using System;
using System.Globalization;
using Xamarin.Forms;

namespace bullytect.Converters
{
	public class NullIntValueConverter : IValueConverter
	{
		public static NullIntValueConverter Instance = new NullIntValueConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null ? string.Empty : value.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || !(value is string))
				return null;

			int i;
			if (int.TryParse((string)value, out i))
				return i;

			return null;
		}
	}
}
