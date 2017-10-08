using System;
using System.Globalization;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using static Bullytect.Core.ViewModels.EditSonViewModel;

namespace Bullytect.Core.Converters
{
	public class IsSchoolNamesCollectionEmptyConverter : IValueConverter
	{
		public static IsSchoolNamesCollectionEmptyConverter Instance = new IsSchoolNamesCollectionEmptyConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            ObservableCollection<SchoolPickerModel> items = (ObservableCollection<SchoolPickerModel>)value;

            return items?.Count == 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
            return value;
		}
	}
}
