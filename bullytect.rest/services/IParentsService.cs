using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using bullytect.rest.models.request;
using bullytect.rest.models.response;
using Refit;

namespace bullytect.rest.services
{
    public interface IParentsService
    {

        #pragma warning disable CS1701

		[Get("/self")]
		Task<APIResponse<ParentDTO>> GetSelfInformation();

		[Get("/self/children")]
		Task<APIResponse<List<SonDTO>>> GetChildrenOfSelfParent();

		[Post("/")]
		Task<APIResponse<ParentDTO>> registerParent([Body] JwtAuthenticationRequestDTO parent);

    }
}
