
namespace Bullytect.Core.Services
{

    using System;
    using System.Collections.Generic;
    using Bullytect.Core.Models.Domain;

    public interface IAlertService
    {
        IObservable<AlertsPageEntity> GetLast10AlertsForSelfParent();
        IObservable<IList<AlertEntity>> GetSelfAlerts();
        IObservable<int> ClearAlertsOfSon(string SonId);
        IObservable<string> DeleteAlertOfSon(string SonId, string AlertId);
        IObservable<IList<AlertEntity>> GetAlertsBySon(string SonId);
        IObservable<int> ClearSelfAlerts();
    }
}
