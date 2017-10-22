using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
    public class ItemToIndexConverter: IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || parameter == null) return -1;
			var index = ((ListView)parameter).ItemsSource.Cast<object>().ToList().IndexOf(value);
			Debug.WriteLine("Index -> " + index);
			return index;
			
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
            throw new NotImplementedException();
		}
    }
}
