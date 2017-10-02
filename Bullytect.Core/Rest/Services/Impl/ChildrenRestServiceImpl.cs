using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reactive.Linq;
using Bullytect.Core.Rest.Models.Request;
using Bullytect.Core.Rest.Models.Response;
using Bullytect.Core.Rest.Utils;

namespace Bullytect.Core.Rest.Services.Impl
{
    public class ChildrenRestServiceImpl: BaseRestServiceImpl,  IChildrenRestService
    {
        
        public ChildrenRestServiceImpl(HttpClient client): base(client){}

        public IObservable<APIResponse<SocialMediaDTO>> DeleteSocialMedia(string idson, string idsocial)
        {
            return Observable.FromAsync(() => DeleteData<APIResponse<SocialMediaDTO>>(ApiEndpoints.DELETE_SOCIAL_MEDIA
                                                                                      .Replace(":idson", idson).Replace(":idsocial", idsocial)));
        }

        public IObservable<APIResponse<IList<SocialMediaDTO>>> GetAllSocialMediaBySonId(string id)
        {
			return Observable.FromAsync(() => GetData<APIResponse<IList<SocialMediaDTO>>>(ApiEndpoints.GET_ALL_SOCIAL_MEDIA_BY_SON_ID.Replace(":id", id)));
        }

        public IObservable<APIResponse<IList<SocialMediaDTO>>> GetInvalidSocialMediaBySonId(string id)
        {
            return Observable.FromAsync(() => GetData<APIResponse<IList<SocialMediaDTO>>>(ApiEndpoints.GET_INVALID_SOCIAL_MEDIA_BY_SON_ID.Replace(":id", id)));
        }

        public IObservable<APIResponse<SonDTO>> GetSonById(string id)
        {
            return Observable.FromAsync(() => GetData<APIResponse<SonDTO>>(ApiEndpoints.GET_SON_BY_ID.Replace(":id", id)));
        }

        public IObservable<APIResponse<IList<SocialMediaDTO>>> SaveAllSocialMedia(string IdSon, IList<SocialMediaDTO> socialMedias)
        {
            return Observable.FromAsync(() => PostData<APIResponse<IList<SocialMediaDTO>>, IList<SocialMediaDTO>>(ApiEndpoints.SAVE_ALL_SOCIAL_MEDIA.Replace(":id", IdSon), socialMedias));
        }

        public IObservable<APIResponse<SocialMediaDTO>> SaveSocialMedia(SaveSocialMediaDTO socialMedia)
        {
            return Observable.FromAsync(() => PostData<APIResponse<SocialMediaDTO>, SaveSocialMediaDTO>(ApiEndpoints.SAVE_SOCIAL_MEDIA, socialMedia));
        }
    }
}
