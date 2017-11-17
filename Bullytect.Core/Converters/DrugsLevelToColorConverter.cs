using System;
using System.Diagnostics;
using System.Globalization;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class DrugsLevelToColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is DrugsLevelEnum))
                    return (Color)Application.Current.Resources["BaseTextColor"];

                switch (((DrugsLevelEnum)value))
                {

                    case DrugsLevelEnum.UNKNOWN:
                        return (Color)Application.Current.Resources["BaseTextColor"];
                    case DrugsLevelEnum.POSITIVE:
                        return (Color)Application.Current.Resources["ErrorColor"];
                    case DrugsLevelEnum.NEGATIVE:
                        return (Color)Application.Current.Resources["OkColor"];
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to convert: " + ex);
            }

            return (Color)Application.Current.Resources["BaseTextColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
