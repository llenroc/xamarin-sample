
namespace Bullytect.Core.Services
{

    using System.IO;
    using System.Threading.Tasks;
    using Plugin.Media.Abstractions;

    public interface IImagesService
    {
        Task<MediaFile> TakePhotoFromFrontCamera();
        Task<MediaFile> PickPhoto();
    }
}
