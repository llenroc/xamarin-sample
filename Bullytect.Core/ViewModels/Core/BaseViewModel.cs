

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
    using Bullytect.Rest.Models.Exceptions;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Plugins.Messenger;
    using MvvmCross.ReactiveUI.Interop;
    using Plugin.Connectivity;
    using ReactiveUI;

    public abstract class BaseViewModel : MvxReactiveViewModel
    {

        protected readonly IUserDialogs _userDialogs;
        protected readonly IMvxMessenger _mvxMessenger;

        public BaseViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger)
        {
            _userDialogs = userDialogs;
            _mvxMessenger = mvxMessenger;
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
            ErrorOccurred = true;

            _userDialogs.HideLoading();

            if (ex is TimeoutOperationException)
            {
                var toastConfig = new ToastConfig(AppResources.Common_Timeout_Operation);
                toastConfig.SetDuration(3000);
                toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(255, 0, 0));

                _userDialogs.Toast(toastConfig);

            }
            else if (ex is HttpRequestException)
            {

                var toastConfig = new ToastConfig(AppResources.Common_Server_Connection_Error);
                toastConfig.SetDuration(3000);
                toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(255, 0, 0));

                _userDialogs.Toast(toastConfig);

			}
            else if (ex is GenericErrorException) {
                var toastConfig = new ToastConfig(AppResources.Common_Server_Error);
				toastConfig.SetDuration(3000);
				toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(255, 0, 0));

				_userDialogs.Toast(toastConfig);
            }
			else if (ex is DataInvalidException)
			{
				var dataInvalidEx = (DataInvalidException)ex;
				FieldErrors = dataInvalidEx.FieldErrors;
			}
            else
            {
                Debug.WriteLine(String.Format("Exception: {0}", ex.ToString()));
                _mvxMessenger.Publish(new ExceptionOcurredMessage(this, ex));
            }
        }


        protected void HandleIsExecuting(bool isLoading, string Text)
        {
            if (isLoading)
            {
                _userDialogs.ShowLoading(Text, MaskType.Black);
            }
            else
            {
                _userDialogs.HideLoading();
            }
        }


        #region commmands 

            public ICommand CloseCommand => new MvxCommand(() => Close(this));

        #endregion


    }
}
