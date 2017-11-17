
using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Helpers;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class AlertDetailViewModel : BaseViewModel
    {

        readonly IAlertService _alertService;


        public AlertDetailViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger,
                                    IAlertService alertService, AppHelper appHelper) : base(userDialogs, mvxMessenger, appHelper)
        {
            _alertService = alertService;

            DeleteAlertCommand = ReactiveCommand
                .CreateFromObservable(() =>  _appHelper.RequestConfirmation(AppResources.Alert_Confirm_Clear)
                                      .SelectMany((_) => alertService.DeleteAlertOfSon(SonIdentity, Identity)).Do((_) => Close(this)));
            
            DeleteAlertCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecutingWithDialogs(isLoading, AppResources.Common_Loading));

			DeleteAlertCommand.ThrownExceptions.Subscribe(HandleExceptions);
        }

        public class AlertParameter
        {
            public string Identity { get; set; }
            public AlertLevelEnum Level { get; set; }
            public string Title { get; set; }
            public string Payload { get; set; }
            public DateTime CreateAt { get; set; }
            public string Since { get; set; }
            public string SonFullName { get; set; }
            public string SonIdentity { get; set; }
            public string ProfileImage { get; set; }
            public AlertCategoryEnum Category { get; set; }
        }


        public void Init(AlertParameter alertParameter)
        {
            Identity = alertParameter.Identity;
            Title = alertParameter.Title;
            Level = alertParameter.Level;
            Payload = alertParameter.Payload;
            CreateAt = alertParameter.CreateAt;
            SonFullName = alertParameter.SonFullName;
            SonIdentity = alertParameter.SonIdentity;
            ProfileImage = alertParameter.ProfileImage;
            Since = alertParameter.Since;
            Category = alertParameter.Category;
        }

        #region properties


        string _identity;

        public string Identity
        {
			get => _identity;
			set => SetProperty(ref _identity, value);

        }

        AlertLevelEnum _level;

        public AlertLevelEnum Level
        {
            get => _level;
            set => SetProperty(ref _level, value);
        }

        string _title;

        public string Title
        {

            get => _title;
            set => SetProperty(ref _title, value);
        }

        string _payload;

        public string Payload
        {

            get => _payload;
            set => SetProperty(ref _payload, value);
        }

        DateTime _createAt;

        public DateTime CreateAt
        {

            get => _createAt;
            set => SetProperty(ref _createAt, value);
        }

        string _sonFullName;

        public string SonFullName
        {

            get => _sonFullName;
            set => SetProperty(ref _sonFullName, value);
        }

        string _sonIdentity;

        public string SonIdentity
        {

            get => _sonIdentity;
            set => SetProperty(ref _sonIdentity, value);
        }


        string _profileImage;

        public string ProfileImage
		{

			get => _profileImage;
			set => SetProperty(ref _profileImage, value);
		}

        string _since;

        public string Since
        {
            get => _since;
            set => SetProperty(ref _since, value);

        }

        AlertCategoryEnum _category;

        public AlertCategoryEnum Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }


        #endregion

        #region commands


            public ReactiveCommand DeleteAlertCommand { get; protected set; } 

            public ICommand NavigateToCommand
                => new MvxCommand<AlertCategoryEnum>((Category) => {

                    switch(Category) {
                        case AlertCategoryEnum.INFORMATION_SON:
                            ShowViewModel<EditSonViewModel>(new { SonIdentity });
                            break;
                        case AlertCategoryEnum.GENERAL_STATISTICS:
                            ShowViewModel<ResultsViewModel>();
                            break;
                        case AlertCategoryEnum.STATISTICS_SON:
                            ShowViewModel<SonStatisticsViewModel>(new SonStatisticsViewModel.SonStatisticsParameter()
                            {
                                Identity = SonIdentity,
                                FullName = SonFullName,
                                ShowChart = SonStatisticsViewModel.FOUR_DIMENSIONS_CHART
                            });
                            break;
                        case AlertCategoryEnum.INFORMATION_EXTRACTION:
                            ShowViewModel<CommentsViewModel>(new { Identity = SonIdentity });
                            break;
                    }

                    
                });

        #endregion


    }
}
