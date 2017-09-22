

namespace Bullytect.Core.Converters
{

	using System;
	using System.Globalization;
	using Xamarin.Forms;
	using System.Collections.Generic;

    public class ContainsKeyVisibilityConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            Dictionary<string, string> fieldErrors = (Dictionary<string, string>)value;
            return fieldErrors.ContainsKey((string)parameter);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
    }
}
