
using System;
using Bullytect.Core.I18N;
using Bullytect.Core.Pages.Common;
using Bullytect.Core.Pages.Common.Templates;
using Bullytect.Core.ViewModels;
using Rg.Plugins.Popup.Services;

namespace Bullytect.Core.Pages.SonStatistics.Settings
{
    public partial class SettingsPage : BaseContentPage<SonStatisticsSettingsViewModel>
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

		async void OnTimeIntervalInfo(object sender, EventArgs args)
		{

            var page = new CommonInfoPopup(AppResources.Settings_Statistics_General_Interval, AppResources.Settings_Statistics_General_Interval_Description);
			await PopupNavigation.PushAsync(page);

		}
    }
}
