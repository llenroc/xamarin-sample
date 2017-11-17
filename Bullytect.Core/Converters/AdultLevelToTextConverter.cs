using System;
using System.Diagnostics;
using System.Globalization;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class AdultLevelToTextConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is AdultLevelEnum))
                    return AppResources.Comments_Detail_Adult_Level_Unknown;

                switch (((AdultLevelEnum)value))
                {

                    case AdultLevelEnum.UNKNOWN:
                        return AppResources.Comments_Detail_Adult_Level_Unknown;
                    case AdultLevelEnum.POSITIVE:
                        return AppResources.Comments_Detail_Adult_Level_Positive;
                    case AdultLevelEnum.NEGATIVE:
                        return AppResources.Comments_Detail_Adult_Level_Negative;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to convert: " + ex);
            }

            return AppResources.Comments_Detail_Adult_Level_Unknown;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
