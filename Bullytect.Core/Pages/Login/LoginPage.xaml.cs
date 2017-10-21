

using System;
using Bullytect.Core.Pages.Common;
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Login
{
    public partial class LoginPage : BaseContentPage<AuthenticationViewModel>
    {
        public LoginPage()
        {
            InitializeComponent();
        }


		async void OnCloseButtonClicked(object sender, EventArgs args)
		{
			await Navigation.PopModalAsync();
		}

    }
}
