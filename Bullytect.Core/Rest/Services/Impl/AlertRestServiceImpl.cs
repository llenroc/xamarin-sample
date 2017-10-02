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

        public IObservable<APIResponse<IList<AlertDTO>>> getAllSelfNotifications()
        {
            return Observable.FromAsync(() => GetData<APIResponse<IList<AlertDTO>>>(ApiEndpoints.GET_ALL_SELF_ALERTS));

		}
    }
}
