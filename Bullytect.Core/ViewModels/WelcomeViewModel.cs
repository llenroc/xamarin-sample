

using MvvmCross.Core.ViewModels;
using System.Windows.Input;
using Acr.UserDialogs;
using MvvmCross.Plugins.Messenger;

namespace Bullytect.Core.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        public WelcomeViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger) : base(userDialogs, mvxMessenger)
        {
        }

        #region commands

        public ICommand GoToLoginCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<AuthenticationViewModel>());
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
