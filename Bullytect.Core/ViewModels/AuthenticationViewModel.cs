using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Bullytect.Core.Commands;
using Bullytect.Core.Config;
using Bullytect.Core.I18N;
using Bullytect.Core.Messages;
using Bullytect.Core.Services;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Plugins.Validation;

namespace Bullytect.Core.ViewModels
{
    public enum AuthenticationStatusEnum {
        LOADING, LOGIN_SUCESS, LOGIN_FAILED
    }

    public class AuthenticationViewModel : BaseViewModel
    {

        readonly IValidator _validator;
        readonly IAuthenticationService _authenticationService;
        readonly IMvxToastService _toastService;
        readonly IUserDialogs _userDialogs;
        readonly IMvxMessenger _mvxMessenger;

        public AuthenticationViewModel(IValidator validator, IAuthenticationService authenticationService, IMvxToastService toastService,
                                      IUserDialogs userDialogs, IMvxMessenger mvxMessenger)
        {
            _validator = validator;
            _authenticationService = authenticationService;
            _toastService = toastService;
            _userDialogs = userDialogs;
            _mvxMessenger = mvxMessenger;

        }

        #region Properties

        AuthenticationStatusEnum _status;

        public AuthenticationStatusEnum Status
        {
            get => _status;
            set
            {
                _status = value;
                RaisePropertyChanged(() => Status);
            }
        }

        string _email;

        [Required("{0} is required")]
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        string _password;

        [Required("{0} is required")]
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

		#endregion



		#region commands


		RelayCommand _logInCommand;

        public RelayCommand LoginCommand =>
            _logInCommand ?? (_logInCommand = new RelayCommand(
                           async () =>
                           {
                               using (new Busy(this))
                               {

                                   try
                                   {
                                        
                                       Status = AuthenticationStatusEnum.LOADING;
                                       
                                       _userDialogs.ShowLoading(AppResources.Login_Authenticating, MaskType.Black);

                                       // get access token
                                       string accessToken = await _authenticationService.LogIn(_email, _password);

                                       Debug.WriteLine(String.Format("Access Token:t {0} ", accessToken));

                                       // save token on preferences.
                                       Settings.AccessToken = accessToken;

                                       Status = AuthenticationStatusEnum.LOGIN_SUCESS;

                                        _mvxMessenger.Publish(new AuthenticatedUserMessage(this));

                                   }
                                   catch (Exception ex)
                                   {

                                       _userDialogs.HideLoading();
									   var toastConfig = new ToastConfig("Toasting...");
									   toastConfig.SetDuration(3000);
									   toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(12, 131, 193));
									   _userDialogs.Toast(toastConfig);

                                       _mvxMessenger.Publish(new ExceptionOcurredMessage(this, ex));

                                       Status = AuthenticationStatusEnum.LOGIN_FAILED;
                                   }

                               }

                           }, () =>
                           {
                               var errors = _validator.Validate(this);
                               return !errors.IsValid || IsBusy;
                           }));
		

        async Task LogOut(bool clearCookies)
        {

            Settings.AccessToken = null;

            if (clearCookies)
            {

            }
        }

        #endregion

    }


}
