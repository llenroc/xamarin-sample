
namespace Bullytect.Core.Services.Impl
{
    using System;
    using System.Diagnostics;
    using System.Reactive.Linq;
    using Bullytect.Core.Config;
    using Bullytect.Rest.Models.Exceptions;
    using Bullytect.Rest.Models.Request;
    using Bullytect.Rest.Models.Response;
    using Bullytect.Rest.Services;
    using Refit;

    public class AuthenticationServiceImpl: IAuthenticationService
    {
        readonly IAuthenticationRestService _authenticationRestService;

        public AuthenticationServiceImpl(IAuthenticationRestService authenticationRestService)
        {
            _authenticationRestService = authenticationRestService;
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
            }).Finally(() => {
                Debug.WriteLine("Log in finished ...");
            });
		}

		public bool IsLoggedIn()
		{
            return Settings.AccessToken != null;
		}
    }
}
