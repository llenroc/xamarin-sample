using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
using Bullytect.Core.Rest.Utils;

namespace Bullytect.Core.Converters
{
	public class ParentProfileImageURLConverter : IValueConverter
	{
		public static SonProfileImageURLConverter Instance = new SonProfileImageURLConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            return !string.IsNullOrEmpty((string)value) ?
                          ImageSource.FromUri(new Uri(ApiEndpoints.GET_PARENT_PROFILE_IMAGE.Replace(":id", (string)value))) :
                          ImageSource.FromFile("user_default.png");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
            return value;
		}
	}
}
