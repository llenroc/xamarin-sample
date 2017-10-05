
namespace Bullytect.Core.Services
{

    using System;
    using Bullytect.Core.Models.Domain;

    public interface IAlertService
    {
        IObservable<AlertsPageEntity> GetLast10AlertsForSelfParent();
    }
}
