using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace bullytect.Converters
{
	public class IsEmptyConverter : IValueConverter
	{
		public static IsEmptyConverter Instance = new IsEmptyConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var list = value as IList;

			if (list == null)
				return false;

			return list.Count > 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var list = value as IList;

			if (list == null)
				return false;

			return list.Count > 0;
		}
	}
}
