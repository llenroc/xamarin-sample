using System;
using System.Collections.Generic;
using System.Diagnostics;
using Acr.UserDialogs;
using Bullytect.Core.I18N;
using Bullytect.Core.Services;
using Bullytect.Rest.Models.Exceptions;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class PasswordRecoveryViewModel : BaseViewModel
    {

        readonly IParentService _parentService;

        public PasswordRecoveryViewModel(IParentService parentService,
                                         IUserDialogs userDialogs, IMvxMessenger mvxMessenger): base(userDialogs, mvxMessenger)
        {
            _parentService = parentService;

            ResetPasswordCommand = ReactiveCommand.CreateFromObservable<string, string>((_) => _parentService.ResetPassword(_email));

            ResetPasswordCommand.Subscribe((_) => {
				Debug.WriteLine(String.Format("Password Reset Request"));
                var toastConfig = new ToastConfig(AppResources.Password_Recovery_Request_Completed);
				toastConfig.SetDuration(3000);
				toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(12, 131, 193));
				_userDialogs.Toast(toastConfig);
				ShowViewModel<AuthenticationViewModel>();
            });


            ResetPasswordCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.Common_Loading));

			ResetPasswordCommand.ThrownExceptions.Subscribe(HandleExceptions);
        }

		#region Properties 

		Dictionary<string, string> _fieldErrors = new Dictionary<string, string>();

		public Dictionary<string, string> FieldErrors
		{
			get => _fieldErrors;
			set => SetProperty(ref _fieldErrors, value);
		}

        string _email;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        #endregion

        #region commands

        public ReactiveCommand<string, string> ResetPasswordCommand { get; protected set; }

		#endregion

		protected override void HandleExceptions(Exception ex)
		{

			if (ex is DataInvalidException)
			{
				var dataInvalidEx = (DataInvalidException)ex;
				FieldErrors = dataInvalidEx.FieldErrors;
			}
			else
			{
				base.HandleExceptions(ex);
			}
		}
    }
}
