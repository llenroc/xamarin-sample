using System;
using System.Diagnostics;
using Android.App;
using Android.Runtime;
using Plugin.FirebasePushNotification;

namespace Bullytect.Droid
{
	[Application]
	public class MainApplication : Application
	{
		public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
		{
		}

		public override void OnCreate()
		{
			base.OnCreate();

            Debug.WriteLine("Init Main Application ...");

			// If debug you should reset the token each time.
            #if DEBUG
			    FirebasePushNotificationManager.Initialize(this, true);
#else
              FirebasePushNotificationManager.Initialize(this,false);
#endif

			//Handle notification when app is closed here
			CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
			{
                Debug.WriteLine("Notification Received");

			};
		}
	}
}
