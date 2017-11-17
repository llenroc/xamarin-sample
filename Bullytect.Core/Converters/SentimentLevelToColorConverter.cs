using System;
using System.Diagnostics;
using System.Globalization;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class SentimentLevelToColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is SentimentLevelEnum))
                    return (Color)Application.Current.Resources["BaseTextColor"];

                switch (((SentimentLevelEnum)value))
                {

                    case SentimentLevelEnum.UNKNOWN:
                        return (Color)Application.Current.Resources["BaseTextColor"];
                    case SentimentLevelEnum.NEUTRO:
                        return (Color)Application.Current.Resources["NotificationColor"];
                    case SentimentLevelEnum.POSITIVE:
                        return (Color)Application.Current.Resources["ErrorColor"];
                    case SentimentLevelEnum.NEGATIVE:
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
