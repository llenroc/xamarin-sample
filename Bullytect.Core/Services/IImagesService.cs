
namespace Bullytect.Core.Services
{

    using System.IO;
    using System.Threading.Tasks;

    public interface IImagesService
    {
        Task<Stream> TakePhotoFromFrontCamera();
        Task<Stream> PickPhoto();
    }
}
