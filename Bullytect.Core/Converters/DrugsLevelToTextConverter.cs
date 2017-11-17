using System;
using System.Diagnostics;
using System.Globalization;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class DrugsLevelToTextConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is DrugsLevelEnum))
                    return AppResources.Comments_Detail_Drugs_Level_Unknown;

                switch (((DrugsLevelEnum)value))
                {

                    case DrugsLevelEnum.UNKNOWN:
                        return AppResources.Comments_Detail_Drugs_Level_Unknown;
                    case DrugsLevelEnum.POSITIVE:
                        return AppResources.Comments_Detail_Drugs_Level_Positive;
                    case DrugsLevelEnum.NEGATIVE:
                        return AppResources.Comments_Detail_Drugs_Level_Negative;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to convert: " + ex);
            }

            return AppResources.Comments_Detail_Drugs_Level_Unknown;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
