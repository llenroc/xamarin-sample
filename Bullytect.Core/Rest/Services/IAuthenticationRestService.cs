using System;
using Refit;
using Bullytect.Core.Rest.Models.Response;
using Bullytect.Core.Rest.Models.Request;

namespace Bullytect.Core.Rest.Services
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
