using System;
using System.Diagnostics;
using Bullytect.Core.Config;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.ViewModels;
using MvvmCross.Core.Navigation;
using MvvmCross.Platform;
using Plugin.DeviceInfo;
using Plugin.FirebasePushNotification;

namespace Bullytect.Core.Services.Impl
{
    public class NotificationServiceImpl : INotificationService
    {

        readonly IDeviceGroupsService _deviceGroupsService;

        public NotificationServiceImpl(IDeviceGroupsService deviceGroupsService)
        {
            _deviceGroupsService = deviceGroupsService;

            initNotificationHandlers();
        }

        protected void initNotificationHandlers()
        {

            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                Debug.WriteLine($"TOKEN REC: {p.Token}");
            };

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) => {
                System.Diagnostics.Debug.WriteLine("Received");
                System.Diagnostics.Debug.WriteLine(p.Data);
            };

			CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
			{
				System.Diagnostics.Debug.WriteLine("Opened");
				if (Settings.AccessToken != null)
				{
					Mvx.Resolve<IMvxNavigationService>()?.Navigate<AlertsViewModel>();
				}

			};
        }

        public IObservable<DeviceEntity> subscribeDevice()
        {
			Debug.WriteLine(String.Format("Device Id: {0}", CrossDeviceInfo.Current.Id));
			Debug.WriteLine(String.Format("FCM Token: {0}", CrossFirebasePushNotification.Current.Token));
			return _deviceGroupsService.saveDevice(CrossDeviceInfo.Current.Id, CrossFirebasePushNotification.Current.Token);
        }

        public IObservable<string> unsubscribeDevice()
        {
            Debug.WriteLine(String.Format("Unsubscribe Device : {0}", CrossDeviceInfo.Current.Id));
            return _deviceGroupsService.Delete(CrossDeviceInfo.Current.Id);
        }

    }
}
