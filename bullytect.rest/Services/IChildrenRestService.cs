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
        IObservable<APIResponse<SonDTO>> GetSonById(string id);

		[Get("/children/{id}/social")]
        [Headers("Authorization: Bearer")]
        IObservable<APIResponse<IList<SocialMediaDTO>>> GetAllSocialMediaBySonId(string id);

		[Get("/children/{id}/social/invalid")]
        [Headers("Authorization: Bearer")]
		IObservable<APIResponse<IList<SocialMediaDTO>>> GetInvalidSocialMediaBySonId(string id);

		[Post("/children/social/save")]
        [Headers("Authorization: Bearer")]
		IObservable<APIResponse<ParentDTO>> SaveSocialMedia([Body] SaveSocialMediaDTO socialMedia);

    }
}
