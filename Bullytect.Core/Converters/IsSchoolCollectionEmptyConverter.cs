using System;
using System.Globalization;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Bullytect.Core.Models.Domain;

namespace Bullytect.Core.Converters
{
    public class IsSchoolCollectionEmptyConverter : IValueConverter
	{
        public static IsSchoolCollectionEmptyConverter Instance = new IsSchoolCollectionEmptyConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            ObservableCollection<SchoolEntity> items = (ObservableCollection<SchoolEntity>)value;

            return items?.Count == 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
            return value;
		}
	}
}
