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
    public interface IParentsRestService
    {

		[Get("/parents/self")]
		Task<APIResponse<ParentDTO>> GetSelfInformation();

		[Get("/parents/self/children")]
		Task<APIResponse<List<SonDTO>>> GetChildrenOfSelfParent();

		[Post("/parents/")]
		Task<APIResponse<ParentDTO>> registerParent([Body] RegisterParentDTO parent);

        [Post("/parents/self")]
        Task<APIResponse<ParentDTO>> updateSelfParent([Body] UpdateParentDTO parent);

    }
}
