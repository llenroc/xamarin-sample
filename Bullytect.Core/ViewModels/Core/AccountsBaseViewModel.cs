using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Config;
using Bullytect.Core.Exceptions;
using Bullytect.Core.Helpers;
using Bullytect.Core.I18N;
using Bullytect.Core.OAuth.Providers.Facebook;
using Bullytect.Core.OAuth.Providers.Google;
using Bullytect.Core.OAuth.Services;
using Bullytect.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

namespace Bullytect.Core.ViewModels.Core
{
    public class AccountsBaseViewModel : BaseViewModel
    {

        protected readonly IOAuthService _oAuthService;
        protected readonly INotificationService _notificationService;
        protected readonly IAuthenticationService _authenticationService;

        public AccountsBaseViewModel(IUserDialogs userDialogs,
                                     IMvxMessenger mvxMessenger, AppHelper appHelper,
                                     IOAuthService oauthService, INotificationService notificationService,
                                     IAuthenticationService authenticationService)
            : base(userDialogs, mvxMessenger, appHelper)
        {

            _oAuthService = oauthService;
            _notificationService = notificationService;
            _authenticationService = authenticationService;

            // Create Reactive Commands
            LoginWithFacebookCommand = ReactiveCommand.CreateFromObservable<Unit, string>(
                (param) =>
                {

                    ResetCommonProps();

                    return oauthService
                            .Authenticate(new ParentFacebookOAuth2())
                            .Do(AuthDict =>
                            {
                                if (!AuthDict.ContainsKey("access_token") || string.IsNullOrEmpty(AuthDict["access_token"]))
                                    throw new OAuthInvalidAccessTokenException();

                                HandleIsExecuting(true, AppResources.Login_Authenticating_Facebook, FontAwesomeFont.Facebook);

                            })
                            .SelectMany(AuthDict => authenticationService.LoginWithFacebook(AuthDict["access_token"]));

                });


            LoginWithFacebookCommand.Subscribe(HandleAuthSuccess);


            LoginWithFacebookCommand.ThrownExceptions.Subscribe(HandleExceptions);


            LoginWithGoogleCommand = ReactiveCommand.CreateFromObservable<Unit, string>(
                (param) =>
                {

                    ResetCommonProps();

                    return oauthService
                            .Authenticate(new ParentGoogleOAuth2())
                            .Do(AuthDict =>
                            {
                                if (!AuthDict.ContainsKey("access_token") || string.IsNullOrEmpty(AuthDict["access_token"]))
                                    throw new OAuthInvalidAccessTokenException();

                                HandleIsExecuting(true, AppResources.Login_Authenticating_Google, FontAwesomeFont.Google);
                            })
                            .SelectMany(AuthDict => authenticationService.LoginWithGoogle(AuthDict["access_token"]));

                });

            LoginWithGoogleCommand.Subscribe(HandleAuthSuccess);

            LoginWithGoogleCommand.ThrownExceptions.Subscribe(HandleExceptions);
        }


        #region commands

        public ReactiveCommand<Unit, string> LoginWithFacebookCommand { get; protected set; }

        public ReactiveCommand<Unit, string> LoginWithGoogleCommand { get; protected set; }

        public ICommand GoToLoginCommand => new MvxCommand(() => ShowViewModel<AuthenticationViewModel>(new AuthenticationViewModel.AuthenticationParameter()
        {
            ReasonForAuthentication = AuthenticationViewModel.NORMAL_AUTHENTICATION
        }));

        #endregion


        #region methods

        protected void HandleAuthSuccess(string jwtToken)
        {
            Debug.WriteLine("JWT Token -> " + jwtToken);
            Settings.AccessToken = jwtToken;
            // Subscribe Device For Push Notifications
            _notificationService.subscribeDevice().Subscribe(device => {
                Settings.Current.DeviceRegistered = true;
                Debug.WriteLine(String.Format("Device Saved: {0}", device.ToString()));
            });
            _userDialogs.ShowSuccess(AppResources.Login_Success);
            ShowViewModel<HomeViewModel>();
        }

        #endregion

    }
}
