using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using bullytect.rest.models.request;
using bullytect.rest.models.response;
using Refit;

namespace bullytect.rest.services
{
    public interface IChildrenService
    {

        #pragma warning disable CS1701

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
