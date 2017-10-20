using System;
using System.Collections.Generic;
using System.IO;
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

        public IObservable<APIResponse<IList<SocialMediaDTO>>> SaveAllSocialMedia(string IdSon, IList<SaveSocialMediaDTO> socialMedias)
        {
            return Observable.FromAsync(() => PostData<APIResponse<IList<SocialMediaDTO>>, IList<SaveSocialMediaDTO>>(ApiEndpoints.SAVE_ALL_SOCIAL_MEDIA.Replace(":id", IdSon), socialMedias));
        }

        public IObservable<APIResponse<SocialMediaDTO>> SaveSocialMedia(SaveSocialMediaDTO socialMedia)
        {
            return Observable.FromAsync(() => PostData<APIResponse<SocialMediaDTO>, SaveSocialMediaDTO>(ApiEndpoints.SAVE_SOCIAL_MEDIA, socialMedia));
        }

        public IObservable<APIResponse<ImageDTO>> UploadProfileImage(string id, Stream stream)
        {
            return Observable.FromAsync(() => PostStreamData<APIResponse<ImageDTO>>(ApiEndpoints.UPLOAD_SON_PROFILE_IMAGE.Replace(":id", id), "profile_image", stream));
        }

		public IObservable<APIResponse<string>> DeleteSonById(string id)
		{
            return Observable.FromAsync(() => DeleteData<APIResponse<string>>(ApiEndpoints.DELETE_SON_BY_ID.Replace(":id", id)));
		}

        public IObservable<APIResponse<SocialMediaActivityStatisticsDTO>> GetSocialMediaActivityStatistics(string id, int daysLimit)
        {
            return Observable.FromAsync(() => GetData<APIResponse<SocialMediaActivityStatisticsDTO>>(new Uri(ApiEndpoints.SOCIAL_ACTIVITY_STATISTICS.Replace(":id", id)).AttachParameters(new Dictionary<string, string>()
			{
                { "days-limit", daysLimit.ToString()}
			})));
        }

        public IObservable<APIResponse<SentimentAnalysisStatisticsDTO>> GetSentimentAnalysisStatistics(string id, int daysLimit)
        {
            return Observable.FromAsync(() => GetData<APIResponse<SentimentAnalysisStatisticsDTO>>(new Uri(ApiEndpoints.SENTIMENT_ANALYSIS_STATISTICS.Replace(":id", id)).AttachParameters(new Dictionary<string, string>()
			{
				{ "days-limit", daysLimit.ToString()}
			})));
        }

        public IObservable<APIResponse<CommunitiesStatisticsDTO>> GetCommunitiesStatistics(string id, int daysLimit)
        {
            return Observable.FromAsync(() => GetData<APIResponse<CommunitiesStatisticsDTO>>(new Uri(ApiEndpoints.COMMUNITIES_STATISTICS.Replace(":id", id)).AttachParameters(new Dictionary<string, string>()
			{
				{ "days-limit", daysLimit.ToString()}
			})));
        }

        public IObservable<APIResponse<DimensionsStatisticsDTO>> GetDimensionsStatistics(string id, int daysLimit)
        {
            return Observable.FromAsync(() => GetData<APIResponse<DimensionsStatisticsDTO>>(new Uri(ApiEndpoints.DIMENSIONS_STATISTICS.Replace(":id", id)).AttachParameters(new Dictionary<string, string>()
			{
				{ "days-limit", daysLimit.ToString()}
			})));
        }
    }
}
