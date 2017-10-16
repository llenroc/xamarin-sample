using System;
using System.Collections.Generic;
using Bullytect.Core.Rest.Models.Response;

namespace Bullytect.Core.Rest.Services
{

    #pragma warning disable CS1701

    public interface IAlertRestService
    {

        IObservable<APIResponse<AlertsPageDTO>> GetLastSelfAlerts(int Count, int LastMinutes, String[] Levels);
        IObservable<APIResponse<IList<AlertDTO>>> GetSelfAlerts();
        IObservable<APIResponse<IList<AlertDTO>>> GetAlertsBySon(string SonId);
        IObservable<APIResponse<string>> ClearAlertsOfSon(string SonId);
        IObservable<APIResponse<string>> DeleteAlertOfSon(string SonId, string AlertId);
        IObservable<APIResponse<string>> ClearSelfAlerts();

    }
}
