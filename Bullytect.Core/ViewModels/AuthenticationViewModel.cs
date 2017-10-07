using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Config;
using Bullytect.Core.I18N;
using Bullytect.Core.OAuth.Providers.Facebook;
using Bullytect.Core.OAuth.Services;
using Bullytect.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;
using Xamarin.Forms;
using Bullytect.Core.Rest.Models.Exceptions;
using System.Collections.Generic;
using Bullytect.Core.Helpers;
using Bullytect.Core.OAuth.Models;
using Xamarin.Auth;

namespace Bullytect.Core.ViewModels
{

    public class AuthenticationViewModel : BaseViewModel
    {
        readonly IAuthenticationService _authenticationService;

        public AuthenticationViewModel(IAuthenticationService authenticationService,
                                       IUserDialogs userDialogs, IMvxMessenger mvxMessenger, IOAuthService oauthService,  AppHelper appHelper): base(userDialogs, mvxMessenger, appHelper)
        {
            _authenticationService = authenticationService;


            // Create Reactive Commands
            LoginCommand = ReactiveCommand.CreateFromObservable<Unit, string>(
                (_) => _authenticationService.LogIn(_email, _password),
                this.WhenAnyValue(x => x.Email, x => x.Password, (email, pass) =>
                    !String.IsNullOrWhiteSpace(email) && !String.IsNullOrWhiteSpace(pass)
                           && email.Length >= 3 && pass.Length >= 6).DistinctUntilChanged());

            LoginCommand.Subscribe(HandleAuthSuccess);


            LoginCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.Login_Authenticating));


            LoginCommand.ThrownExceptions.Subscribe(HandleExceptions);


			// Create Reactive Commands
			LoginWithFacebookCommand = ReactiveCommand.CreateFromObservable<Unit, string>(
				(param) => {

                    _userDialogs.ShowLoading(AppResources.Login_Authenticating);
                            
                    return oauthService
                        .Authenticate(new FacebookOAuth2())
                        .Where(AccessToken => !string.IsNullOrEmpty(AccessToken))
						.SelectMany(accessToken => authenticationService.LoginWithFacebook(accessToken))
						.Do(_ => _userDialogs.HideLoading());
                    
				});


            LoginWithFacebookCommand.Subscribe(HandleAuthSuccess);

			LoginWithFacebookCommand.ThrownExceptions.Subscribe(HandleExceptions);
        }



        #region Properties

        public string _reasonForAuthentication = NORMAL_AUTHENTICATION;

        public string ReasonForAuthentication
        {
            get => _reasonForAuthentication;
            set => SetProperty(ref _reasonForAuthentication, value);
        }

        string _email;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        string _password;

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        #endregion

        public const string NORMAL_AUTHENTICATION = "NORMAL_AUTHENTICATION";
        public const string SIGN_OUT = "SIGN_OUT";
        public const string SESSION_EXPIRED = "SESSION_EXPIRED";
        public const string SIGN_UP = "SIGN_UP";

		public class AuthenticationParameter
		{
			public string ReasonForAuthentication { get; set; }
		}


        public void Init(AuthenticationParameter authenticationParameter)
        {
            ReasonForAuthentication = authenticationParameter.ReasonForAuthentication;
        }


        public override void Start()
        {


            if (ReasonForAuthentication.Equals(SIGN_OUT))
            {
                _appHelper.Toast(AppResources.Common_SignOut, System.Drawing.Color.FromArgb(255, 0, 0));
            }
            else if (ReasonForAuthentication.Equals(SESSION_EXPIRED)) {
                _appHelper.Toast(AppResources.Common_Invalid_Session, System.Drawing.Color.FromArgb(255, 0, 0));

            } else if(ReasonForAuthentication.Equals(SIGN_UP)) {
                _appHelper.Toast(AppResources.Signup_Account_Created, System.Drawing.Color.FromArgb(12, 131, 193));
            }
        }

        #region commands

        public ReactiveCommand<Unit, string> LoginCommand { get; protected set; }

        public ReactiveCommand<Unit, string> LoginWithFacebookCommand { get; protected set; }

        public ICommand GoToPasswordRecoveryCommand => new MvxCommand(() => ShowViewModel<PasswordRecoveryViewModel>());

		#endregion

		protected override void HandleExceptions(Exception ex)
		{

			if (ex is AuthenticationFailedException)
			{
                _appHelper.Toast(AppResources.Login_Failed, System.Drawing.Color.FromArgb(255, 0, 0));

			} else if(ex is AccountDisabledException)
            {
                _appHelper.Toast(AppResources.Account_Disabled, System.Drawing.Color.FromArgb(255, 0, 0));


            }
			else
			{
				base.HandleExceptions(ex);
			}
		}

        void HandleAuthSuccess(string jwtToken) {
			Debug.WriteLine("JWT Token -> " + jwtToken);
			_userDialogs.ShowSuccess(AppResources.Login_Success);
            //var mvxBundle = new MvxBundle(new Dictionary<string, string> { { "NavigationCommand", "StackClear" } });
            ShowViewModel<HomeViewModel>();
        }
       

    }


}
