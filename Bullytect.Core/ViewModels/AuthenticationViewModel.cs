using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Bullytect.Core.Config;
using Bullytect.Core.Services;
using Bullytect.Utils.Helpers;
using Xamarin.Forms;

namespace Bullytect.Core.ViewModels
{
    public enum AuthenticationStatusEnum {
        LOADING, LOGIN_SUCESS, LOGIN_FAILED
    }

    public class AuthenticationViewModel : BaseViewModel
    {

        readonly IAuthenticationService _authenticationService;

        public AuthenticationViewModel(IAuthenticationService authenticationService)
        {
            this._authenticationService = authenticationService;

        }

        #region Properties

        AuthenticationStatusEnum _status;

        public AuthenticationStatusEnum Status
        {
            get => _status;
            set {
                _status = value;
                RaisePropertyChanged(() => Status);
            }
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

        async public Task logIn()
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
                catch (Exception ex)
                {
                    MessagingCenter.Send(new object(), EventTypeName.EXCEPTION_OCCURRED, ex);
                    Status = AuthenticationStatusEnum.LOGIN_FAILED;
                }

            }
        }

        async public Task LogOut(bool clearCookies)
        {
            
            Settings.AccessToken = null;

            if (clearCookies)
            {

            }
        }

	}


}
