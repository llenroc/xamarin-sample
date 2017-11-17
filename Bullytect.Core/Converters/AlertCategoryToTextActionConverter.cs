using System;
using System.Diagnostics;
using System.Globalization;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class AlertCategoryToTextActionConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is AlertCategoryEnum))
                    return string.Empty;

                switch (((AlertCategoryEnum)value))
                {

                    case AlertCategoryEnum.DEFAULT:
                        return string.Empty;
                    case AlertCategoryEnum.GENERAL_STATISTICS:
                        return AppResources.Alerts_Category_General_Statistics_Action_Text;
                    case AlertCategoryEnum.INFORMATION_SON:
                        return AppResources.Alerts_Category_Information_Son_Action_Text;
                    case AlertCategoryEnum.STATISTICS_SON:
                        return AppResources.Alerts_Category_Statistics_Son_Action_Text;
                    case AlertCategoryEnum.INFORMATION_EXTRACTION:
                        return AppResources.Alerts_Category_Information_Extraction_Action_Text;
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
