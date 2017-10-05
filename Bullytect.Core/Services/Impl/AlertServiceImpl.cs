using System;
using Bullytect.Core.Rest.Services;
using System.Reactive.Linq;
using Bullytect.Core.Rest.Models.Response;
using AutoMapper;
using System.Diagnostics;
using Bullytect.Core.Models.Domain;

namespace Bullytect.Core.Services.Impl
{
    public class AlertServiceImpl: BaseService, IAlertService
    {

        readonly IAlertRestService _alertRestService;

        public AlertServiceImpl(IAlertRestService alertRestService)
        {
            _alertRestService = alertRestService;
        }

        public IObservable<AlertsPageEntity> GetLast10AlertsForSelfParent()
        {
			Debug.WriteLine("Get Last 10 Alerts For Self Parent");

			var observable = _alertRestService
                .GetSelfAlerts(10)
				.Select((APIResponse<AlertsPageDTO> response) => response.Data)
				.Select((AlertsPage) => Mapper.Map<AlertsPageDTO, AlertsPageEntity>(AlertsPage))
				.Finally(() => {
					Debug.WriteLine("Get Last 10 Alerts For Self Parent finished ...");
				});

			return operationDecorator(observable);
        }
    }
}
