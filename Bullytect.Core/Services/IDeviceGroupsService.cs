
using System;
using Bullytect.Core.Models.Domain;

namespace Bullytect.Core.Services
{
    public interface IDeviceGroupsService
    {

        IObservable<DeviceEntity> saveDevice(string deviceId, string token);

    }
}
