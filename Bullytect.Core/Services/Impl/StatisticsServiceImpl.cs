using System;
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
    }
}
