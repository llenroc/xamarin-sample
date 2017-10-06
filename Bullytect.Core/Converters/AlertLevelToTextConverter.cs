using System;
using System.Diagnostics;
using System.Globalization;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class AlertLevelToTextConverter: IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{

			try
			{
                if (!(value is AlertLevelEnum))
                    return AppResources.Alerts_Info_Text;

				switch (((AlertLevelEnum)value))
				{

					case AlertLevelEnum.WARNING:
                        return AppResources.Alerts_Warning_Text;
					case AlertLevelEnum.INFO:
						return AppResources.Alerts_Info_Text;
					case AlertLevelEnum.SUCCESS:
                        return AppResources.Alerts_Success_Text;
					case AlertLevelEnum.DANGER:
                        return AppResources.Alerts_Error_Text;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to convert: " + ex);
			}

			return AppResources.Alerts_Info_Text;



		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
    }
}
