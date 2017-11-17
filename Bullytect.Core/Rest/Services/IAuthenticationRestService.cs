using System;
using Bullytect.Core.Rest.Models.Response;
using Bullytect.Core.Rest.Models.Request;

namespace Bullytect.Core.Rest.Services
{

    #pragma warning disable CS1701

    public interface IAuthenticationRestService
    {
		IObservable<APIResponse<JwtAuthenticationResponseDTO>> getAuthorizationToken(JwtAuthenticationRequestDTO authorizationRequest);
        IObservable<APIResponse<JwtAuthenticationResponseDTO>> getAuthorizationTokenByFacebook(JwtSocialAuthenticationRequestDTO authorizationRequest);
        IObservable<APIResponse<JwtAuthenticationResponseDTO>> getAuthorizationTokenByGoogle(JwtSocialAuthenticationRequestDTO authorizationRequest);
    }
}
