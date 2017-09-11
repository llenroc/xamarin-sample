

using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Bullytect.Core.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {

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


        #endregion
    }
}
