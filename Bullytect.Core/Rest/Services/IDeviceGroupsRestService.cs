

namespace Bullytect.Core.Rest.Services
{

	using System;
	using Refit;
    using Bullytect.Core.Rest.Models.Response;
    using Bullytect.Core.Rest.Models.Request;

#pragma warning disable CS1701

    [Headers("Accept: application/json")]
    public interface IDeviceGroupsRestService
    {
		[Post("/device-groups/devices/save")]
        [Headers("Authorization: Bearer")]
        IObservable<APIResponse<DeviceDTO>> save([Body] SaveDeviceDTO saveDevice);
    }
}
