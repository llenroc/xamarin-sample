using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using AutoMapper;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Utils;
using Refit;
using Bullytect.Core.Rest.Services;
using Bullytect.Core.Rest.Models.Response;

namespace Bullytect.Core.Services.Impl
{
    public class AlertServiceImpl: BaseService, IAlertService
    {

        readonly IAlertRestService _alertRestService;

        public AlertServiceImpl(IAlertRestService alertRestService)
        {
            _alertRestService = alertRestService;
        }

        public IObservable<IList<AlertEntity>> GetAllSelfNotifications()
        {
            Debug.WriteLine("Get All Self Notifications");

            var observable = _alertRestService
                .getAllSelfNotifications()
                .Select((response) => response.Data)
                .Select((alerts) => Mapper.Map<IList<AlertDTO>, IList<AlertEntity>>(alerts))
                .Finally(() =>
                {
                    Debug.WriteLine("Get All Self Notifications finished ...");
                });


            return operationDecorator(observable);              

        }
    }
}
