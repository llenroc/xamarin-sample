using System;
using System.Collections.Generic;
using Bullytect.Core.I18N;
using Bullytect.Core.Pages.Common.Templates;
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Results.Settings
{
    public partial class SettingsPage : MvxContentPage<ResultsSettingsViewModel>
    {
        public SettingsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

		async void OnIterationsCountInfo(object sender, EventArgs args)
		{

            var page = new CommonInfoPopup(AppResources.Results_Settings_Iterations_Count, AppResources.Results_Settings_Iterations_Count_Description);
			await PopupNavigation.PushAsync(page);

		}
    }
}
