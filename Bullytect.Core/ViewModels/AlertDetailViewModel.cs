
using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Helpers;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
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
                                      .SelectMany((_) => alertService.DeleteAlertOfSon(SonIdentity, Identity)).Finally(() => Close(this)));
            
            DeleteAlertCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.Common_Loading));

			DeleteAlertCommand.ThrownExceptions.Subscribe(HandleExceptions);
        }

        public class AlertParameter
        {
            public string Identity { get; set; }
            public AlertLevelEnum Level { get; set; }
            public string Title { get; set; }
            public string Payload { get; set; }
            public DateTime CreateAt { get; set; }
            public string SonFullName { get; set; }
            public string SonIdentity { get; set; }
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


        #endregion

        #region commands


            public ReactiveCommand DeleteAlertCommand { get; protected set; }  

        #endregion


    }
}
