
using System;
using System.Diagnostics;
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Alerts
{
    public partial class AlertsPage : MvxContentPage<AlertsViewModel>
    {
        public AlertsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

    }
}
