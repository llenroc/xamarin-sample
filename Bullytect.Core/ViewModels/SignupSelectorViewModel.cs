using System;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Helpers;
using Bullytect.Core.OAuth.Services;
using Bullytect.Core.Services;
using Bullytect.Core.ViewModels.Core;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace Bullytect.Core.ViewModels
{
    public class SignupSelectorViewModel : AccountsBaseViewModel
    {
        public SignupSelectorViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger, AppHelper appHelper, IOAuthService oauthService, INotificationService notificationService, IAuthenticationService authenticationService) : base(userDialogs, mvxMessenger, appHelper, oauthService, notificationService, authenticationService)
        {
        }

        #region commands 


        public ICommand GoToSignupCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<SignupViewModel>());
            }
        }
            

        #endregion


    }
}
