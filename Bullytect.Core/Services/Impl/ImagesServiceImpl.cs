
using System.IO;
using System.Threading.Tasks;
using Plugin.Media;

namespace Bullytect.Core.Services.Impl
{
    public class ImagesServiceImpl: BaseService, IImagesService
    {

        public async Task<Stream> PickPhoto()
        {
			await CrossMedia.Current.Initialize();

			var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions()
			{


			});

			var stream = file.GetStream();
			file.Dispose();
            return stream;
        }

        public async Task<Stream> TakePhotoFromFrontCamera()
        {
			await CrossMedia.Current.Initialize();

			/*if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{

			}*/

			var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
			{
				SaveToAlbum = true,
				DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front
			});

			/*if (file == null)
				return;*/

			//await DisplayAlert("File Location", file.Path, "OK");

			var stream = file.GetStream();
			file.Dispose();
			return stream;
        }
    }
}
