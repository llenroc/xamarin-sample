using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Bullytect.Core.Config;
using Bullytect.Core.I18N;
using Bullytect.Core.Messages;
using Bullytect.Core.Services;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{

    public class AuthenticationViewModel : BaseViewModel
    {
        readonly IAuthenticationService _authenticationService;
        readonly IUserDialogs _userDialogs;
        readonly IMvxMessenger _mvxMessenger;

        public AuthenticationViewModel(IAuthenticationService authenticationService,
                                      IUserDialogs userDialogs, IMvxMessenger mvxMessenger)
        {
            _authenticationService = authenticationService;
            _userDialogs = userDialogs;
            _mvxMessenger = mvxMessenger;


            // Create Reactive Commands
            LoginCommand = ReactiveCommand.CreateFromObservable<string>(
                LoginObservable, this.WhenAnyValue(x => x.Email, x => x.Password, (email, pass) =>
                    !String.IsNullOrWhiteSpace(email) && !String.IsNullOrWhiteSpace(pass)
                           && email.Length >= 3 && pass.Length >= 6).DistinctUntilChanged());


			LoginCommand.IsExecuting.Subscribe((isLoading) => {
				if (isLoading)
				{
                    _userDialogs.ShowLoading(AppResources.Login_Authenticating ,MaskType.Black);
                } else {
                    _userDialogs.HideLoading();
                }
			});


            LoginCommand.ThrownExceptions.Subscribe((ex) =>
            {
                Debug.WriteLine(String.Format("Exception: {0}", ex.ToString()));
                _mvxMessenger.Publish(new ExceptionOcurredMessage(this, ex));
            });


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


        public ReactiveCommand LoginCommand { get; protected set; }


        #endregion


		IObservable<string> LoginObservable() {
            
            return _authenticationService.LogIn(_email, _password, (authFailed) =>
            {
                Debug.WriteLine(String.Format("Response Message: {0}", authFailed.Response.Data));
                var toastConfig = new ToastConfig(AppResources.Login_Failed);
                toastConfig.SetDuration(3000);
                toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(12, 131, 193));
                _userDialogs.Toast(toastConfig);
            }).Do((accessToken) => {
                if (!String.IsNullOrEmpty(accessToken))
                {
                    Debug.WriteLine(String.Format("Access Token: {0} ", accessToken));
                    _mvxMessenger.Publish(new AuthenticatedUserMessage(this));
                }
            });

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
