using System;
using System.Diagnostics;
using System.Globalization;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class AlertCategoryToTextConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is AlertCategoryEnum))
                    return AppResources.Alerts_Category_Default;

                switch (((AlertCategoryEnum)value))
                {

                    case AlertCategoryEnum.DEFAULT:
                        return AppResources.Alerts_Category_Default;
                    case AlertCategoryEnum.GENERAL_STATISTICS:
                        return AppResources.Alerts_Category_General_Statistics;
                    case AlertCategoryEnum.INFORMATION_SON:
                        return AppResources.Alerts_Category_Information_Son;
                    case AlertCategoryEnum.STATISTICS_SON:
                        return AppResources.Alerts_Category_Statistics_Son;
                    case AlertCategoryEnum.INFORMATION_EXTRACTION:
                        return AppResources.Alerts_Category_Information_Extraction;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to convert: " + ex);
            }

            return AppResources.Alerts_Category_Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
