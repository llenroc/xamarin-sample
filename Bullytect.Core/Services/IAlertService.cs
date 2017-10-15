
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
        IObservable<int> ClearAlertsOfSon(string SonId);
        IObservable<string> DeleteAlertOfSon(string SonId, string AlertId);
        IObservable<IList<AlertEntity>> GetAlertsBySon(string SonId);
        IObservable<int> ClearSelfAlerts();
        IObservable<IList<AlertCategoryModel>> GetAllAlertsCategories();
    }
}
