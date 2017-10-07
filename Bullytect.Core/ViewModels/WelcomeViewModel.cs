

using MvvmCross.Core.ViewModels;
using System.Windows.Input;
using Acr.UserDialogs;
using MvvmCross.Plugins.Messenger;
using Bullytect.Core.Services;
using Bullytect.Core.Helpers;

namespace Bullytect.Core.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        public WelcomeViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger, AppHelper appHelper) : base(userDialogs, mvxMessenger, appHelper)
        {
        }

        #region commands

        public ICommand GoToLoginCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<AuthenticationViewModel>(new AuthenticationViewModel.AuthenticationParameter()
                {
                    ReasonForAuthentication = AuthenticationViewModel.NORMAL_AUTHENTICATION
                }));
            }
        }


		public ICommand GoToSignupCommand
		{
			get
			{
				return new MvxCommand(() => ShowViewModel<SignupViewModel>());
			}
		}

		public ICommand GoToWalkthroughCommand
		{
			get
			{
				return new MvxCommand(() => ShowViewModel<WalkthroughViewModel>());
			}
		}

        #endregion

    }
}
