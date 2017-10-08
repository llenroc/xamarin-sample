using System;
using System.Collections.Generic;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Pages.Settings.Templates;
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
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
