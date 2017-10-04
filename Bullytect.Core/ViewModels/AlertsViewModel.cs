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

namespace Bullytect.Core.ViewModels
{
    public class AlertsViewModel : BaseViewModel
    {

        readonly IAlertService _alertService;

        protected ObservableAsPropertyHelper<IList<AlertEntity>> _alertList;
        public IList<AlertEntity> AlertList
		{
			get { return _alertList.Value; }
		}

        public AlertsViewModel(IAlertService alertService, IUserDialogs userDialogs, IMvxMessenger mvxMessenger, IImagesService imagesService): base(userDialogs, mvxMessenger, imagesService)
        {
            _alertService = alertService;

            LoadNotificationsCommand = ReactiveCommand.CreateFromObservable<string, IList<AlertEntity>>((param) => _alertService.GetAllSelfNotifications());

			LoadNotificationsCommand.ToProperty(this, x => x.AlertList, out _alertList);

            LoadNotificationsCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.Loading_Alerts));

			LoadNotificationsCommand.ThrownExceptions.Subscribe(HandleExceptions);
        }

		public override void Start()
		{
			LoadNotificationsCommand.Execute(null);
		}


		#region commands

		public ReactiveCommand<string, IList<AlertEntity>> LoadNotificationsCommand { get; protected set; }


        public ICommand ShowAlertDetailCommand => new MvxCommand<AlertEntity>((AlertEntity AlertEntity) => ShowViewModel<AlertDetailViewModel>(new AlertDetailViewModel.AlertParameter() {
            Level = AlertEntity.Level,
            Payload = AlertEntity.Payload,
            CreateAt = AlertEntity.CreateAt,
            SonFullName = AlertEntity.Son.FullName
        }));


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
