using System;
using System.Collections.Generic;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Pages.Common.Templates;
using Bullytect.Core.Pages.Settings.Templates;
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Settings
{
    public partial class SettingsPage : MvxContentPage<SettingsViewModel>
    {
        public SettingsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            ViewModel.AlertsCategoriesLoaded += ViewModel_OnAlertsCategoriesLoaded;
        }

        protected override void OnDisappearing()
        {
            ViewModel.AlertsCategoriesLoaded -= ViewModel_OnAlertsCategoriesLoaded;
        }


		async void OnCountNewAlertsInfo(object sender, EventArgs args)
		{

            var page = new CommonInfoPopup(AppResources.Settings_Alerts_Count_New_Alerts, AppResources.Settings_Alerts_Count_New_Alerts_Description);
			await PopupNavigation.PushAsync(page);

		}


		async void OnAntiquityOfAlertsInfo(object sender, EventArgs args)
		{

            var page = new CommonInfoPopup(AppResources.Settings_Antiquity_Of_Alerts, AppResources.Settings_Antiquity_Of_Alerts_Description);
			await PopupNavigation.PushAsync(page);

		}
		

        void ViewModel_OnAlertsCategoriesLoaded(Object sender, List<AlertCategoryEntity> AlertsCategories)
		{
			var allCell = new AlertCategoryCell
			{
                BindingContext = ViewModel.AllCategory
			};

			TableSectionCategories.Add(allCell);

			foreach (var item in AlertsCategories)
			{
				TableSectionCategories.Add(new AlertCategoryCell
				{
					BindingContext = item
				});
			}
		}
    }
}
