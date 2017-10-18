
using System;
using Bullytect.Core.I18N;
using Bullytect.Core.Pages.Common.Templates;
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.SonStatistics.Settings
{
    public partial class SettingsPage : MvxContentPage<SonStatisticsSettingsViewModel>
    {
        public SettingsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

		async void OnTimeIntervalInfo(object sender, EventArgs args)
		{

            var page = new CommonInfoPopup(AppResources.Settings_Statistics_Son_General_Interval, AppResources.Settings_Statistics_Son_General_Interval_Description);
			await PopupNavigation.PushAsync(page);

		}
    }
}
