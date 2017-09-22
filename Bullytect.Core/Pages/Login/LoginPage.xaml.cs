

using System;
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Login
{
    public partial class LoginPage : MvxContentPage<AuthenticationViewModel>
    {
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }


		async void OnCloseButtonClicked(object sender, EventArgs args)
		{
			await Navigation.PopModalAsync();
		}

    }
}
