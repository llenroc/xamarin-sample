using System;
using System.Threading.Tasks;
using bullytect.config;

namespace bullytect.ViewModels
{
    public class AuthenticationViewModel: BaseViewModel
    {

		#region Properties

		string _authenticationStatus;

		public string AuthenticationStatus
        {
            get
            {
                return _authenticationStatus;
            }
            set
            {
                SetPropertyChanged(ref _authenticationStatus, value);
            }
        }

        #endregion

        async public Task Authenticate()
        {

        }

		async public Task LogOut(bool clearCookies)
		{
            
			//App.Instance.CurrentAthlete = null;

			Settings.AccessToken = null;

			if (clearCookies)
			{
				
			}
		}
	}
}
