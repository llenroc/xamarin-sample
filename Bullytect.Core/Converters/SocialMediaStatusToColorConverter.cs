using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Bullytect.Core.Models.Domain;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class SocialMediaStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
			try
			{
				IList<SocialMediaEntity> SocialMedias = (IList<SocialMediaEntity>)value;

				if (SocialMedias == null || SocialMedias.Count() == 0)
					return false;

				var socialMedia = SocialMedias.SingleOrDefault(social => social.Type.Equals((string)parameter));

                if(socialMedia != null) {
                    return socialMedia.InvalidToken ? 
                                      (Color)Application.Current.Resources["WarningColor"] : 
                                      (Color)Application.Current.Resources["OkColor"];
                } else {
                    return (Color)Application.Current.Resources["WarningColor"];
                }

			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to convert: " + ex);
			}

			return (Color)Application.Current.Resources["WarningColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
