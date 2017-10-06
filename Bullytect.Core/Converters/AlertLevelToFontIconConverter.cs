using System;
using System.Diagnostics;
using System.Globalization;
using Bullytect.Core.Helpers;
using Bullytect.Core.Models.Domain;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class AlertLevelToFontIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

			try
			{
				if (!(value is AlertLevelEnum))
					return FontAwesomeFont.QuestionCircle;

                switch(((AlertLevelEnum)value)) {

                    case AlertLevelEnum.WARNING:
                        return FontAwesomeFont.Warning;
                    case AlertLevelEnum.INFO:
                        return FontAwesomeFont.Info;
                    case AlertLevelEnum.SUCCESS:
                        return FontAwesomeFont.CheckCircle;
                    case AlertLevelEnum.DANGER:
                        return FontAwesomeFont.ExclamationTriangle;
                }
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to convert: " + ex);
			}

			return FontAwesomeFont.QuestionCircle;



        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
