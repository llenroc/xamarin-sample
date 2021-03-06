﻿using System;
using System.Collections.Generic;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Rest.Models.Response;

namespace Bullytect.Core.Rest.Services
{

    #pragma warning disable CS1701

    public interface IAlertRestService
    {

        IObservable<APIResponse<AlertsPageDTO>> GetLastSelfAlerts(int Count, int LastMinutes, String[] Levels);
        IObservable<APIResponse<IList<AlertDTO>>> GetSelfAlerts(int Count, int DaysAgo, IList<AlertLevelEnum> Levels);
        IObservable<APIResponse<IList<AlertDTO>>> GetAlertsBySon(string SonId, int Count, int DaysAgo, IList<AlertLevelEnum> Levels);
        IObservable<APIResponse<string>> ClearAlertsOfSon(string SonId);
        IObservable<APIResponse<string>> DeleteAlertOfSon(string SonId, string AlertId);
        IObservable<APIResponse<string>> ClearSelfAlerts();

    }
}
