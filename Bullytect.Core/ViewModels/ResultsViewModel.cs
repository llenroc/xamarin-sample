
using System;
using System.Collections.Generic;
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

namespace Bullytect.Core.ViewModels
{
    public class ResultsViewModel : BaseViewModel
    {

        readonly IStatisticsService _statisticsService;

		const int COMMENTS_ANALYZED_CHART_POS = 0;
        const int SYSTEM_ALERTS_CHART_POS = 1;
        const int SOCIAL_MEDIA_LIKES_CHART_POS = 2;
        const int MOST_ACTIVE_FRIENDS_POS = 3;
        const int NEW_FRIENDS_POS = 4;

        public ResultsViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger,
                                AppHelper appHelper, IStatisticsService statisticsService) : base(userDialogs, mvxMessenger, appHelper)
        {

            _statisticsService = statisticsService;

            RefreshCommand = ReactiveCommand.CreateFromObservable<Unit, object>((_) => RefreshCurrentPage(force: true));

			RefreshCommand.Subscribe(HandlerRefreshCurrentPage);

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

        public ObservableRangeCollection<UserListModel> MostActiveFriends { get; } = new ObservableRangeCollection<UserListModel>();

        public ObservableRangeCollection<UserListModel> NewFriends { get; } = new ObservableRangeCollection<UserListModel>();

        #endregion

        #region commands

        public ReactiveCommand<Unit, object> RefreshCommand { get; protected set; }

        public ICommand RefreshChartCommand
        {
            get
            {
                return new MvxCommand<int>((pos) =>
                {
                    RefreshCurrentPage().Catch<object, Exception>(ex =>
                    {
                        HandleExceptions(ex);
                        IsBusy = false;
                        return Observable.Empty<object>();

                    }).Subscribe(HandlerRefreshCurrentPage);

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


		void HandlerRefreshCurrentPage(object Data)
		{

			IsBusy = false;
            ErrorOccurred = false;

			switch (Position)
			{

				case COMMENTS_ANALYZED_CHART_POS:
                    CommentsAnalyzedChart = Data as ChartModel;
					break;

				case SYSTEM_ALERTS_CHART_POS:
					SystemAlertsChart = Data as ChartModel;
					break;

				case SOCIAL_MEDIA_LIKES_CHART_POS:
					SocialMediaLikesChart = Data as ChartModel;
					break;
                case MOST_ACTIVE_FRIENDS_POS:
                    MostActiveFriends.ReplaceRange(Data as IList<UserListModel>);
                    break;
				case NEW_FRIENDS_POS:
                    NewFriends.ReplaceRange(Data as IList<UserListModel>);
					break;

			}
		}

		IObservable<object> RefreshCurrentPage(bool force = false)
		{

            IObservable<object> observable = Observable.Empty<object>(); ;

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

                case MOST_ACTIVE_FRIENDS_POS:
                    IsBusy = true;
                    observable = _statisticsService.GetMostActiveFriends();
                    break;

                case NEW_FRIENDS_POS:
                    IsBusy = true;
                    observable = _statisticsService.GetNewFriends();
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
