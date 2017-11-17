using System;
using System.Diagnostics;
using System.Globalization;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class BullyingLevelToColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is BullyingLevelEnum))
                    return (Color)Application.Current.Resources["BaseTextColor"];

                switch (((BullyingLevelEnum)value))
                {

                    case BullyingLevelEnum.UNKNOWN:
                        return (Color)Application.Current.Resources["BaseTextColor"];
                    case BullyingLevelEnum.POSITIVE:
                        return (Color)Application.Current.Resources["ErrorColor"];
                    case BullyingLevelEnum.NEGATIVE:
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
