using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Bullytect.Core.Commands;
using Bullytect.Core.Config;
using Bullytect.Core.Services;
using Bullytect.Utils.Helpers;
using MvvmCross.Plugins.Validation;
using Refit;
using Xamarin.Forms;

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

        public AuthenticationViewModel(IValidator validator, IAuthenticationService authenticationService, IMvxToastService toastService)
        {
            _validator = validator;
            _authenticationService = authenticationService;
            _toastService = toastService;

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
                                       // get access token
                                       string accessToken = await _authenticationService.LogIn(_email, _password);

                                       Debug.WriteLine(String.Format("Access Token:t {0} ", accessToken));

                                       // save token on preferences.
                                       Settings.AccessToken = accessToken;

                                       Status = AuthenticationStatusEnum.LOGIN_SUCESS;

                                   }
                                   catch (ApiException ex)
                                   {
                                       Debug.WriteLine("Api Exception ...");
                                   }
                                   catch (Exception ex)
                                   {

                                       MessagingCenter.Send(new object(), EventTypeName.EXCEPTION_OCCURRED, ex);
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
