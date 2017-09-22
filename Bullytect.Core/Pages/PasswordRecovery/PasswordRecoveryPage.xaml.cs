
using System;
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.PasswordRecovery
{
    public partial class PasswordRecoveryPage : MvxContentPage<PasswordRecoveryViewModel>
    {
        public PasswordRecoveryPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
