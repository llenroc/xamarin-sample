

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
    using System.IO;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Bullytect.Core.Services;
    using Plugin.Media.Abstractions;

    public abstract class BaseViewModel : MvxReactiveViewModel
    {

        protected readonly IUserDialogs _userDialogs;
        protected readonly IMvxMessenger _mvxMessenger;
        protected readonly IImagesService _imagesService;

        public BaseViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger, IImagesService imagesService)
        {
            _userDialogs = userDialogs;
            _mvxMessenger = mvxMessenger;
            _imagesService = imagesService;
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
                var toastConfig = new ToastConfig(AppResources.Common_Timeout_Operation);
                toastConfig.SetDuration(3000);
                toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(255, 0, 0));

                _userDialogs.Toast(toastConfig);

            }
            else if (ex is HttpRequestException)
            {
                ErrorOccurred = true;

                var toastConfig = new ToastConfig(AppResources.Common_Server_Connection_Error);
                toastConfig.SetDuration(3000);
                toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(255, 0, 0));

                _userDialogs.Toast(toastConfig);

			}
            else if (ex is GenericErrorException) {
                ErrorOccurred = true;
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

		protected IObservable<MediaFile> PickPhotoStream()
		{

            return Observable.FromAsync<string>((_) => _userDialogs.ActionSheetAsync(
                AppResources.Profile_Select_Profile_Image,
                AppResources.Common_Cancel_Operation, null, null,
                new string[] { AppResources.Profile_Select_Profile_Image_From_Camera, AppResources.Profile_Select_Profile_Image_From_Galery }))
                             .Where((action => !action.Equals(AppResources.Common_Cancel_Operation)))
                             .SelectMany((action) =>
                             {

                                 Task<MediaFile> photoSelectedTask;
                                 if (action.Equals(AppResources.Profile_Select_Profile_Image_From_Camera))
                                 {
                                     photoSelectedTask = _imagesService.TakePhotoFromFrontCamera();
                                 }
                                 else
                                 {
                                     photoSelectedTask = _imagesService.PickPhoto();
                                 }

                                return Observable.FromAsync<MediaFile>((_) => photoSelectedTask);
                             });


		}


        #region commmands 

            public ICommand CloseCommand => new MvxCommand(() => Close(this));

        #endregion


    }
}
