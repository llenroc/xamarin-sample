using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
	public class InverseBoolConverter : IValueConverter
	{
		public static InverseBoolConverter Instance = new InverseBoolConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            return !(bool)value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{

			return !(bool)value;
		}
	}
}
