

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
    using System.IO;
    using Newtonsoft.Json;
    using Bullytect.Core.Utils;
    using MvvmCross.Platform.Core;
    using PCLCrypto;
    using System.Text;
    using Bullytect.Core.Services;
    using MvvmCross.Platform;

    public abstract class BaseViewModel : MvxReactiveViewModel
    {

        protected readonly IUserDialogs _userDialogs;
        protected readonly IMvxMessenger _mvxMessenger;
        protected readonly AppHelper _appHelper;

        public BaseViewModel(IUserDialogs userDialogs, 
                             IMvxMessenger mvxMessenger, AppHelper appHelper)
        {
            _userDialogs = userDialogs;
            _mvxMessenger = mvxMessenger;
            _appHelper = appHelper;
            {}

			SignOutCommand = ReactiveCommand
                .CreateFromObservable<Unit, bool>((_) => 
                                                  _appHelper.RequestConfirmation(AppResources.Profile_Confirm_SignOut)
                                                    .Where((confirmed) => confirmed));

            SignOutCommand.ThrownExceptions.Subscribe(HandleExceptions);

			SignOutCommand.Subscribe((_) =>
			{
                if(Settings.Current.DeviceRegistered) {
                    Mvx.Resolve<INotificationService>()?.unsubscribeDevice().Subscribe((result) => {
                        Settings.Current.DeviceRegistered = false;
                        Debug.WriteLine("Device successfully removed from device group ");
                    });
                }

                Bullytect.Core.Config.Settings.AccessToken = null;
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

		bool _isBusy;

		public bool IsBusy
		{
			get => _isBusy;
			set => SetProperty(ref _isBusy, value);
		}

        bool _isTimeout = false;

        public bool IsTimeout
        {
            get => _isTimeout;
            set => SetProperty(ref _isTimeout, value);
        }

		string _loadingText = AppResources.Profile_Saving_Changes;

		public string LoadingText
		{
			get => _loadingText;
			set => SetProperty(ref _loadingText, value);
		}

       
		#region IsDirty

		private string _cleanHash;

		protected string CleanHash
		{
			get { return _cleanHash; }
		}

		private bool? _isDirtyMonitoring;

		/// <summary>
		/// Set this to true to start monitoring for changes to this object.
		/// </summary>
		public bool IsDirtyMonitoring
		{
			get
			{
				if (!_isDirtyMonitoring.HasValue)
				{
					return false;
				}

				return _isDirtyMonitoring.Value;
			}
			set
			{
				if (value)
				{
					// starts the monitoring and stores non-nulls
					// and ignores default bools values in binding 
					// situations where RaiseAllPropertyChanged() has
					// been used
					_isDirtyMonitoring = true;
					// IsDirty = false;
					_cleanHash = GetObjectHash();
				}
			}
		}

		public bool IsDirty
		{
			get
			{
				if (_cleanHash == null)
				{
					return false;
				}

				return !string.IsNullOrEmpty(CleanHash) && GetObjectHash() != CleanHash;
			}
		}

		/// Gets the object hash from the objects property values.
		private string GetObjectHash()
		{
			string md5;
			try
			{
				using (var ms = new MemoryStream())
				{
					using (StreamWriter sw = new StreamWriter(ms))
					{
						using (JsonWriter writer = new JsonTextWriter(sw))
						{
							JsonSerializer serializer = new JsonSerializer
							{
								ContractResolver = IsDirtyViewModelJsonContractResolver.Instance
							};
							serializer.Serialize(writer, this);
							writer.Flush();
							serializer.DisposeIfDisposable();
							md5 = GetMd5Sum(ms.ToArray());
						}
					}
				}
			}
			catch (Exception ex)
			{
				// you should make this more specific really :) OK for testing
				// but since ViewModels are often not directly created
				// throwing exceptions isn't a great idea really
				throw new Exception("Cannot calculate hash.", ex);
			}

			return md5;
		}


		/// <summary>
		/// Gets the MD5 sum from the buffer byte data.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <returns>a string MD5 value</returns>
		private static string GetMd5Sum(byte[] buffer)
		{
            IHashAlgorithmProvider algoProv = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Md5);
		    byte[] hash = algoProv.HashData(buffer);
			var hex = new StringBuilder(hash.Length * 2);
			foreach (byte b in hash)
				hex.AppendFormat("{0:x2}", b);

			return hex.ToString();
        }

		#endregion

		protected virtual void HandleExceptions(Exception ex)
        {


            _userDialogs.HideLoading();

            if (ex is TimeoutOperationException)
            {
                IsTimeout = true;
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


		protected void HandleIsExecutingWithDialogs(bool isLoading, string Text)
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

		protected void HandleIsExecuting(bool isLoading, string Text)
        {
            ResetCommonProps();
            IsBusy = isLoading;
            LoadingText = Text;
        }


		protected void ResetCommonProps(){
            IsTimeout = false;
            ErrorOccurred = false;
            DataFound = true;
            FieldErrors = new Dictionary<string, string>();
        }

        protected virtual void OnBackPressed() => Close(this);
		
        #region commmands 

            public ICommand CloseCommand => new MvxCommand(() => Close(this));

		    public ReactiveCommand<Unit, bool> SignOutCommand { get; protected set; }

            public ICommand BackPressedCommand => new MvxCommand(() => OnBackPressed());

        #endregion


    }
}
