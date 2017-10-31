using System;
using Bullytect.Core.Models.Domain;

namespace Bullytect.Core.Services
{
    public interface INotificationService
    {
        IObservable<DeviceEntity> subscribeDevice();
        IObservable<string> unsubscribeDevice();
    }
}
