using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reactive.Linq;
using Bullytect.Core.Rest.Models.Request;
using Bullytect.Core.Rest.Models.Response;
using Bullytect.Core.Rest.Utils;

namespace Bullytect.Core.Rest.Services.Impl
{
    public class DeviceGroupsRestServiceImpl: BaseRestServiceImpl,  IDeviceGroupsRestService
    {

        public DeviceGroupsRestServiceImpl(HttpClient client): base(client)
        {
        }

        public IObservable<APIResponse<string>> Delete(string DeviceId)
        {
            return Observable.FromAsync(() => DeleteData<APIResponse<string>>(ApiEndpoints.DELETE_DEVICE_FROM_GROUP.Replace(":id", DeviceId)));
        }

        public IObservable<APIResponse<DeviceDTO>> save(SaveDeviceDTO saveDevice)
        {
            Debug.WriteLine("Device Id" + saveDevice.DeviceId);
            Debug.WriteLine("Registration Token" + saveDevice.RegistrationToken);

            return Observable.FromAsync(() => PostData<APIResponse<DeviceDTO>, SaveDeviceDTO>(ApiEndpoints.SAVE_DEVICE, saveDevice));
        }
    }
}
