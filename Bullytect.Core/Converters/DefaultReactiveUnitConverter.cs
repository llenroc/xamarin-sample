using System;
using System.Globalization;
using System.Reactive;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
	public class DefaultReactiveUnitConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            return Unit.Default;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
