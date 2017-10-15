using System;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Pages.Common.Templates;
using Bullytect.Core.ViewModels.Core.Models;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Home.Settings.Templates
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
			var category = BindingContext as AlertCategoryModel;
			if (category == null)
				return;
			
		}


		async void OnAlertCategoryInfo(object sender, EventArgs args)
		{

            var alertCategory = BindingContext as AlertCategoryModel;
            var page = new CommonInfoPopup(alertCategory.Name, alertCategory.Description);
			await PopupNavigation.PushAsync(page);

		}
	}
}
