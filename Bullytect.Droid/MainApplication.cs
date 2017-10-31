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

            #if DEBUG
			    FirebasePushNotificationManager.Initialize(this, true);
#else
              FirebasePushNotificationManager.Initialize(this,false);
#endif
        }
	}
}
