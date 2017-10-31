

namespace Bullytect.Core.Rest.Services
{

	using System;
    using Bullytect.Core.Rest.Models.Response;
    using Bullytect.Core.Rest.Models.Request;

#pragma warning disable CS1701

    public interface IDeviceGroupsRestService
    {
        IObservable<APIResponse<DeviceDTO>> save(SaveDeviceDTO saveDevice);
        IObservable<APIResponse<string>> Delete(string DeviceId);
    }
}
