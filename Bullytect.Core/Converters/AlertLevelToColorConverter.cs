using System;
using System.Diagnostics;
using System.Globalization;
using Bullytect.Core.Models.Domain;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class AlertLevelToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
			try
			{
                if (!(value is AlertLevelEnum))
                    return (Color)Application.Current.Resources["OkColor"];

				switch (((AlertLevelEnum)value))
				{

					case AlertLevelEnum.WARNING:
						return (Color)Application.Current.Resources["WarningColor"];
					case AlertLevelEnum.INFO:
						return (Color)Application.Current.Resources["NotificationColor"];
					case AlertLevelEnum.SUCCESS:
						return (Color)Application.Current.Resources["OkColor"];
					case AlertLevelEnum.DANGER:
						return (Color)Application.Current.Resources["ErrorColor"];
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to convert: " + ex);
			}

			return (Color)Application.Current.Resources["OkColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
