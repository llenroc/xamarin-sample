using System;
using System.Diagnostics;
using Android.App;
using Android.Runtime;
using Plugin.PushNotification;

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
			    PushNotificationManager.Initialize(this, true);
            #else
              PushNotificationManager.Initialize(this,false);
            #endif

			//Handle notification when app is closed here
			CrossPushNotification.Current.OnNotificationReceived += (s, p) =>
			{


			};
		}
	}
}
