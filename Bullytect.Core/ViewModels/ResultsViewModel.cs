
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Helpers;
using Bullytect.Core.Services;
using Bullytect.Core.ViewModels.Core.Models;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using MvvmHelpers;
using ReactiveUI;
using SkiaSharp;

namespace Bullytect.Core.ViewModels
{
    public class ResultsViewModel : BaseViewModel
    {

        readonly IStatisticsService _statisticsService;

		const int COMMENTS_ANALYZED_CHART_POS = 0;
        const int SYSTEM_ALERTS_CHART_POS = 1;
        const int SOCIAL_MEDIA_LIKES_CHART_POS = 2;

        public ResultsViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger,
                                AppHelper appHelper, IStatisticsService statisticsService) : base(userDialogs, mvxMessenger, appHelper)
        {

            _statisticsService = statisticsService;

			RefreshCommand = ReactiveCommand.CreateFromObservable<Unit, ChartModel>((_) => RefreshCurrentChart(force: true));

			RefreshCommand.Subscribe(HandlerRefreshCurrentChart);

			RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);

        }

        #region properties

        int _position;

        public int Position
        {

            get => _position;
            set => SetProperty(ref _position, value);

        }

        ChartModel _commentsAnalyzedChart;

        public ChartModel CommentsAnalyzedChart
        {

            get => _commentsAnalyzedChart;
            set => SetProperty(ref _commentsAnalyzedChart, value);
        }

        ChartModel _systemAlertsChart;

        public ChartModel SystemAlertsChart
        {

            get => _systemAlertsChart;
            set => SetProperty(ref _systemAlertsChart, value);
        }


        ChartModel _socialMediaLikesChart;

        public ChartModel SocialMediaLikesChart
        {

            get => _socialMediaLikesChart;
            set => SetProperty(ref _socialMediaLikesChart, value);
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
                    RefreshCurrentChart().Catch<ChartModel, Exception>(ex =>
                    {
                        HandleExceptions(ex);
                        IsBusy = false;
                        return Observable.Empty<ChartModel>();

                    }).Subscribe(HandlerRefreshCurrentChart);

                });
            }
        }

        public ICommand GoToSettingsCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<ResultsSettingsViewModel>());
            }
        }

		#endregion

		#region methods


		void HandlerRefreshCurrentChart(ChartModel Chart)
		{

			IsBusy = false;
            ErrorOccurred = false;

			switch (Position)
			{

				case COMMENTS_ANALYZED_CHART_POS:
					CommentsAnalyzedChart = Chart;
					break;

				case SYSTEM_ALERTS_CHART_POS:
					SystemAlertsChart = Chart;
					break;

				case SOCIAL_MEDIA_LIKES_CHART_POS:
					SocialMediaLikesChart = Chart;
					break;

			}
		}

		IObservable<ChartModel> RefreshCurrentChart(bool force = false)
		{

			IObservable<ChartModel> observable = Observable.Empty<ChartModel>(); ;

			switch (Position)
			{

				case COMMENTS_ANALYZED_CHART_POS:
					if (force || CommentsAnalyzedChart == null)
					{
						IsBusy = true;
                        observable = _statisticsService.GetCommentsAnalyzedStatistics();
					}
					break;

				case SYSTEM_ALERTS_CHART_POS:
					if (force || SystemAlertsChart == null)
					{
						IsBusy = true;
                        observable = _statisticsService.GetAlertsStatistics();
					}
					break;

				case SOCIAL_MEDIA_LIKES_CHART_POS:
					if (force || SocialMediaLikesChart == null)
					{
						IsBusy = true;
                        observable = _statisticsService.GetSocialMediaLikesStatistics();
					}
					break;

				default:
					if (force || SocialMediaLikesChart == null)
					{
						IsBusy = true;
						observable = _statisticsService.GetSocialMediaLikesStatistics();
					}
					break;
			}

			return observable.DefaultIfEmpty();

		}

        #endregion
    }
}
