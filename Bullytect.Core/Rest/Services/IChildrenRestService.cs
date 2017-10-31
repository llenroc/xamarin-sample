﻿using System;
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

        IObservable<APIResponse<IList<SocialMediaDTO>>> SaveAllSocialMedia(string IdSon, IList<SaveSocialMediaDTO> socialMedias);

		IObservable<APIResponse<SocialMediaDTO>> DeleteSocialMedia(string idson, string idsocial);

        IObservable<APIResponse<ImageDTO>> UploadProfileImage(string id, Stream stream);

        IObservable<APIResponse<String>> DeleteSonById(string id);

        IObservable<APIResponse<SocialMediaActivityStatisticsDTO>> GetSocialMediaActivityStatistics(string id, int daysLimit);

        IObservable<APIResponse<SentimentAnalysisStatisticsDTO>> GetSentimentAnalysisStatistics(string id, int daysLimit);

        IObservable<APIResponse<CommunitiesStatisticsDTO>> GetCommunitiesStatistics(string id, int daysLimit);

        IObservable<APIResponse<DimensionsStatisticsDTO>> GetDimensionsStatistics(string id, int daysLimit);

        IObservable<APIResponse<CommentsAnalyzedStatisticsDTO>> GetCommentsAnalyzedStatistics(String[] Ids, int daysLimit);

        IObservable<APIResponse<SocialMediaLikesStatisticsDTO>> GetSocialMediaLikesStatistics(String[] Ids, int daysLimit);

        IObservable<APIResponse<AlertsStatisticsDTO>> GetAlertsStatistics(String[] Ids, int daysLimit);

        IObservable<APIResponse<MostActiveFriendsDTO>> GetMostActiveFriends(String[] Ids, int daysLimit);

        IObservable<APIResponse<NewFriendsDTO>> GetNewFriends(String[] Ids, int daysLimit);
    }
}
