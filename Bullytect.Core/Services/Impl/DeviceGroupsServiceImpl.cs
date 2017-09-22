﻿namespace Bullytect.Core.Services.Impl
{

	using System;
	using System.Diagnostics;
	using System.Reactive.Linq;
	using AutoMapper;
	using Bullytect.Core.Models.Domain;
	using Bullytect.Rest.Models.Request;
	using Bullytect.Rest.Models.Response;
	using Bullytect.Rest.Services;

    public class DeviceGroupsServiceImpl: BaseService, IDeviceGroupsService
    {

		readonly IDeviceGroupsRestService _deviceGroupsRestService;

		public DeviceGroupsServiceImpl(IDeviceGroupsRestService deviceGroupsRestService)
		{
			_deviceGroupsRestService = deviceGroupsRestService;
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
