using System;
using System.Threading.Tasks;
using Bullytect.Rest.Models.Request;
using Bullytect.Rest.Models.Response;
using Refit;

namespace Bullytect.Rest.Services
{

    #pragma warning disable CS1701

	[Headers("Accept: application/json")]
    public interface IDeviceGroupsRestService
    {
		[Put("/device-groups/devices/{token}/save")]
		Task<APIResponse<DeviceDTO>> saveToken(string token);

    }
}
