
namespace Bullytect.Core.Services.Impl
{
    using System;
    using System.Diagnostics;
    using System.Reactive.Linq;
    using Bullytect.Core.Config;
    using MvvmCross.Plugins.Messenger;
    using Bullytect.Core.Rest.Services;
    using Bullytect.Core.Rest.Models.Request;

    public class AuthenticationServiceImpl: BaseService, IAuthenticationService
    {
        readonly IAuthenticationRestService _authenticationRestService;
        readonly IMvxMessenger _mvxMessenger;
        readonly INotificationService _notificationService;

        public AuthenticationServiceImpl(IAuthenticationRestService authenticationRestService, 
                                         IMvxMessenger mvxMessenger, INotificationService notificationService)
        {
            _authenticationRestService = authenticationRestService;
            _mvxMessenger = mvxMessenger;
            _notificationService = notificationService;
            Debug.WriteLine(GetType().Name + " was created on context");
        }

        public IObservable<string> LogIn(string email, string password)
        {
            Debug.WriteLine(String.Format("Login with {0}/{1}", email, password));

            var observable =  _authenticationRestService.getAuthorizationToken(new JwtAuthenticationRequestDTO()
            {
                Email = email,
                Password = password
            })
            .Select(response => response.Data.Token)
            .Finally(() => {
                Debug.WriteLine("Log in finished ...");
            });


            return operationDecorator(observable);
		}

        public IObservable<string> LoginWithFacebook(string accessToken){
            Debug.WriteLine("Log To Facebook ...");

            var observable =  _authenticationRestService
                    .getAuthorizationTokenByFacebook(new JwtFacebookAuthenticationRequestDTO() { Token = accessToken })
                    .Select(response => response.Data.Token)
                    .Finally(() => {
                        Debug.WriteLine("Log To Facebook finished ...");
                    });

            return operationDecorator(observable);
        }

		public bool IsLoggedIn()
		{
            return Settings.AccessToken != null;
		}
    }
}
