using System;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using ReactiveUI;

namespace Bullytect.Core.Utils
{
    public static class CommandFactory
    {

        public static ReactiveCommand<string, ImageEntity> CreateTakePhotoCommand(IParentService parentService, IImagesService imagesService, IUserDialogs userDialogs) {

			return ReactiveCommand.CreateFromObservable<string, ImageEntity>((param) =>
			{


				return Observable.FromAsync<string>((_) => userDialogs.ActionSheetAsync(AppResources.Profile_Select_Profile_Image,
				AppResources.Common_Cancel_Operation, null, null, new string[] { AppResources.Profile_Select_Profile_Image_From_Camera, AppResources.Profile_Select_Profile_Image_From_Galery }))
								 .Where((action => !action.Equals(AppResources.Common_Cancel_Operation)))
								 .SelectMany((action) => {

									 Task<Stream> photoSelectedTask;
									 if (action.Equals(AppResources.Profile_Select_Profile_Image_From_Camera))
									 {
										 photoSelectedTask = imagesService.TakePhotoFromFrontCamera();
									 }
									 else
									 {
										 photoSelectedTask = imagesService.PickPhoto();
									 }

									 return Observable.FromAsync<Stream>((_) => photoSelectedTask);
								 }).Do((_) => userDialogs.ShowLoading(AppResources.Profile_Updating_Profile_Image))
								 .SelectMany((FileStream) => parentService.UploadProfileImage(FileStream)).Do((_) => userDialogs.HideLoading());


			});

        }
    }
}
