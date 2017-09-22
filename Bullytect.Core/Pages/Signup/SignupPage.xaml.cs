

using System;
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Signup
{
    public partial class SignupPage : MvxContentPage<SignupViewModel>
    {
        public SignupPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

    }
}
