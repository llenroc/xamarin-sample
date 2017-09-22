using System;
using MvvmCross.Platform.Converters;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Globalization;


namespace Bullytect.Core.Converters
{
	public class ValidationErrorFormatValueConverter : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{

            Dictionary<string, string> fieldErrors = (Dictionary<string, string>)value;

			if (fieldErrors == null || !fieldErrors.ContainsKey((string)parameter))
				return "";

			var outValue = String.Empty;
			fieldErrors.TryGetValue((string)parameter, out outValue);

			return outValue;


		}


		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
