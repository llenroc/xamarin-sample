using System;
using System.Collections.Generic;
using Bullytect.Core.Rest.Models.Response;

namespace Bullytect.Core.Rest.Services
{

    #pragma warning disable CS1701

    public interface IAlertRestService
    {

		IObservable<APIResponse<IList<AlertDTO>>> getAllSelfNotifications();

    }
}
