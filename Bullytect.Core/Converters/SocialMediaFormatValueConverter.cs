using System;
using MvvmCross.Platform.Converters;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Globalization;
using Bullytect.Core.Models.Domain;
using System.Linq;

namespace Bullytect.Core.Converters
{
	public class SocialMediaFormatValueConverter : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{

            IList<SocialMediaEntity> SocialMedias = (IList<SocialMediaEntity>)value;

            if (SocialMedias == null || SocialMedias.Count() == 0)
                return false;

            var socialMedia = SocialMedias.SingleOrDefault(social => social.Type.Equals((string)parameter));

            return socialMedia != null && !socialMedia.InvalidToken;
		}


		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
            return value;
		}
	}
}
