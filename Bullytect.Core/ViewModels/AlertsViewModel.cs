using System;
using System.Collections.Generic;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;
using Bullytect.Core.Rest.Models.Exceptions;
using System.Reactive;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Bullytect.Core.Helpers;
using System.Linq;

namespace Bullytect.Core.ViewModels
{
    public class AlertsViewModel : BaseViewModel
    {

        readonly IAlertService _alertService;

        public AlertsViewModel(IAlertService alertService, IUserDialogs userDialogs, 
                               IMvxMessenger mvxMessenger, AppHelper appHelper): base(userDialogs, mvxMessenger, appHelper)
        {
            _alertService = alertService;

			ClearAlertsCommand = ReactiveCommand
				.CreateFromObservable<Unit, int>((param) =>
				{
                    return _appHelper.RequestConfirmation(AppResources.Alerts_Confirm_Clear)
                                     .Do((_) => _userDialogs.ShowLoading(AppResources.Alerts_Deleting_Alerts))
                                     .SelectMany((_) => string.IsNullOrEmpty(SonIdentity) 
                                                 ? _alertService.ClearSelfAlerts() : _alertService.ClearAlertsOfSon(SonIdentity))
									 .Do((_) => _userDialogs.HideLoading());
				});

            ClearAlertsCommand.Subscribe((alertsDeleted) => {
                Debug.WriteLine("Alerts Deleted -> " + alertsDeleted);
                Alerts = null;
                DataFound = false;
            });

            ClearAlertsCommand.ThrownExceptions.Subscribe(HandleExceptions);

            LoadAlertsCommand = ReactiveCommand
                .CreateFromObservable<Unit, IList<AlertEntity>>((param) => string.IsNullOrEmpty(SonIdentity) ? alertService.GetSelfAlerts() : _alertService.GetAlertsBySon(SonIdentity));

            LoadAlertsCommand.Subscribe((AlertsEntities) => {
                Alerts = new ObservableCollection<AlertEntity>(AlertsEntities);
            });

            LoadAlertsCommand.IsExecuting.ToProperty(this, x => x.IsBusy, out _isBusy);

			LoadAlertsCommand.ThrownExceptions.Subscribe(HandleExceptions);

            DeleteAlertCommand = ReactiveCommand
                .CreateFromObservable<AlertEntity, string>((AlertEntity) => 
                                                           alertService.DeleteAlertOfSon(AlertEntity.Son.Identity, AlertEntity.Identity)
                                                           .Do((_) => Alerts.Remove(AlertEntity)));

			DeleteAlertCommand.IsExecuting.ToProperty(this, x => x.IsBusy, out _isBusy);

            DeleteAlertCommand.Subscribe((_) => {

                _userDialogs.ShowSuccess(AppResources.Alerts_Deleted);

                if (Alerts.Count() == 0)
                    DataFound = false;

            });

            DeleteAlertCommand.ThrownExceptions.Subscribe(HandleExceptions);
        }

        public void Init(string Identity) {
            SonIdentity = Identity;
        }

        #region properties

        string _sonIdentity;

        public string SonIdentity
        {
			get => _sonIdentity;
			set => SetProperty(ref _sonIdentity, value);
        }

        ObservableCollection<AlertEntity> _alerts;

		public ObservableCollection<AlertEntity> Alerts
		{
			get => _alerts;
			set => SetProperty(ref _alerts, value);
		}

        #endregion


        #region commands

        public ReactiveCommand<Unit, IList<AlertEntity>> LoadAlertsCommand { get; protected set; }

        public ReactiveCommand<Unit, int> ClearAlertsCommand { get; protected set; }

        public ICommand ShowAlertDetailCommand => new MvxCommand<AlertEntity>((AlertEntity AlertEntity) => ShowViewModel<AlertDetailViewModel>(new AlertDetailViewModel.AlertParameter() {
            Identity = AlertEntity.Identity,
            Title = AlertEntity.Title,
            Level = AlertEntity.Level,
            Payload = AlertEntity.Payload,
            CreateAt = AlertEntity.CreateAt,
            SonFullName = AlertEntity.Son.FullName
        }));

        public ReactiveCommand<AlertEntity, string> DeleteAlertCommand { get; protected set; }


		#endregion

		protected override void HandleExceptions(Exception ex)
		{

			if (ex is NoAlertsFoundException)
			{
				DataFound = false;
			}
			else
			{
				base.HandleExceptions(ex);
			}
		}
	}
}
