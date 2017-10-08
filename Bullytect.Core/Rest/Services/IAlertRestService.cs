using System;
using System.Collections.Generic;
using Bullytect.Core.Rest.Models.Response;

namespace Bullytect.Core.Rest.Services
{

    #pragma warning disable CS1701

    public interface IAlertRestService
    {

        IObservable<APIResponse<AlertsPageDTO>> GetLastSelfAlerts(int Count, bool OnlyNews, String[] Levels);
        IObservable<APIResponse<IList<AlertDTO>>> GetSelfAlerts();
        IObservable<APIResponse<IList<AlertDTO>>> GetAlertsBySon(string SonId);
        IObservable<int> ClearAlertsOfSon(string SonId);
        IObservable<string> DeleteAlertOfSon(string SonId, string AlertId);
        IObservable<int> ClearSelfAlerts();

    }
}
