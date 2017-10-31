using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Bullytect.Core.Config;
using Bullytect.Core.I18N;
using Bullytect.Core.Services;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace Bullytect.Core.Helpers
{
    public class AppHelper
    {

        protected readonly IImagesService _imagesService;
        protected readonly IUserDialogs _userDialogs;


        public AppHelper(IUserDialogs userDialogs, IImagesService imagesService)
        {
            _imagesService = imagesService;
            _userDialogs = userDialogs;

        }

        public void Toast(string Message, System.Drawing.Color color) {
			var toastConfig = new ToastConfig(Message);
			toastConfig.SetDuration(SharedConfig.COMMON_TOAST_DURATION);
			toastConfig.SetBackgroundColor(color);
			_userDialogs.Toast(toastConfig);
        }

        public IObservable<bool> RequestConfirmation(string Message) {

			return Observable.FromAsync<bool>((_) => _userDialogs.ConfirmAsync(new ConfirmConfig()
			{
				Title = Message
                 

			})).Where((confirmed) => confirmed);

        }

        public void ShowAlert(string Message) {

            _userDialogs.Alert(Message);

        }

		public IObservable<MediaFile> PickPhotoStream()
		{

			return Observable.FromAsync<string>((_) => _userDialogs.ActionSheetAsync(
                AppResources.Profile_Select_Profile_Image,
                AppResources.Common_Cancel_Operation, null, null,
				new string[] { AppResources.Profile_Select_Profile_Image_From_Camera, AppResources.Profile_Select_Profile_Image_From_Galery }))
							 .Select((action => !action.Equals(AppResources.Common_Cancel_Operation)))
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
    }
}
