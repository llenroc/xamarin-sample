using System;
using Bullytect.Core.Pages.Common.Controls;
using Bullytect.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRendererAttribute(typeof(CustomDatePicker), typeof(CustomDatePickerRenderer))]
namespace Bullytect.Droid.Renderers
{
    public class CustomDatePickerRenderer: DatePickerRenderer
    {
		protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
		{
			base.OnElementChanged(e);

			CustomDatePicker datePicker = (CustomDatePicker)Element;

			if (datePicker != null)
			{
				SetTextColor(datePicker);
			}

			if (e.OldElement == null)
			{
				//Wire events
			}

			if (e.NewElement == null)
			{
				//Unwire events
			}
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (Control == null)
			{
				return;
			}

			CustomDatePicker datePicker = (CustomDatePicker)Element;

			if (e.PropertyName == CustomDatePicker.TextColorProperty.PropertyName)
			{
				this.Control.SetTextColor(datePicker.TextColor.ToAndroid());
			}
		}

		void SetTextColor(CustomDatePicker datePicker)
		{
			this.Control.SetTextColor(datePicker.TextColor.ToAndroid());
		}
	}
}
