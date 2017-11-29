using System;
using System.Collections.Generic;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Pages.Common;
using Bullytect.Core.Pages.Common.Templates;
using Bullytect.Core.Pages.Home.Settings.Templates;
using Bullytect.Core.ViewModels;
using Bullytect.Core.ViewModels.Core.Models;
using Rg.Plugins.Popup.Services;

namespace Bullytect.Core.Pages.Home.Settings
{
    public partial class SettingsPage : BaseContentPage<HomeSettingsViewModel>
    {
        public SettingsPage()
        {
            InitializeComponent();
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

        async void OnPrivacyPolicyTapped(object sender, EventArgs args){

            var page = new CommonInfoPopup(AppResources.Privacy_Policy_Title, AppResources.Privacy_Policy_Description);
            await PopupNavigation.PushAsync(page);
        }

		async void OnAntiquityOfAlertsInfo(object sender, EventArgs args)
		{

            var page = new CommonInfoPopup(AppResources.Settings_Antiquity_Of_Alerts, AppResources.Settings_Antiquity_Of_Alerts_Description);
			await PopupNavigation.PushAsync(page);

		}

		async void OnPushNotificationsInfo(object sender, EventArgs args)
		{

            var page = new CommonInfoPopup(AppResources.Settings_Push_Notifications, 
                                           AppResources.Settings_Push_Notifications_Description);
			await PopupNavigation.PushAsync(page);

		}

        async void OnRemoveAlertsInfo(object sender, EventArgs args){

            var page = new CommonInfoPopup(AppResources.Settings_Remove_Alerts,
                                           AppResources.Settings_Remove_Alerts_Description);
			await PopupNavigation.PushAsync(page);

        }


        void ViewModel_OnAlertsCategoriesLoaded(Object sender, List<AlertCategoryModel> AlertsCategories)
		{
            TableSectionCategories.Clear();

			TableSectionCategories.Add(new CommonCategoryCell
			{
				BindingContext = ViewModel.AllCategory
			});

			foreach (var item in AlertsCategories)
			{
                var AlertCategoryCell = new AlertCategoryCell
                {
                    BindingContext = item
                };

				TableSectionCategories.Add(AlertCategoryCell);
			}
		}
    }
}
