using System;
using System.Globalization;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
	public class ValueGreaterThanZeroConverter : IValueConverter
	{
		public static ValueGreaterThanZeroConverter Instance = new ValueGreaterThanZeroConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{

            return value != null && value.GetType() == typeof(int) && (int)value > 0;
 
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value != null && value.GetType() == typeof(int) && (int)value > 0;
		}
	}
}
