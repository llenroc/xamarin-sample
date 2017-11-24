using System;
using System.Reactive;
using Acr.UserDialogs;
using Bullytect.Core.Helpers;
using Bullytect.Core.I18N;
using Bullytect.Core.Services;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class PasswordRecoveryViewModel : BaseViewModel
    {

        readonly IParentService _parentService;

        public PasswordRecoveryViewModel(IParentService parentService,
                                         IUserDialogs userDialogs, IMvxMessenger mvxMessenger, AppHelper appHelper): base(userDialogs, mvxMessenger, appHelper)
        {
            _parentService = parentService;

            ResetPasswordCommand = ReactiveCommand.CreateFromObservable<Unit, string>((_) => _parentService.ResetPassword(_email));

            ResetPasswordCommand.Subscribe((_) => {
                _appHelper.Toast(AppResources.Password_Recovery_Request_Completed, System.Drawing.Color.FromArgb(12, 131, 193));
            });


            ResetPasswordCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.Common_Loading));

			ResetPasswordCommand.ThrownExceptions.Subscribe(HandleExceptions);
        }

		#region Properties 


        string _email;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        #endregion

        #region commands

        public ReactiveCommand<Unit, string> ResetPasswordCommand { get; protected set; }

		#endregion
    }
}
