
using Bullytect.Core.Exceptions;
using Plugin.Media;
using System.IO;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;

namespace Bullytect.Core.Services.Impl
{
    public class ImagesServiceImpl: BaseService, IImagesService
    {

        public async Task<MediaFile> PickPhoto()
        {
            await CrossMedia.Current.Initialize();

			var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions()
			{


			});

            return file;
        }

        public async Task<MediaFile> TakePhotoFromFrontCamera()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{
                throw new CanNotTakePhotoFromCameraException();
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
			{
				SaveToAlbum = true,
				DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front
			});

           if (file == null)
                throw new CanNotTakePhotoFromCameraException();

   
			return file;
        }
    }
}
