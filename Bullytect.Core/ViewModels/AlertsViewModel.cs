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
using System.Diagnostics;
using Bullytect.Core.Helpers;
using System.Linq;
using MvvmHelpers;
using Bullytect.Core.ViewModels.Core.Models;
using Rg.Plugins.Popup.Services;
using Bullytect.Core.Pages.Alerts.Popup;

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
				.CreateFromObservable<Unit, string>((param) =>
				{
                    return _appHelper.RequestConfirmation(AppResources.Alerts_Confirm_Clear)
                                     .Do((_) => _userDialogs.ShowLoading(AppResources.Alerts_Deleting_Alerts))
                                     .SelectMany((_) => string.IsNullOrEmpty(SonIdentity) 
                                                 ? _alertService.ClearSelfAlerts() : _alertService.ClearAlertsOfSon(SonIdentity))
									 .Do((_) => _userDialogs.HideLoading());
				});

            ClearAlertsCommand.Subscribe((alertsDeleted) => {
                Debug.WriteLine("Alerts Deleted -> " + alertsDeleted);
                Alerts.Clear();
                DataFound = false;
            });

            ClearAlertsCommand.ThrownExceptions.Subscribe(HandleExceptions);

            RefreshCommand = ReactiveCommand
                .CreateFromObservable<Unit, IList<AlertEntity>>((param) => string.IsNullOrEmpty(SonIdentity) ? alertService.GetSelfAlerts() : _alertService.GetAlertsBySon(SonIdentity));

            RefreshCommand.Subscribe((AlertsEntities) => {
                Alerts.ReplaceRange(AlertsEntities);
                IsTimeout = false;
            });

            RefreshCommand.IsExecuting.Subscribe((IsLoading) => IsBusy = IsLoading);

			RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);

            DeleteAlertCommand = ReactiveCommand
                .CreateFromObservable<AlertEntity, string>((AlertEntity) => 
                                                           alertService.DeleteAlertOfSon(AlertEntity.Son.Identity, AlertEntity.Identity)
                                                           .Do((_) => Alerts.Remove(AlertEntity)));

			DeleteAlertCommand.IsExecuting.Subscribe((IsLoading) => IsBusy = IsLoading);

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

        public ObservableRangeCollection<AlertEntity> Alerts { get; } = new ObservableRangeCollection<AlertEntity>();


        public List<PickerOptionModel> AlertsOptionsList { get; set; } = new List<PickerOptionModel>()
        {
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Alerts_Last_Alerts, 20), Value = 20 },
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Alerts_Last_Alerts, 40), Value = 40 },
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Alerts_Last_Alerts, 60), Value = 60 },
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Alerts_Last_Alerts, 80), Value = 80 }
        };

        public List<PickerOptionModel> AntiquityOfAlertsOptionsList { get; set; } = new List<PickerOptionModel>()
        {
            new PickerOptionModel(){ Description = AppResources.Settings_Antiquity_Of_Alerts_No_Filter, Value = 0 },
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Antiquity_Of_Alerts_Description, 15), Value = 15 },
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Antiquity_Of_Alerts_Description, 30), Value = 30 },
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Antiquity_Of_Alerts_Description, 45), Value = 45 },
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Antiquity_Of_Alerts_Description, 60), Value = 60 }
        };

        PickerOptionModel _alertsOption;

        public PickerOptionModel AlertsOption
        {
            get => _alertsOption ?? AlertsOptionsList.First();

            set => SetProperty(ref _alertsOption, value);
        }

        PickerOptionModel _antiquityOfAlertsOption;

        public PickerOptionModel AntiquityOfAlertsOption
        {
            get => _antiquityOfAlertsOption ?? AntiquityOfAlertsOptionsList.First();

            set => SetProperty(ref _antiquityOfAlertsOption, value);
        }

        #endregion


        #region commands

        public ReactiveCommand<Unit, IList<AlertEntity>> RefreshCommand { get; protected set; }

        public ReactiveCommand<Unit, string> ClearAlertsCommand { get; protected set; }

        public ICommand ShowAlertDetailCommand => new MvxCommand<AlertEntity>((AlertEntity AlertEntity) => ShowViewModel<AlertDetailViewModel>(new AlertDetailViewModel.AlertParameter() {
            Identity = AlertEntity.Identity,
            Title = AlertEntity.Title,
            Level = AlertEntity.Level,
            Payload = AlertEntity.Payload,
            CreateAt = AlertEntity.CreateAt,
            SonFullName = AlertEntity.Son.FullName,
            SonIdentity = AlertEntity.Son.Identity,
            ProfileImage = AlertEntity.Son.ProfileImage,
            Category = AlertEntity.Category,
            Since = AlertEntity.Since
                                     
        }));

        public ReactiveCommand<AlertEntity, string> DeleteAlertCommand { get; protected set; }

        public ICommand OpenFilterAlertsCommand
                        => new MvxCommand(async () =>
                        {
                            if (PopupNavigation.PopupStack.Count > 0)
                            {
                                await PopupNavigation.PopAllAsync();
                            }
                            await PopupNavigation.PushAsync(new FilterAlertsPopup());
                        });


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
