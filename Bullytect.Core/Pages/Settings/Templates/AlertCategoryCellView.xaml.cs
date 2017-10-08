using System;
using System.Collections.Generic;
using Bullytect.Core.Models.Domain;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Settings.Templates
{

	public class AlertCategoryCell : ViewCell
	{
		public AlertCategoryCell()
		{
		
			View = new AlertCategoryCellView();
		}
	}

	public partial class AlertCategoryCellView : ContentView
	{
		public AlertCategoryCellView()
		{
			InitializeComponent();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			var category = BindingContext as AlertCategoryEntity;
			if (category == null)
				return;
			
		}
	}
}
