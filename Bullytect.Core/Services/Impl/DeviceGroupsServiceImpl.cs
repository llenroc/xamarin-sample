namespace Bullytect.Core.Services.Impl
{

	using System;
	using System.Diagnostics;
	using System.Reactive.Linq;
	using AutoMapper;
    using Bullytect.Core.Rest.Services;
    using Bullytect.Core.Models.Domain;
    using Bullytect.Core.Rest.Models.Request;
    using Bullytect.Core.Rest.Models.Response;

    public class DeviceGroupsServiceImpl: BaseService, IDeviceGroupsService
    {

		readonly IDeviceGroupsRestService _deviceGroupsRestService;

		public DeviceGroupsServiceImpl(IDeviceGroupsRestService deviceGroupsRestService)
		{
			_deviceGroupsRestService = deviceGroupsRestService;
		}

        public IObservable<string> Delete(string DeviceId)
		{
			Debug.WriteLine("Delete Device ...");
			var observable = _deviceGroupsRestService
                .Delete(DeviceId)
				.Select(response => response.Data)
				.Finally(() => {
					Debug.WriteLine("Dlete Device finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<DeviceEntity> saveDevice(string deviceId, string token)
        {
            Debug.WriteLine("Save Token ...");
            var observable =  _deviceGroupsRestService
                .save(new SaveDeviceDTO()
                {
                    DeviceId = deviceId,
                    RegistrationToken = token
                })
                .Select(response => response.Data)
                .Select((DeviceDTO deviceDto) => Mapper.Map<DeviceDTO, DeviceEntity>(deviceDto))
                .Finally(() => {
                    Debug.WriteLine("Save Device finished ...");
                });

            return operationDecorator(observable);
        }
    }
}
