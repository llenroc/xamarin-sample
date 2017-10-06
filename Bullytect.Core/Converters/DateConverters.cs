using System;
using System.Diagnostics;
using System.Globalization;
using Bullytect.Core.Utils;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{

    class DateTimeToElapsedTimeConverter : IValueConverter 
    {

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
				if (!(value is DateTime))
					return string.Empty;
                
                return ((DateTime)value).ElapsedTime();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to convert: " + ex);
			}

			return string.Empty;
		}


		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
    }

}
