
namespace Bullytect.Core.Services.Impl
{
    using System;
    using System.Diagnostics;
    using System.Reactive.Linq;
    using Bullytect.Core.Config;
    using Bullytect.Core.Messages;
    using Bullytect.Rest.Models.Exceptions;
    using Bullytect.Rest.Models.Request;
    using Bullytect.Rest.Models.Response;
    using Bullytect.Rest.Services;
    using MvvmCross.Plugins.Messenger;
    using Refit;

    public class AuthenticationServiceImpl: BaseService, IAuthenticationService
    {
        readonly IAuthenticationRestService _authenticationRestService;
        readonly IMvxMessenger _mvxMessenger;

        public AuthenticationServiceImpl(IAuthenticationRestService authenticationRestService, IMvxMessenger mvxMessenger)
        {
            _authenticationRestService = authenticationRestService;
            _mvxMessenger = mvxMessenger;
            Debug.WriteLine(GetType().Name + " was created on context");
        }

        public IObservable<string> LogIn(string email, string password)
        {
            Debug.WriteLine(String.Format("Login with {0}/{1}", email, password));

            var observable =  _authenticationRestService.getAuthorizationToken(new JwtAuthenticationRequestDTO()
            {
                Email = email,
                Password = password
            }).Select(response => response.Data.Token).Catch<string, ApiException>(ex => {
                var response = ex.GetContentAs<APIResponse<string>>();
                return Observable.Throw<string>(new AuthenticationFailedException(response));
            }).Do((jwtToken) => {
                if (!String.IsNullOrEmpty(jwtToken))
                {
                    Debug.WriteLine(String.Format("Jwt Access Token: {0} ", jwtToken));
                    Settings.AccessToken = jwtToken;
                    _mvxMessenger.Publish(new AuthenticatedUserMessage(this) {
                        JwtToken = jwtToken
                    });
				}
			}).Finally(() => {
                Debug.WriteLine("Log in finished ...");
            });


            return operationDecorator(observable);
		}

        public IObservable<string> LoginWithFacebook(string accessToken){
            Debug.WriteLine("Log To Facebook ...");

            var observable =  _authenticationRestService
                    .getAuthorizationTokenByFacebook(new JwtFacebookAuthenticationRequestDTO() { Token = accessToken })
                    .Select(response => response.Data.Token)
					.Do((jwtToken) => {
                        if (!String.IsNullOrEmpty(jwtToken))
                        {
                            Debug.WriteLine(String.Format("Jwt Access Token -> {0} ", jwtToken));
							Debug.WriteLine("Notify Authentication Success");
                            Settings.AccessToken = jwtToken;
                            _mvxMessenger.Publish(new AuthenticatedUserMessage(this) {
                                JwtToken = jwtToken
                            });
                        }
                    })
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
