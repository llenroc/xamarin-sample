
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Helpers;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Rest.Models.Exceptions;
using Bullytect.Core.Services;
using Bullytect.Core.ViewModels.Core.Models;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class SonProfileViewModel : BaseViewModel
    {

        readonly IParentService _parentService;
        readonly IStatisticsService _statisticsService;

        public SonProfileViewModel(IUserDialogs userDialogs,
                                   IMvxMessenger mvxMessenger, AppHelper appHelper,
                                   IParentService parentService, IStatisticsService statisticsService) : base(userDialogs, mvxMessenger, appHelper)
        {

            _parentService = parentService;
            _statisticsService = statisticsService;


            DeleteSonCommand = ReactiveCommand
                .CreateFromObservable(() => _appHelper.RequestConfirmation(AppResources.Son_Profile_Delete_Confirm)
                                      .SelectMany((confirmed) => confirmed ? _parentService.DeleteSonById(Identity) : Observable.Empty<string>())
                                      .Do((_) => Close(this)));

            DeleteSonCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.Common_Loading));

            DeleteSonCommand.ThrownExceptions.Subscribe(HandleExceptions);

            RefreshCommand = ReactiveCommand.CreateFromObservable<Unit, PageModel>((_) =>
                     Observable.Zip(
                        _parentService.GetSonById(Identity),
                        _statisticsService.GetDimensionsStatistics(Identity, 1).OnErrorResumeNext(Observable.Return(new ChartModel())), 
                        (SonEntity SonInformation, ChartModel ChartModel) => new PageModel()
                        {
                            SonEntity = SonInformation,
                            DimensionChart = ChartModel
                        }));

            RefreshCommand.IsExecuting.Subscribe((IsLoading) => IsBusy = IsLoading);

            RefreshCommand.Subscribe((PageModel) =>
            {
                ResetCommonProps();
                SonEntity.HydrateWith(PageModel.SonEntity);

                if (PageModel.DimensionChart.Entries?.Count > 0)
                    FourDimensionsChart = PageModel.DimensionChart;
                else
                    DataFound = false;
            });

            RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);

        }

        string _identity;

        public string Identity
        {
            get => _identity;
            set => SetProperty(ref _identity, value);
        }

        SonEntity _sonEntity = new SonEntity();

        public SonEntity SonEntity {

            get => _sonEntity;
            set => SetProperty(ref _sonEntity, value);

        }

        ChartModel _fourDimensionsChart;

        public ChartModel FourDimensionsChart
        {
            get => _fourDimensionsChart;
            set => SetProperty(ref _fourDimensionsChart, value);

        }

        public void Init(string Identity)
        {
            this.Identity = Identity;
        }


        #region commands

        public ReactiveCommand<Unit, PageModel> RefreshCommand { get; protected set; }

        public ICommand EditSonCommand => new MvxCommand(() => ShowViewModel<EditSonViewModel>(new { SonIdentity = Identity }));

        public ReactiveCommand DeleteSonCommand { get; protected set; }

        public ICommand GoToRelationsCommand => new MvxCommand(() => ShowViewModel<RelationsViewModel>(new { SonIdentity = Identity }));

        public ICommand GoToAlertsCommand
        {
            get => new MvxCommand(() => ShowViewModel<AlertsViewModel>(new { Identity }));
        }

        public ICommand ShowSonStatisticsCommand
        {
            get => new MvxCommand(() => ShowViewModel<SonStatisticsViewModel>(new SonStatisticsViewModel.SonStatisticsParameter()
            {
                Identity = Identity,
                FullName = SonEntity.FullName
            }));
        }

        #endregion

        #region methods

        protected override void HandleExceptions(Exception ex)
        {
            IsBusy = false;

            if (ex is NoDimensionsStatisticsForThisPeriodException)
            {

                DataFound = false;

            }
            else
            {

                base.HandleExceptions(ex);
            }

        }

        #endregion

        #region models

        public class PageModel
        {

            public SonEntity SonEntity { get; set; } = new SonEntity();
            public ChartModel DimensionChart { get; set; }
        }

        #endregion


    }
}
