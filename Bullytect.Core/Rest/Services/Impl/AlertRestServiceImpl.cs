using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reactive.Linq;
using Bullytect.Core.Rest.Models.Response;
using Bullytect.Core.Rest.Utils;

namespace Bullytect.Core.Rest.Services.Impl
{
    public class AlertRestServiceImpl: BaseRestServiceImpl,  IAlertRestService
    {

        public AlertRestServiceImpl(HttpClient client): base(client)
        {
        }

        public IObservable<int> ClearAlertsOfSon(string SonId)
        {
            return Observable.FromAsync(() => DeleteData<int>(ApiEndpoints.CLEAR_ALERTS_FOR_SON.Replace(":id", SonId)));
        }

        public IObservable<int> ClearSelfAlerts()
        {
            return Observable.FromAsync(() => DeleteData<int>(ApiEndpoints.CLEAR_SELF_ALERTS));
        }

        public IObservable<string> DeleteAlertOfSon(string SonId, string AlertId)
        {
            return Observable.FromAsync(() => DeleteData<string>(ApiEndpoints.DELETE_ALERT.Replace(":son", SonId).Replace(":alert", AlertId)));
        }

        public IObservable<APIResponse<IList<AlertDTO>>> GetAlertsBySon(string SonId)
        {
            return Observable.FromAsync(() => GetData<APIResponse<IList<AlertDTO>>>(ApiEndpoints.GET_ALERTS_BY_SON.Replace(":id", SonId)));
        }

        public IObservable<APIResponse<AlertsPageDTO>> GetLastSelfAlerts(int Count, int LastMinutes, String[] Levels)
        {
			var queryParams = new Dictionary<string, string>()
			{
				{ "count", Count.ToString()},
				{ "last_minutes", LastMinutes.ToString()}
            };

            if (Levels?.Length > 0)
                queryParams.Add("levels", string.Join(",", Levels));

            return Observable.FromAsync(() => GetData<APIResponse<AlertsPageDTO>>(new Uri(ApiEndpoints.GET_LAST_SELF_ALERTS).AttachParameters(queryParams)));
        }

        public IObservable<APIResponse<IList<AlertDTO>>> GetSelfAlerts()
        {
            return Observable.FromAsync(() => GetData<APIResponse<IList<AlertDTO>>>(ApiEndpoints.GET_SELF_ALERTS));
        }
    }
}
