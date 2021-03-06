﻿using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Helpers;
using Bullytect.Core.Rest.Models.Exceptions;
using Bullytect.Core.Services;
using Bullytect.Core.ViewModels.Core.Models;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class SonStatisticsViewModel : BaseViewModel
    {

        readonly IStatisticsService _statisticsService;


        public const int FOUR_DIMENSIONS_CHART = 0;
        public const int SOCIAL_MEDIA_ACTIVITIES_CHART = 1;
        public const int SENTIMENT_ANALYSIS_CHART = 2;
        public const int COMMUNITIES_CHART = 3;


        public SonStatisticsViewModel(IUserDialogs userDialogs,
                                      IMvxMessenger mvxMessenger, AppHelper appHelper, IStatisticsService statisticsService) : base(userDialogs, mvxMessenger, appHelper)
        {
            _statisticsService = statisticsService;


            RefreshCommand = ReactiveCommand.CreateFromObservable<Unit, ChartModel>((_) => RefreshCurrentChart(force: true));

            RefreshCommand.Subscribe(HandlerRefreshCurrentChart);

			RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);

        }


        public class SonStatisticsParameter
        {
            public string Identity { get; set; }
            public string FullName { get; set; }
            public int ShowChart { get; set; } = FOUR_DIMENSIONS_CHART;
        }


        public void Init(SonStatisticsParameter sonStatisticsParameter)
        {
            Identity = sonStatisticsParameter.Identity;
            FullName = sonStatisticsParameter.FullName;
            Position = sonStatisticsParameter.ShowChart;
        }

        #region properties


        string _identity;

        public string Identity
        {
            get => _identity;
            set => SetProperty(ref _identity, value);
        }

        string _fullName;

        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }


		#endregion


		#region commands

		public ReactiveCommand<Unit, ChartModel> RefreshCommand { get; protected set; }

		public ICommand RefreshChartCommand
        {
            get
            {
                return new MvxCommand<int>((pos) =>
                {
                    RefreshCurrentChart().Catch<ChartModel, Exception>(ex => {
                        HandleExceptions(ex);
                        return Observable.Empty<ChartModel>();

                    }).Subscribe(HandlerRefreshCurrentChart);

                });
            }
        }

        public ICommand GoToSettingsCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<SonStatisticsSettingsViewModel>());
            }
        }

        public ICommand ShowCommentsCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<CommentsViewModel>(new { SonIdentity = Identity }));
            }
        }


        #endregion

        #region properties

        int _position;

        public int Position {

            get => _position;
            set => SetProperty(ref _position, value);

        }

        ChartModel _socialMediaActivitiesChart;

        public ChartModel SocialMediaActivitiesChart
        {
            get => _socialMediaActivitiesChart;

            set => SetProperty(ref _socialMediaActivitiesChart, value);
        }

        ChartModel _fourDimensionsChart;

        public ChartModel FourDimensionsChart
        {
            get => _fourDimensionsChart;
            set => SetProperty(ref _fourDimensionsChart, value);

        }

        ChartModel _sentimentAnalysisChart;

        public ChartModel SentimentAnalysisChart
        {
            get => _sentimentAnalysisChart;
            set => SetProperty(ref _sentimentAnalysisChart, value);
        }


        ChartModel _communitiesCharts;

        public ChartModel CommunitiesCharts
        {
            get => _communitiesCharts;
            set => SetProperty(ref _communitiesCharts, value);
        }


        #endregion


        #region methods


        void HandlerRefreshCurrentChart(ChartModel Chart) {

            IsBusy = false;

			switch (Position)
			{

                case SOCIAL_MEDIA_ACTIVITIES_CHART:
					SocialMediaActivitiesChart = Chart;
					break;

				case FOUR_DIMENSIONS_CHART:
					FourDimensionsChart = Chart;
					break;

				case SENTIMENT_ANALYSIS_CHART:
					SentimentAnalysisChart = Chart;
					break;

				default:
					CommunitiesCharts = Chart;
					break;
			}
        }

        IObservable<ChartModel> RefreshCurrentChart(bool force = false){

            IObservable<ChartModel> observable = Observable.Empty<ChartModel>();

			ErrorOccurred = false;
			DataFound = true;

            switch(Position){

                case SOCIAL_MEDIA_ACTIVITIES_CHART:
                    if(force || SocialMediaActivitiesChart == null) {
                        IsBusy = true;
                        observable = _statisticsService.GetSocialMediaActivityStatistics(Identity);
                    }
                    break;

                case FOUR_DIMENSIONS_CHART:
					if (force || FourDimensionsChart == null)
					{
						IsBusy = true;
                        observable = _statisticsService.GetDimensionsStatistics(Identity);
					}
                    break;

                case SENTIMENT_ANALYSIS_CHART:
					if (force || SentimentAnalysisChart == null)
					{
						IsBusy = true;
                        observable = _statisticsService.GetSentimentAnalysisStatistics(Identity);
                    }
                    break;

                default:
					if (force || CommunitiesCharts == null)
					{
						IsBusy = true;
                        observable = _statisticsService.GetCommunitiesStatistics(Identity);
                    }
                    break;
            }

            return observable.DefaultIfEmpty();

        }

		protected override void HandleExceptions(Exception ex)
		{
            IsBusy = false;

			if (
				ex is SocialMediaActivityStatisticsNotFoundException ||
				ex is NoDimensionsStatisticsForThisPeriodException ||
				ex is NoSentimentAnalysisStatisticsForThisPeriodException ||
				ex is NoCommunityStatisticsForThisPeriodException)
			{

				DataFound = false;

			}
			else
			{

				base.HandleExceptions(ex);
			}

		}

        #endregion
    }
}
