using System;
using System.Collections.Generic;
using Refit;
using Bullytect.Core.Rest.Models.Response;
using Bullytect.Core.Rest.Models.Request;

namespace Bullytect.Core.Rest.Services
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
		IObservable<APIResponse<SocialMediaDTO>> SaveSocialMedia([Body] SaveSocialMediaDTO socialMedia);

		[Post("/children/{id}/social/save/all")]
		[Headers("Authorization: Bearer")]
        IObservable<APIResponse<IList<SocialMediaDTO>>> SaveAllSocialMedia([AliasAs("id")] string IdSon, [Body] IList<SocialMediaDTO> socialMedias);

        [Delete("/{idson}/social/delete/{idsocial}")]
		[Headers("Authorization: Bearer")]
		IObservable<APIResponse<SocialMediaDTO>> DeleteSocialMedia([AliasAs("idson")] string idson, [AliasAs("idsocial")] string idsocial);

    }
}
