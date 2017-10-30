using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Bullytect.Core.Helpers;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.ViewModels.Core.Models;
using Bullytect.Utils.Helpers;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class SocialMediaToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            string TextIcon;
            

            SocialMediaTypeEnum socialMedia = ((string)value).ToEnum<SocialMediaTypeEnum>();
			if (socialMedia.Equals(SocialMediaTypeEnum.FACEBOOK))
			{
                TextIcon = FontAwesomeFont.Facebook;
			}
			else if (socialMedia.Equals(SocialMediaTypeEnum.INSTAGRAM))
			{
				TextIcon = FontAwesomeFont.Instagram;
			}
			else
			{
				TextIcon = FontAwesomeFont.Youtube;
			}
			return TextIcon;
			
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
