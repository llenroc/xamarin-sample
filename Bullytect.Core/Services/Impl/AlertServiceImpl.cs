using System;
using Bullytect.Core.Rest.Services;
using System.Reactive.Linq;
using Bullytect.Core.Rest.Models.Response;
using AutoMapper;
using System.Diagnostics;
using Bullytect.Core.Models.Domain;
using System.Collections.Generic;
using System.Collections;

namespace Bullytect.Core.Services.Impl
{
    public class AlertServiceImpl: BaseService, IAlertService
    {

        readonly IAlertRestService _alertRestService;

        public AlertServiceImpl(IAlertRestService alertRestService)
        {
            _alertRestService = alertRestService;
        }

        public IObservable<int> ClearAlertsOfSon(string SonId)
        {

            Debug.WriteLine("Clear Alerts Of Son with id : " + SonId);

			var observable = _alertRestService
                .ClearAlertsOfSon(SonId)
				.Finally(() => {
					Debug.WriteLine("Clear Alerts Of Son finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<int> ClearSelfAlerts()
        {
            Debug.WriteLine("Clear Self Alerts");

			var observable = _alertRestService
                .ClearSelfAlerts()
				.Finally(() => {
					Debug.WriteLine("Clear Self Alerts Finished ");
				});

			return operationDecorator(observable);
        }

        public IObservable<string> DeleteAlertOfSon(string SonId, string AlertId)
        {
			Debug.WriteLine("Delete Alerts Of Son : " + SonId);

			var observable = _alertRestService
				.DeleteAlertOfSon(SonId, AlertId)
				.Finally(() => {
					Debug.WriteLine("Delete Alerts Of Son finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<IList<AlertEntity>> GetAlertsBySon(string SonId)
        {
			Debug.WriteLine("Get Alerts By Son : " + SonId);

			var observable = _alertRestService
				.GetAlertsBySon(SonId)
                .Select((response) => response.Data)
				.Select((Alerts) => Mapper.Map<IList<AlertDTO>, IList<AlertEntity>>(Alerts))
				.Finally(() => {
					Debug.WriteLine("Get Alerts By Son Finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<AlertsPageEntity> GetLast10AlertsForSelfParent()
        {
			Debug.WriteLine("Get Last 10 Alerts For Self Parent");

			var observable = _alertRestService
                .GetLastSelfAlerts(10)
				.Select((APIResponse<AlertsPageDTO> response) => response.Data)
				.Select((AlertsPage) => Mapper.Map<AlertsPageDTO, AlertsPageEntity>(AlertsPage))
				.Finally(() => {
					Debug.WriteLine("Get Last 10 Alerts For Self Parent finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<IList<AlertEntity>> GetSelfAlerts()
        {
			Debug.WriteLine("Get Self Alerts");

			var observable = _alertRestService
                .GetSelfAlerts()
                .Select((response) => response.Data)
                .Select((Alerts) => Mapper.Map<IList<AlertDTO>, IList<AlertEntity>>(Alerts))
				.Finally(() => {
					Debug.WriteLine("Get Self Alerts ...");
				});

			return operationDecorator(observable);
        }
    }
}
