using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bullytect.Rest.Models.Request;
using Bullytect.Rest.Models.Response;
using Refit;

namespace Bullytect.Rest.Services
{

    #pragma warning disable CS1701

	[Headers("Accept: application/json")]
    public interface IParentsService
    {

		[Get("/self")]
		Task<APIResponse<ParentDTO>> GetSelfInformation();

		[Get("/self/children")]
		Task<APIResponse<List<SonDTO>>> GetChildrenOfSelfParent();

		[Post("/")]
		Task<APIResponse<ParentDTO>> registerParent([Body] JwtAuthenticationRequestDTO parent);

    }
}
