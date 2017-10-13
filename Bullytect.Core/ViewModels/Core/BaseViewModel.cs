

namespace Bullytect.Core.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Windows.Input;
    using Acr.UserDialogs;
    using Bullytect.Core.Exceptions;
    using Bullytect.Core.I18N;
    using Bullytect.Core.Messages;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Plugins.Messenger;
    using MvvmCross.ReactiveUI.Interop;
    using Plugin.Connectivity;
    using ReactiveUI;
    using Bullytect.Core.Rest.Models.Exceptions;
    using System.Reactive.Linq;
    using Bullytect.Core.Helpers;
    using System.Reactive;
    using Bullytect.Core.Config;

    public abstract class BaseViewModel : MvxReactiveViewModel
    {

        protected readonly IUserDialogs _userDialogs;
        protected readonly IMvxMessenger _mvxMessenger;
        protected readonly AppHelper _appHelper;

        public BaseViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger, AppHelper appHelper)
        {
            _userDialogs = userDialogs;
            _mvxMessenger = mvxMessenger;
            _appHelper = appHelper;


			SignOutCommand = ReactiveCommand
                .CreateFromObservable<Unit, bool>((_) => _appHelper.RequestConfirmation(AppResources.Profile_Confirm_SignOut));

			SignOutCommand.Subscribe((_) =>
			{
                Bullytect.Core.Config.Settings.AccessToken = null;
				//var mvxBundle = new MvxBundle(new Dictionary<string, string> { { "NavigationCommand", "StackClear" } });
				ShowViewModel<AuthenticationViewModel>(new AuthenticationViewModel.AuthenticationParameter()
				{
					ReasonForAuthentication = AuthenticationViewModel.SIGN_OUT
				});
			});
        }

        Dictionary<string, string> _fieldErrors = new Dictionary<string, string>();

		public Dictionary<string, string> FieldErrors
		{
			get => _fieldErrors;
			set => SetProperty(ref _fieldErrors, value);
		}

        bool _dataFound = true;

        public bool DataFound
        {
            get => _dataFound;
            set => SetProperty(ref _dataFound, value);
        }


        bool _errorOccurred = false;

		public bool ErrorOccurred
		{
			get => _errorOccurred;
			set => SetProperty(ref _errorOccurred, value);
		}

        public bool HaveInternet
        {

            get
            {

                if (!CrossConnectivity.IsSupported)
                    return true;

                //Do this only if you need to and aren't listening to any other events as they will not fire.
                using (var connectivity = CrossConnectivity.Current)
                {
                    return connectivity.IsConnected;
                }
            }
        }

        protected ObservableAsPropertyHelper<bool> _isBusy;
        public bool IsBusy
        {
            get { return _isBusy != null ? _isBusy.Value : false; }
        }

        protected virtual void HandleExceptions(Exception ex)
        {


            _userDialogs.HideLoading();

            if (ex is TimeoutOperationException)
            {
                _appHelper.Toast(AppResources.Common_Timeout_Operation, System.Drawing.Color.FromArgb(255, 0, 0));

            }
            else if (ex is HttpRequestException)
            {
                ErrorOccurred = true;
                _appHelper.Toast(AppResources.Common_Server_Connection_Error, System.Drawing.Color.FromArgb(255, 0, 0));

            }
            else if (ex is GenericErrorException)
            {
                ErrorOccurred = true;
                _appHelper.Toast(AppResources.Common_Server_Error, System.Drawing.Color.FromArgb(255, 0, 0));
            }
            else if (ex is DataInvalidException)
            {
                var dataInvalidEx = (DataInvalidException)ex;
                FieldErrors = dataInvalidEx.FieldErrors;
            }
            else if (ex is ApiUnauthorizedAccessException) {

                Settings.AccessToken = null;
                ShowViewModel<AuthenticationViewModel>(new AuthenticationViewModel.AuthenticationParameter() {
                    ReasonForAuthentication = AuthenticationViewModel.SESSION_EXPIRED
                });

            }
            else
            {
                ErrorOccurred = true;
                Debug.WriteLine(String.Format("Exception: {0}", ex.ToString()));
                _mvxMessenger.Publish(new ExceptionOcurredMessage(this, ex));
            }
        }


        protected void HandleIsExecuting(bool isLoading, string Text)
        {
            if (isLoading)
            {
                ErrorOccurred = false;
                _userDialogs.ShowLoading(Text, MaskType.Black);
            }
            else
            {
                _userDialogs.HideLoading();
            }
        }

        protected void ResetCommonProps(){
            ErrorOccurred = false;
            DataFound = true;
            FieldErrors = new Dictionary<string, string>();
        }

		
        #region commmands 

            public ICommand CloseCommand => new MvxCommand(() => Close(this));

		    public ReactiveCommand<Unit, bool> SignOutCommand { get; protected set; }

        #endregion


    }
}
