using System;
using System.Collections.Generic;
using Bullytect.Core.Rest.Models.Response;
using Bullytect.Core.Rest.Models.Request;
using System.IO;

namespace Bullytect.Core.Rest.Services
{
    #pragma warning disable CS1701

	public interface IChildrenRestService
    {

        IObservable<APIResponse<SonDTO>> GetSonById(string id);

        IObservable<APIResponse<IList<SocialMediaDTO>>> GetAllSocialMediaBySonId(string id);

		IObservable<APIResponse<IList<SocialMediaDTO>>> GetInvalidSocialMediaBySonId(string id);

		IObservable<APIResponse<SocialMediaDTO>> SaveSocialMedia(SaveSocialMediaDTO socialMedia);

        IObservable<APIResponse<IList<SocialMediaDTO>>> SaveAllSocialMedia(string IdSon, IList<SocialMediaDTO> socialMedias);

		IObservable<APIResponse<SocialMediaDTO>> DeleteSocialMedia(string idson, string idsocial);

        IObservable<APIResponse<ImageDTO>> UploadProfileImage(string id, Stream stream);
 
    }
}
