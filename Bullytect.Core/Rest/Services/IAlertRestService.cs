using System;
using System.Collections.Generic;
using Refit;
using Bullytect.Core.Rest.Models.Response;

namespace Bullytect.Core.Rest.Services
{

    #pragma warning disable CS1701

	[Headers("Accept: application/json")]
    public interface IAlertRestService
    {

		[Post("/alerts/self/all/")]
		IObservable<APIResponse<IList<AlertDTO>>> getAllSelfNotifications();

    }
}
