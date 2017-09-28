using System;
using System.Globalization;
using Xamarin.Forms;

namespace Bullytect.Core.Converters
{
	public class SwitchChangeEventArgsConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var eventArgs = value as ToggledEventArgs;
			if (eventArgs == null)
				throw new ArgumentException("Expected ToggledEventArgs as value", "value");

			return eventArgs.Value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
