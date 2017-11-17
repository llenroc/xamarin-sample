using System;
using System.Diagnostics;
using System.Globalization;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class BullyingLevelToTextConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is BullyingLevelEnum))
                    return AppResources.Comments_Detail_Bullying_Level_Unknown;

                switch (((BullyingLevelEnum)value))
                {

                    case BullyingLevelEnum.UNKNOWN:
                        return AppResources.Comments_Detail_Bullying_Level_Unknown;
                    case BullyingLevelEnum.POSITIVE:
                        return AppResources.Comments_Detail_Bullying_Level_Positive;
                    case BullyingLevelEnum.NEGATIVE:
                        return AppResources.Comments_Detail_Bullying_Level_Negative;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to convert: " + ex);
            }

            return AppResources.Comments_Detail_Bullying_Level_Unknown;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
