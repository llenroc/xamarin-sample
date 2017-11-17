using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.I18N;
using Bullytect.Core.OAuth.Services;
using Bullytect.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;
using Bullytect.Core.Rest.Models.Exceptions;
using Bullytect.Core.Helpers;
using Bullytect.Core.ViewModels.Core;

namespace Bullytect.Core.ViewModels
{

    public class AuthenticationViewModel : AccountsBaseViewModel
    {
        

        public AuthenticationViewModel(IAuthenticationService authenticationService,
                                       IUserDialogs userDialogs, IMvxMessenger mvxMessenger,
                                       IOAuthService oauthService,  AppHelper appHelper, INotificationService notificationService)
            : base(userDialogs, mvxMessenger, appHelper, oauthService, notificationService, authenticationService)
        {

            // Create Reactive Commands
            LoginCommand = ReactiveCommand.CreateFromObservable<Unit, string>(
                (_) =>  _authenticationService.LogIn(_email, _password),
                this.WhenAnyValue(x => x.Email, x => x.Password, (email, pass) =>
                    !String.IsNullOrWhiteSpace(email) && !String.IsNullOrWhiteSpace(pass)
                           && email.Length >= 3 && pass.Length >= 6).DistinctUntilChanged());

            LoginCommand.Subscribe(HandleAuthSuccess);


            LoginCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecutingWithDialogs(isLoading, AppResources.Login_Authenticating));


            LoginCommand.ThrownExceptions.Subscribe(HandleExceptions);


			
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
        public const string ACCOUNT_DELETED = "ACCOUNT_DELETED";

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
                _appHelper.Toast(AppResources.Common_SignOut, System.Drawing.Color.FromArgb(12, 131, 193));
            }
            else if (ReasonForAuthentication.Equals(SESSION_EXPIRED))
            {
                _appHelper.Toast(AppResources.Common_Invalid_Session, System.Drawing.Color.FromArgb(255, 0, 0));
            }
            else if (ReasonForAuthentication.Equals(SIGN_UP)) { 
            
                _appHelper.ShowAlert(AppResources.Signup_Account_Created);

            } else if(ReasonForAuthentication.Equals(ACCOUNT_DELETED)) {
                _appHelper.Toast(AppResources.Profile_Account_Deleted, System.Drawing.Color.FromArgb(12, 131, 193));
            }
        }

        protected override void OnBackPressed() => ShowViewModel<WelcomeViewModel>();

        #region commands

        public ReactiveCommand<Unit, string> LoginCommand { get; protected set; }

        public ICommand GoToPasswordRecoveryCommand => new MvxCommand(() => ShowViewModel<PasswordRecoveryViewModel>());

        #endregion

        protected override void HandleExceptions(Exception ex)
        {

            if (ex is AuthenticationFailedException)
            {
                _appHelper.Toast(AppResources.Login_Failed, System.Drawing.Color.FromArgb(255, 0, 0));

            }
            else if (ex is AccountDisabledException)
            {
                _appHelper.Toast(AppResources.Account_Disabled, System.Drawing.Color.FromArgb(255, 0, 0));

            } else if (ex is AccountLockedException) {

                _appHelper.Toast(AppResources.Account_Locked, System.Drawing.Color.FromArgb(255, 0, 0));

            } else if(ex is EmailAlreadyExistsException) {

                _userDialogs.HideLoading();

                _appHelper.Toast(AppResources.Authentication_Email_Already_Exists, System.Drawing.Color.FromArgb(255, 0, 0));
            }
			else
			{
				base.HandleExceptions(ex);
			}
		}

    }


}
