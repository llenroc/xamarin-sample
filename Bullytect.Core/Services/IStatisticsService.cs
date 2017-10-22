using System;
using Bullytect.Core.ViewModels.Core.Models;

namespace Bullytect.Core.Services
{
    public interface IStatisticsService
    {

		IObservable<ChartModel> GetSocialMediaActivityStatistics(string id);

		IObservable<ChartModel> GetSentimentAnalysisStatistics(string id);

		IObservable<ChartModel> GetCommunitiesStatistics(string id);

		IObservable<ChartModel> GetDimensionsStatistics(string id);

        IObservable<ChartModel> GetCommentsAnalyzedStatistics();

        IObservable<ChartModel> GetSocialMediaLikesStatistics();

        IObservable<ChartModel> GetAlertsStatistics();
    }
}
