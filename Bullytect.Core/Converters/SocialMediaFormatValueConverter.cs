using System;
using MvvmCross.Platform.Converters;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Globalization;


namespace Bullytect.Core.Converters
{
	public class SocialMediaFormatValueConverter : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{

            IList<SocialMediaEntity> SocialMedias = (IList<SocialMediaEntity>)value;

			var item = SocialMedias
                .Cast<SocialMediaEntity>()
                .SingleOrDefault(social => social.Type.Equals((string)parameter));


		}


		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
