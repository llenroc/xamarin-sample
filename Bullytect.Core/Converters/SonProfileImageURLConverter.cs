﻿using System;
using System.Globalization;
using Xamarin.Forms;
using Bullytect.Core.Rest.Utils;

namespace Bullytect.Core.Converters
{
	public class SonProfileImageURLConverter : IValueConverter
	{
		public static SonProfileImageURLConverter Instance = new SonProfileImageURLConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{

			return !string.IsNullOrEmpty((string)value) ?
						  ImageSource.FromUri(new Uri(ApiEndpoints.GET_SON_PROFILE_IMAGE.Replace(":id", (string)value))) :
						  ImageSource.FromFile("user_default.png");

		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
            return value;
		}
	}
}
