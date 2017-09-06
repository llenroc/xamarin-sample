
using System.Threading.Tasks;
using Bullytect.Core.Models.Domain;

namespace Bullytect.Core.Services
{
    public interface IDeviceGroupsService
    {

        Task<DeviceEntity> saveToken(string token);

    }
}
