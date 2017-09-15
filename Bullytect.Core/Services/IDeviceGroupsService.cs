
using System;
using System.Threading.Tasks;
using Bullytect.Core.Models.Domain;

namespace Bullytect.Core.Services
{
    public interface IDeviceGroupsService
    {

        IObservable<DeviceEntity> saveDevice(string deviceId, string token);

    }
}
