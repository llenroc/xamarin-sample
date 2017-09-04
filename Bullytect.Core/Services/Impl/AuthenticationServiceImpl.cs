
namespace Bullytect.Core.Services.Impl
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Bullytect.Rest.Models.Request;
    using Bullytect.Rest.Services;

    public class AuthenticationServiceImpl: IAuthenticationService
    {
        readonly IAuthenticationRestService _authenticationRestService;

        public AuthenticationServiceImpl(IAuthenticationRestService authenticationRestService)
        {
            _authenticationRestService = authenticationRestService;
            Debug.WriteLine(GetType().Name + " was created on context");
        }

        public Task<string> LogIn(string email, string password)
        {
            Debug.WriteLine(String.Format("Login with {0}/{1}", email, password));

            return _authenticationRestService.getAuthorizationToken(new JwtAuthenticationRequestDTO()
            {
                Email = email,
                Password = password
            }).ContinueWith(t => t.Result.Data.Token, TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}
