using System;
using System.Collections.Generic;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Pages.Common.Templates;
using Bullytect.Core.Pages.Results.Settings.Templates;
using Bullytect.Core.ViewModels;
using Bullytect.Core.ViewModels.Core.Models;
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

		protected override void OnAppearing()
		{
            ViewModel.SonCategoriesLoaded += ViewModel_OnSonCategoriesLoaded;
		}

		protected override void OnDisappearing()
		{
			ViewModel.SonCategoriesLoaded -= ViewModel_OnSonCategoriesLoaded;
		}

		void ViewModel_OnSonCategoriesLoaded(Object sender, List<SonCategoryModel> Categories)
		{
            
            TableSectionChildren.Clear();

			TableSectionChildren.Add(new CommonCategoryCell
			{
				BindingContext = ViewModel.AllCategory
			});

			foreach (var item in Categories)
			{
                TableSectionChildren.Add(new SonCategoryCell
				{
					BindingContext = item
				});
			}
		}
    }
}
