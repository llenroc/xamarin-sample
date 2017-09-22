using System;
using System.Collections.Generic;
using Bullytect.Rest.Models.Response;
using Refit;

namespace Bullytect.Rest.Services
{

    #pragma warning disable CS1701

	[Headers("Accept: application/json")]
    public interface IAlertRestService
    {

		[Post("/alerts/self/all/")]
		IObservable<APIResponse<IList<AlertDTO>>> getAllSelfNotifications();

    }
}
