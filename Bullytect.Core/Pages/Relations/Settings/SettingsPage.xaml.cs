using System;
using System.Collections.Generic;
using Bullytect.Core.I18N;
using Bullytect.Core.Pages.Common;
using Bullytect.Core.Pages.Common.Templates;
using Bullytect.Core.ViewModels;
using Bullytect.Core.ViewModels.Core.Models;
using Rg.Plugins.Popup.Services;

namespace Bullytect.Core.Pages.Relations.Settings
{
    public partial class SettingsPage : BaseContentPage<RelationsSettingsViewModel>
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
