using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class SocialMediaStatusToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
			try
			{
				IList<SocialMediaEntity> SocialMedias = (IList<SocialMediaEntity>)value;

				if (SocialMedias == null || SocialMedias.Count() == 0)
					return AppResources.EditSon_Social_Media_Unconfigured_Access_Token;

				var socialMedia = SocialMedias.SingleOrDefault(social => social.Type.Equals((string)parameter));

                if(socialMedia != null) {

                    return socialMedia.InvalidToken ? AppResources.EditSon_Social_Media_Invalid_Access_Token : AppResources.EditSon_Social_Media_Valid_Access_Token;

                } else {
                    return AppResources.EditSon_Social_Media_Unconfigured_Access_Token;
                }

			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to convert: " + ex);
			}
			return  AppResources.EditSon_Social_Media_Unconfigured_Access_Token;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
