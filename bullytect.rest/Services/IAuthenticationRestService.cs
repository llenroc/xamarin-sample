using System;
using Bullytect.Rest.Models.Request;
using Bullytect.Rest.Models.Response;
using Refit;

namespace Bullytect.Rest.Services
{

    #pragma warning disable CS1701

	[Headers("Accept: application/json")]
    public interface IAuthenticationRestService
    {

		[Post("/parents/auth/")]
		IObservable<APIResponse<JwtAuthenticationResponseDTO>> getAuthorizationToken([Body] JwtAuthenticationRequestDTO authorizationRequest);

        [Post("/parents/auth/facebook")]
        IObservable<APIResponse<JwtAuthenticationResponseDTO>> getAuthorizationTokenByFacebook([Body] JwtFacebookAuthenticationRequestDTO authorizationRequest);


    }
}
