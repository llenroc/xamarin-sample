

using System;
using System.Windows.Input;
using Bullytect.Core.Pages.Results;
using Bullytect.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Core;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Home
{
    public partial class HomePage : MvxContentPage<HomeViewModel>
    {
        public HomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing() {
            /*if(ViewModel.SelfParent == null || ViewModel.AlertsPage.Alerts?.Count == 0)
                RefreshLayout.RefreshCommand?.Execute(null);*/


        }

		async void OnUpcomingAppointmentsButtonClicked(object sender, EventArgs e)
		{
            await Navigation.PushAsync(new ResultsPage());
		}
    }
}
