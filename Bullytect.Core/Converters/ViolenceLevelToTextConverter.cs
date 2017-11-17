using System;
using System.Diagnostics;
using System.Globalization;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class ViolenceLevelToTextConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is ViolenceLevelEnum))
                    return AppResources.Comments_Detail_Violence_Level_Unknown;

                switch (((ViolenceLevelEnum)value))
                {

                    case ViolenceLevelEnum.UNKNOWN:
                        return AppResources.Comments_Detail_Violence_Level_Unknown;
                    case ViolenceLevelEnum.POSITIVE:
                        return AppResources.Comments_Detail_Violence_Level_Positive;
                    case ViolenceLevelEnum.NEGATIVE:
                        return AppResources.Comments_Detail_Violence_Level_Negative;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to convert: " + ex);
            }

            return AppResources.Comments_Detail_Violence_Level_Unknown;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
