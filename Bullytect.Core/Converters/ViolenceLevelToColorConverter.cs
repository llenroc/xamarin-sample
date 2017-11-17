using System;
using System.Diagnostics;
using System.Globalization;
using Bullytect.Core.Models.Domain;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class ViolenceLevelToColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is ViolenceLevelEnum))
                    return (Color)Application.Current.Resources["BaseTextColor"];

                switch (((ViolenceLevelEnum)value))
                {

                    case ViolenceLevelEnum.UNKNOWN:
                        return (Color)Application.Current.Resources["BaseTextColor"];
                    case ViolenceLevelEnum.POSITIVE:
                        return (Color)Application.Current.Resources["ErrorColor"];
                    case ViolenceLevelEnum.NEGATIVE:
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
