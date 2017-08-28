using System;
using System.Threading.Tasks;
using bullytect.rest.models.request;
using bullytect.rest.models.response;
using Refit;

namespace bullytect.rest.services
{
    public interface IAuthenticationService
    {

        #pragma warning disable CS1701

		[Post("/")]
		Task<APIResponse<JwtAuthenticationResponseDTO>> getAuthorizationToken([Body] JwtAuthenticationRequestDTO authorizationRequest);

    }
}
