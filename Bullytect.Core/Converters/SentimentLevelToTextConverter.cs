using System;
using System.Diagnostics;
using System.Globalization;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class SentimentLevelToTextConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is SentimentLevelEnum))
                    return AppResources.Comments_Detail_Sentiment_Level_Unknown;

                switch (((SentimentLevelEnum)value))
                {

                    case SentimentLevelEnum.UNKNOWN:
                        return AppResources.Comments_Detail_Sentiment_Level_Unknown;
                    case SentimentLevelEnum.NEUTRO:
                        return AppResources.Comments_Detail_Sentiment_Level_Neutro;
                    case SentimentLevelEnum.POSITIVE:
                        return AppResources.Comments_Detail_Sentiment_Level_Positive;
                    case SentimentLevelEnum.NEGATIVE:
                        return AppResources.Comments_Detail_Sentiment_Level_Negative;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to convert: " + ex);
            }

            return AppResources.Comments_Detail_Sentiment_Level_Unknown;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
