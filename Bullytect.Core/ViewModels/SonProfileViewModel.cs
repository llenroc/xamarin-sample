
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

            RefreshCommand = ReactiveCommand.CreateFromObservable<Unit, ChartModel>((_) => _statisticsService.GetDimensionsStatistics(Identity, 1));

            RefreshCommand.Subscribe((Chart) =>
            {
                FourDimensionsChart = Chart;
            });

            RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);

        }

        public class SonParameter
        {
            public string Identity { get; set; }
            public string FullName { get; set; }
            public DateTime Birthdate { get; set; }
            public string School { get; set; }
            public string ProfileImage { get; set; }
        }


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

        DateTime _birthdate;

        public DateTime Birthdate
        {

            get => _birthdate;
            set => SetProperty(ref _birthdate, value);
        }

        string _school;

        public string School
        {

            get => _school;
            set => SetProperty(ref _school, value);
        }

        string _profileImage;

        public string ProfileImage
        {
            get => _profileImage;
            set => SetProperty(ref _profileImage, value);
        }


        ChartModel _fourDimensionsChart;

        public ChartModel FourDimensionsChart
        {
            get => _fourDimensionsChart;
            set => SetProperty(ref _fourDimensionsChart, value);

        }

        public void Init(SonParameter sonParameter)
        {
            Identity = sonParameter.Identity;
            FullName = sonParameter.FullName;
            Birthdate = sonParameter.Birthdate;
            School = sonParameter.School;
            ProfileImage = sonParameter.ProfileImage;
        }


        #region commands

        public ReactiveCommand<Unit, ChartModel> RefreshCommand { get; protected set; }

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
                FullName = FullName
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


    }
}
