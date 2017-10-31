using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using AutoMapper;
using Bullytect.Core.Config;
using Bullytect.Core.Rest.Models.Response;
using Bullytect.Core.Rest.Services;
using Bullytect.Core.ViewModels.Core.Models;
using static Bullytect.Core.Rest.Models.Response.MostActiveFriendsDTO;

namespace Bullytect.Core.Services.Impl
{
    public class StatisticsServiceImpl : BaseService, IStatisticsService
    {

        readonly IChildrenRestService _childrenRestService;

        public StatisticsServiceImpl(IChildrenRestService childrenRestService){
            _childrenRestService = childrenRestService;
        }

        public IObservable<ChartModel> GetAlertsStatistics()
        {

			Debug.WriteLine("Get Alerts Statistics");

			var observable = _childrenRestService
                .GetAlertsStatistics(
					Ids: String.IsNullOrEmpty(Settings.Current.FilteredSonCategories) ?
						new string[] { } : Settings.Current.FilteredSonCategories.Split(','),
					daysAgo: Settings.Current.TimeInterval)
				.Select(response => response.Data)
				.Select((AlertsStatisticsDTO data) => Mapper.Map<AlertsStatisticsDTO, ChartModel>(data))
				.Finally(() => {
					Debug.WriteLine("Get Alerts Statistics finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<ChartModel> GetCommentsStatistics()
        {
			Debug.WriteLine("Get Comments Statistics");
			
			var observable = _childrenRestService
                .GetCommentsStatistics(
					Ids: String.IsNullOrEmpty(Settings.Current.FilteredSonCategories) ?
						new string[] { } : Settings.Current.FilteredSonCategories.Split(','),
                    daysAgo: Settings.Current.TimeInterval)
				.Select(response => response.Data)
                .Select((CommentsStatisticsDTO data) => Mapper.Map<CommentsStatisticsDTO, ChartModel>(data))
				.Finally(() => {
					Debug.WriteLine("Get Comments Statistics finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<ChartModel> GetCommunitiesStatistics(string id)
        {
			Debug.WriteLine("Get Communities Statistics");

			var observable = _childrenRestService
                .GetCommunitiesStatistics(id, Settings.Current.SonStatisticsTimeInterval)
                .Select(response => response.Data)
				.Select((CommunitiesStatisticsDTO data) => Mapper.Map<CommunitiesStatisticsDTO, ChartModel>(data))
				.Finally(() => {
					Debug.WriteLine("Get Communities Statistics finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<ChartModel> GetDimensionsStatistics(string id)
        {
			Debug.WriteLine("Get Dimensions Statistics");

			var observable = _childrenRestService
                .GetDimensionsStatistics(id, Settings.Current.SonStatisticsTimeInterval)
				.Select(response => response.Data)
				.Select((DimensionsStatisticsDTO data) => Mapper.Map<DimensionsStatisticsDTO, ChartModel>(data))
				.Finally(() => {
					Debug.WriteLine("Get Dimensions Statistics finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<IList<UserListModel>> GetMostActiveFriends()
        {
			Debug.WriteLine("Get Most Active Friends");

			var observable = _childrenRestService
				.GetMostActiveFriends(Ids: String.IsNullOrEmpty(Settings.Current.FilteredSonCategories) ?
						new string[] { } : Settings.Current.FilteredSonCategories.Split(','),
					daysAgo: Settings.Current.TimeInterval)
                .Select(response => response.Data.Users)
                .Select((IList<MostActiveFriendsDTO.UserDTO> data) => Mapper.Map<IList<MostActiveFriendsDTO.UserDTO>, IList<UserListModel>>(data))
				.Finally(() => {
					Debug.WriteLine("Get Most Active Friends finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<IList<UserListModel>> GetNewFriends()
        {
			Debug.WriteLine("Get New Friends");

			var observable = _childrenRestService
                .GetNewFriends(Ids: String.IsNullOrEmpty(Settings.Current.FilteredSonCategories) ?
						new string[] { } : Settings.Current.FilteredSonCategories.Split(','),
                               daysAgo: Settings.Current.TimeInterval)
                .Select(response => response.Data.Users)
                .Select((IList<NewFriendsDTO.UserDTO> data) => Mapper.Map<IList<NewFriendsDTO.UserDTO>, IList<UserListModel>>(data))
				.Finally(() => {
					Debug.WriteLine("Get New Friends finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<ChartModel> GetSentimentAnalysisStatistics(string id)
        {
			Debug.WriteLine("Get Sentiment Analysis Statistics");

			var observable = _childrenRestService
                .GetSentimentAnalysisStatistics(id, Settings.Current.SonStatisticsTimeInterval)
				.Select(response => response.Data)
				.Select((SentimentAnalysisStatisticsDTO data) => Mapper.Map<SentimentAnalysisStatisticsDTO, ChartModel>(data))
				.Finally(() => {
					Debug.WriteLine("Get Sentiment Analysis Statistics finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<ChartModel> GetSocialMediaActivityStatistics(string id)
        {
			Debug.WriteLine("Get Social Media Activity Statistics");

			var observable = _childrenRestService
                .GetSocialMediaActivityStatistics(id, Settings.Current.SonStatisticsTimeInterval)
				.Select(response => response.Data)
				.Select((SocialMediaActivityStatisticsDTO data) => Mapper.Map<SocialMediaActivityStatisticsDTO, ChartModel>(data))
				.Finally(() => {
					Debug.WriteLine("Get Social Media Activity finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<ChartModel> GetSocialMediaLikesStatistics()
        {

			Debug.WriteLine("Get Social Media Likes Statistics");

			var observable = _childrenRestService
                .GetSocialMediaLikesStatistics(
					Ids: String.IsNullOrEmpty(Settings.Current.FilteredSonCategories) ?
						new string[] { } : Settings.Current.FilteredSonCategories.Split(','),
					daysAgo: Settings.Current.TimeInterval)
				.Select(response => response.Data)
                .Select((SocialMediaLikesStatisticsDTO data) => Mapper.Map<SocialMediaLikesStatisticsDTO, ChartModel>(data))
				.Finally(() => {
					Debug.WriteLine("Get Social Media Likes Statistics finished ...");
				});

			return operationDecorator(observable);
        }
    }
}
