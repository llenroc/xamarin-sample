using System;
using System.Collections.Generic;
using Bullytect.Core.ViewModels.Core.Models;

namespace Bullytect.Core.Services
{
    public interface IStatisticsService
    {

        IObservable<ChartModel> GetSocialMediaActivityStatistics(string Id);

        IObservable<ChartModel> GetSentimentAnalysisStatistics(string Id);

        IObservable<ChartModel> GetCommunitiesStatistics(string Id);

        IObservable<ChartModel> GetDimensionsStatistics(string Id);

        IObservable<ChartModel> GetDimensionsStatistics(string Id, int DaysAgo);

        IObservable<ChartModel> GetCommentsStatistics(string Id);

        IObservable<ChartModel> GetSocialMediaLikesStatistics(string Id);

        IObservable<ChartModel> GetAlertsStatistics(string Id);

        IObservable<IList<UserListModel>> GetMostActiveFriends(string Id);

        IObservable<IList<UserListModel>> GetNewFriends(string Id);

    }
}
