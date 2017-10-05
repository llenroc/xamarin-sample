using System;
using System.Globalization;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
	public class ValueEqualToZeroConverter : IValueConverter
	{
		public static ValueEqualToZeroConverter Instance = new ValueEqualToZeroConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{

            return value != null && value.GetType() == typeof(int) && (int)value == 0;
 
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value != null && value.GetType() == typeof(int) && (int)value == 0;
		}
	}
}
