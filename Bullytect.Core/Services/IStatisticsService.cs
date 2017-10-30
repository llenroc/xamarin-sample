using System;
using System.Collections.Generic;
using Bullytect.Core.ViewModels.Core.Models;

namespace Bullytect.Core.Services
{
    public interface IStatisticsService
    {

		IObservable<ChartModel> GetSocialMediaActivityStatistics(string id);

		IObservable<ChartModel> GetSentimentAnalysisStatistics(string id);

		IObservable<ChartModel> GetCommunitiesStatistics(string id);

		IObservable<ChartModel> GetDimensionsStatistics(string id);

        IObservable<ChartModel> GetCommentsStatistics();

        IObservable<ChartModel> GetSocialMediaLikesStatistics();

        IObservable<ChartModel> GetAlertsStatistics();

        IObservable<IList<UserListModel>> GetMostActiveFriends();

        IObservable<IList<UserListModel>> GetNewFriends();

    }
}
