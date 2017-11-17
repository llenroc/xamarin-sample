using System;
using System.Diagnostics;
using System.Globalization;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class AdultLevelToColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is AdultLevelEnum))
                    return (Color)Application.Current.Resources["BaseTextColor"];

                switch (((AdultLevelEnum)value))
                {

                    case AdultLevelEnum.UNKNOWN:
                        return (Color)Application.Current.Resources["BaseTextColor"];
                    case AdultLevelEnum.POSITIVE:
                        return (Color)Application.Current.Resources["ErrorColor"];
                    case AdultLevelEnum.NEGATIVE:
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
