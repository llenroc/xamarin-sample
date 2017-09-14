
namespace Bullytect.Core.Services.Impl
{
    using System;
    using System.Diagnostics;
    using System.Reactive.Linq;
    using Bullytect.Core.Config;
    using Bullytect.Core.Messages;
    using Bullytect.Core.OAuth.Providers.Facebook;
    using Bullytect.Core.OAuth.Services;
    using Bullytect.Rest.Models.Exceptions;
    using Bullytect.Rest.Models.Request;
    using Bullytect.Rest.Models.Response;
    using Bullytect.Rest.Services;
    using MvvmCross.Plugins.Messenger;
    using Refit;
    using Xamarin.Forms;

    public class AuthenticationServiceImpl: IAuthenticationService
    {
        readonly IAuthenticationRestService _authenticationRestService;
        readonly IMvxMessenger _mvxMessenger;

        public AuthenticationServiceImpl(IAuthenticationRestService authenticationRestService, IMvxMessenger mvxMessenger)
        {
            _authenticationRestService = authenticationRestService;
            _mvxMessenger = mvxMessenger;
            Debug.WriteLine(GetType().Name + " was created on context");
        }

        public IObservable<string> LogIn(string email, string password, Action<AuthenticationFailedException> errorHandler)
        {
            Debug.WriteLine(String.Format("Login with {0}/{1}", email, password));

            return _authenticationRestService.getAuthorizationToken(new JwtAuthenticationRequestDTO()
            {
                Email = email,
                Password = password
            }).Select(response => response.Data.Token).Catch<string, ApiException>(ex => {
                var response = ex.GetContentAs<APIResponse<string>>();
                errorHandler(new AuthenticationFailedException(response));
                return Observable.Return<string>(String.Empty);
            }).Do((accessToken) => {
                if (!String.IsNullOrEmpty(accessToken))
                {
                    Debug.WriteLine(String.Format("Access Token: {0} ", accessToken));
					_mvxMessenger.Publish(new AuthenticatedUserMessage(this));
				}
			}).Finally(() => {
                Debug.WriteLine("Log in finished ...");
            });
		}

        public IObservable<string> LoginToFacebook(){
            Debug.WriteLine("Log To Facebook ...");
            var oauthService = DependencyService.Get<IOAuth>();
            return oauthService.authenticate(new FacebookOAuth2())
                               .SelectMany(accessToken => _authenticationRestService.getAuthorizationTokenByFacebook(new JwtFacebookAuthenticationRequestDTO() { Token  =  accessToken }))
                               .Select(response => response.Data.Token)
							   .Do((accessToken) => {
								   if (!String.IsNullOrEmpty(accessToken))
								   {
									   Debug.WriteLine(String.Format("Access Token -> {0} ", accessToken));
                                       Debug.WriteLine("Notify Authentication Success");
									   _mvxMessenger.Publish(new AuthenticatedUserMessage(this));
								   }
							   })
							   .Finally(() => {
								   Debug.WriteLine("Log To Facebook finished ...");
							   }); 
        }

		public bool IsLoggedIn()
		{
            return Settings.AccessToken != null;
		}
    }
}
