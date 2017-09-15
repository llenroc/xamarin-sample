

namespace Bullytect.Rest.Services
{

	using System;
	using System.Threading.Tasks;
	using Bullytect.Rest.Models.Request;
	using Bullytect.Rest.Models.Response;
	using Refit;

    #pragma warning disable CS1701

	[Headers("Accept: application/json")]
    public interface IDeviceGroupsRestService
    {
		[Post("/device-groups/devices/save")]
        IObservable<APIResponse<DeviceDTO>> save([Body] SaveDeviceDTO saveDevice);
    }
}
