using System;
using System.Collections.Generic;
using Bullytect.Core.ViewModels.Core.Models;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Common.Templates
{
	public class CommonCategoryCell : ViewCell
	{
		public CommonCategoryCell()
		{

			View = new CommonCategoryCellView();
		}
	}

	public partial class CommonCategoryCellView : ContentView
	{
		public CommonCategoryCellView()
		{
			InitializeComponent();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			var category = BindingContext as CategoryModel;
			if (category == null)
				return;

		}


		async protected void OnCategoryInfo(object sender, EventArgs args)
		{

			var category = BindingContext as CategoryModel;
			var page = new CommonInfoPopup(category.Name, category.Description);
			await PopupNavigation.PushAsync(page);

		}
	}
}
