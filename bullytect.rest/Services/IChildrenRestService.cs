using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bullytect.Rest.Models.Response;
using Bullytect.Rest.Models.Request;
using Refit;

namespace Bullytect.Rest.Services
{
    #pragma warning disable CS1701

    [Headers("Accept: application/json")]
	public interface IChildrenRestService
    {
		[Get("/children/{id}")]
        [Headers("Authorization: Bearer")]
		Task<APIResponse<SonDTO>> getSonById(string id);

		[Get("/children/{id}/social")]
        [Headers("Authorization: Bearer")]
		Task<APIResponse<List<SocialMediaDTO>>> GetAllSocialMediaBySonId(string id);

		[Get("/children/{id}/social/invalid")]
        [Headers("Authorization: Bearer")]
		Task<APIResponse<List<SocialMediaDTO>>> GetInvalidSocialMediaBySonId(string id);

		[Post("/children/social/save")]
        [Headers("Authorization: Bearer")]
		Task<APIResponse<ParentDTO>> SaveSocialMedia([Body] SaveSocialMediaDTO socialMedia);

    }
}
