﻿using System;
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

        public IObservable<APIResponse<DeviceDTO>> save(SaveDeviceDTO saveDevice)
        {
            return Observable.FromAsync(() => PostData<APIResponse<DeviceDTO>, SaveDeviceDTO>(ApiEndpoints.SAVE_DEVICE, saveDevice));
        }
    }
}