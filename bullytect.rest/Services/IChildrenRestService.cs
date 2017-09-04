using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bullytect.Rest.Models.Response;
using Refit;

namespace Bullytect.Rest.Services
{
    #pragma warning disable CS1701

    [Headers("Accept: application/json")]
	public interface IChildrenService
    {
		[Get("/{id}")]
		Task<APIResponse<SonDTO>> getSonById(string id);

		[Get("/{id}/social")]
		Task<APIResponse<List<SocialMediaDTO>>> GetAllSocialMediaBySonId(string id);

		[Get("/{id}/social/invalid")]
		Task<APIResponse<List<SocialMediaDTO>>> GetInvalidSocialMediaBySonId(string id);

		[Post("/social/save")]
		Task<APIResponse<ParentDTO>> SaveSocialMedia([Body] SaveSocialMediaDTO socialMedia);

    }
}
