using System;
using System.Threading.Tasks;
using Bullytect.Rest.Models.Request;
using Bullytect.Rest.Models.Response;
using Refit;

namespace Bullytect.Rest.Services
{

    #pragma warning disable CS1701

	[Headers("Accept: application/json")]
    public interface IAuthenticationRestService
    {

		[Post("/")]
		Task<APIResponse<JwtAuthenticationResponseDTO>> getAuthorizationToken([Body] JwtAuthenticationRequestDTO authorizationRequest);

    }
}
