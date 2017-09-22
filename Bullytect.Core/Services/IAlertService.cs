
namespace Bullytect.Core.Services
{

    using System;
    using System.Collections.Generic;
    using Bullytect.Core.Models.Domain;

    public interface IAlertService
    {
        IObservable<IList<AlertEntity>> GetAllSelfNotifications();
    }
}
