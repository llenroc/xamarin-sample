using System;
using System.Globalization;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Bullytect.Core.Converters
{
	public class IsSchoolNamesCollectionNotEmptyConverter : IValueConverter
	{
		public static IsSchoolNamesCollectionNotEmptyConverter Instance = new IsSchoolNamesCollectionNotEmptyConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            ObservableCollection<string> items = (ObservableCollection<string>)value;

            return items?.Count > 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
            return value;
		}
	}
}
