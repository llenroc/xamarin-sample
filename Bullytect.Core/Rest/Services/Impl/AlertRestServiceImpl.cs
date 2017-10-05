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


        public IObservable<APIResponse<AlertsPageDTO>> GetSelfAlerts(int count)
        {
			return Observable.FromAsync(() => GetData<APIResponse<AlertsPageDTO>>(new Uri(ApiEndpoints.GET_SELF_ALERTS).AttachParameters(new Dictionary<string, string>()
			{
				{ "count", "10"}
			})));
        }
    }
}
