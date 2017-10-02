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

namespace Bullytect.Core.ViewModels
{

    public class AuthenticationViewModel : BaseViewModel
    {
        readonly IAuthenticationService _authenticationService;

        public AuthenticationViewModel(IAuthenticationService authenticationService,
                                       IUserDialogs userDialogs, IMvxMessenger mvxMessenger): base(userDialogs, mvxMessenger)
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

					var oauthService = DependencyService.Get<IOAuth>();
					return oauthService
						.authenticate(new FacebookOAuth2())
                        .Where(AccessToken => !string.IsNullOrEmpty(AccessToken))
						.Do(_ => _userDialogs.ShowLoading(AppResources.Login_Authenticating))
						.SelectMany(accessToken => authenticationService.LoginWithFacebook(accessToken))
						.Do(_ => _userDialogs.HideLoading());
				});


			LoginWithFacebookCommand.Subscribe(HandleAuthSuccess);

			LoginWithFacebookCommand.ThrownExceptions.Subscribe(HandleExceptions);
        }

		#region Properties
      

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


		#region commands

        public ReactiveCommand<Unit, string> LoginCommand { get; protected set; }

        public ReactiveCommand<Unit, string> LoginWithFacebookCommand { get; protected set; }

        public ICommand GoToPasswordRecoveryCommand => new MvxCommand(() => ShowViewModel<PasswordRecoveryViewModel>());

		#endregion

		protected override void HandleExceptions(Exception ex)
		{

			if (ex is AuthenticationFailedException)
			{
				var toastConfig = new ToastConfig(AppResources.Login_Failed);
				toastConfig.SetDuration(3000);
				toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(255, 0, 0));
				_userDialogs.Toast(toastConfig);
			}
			else
			{
				base.HandleExceptions(ex);
			}
		}

        void HandleAuthSuccess(string jwtToken) {
			Debug.WriteLine("JWT Token -> " + jwtToken);
			_userDialogs.ShowSuccess(AppResources.Login_Success);
			ShowViewModel<HomeViewModel>();
        }

		
        async Task LogOut(bool clearCookies)
        {

            Settings.AccessToken = null;

            if (clearCookies)
            {

            }
        }

       

    }


}
