using System;
using System.Net.Http;
using System.Reactive.Linq;
using Bullytect.Core.Rest.Models.Request;
using Bullytect.Core.Rest.Models.Response;
using Bullytect.Core.Rest.Utils;

namespace Bullytect.Core.Rest.Services.Impl
{
    public class AuthenticationRestServiceImpl: BaseRestServiceImpl,  IAuthenticationRestService
    {

        public AuthenticationRestServiceImpl(HttpClient client): base(client)
        {
        }

        public IObservable<APIResponse<JwtAuthenticationResponseDTO>> getAuthorizationToken(JwtAuthenticationRequestDTO authorizationRequest)
        {
            return Observable.FromAsync(() => PostData<APIResponse<JwtAuthenticationResponseDTO>, JwtAuthenticationRequestDTO>(ApiEndpoints.GET_AUTHORIZATION_TOKEN, authorizationRequest));
        }

        public IObservable<APIResponse<JwtAuthenticationResponseDTO>> getAuthorizationTokenByFacebook(JwtSocialAuthenticationRequestDTO authorizationRequest)
        {
            return Observable.FromAsync(() => PostData<APIResponse<JwtAuthenticationResponseDTO>, JwtSocialAuthenticationRequestDTO>(ApiEndpoints.GET_AUTHORIZATION_TOKEN_BY_FACEBOOK, authorizationRequest));
        }

        public IObservable<APIResponse<JwtAuthenticationResponseDTO>> getAuthorizationTokenByGoogle(JwtSocialAuthenticationRequestDTO authorizationRequest)
        {
            return Observable.FromAsync(() => PostData<APIResponse<JwtAuthenticationResponseDTO>, JwtSocialAuthenticationRequestDTO>(ApiEndpoints.GET_AUTHORIZATION_TOKEN_BY_GOOGLE, authorizationRequest));
        }
    }
}
