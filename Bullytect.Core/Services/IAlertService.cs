
namespace Bullytect.Core.Services
{

    using System;
    using System.Collections.Generic;
    using Bullytect.Core.Models.Domain;
    using Bullytect.Core.ViewModels.Core.Models;

    public interface IAlertService
    {
        IObservable<AlertsPageEntity> GetLastAlertsForSelfParent();
        IObservable<IList<AlertEntity>> GetSelfAlerts();
        IObservable<string> ClearAlertsOfSon(string SonId);
        IObservable<string> DeleteAlertOfSon(string SonId, string AlertId);
        IObservable<IList<AlertEntity>> GetAlertsBySon(string SonId);
        IObservable<string> ClearSelfAlerts();
        IObservable<IList<AlertCategoryModel>> GetAllAlertsCategories();
        IObservable<IList<AlertsBySon>> GetAlertsBySon();
    }
}
