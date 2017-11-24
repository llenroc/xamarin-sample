using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reactive.Linq;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Rest.Models.Response;
using Bullytect.Core.Rest.Utils;

namespace Bullytect.Core.Rest.Services.Impl
{
    public class AlertRestServiceImpl: BaseRestServiceImpl,  IAlertRestService
    {

        public AlertRestServiceImpl(HttpClient client): base(client)
        {
        }

        public IObservable<APIResponse<string>> ClearAlertsOfSon(string SonId)
        {
            return Observable.FromAsync(() => DeleteData<APIResponse<string>>(ApiEndpoints.CLEAR_ALERTS_FOR_SON.Replace(":id", SonId)));
        }

        public IObservable<APIResponse<string>> ClearSelfAlerts()
        {
            return Observable.FromAsync(() => DeleteData<APIResponse<string>>(ApiEndpoints.CLEAR_SELF_ALERTS));
        }

        public IObservable<APIResponse<string>> DeleteAlertOfSon(string SonId, string AlertId)
        {
            return Observable.FromAsync(() => DeleteData<APIResponse<string>>(ApiEndpoints.DELETE_ALERT.Replace(":son", SonId).Replace(":alert", AlertId)));
        }

        public IObservable<APIResponse<IList<AlertDTO>>> GetAlertsBySon(string SonId, int Count, int DaysAgo, IList<AlertLevelEnum> Levels)
        {
            var queryParams = new Dictionary<string, string>()
            {
                { "count", Count.ToString()},
                { "days_ago", DaysAgo.ToString()}
            };

            if (Levels?.Count > 0)
                queryParams.Add("levels", string.Join(",", Levels));
            
            return Observable.FromAsync(() => GetData<APIResponse<IList<AlertDTO>>>(new Uri(ApiEndpoints.GET_ALERTS_BY_SON.Replace(":id", SonId)).AttachParameters(queryParams)));
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

        public IObservable<APIResponse<IList<AlertDTO>>> GetSelfAlerts(int Count, int DaysAgo, IList<AlertLevelEnum> Levels)
        {
            var queryParams = new Dictionary<string, string>()
            {
                { "count", Count.ToString()},
                { "days_ago", DaysAgo.ToString()}
            };

            if (Levels?.Count > 0)
                queryParams.Add("levels", string.Join(",", Levels));

            return Observable.FromAsync(() => GetData<APIResponse<IList<AlertDTO>>>(new Uri(ApiEndpoints.GET_SELF_ALERTS).AttachParameters(queryParams)));
        }
    }
}
