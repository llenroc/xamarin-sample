﻿

namespace Bullytect.Core.Converters
{

	using System;
	using System.Globalization;
	using Xamarin.Forms;

    public class NullVisibilityConverter: IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null ? false : true;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
    }
}
