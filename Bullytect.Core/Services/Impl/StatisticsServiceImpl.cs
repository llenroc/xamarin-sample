using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using AutoMapper;
using Bullytect.Core.Config;
using Bullytect.Core.Rest.Models.Response;
using Bullytect.Core.Rest.Services;
using Bullytect.Core.ViewModels.Core.Models;

namespace Bullytect.Core.Services.Impl
{
    public class StatisticsServiceImpl : BaseService, IStatisticsService
    {

        readonly IChildrenRestService _childrenRestService;

        public StatisticsServiceImpl(IChildrenRestService childrenRestService){
            _childrenRestService = childrenRestService;
        }

        public IObservable<ChartModel> GetAlertsStatistics(string Id)
        {

            Debug.WriteLine("Get Alerts Statistics for {0}", Id);

			var observable = _childrenRestService
                .GetAlertsStatistics(
                    Ids: new string[] { Id },
					daysAgo: Settings.Current.TimeInterval)
				.Select(response => response.Data)
				.Select((AlertsStatisticsDTO data) => Mapper.Map<AlertsStatisticsDTO, ChartModel>(data))
				.Finally(() => {
					Debug.WriteLine("Get Alerts Statistics finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<ChartModel> GetCommentsStatistics(string Id)
        {
            Debug.WriteLine("Get Comments Statistics for {0}", Id);
			
			var observable = _childrenRestService
                .GetCommentsStatistics(
                    Ids: new string[] { Id },
                    daysAgo: Settings.Current.TimeInterval)
				.Select(response => response.Data)
                .Select((CommentsStatisticsDTO data) => Mapper.Map<CommentsStatisticsDTO, ChartModel>(data))
				.Finally(() => {
					Debug.WriteLine("Get Comments Statistics finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<ChartModel> GetCommunitiesStatistics(string Id)
        {
            Debug.WriteLine("Get Communities Statistics for {0}", Id);

			var observable = _childrenRestService
                .GetCommunitiesStatistics(Id, Settings.Current.SonStatisticsTimeInterval)
                .Select(response => response.Data)
				.Select((CommunitiesStatisticsDTO data) => Mapper.Map<CommunitiesStatisticsDTO, ChartModel>(data))
				.Finally(() => {
					Debug.WriteLine("Get Communities Statistics finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<ChartModel> GetDimensionsStatistics(string Id)
        {
            return GetDimensionsStatistics(Id, Settings.Current.SonStatisticsTimeInterval);
        }

        public IObservable<ChartModel> GetDimensionsStatistics(string Id, int DaysAgo)
        {
            Debug.WriteLine("Get Dimensions Statistics for {0}", Id);

            var observable = _childrenRestService
                .GetDimensionsStatistics(Id, DaysAgo)
                .Select(response => response.Data)
                .Select((DimensionsStatisticsDTO data) => Mapper.Map<DimensionsStatisticsDTO, ChartModel>(data))
                .Finally(() => {
                    Debug.WriteLine("Get Dimensions Statistics finished ...");
                });

            return operationDecorator(observable);
        }

        public IObservable<IList<UserListModel>> GetMostActiveFriends(string Id)
        {
			Debug.WriteLine("Get Most Active Friends");

			var observable = _childrenRestService
                .GetMostActiveFriends(Ids: new string[] { Id },
					daysAgo: Settings.Current.TimeInterval)
                .Select(response => response.Data.Users)
                .Select((IList<MostActiveFriendsDTO.UserDTO> data) => Mapper.Map<IList<MostActiveFriendsDTO.UserDTO>, IList<UserListModel>>(data))
				.Finally(() => {
					Debug.WriteLine("Get Most Active Friends finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<IList<UserListModel>> GetNewFriends(string Id)
        {
            Debug.WriteLine("Get New Friends for {0}", Id);

			var observable = _childrenRestService
                .GetNewFriends(
                    Ids: new string[] { Id },
                    daysAgo: Settings.Current.TimeInterval)
                .Select(response => response.Data.Users)
                .Select((IList<NewFriendsDTO.UserDTO> data) => Mapper.Map<IList<NewFriendsDTO.UserDTO>, IList<UserListModel>>(data))
				.Finally(() => {
					Debug.WriteLine("Get New Friends finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<ChartModel> GetSentimentAnalysisStatistics(string Id)
        {
            Debug.WriteLine("Get Sentiment Analysis Statistics for {0}", Id);

			var observable = _childrenRestService
                .GetSentimentAnalysisStatistics(Id, Settings.Current.SonStatisticsTimeInterval)
				.Select(response => response.Data)
				.Select((SentimentAnalysisStatisticsDTO data) => Mapper.Map<SentimentAnalysisStatisticsDTO, ChartModel>(data))
				.Finally(() => {
					Debug.WriteLine("Get Sentiment Analysis Statistics finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<ChartModel> GetSocialMediaActivityStatistics(string Id)
        {
            Debug.WriteLine("Get Social Media Activity Statistics for {0}", Id);

			var observable = _childrenRestService
                .GetSocialMediaActivityStatistics(Id, Settings.Current.SonStatisticsTimeInterval)
				.Select(response => response.Data)
				.Select((SocialMediaActivityStatisticsDTO data) => Mapper.Map<SocialMediaActivityStatisticsDTO, ChartModel>(data))
				.Finally(() => {
					Debug.WriteLine("Get Social Media Activity finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<ChartModel> GetSocialMediaLikesStatistics(string Id)
        {

            Debug.WriteLine("Get Social Media Likes Statistics for {0}", Id);

			var observable = _childrenRestService
                .GetSocialMediaLikesStatistics(
                    Ids: new string[] { Id },
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
