using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using Bullytect.Rest.Models.Exceptions;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class NotificationViewModel: BaseViewModel
    {

        readonly IAlertService _alertService;

        protected ObservableAsPropertyHelper<IList<AlertEntity>> _notificationList;
        public IList<AlertEntity> NotificationList
		{
			get { return _notificationList.Value; }
		}

        public NotificationViewModel(IAlertService alertService, IUserDialogs userDialogs, IMvxMessenger mvxMessenger): base(userDialogs, mvxMessenger)
        {
            _alertService = alertService;

            LoadNotificationsCommand = ReactiveCommand.CreateFromObservable<string, IList<AlertEntity>>((param) => _alertService.GetAllSelfNotifications());

			LoadNotificationsCommand.ToProperty(this, x => x.NotificationList, out _notificationList);

			LoadNotificationsCommand.IsExecuting.ToProperty(this, x => x.IsBusy, out _isBusy);

			LoadNotificationsCommand.ThrownExceptions.Subscribe(HandleExceptions);
        }

		public override void Start()
		{
			LoadNotificationsCommand.Execute(null);
		}


		#region commands

		public ReactiveCommand<string, IList<AlertEntity>> LoadNotificationsCommand { get; protected set; }

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
