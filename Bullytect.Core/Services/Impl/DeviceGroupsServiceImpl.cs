using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using Bullytect.Core.Models.Domain;
using Bullytect.Rest.Models.Response;
using Bullytect.Rest.Services;

namespace Bullytect.Core.Services.Impl
{
    public class DeviceGroupsServiceImpl: IDeviceGroupsService
    {

		readonly IDeviceGroupsRestService _deviceGroupsRestService;

		public DeviceGroupsServiceImpl(IDeviceGroupsRestService deviceGroupsRestService)
		{
			_deviceGroupsRestService = deviceGroupsRestService;
		}
   

        async public Task<DeviceEntity> saveToken(string token)
        {
            Debug.WriteLine("Save Token ...");

            return await _deviceGroupsRestService.saveToken(token)
									.ContinueWith(t => t.Result.Data, TaskContinuationOptions.OnlyOnRanToCompletion)
									.ContinueWith(t => Mapper.Map<DeviceDTO, DeviceEntity>(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}
