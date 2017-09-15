
using System;
using System.Diagnostics;
using System.Reactive.Linq;
using Acr.UserDialogs;
using Bullytect.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;
using Bullytect.Core.I18N;
using Bullytect.Core.Messages;
using System.Windows.Input;
using System.Collections.Generic;

namespace Bullytect.Core.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {

        readonly IAuthenticationService _authenticationService;
        readonly IUserDialogs _userDialogs;
        readonly IMvxMessenger _mvxMessenger;

        public WelcomeViewModel(IAuthenticationService authenticationService, 
                                IUserDialogs userDialogs, IMvxMessenger mvxMessenger){
            _authenticationService = authenticationService;
            _userDialogs = userDialogs;
            _mvxMessenger = mvxMessenger;

			// Create Reactive Commands
			LoginWithFacebookCommand = ReactiveCommand.CreateFromObservable<string, string>(
                (param) => _authenticationService.LoginToFacebook());


            LoginWithFacebookCommand.Subscribe(token => {
                Debug.WriteLine("JWT Token -> " + token);
                _userDialogs.ShowSuccess(AppResources.Login_Success);
				var mvxBundle = new MvxBundle(new Dictionary<string, string> { { "NavigationCommand", "StackClear" } });
				ShowViewModel<HomeViewModel>(presentationBundle: mvxBundle);
            });


			LoginWithFacebookCommand.IsExecuting.Subscribe((isLoading) => {
				/*if (isLoading)
				{
                    _userDialogs.ShowLoading(AppResources.Login_Authenticating, MaskType.Black);
				}
				else
				{
					_userDialogs.HideLoading();
				}*/
			});

			LoginWithFacebookCommand.ThrownExceptions.Subscribe((ex) =>
			{
				Debug.WriteLine(String.Format("Exception: {0}", ex.ToString()));
				_mvxMessenger.Publish(new ExceptionOcurredMessage(this, ex));
			});

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

        public ReactiveCommand<string, string> LoginWithFacebookCommand { get; protected set; }
     

        #endregion

    }
}
