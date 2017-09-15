
using System;
using Foundation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.iOS;
using MvvmCross.Platform;
using Plugin.FirebasePushNotification;
using UIKit;

namespace Bullytect.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxFormsApplicationDelegate
    {

		public override UIWindow Window { get; set; }

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			Window = new UIWindow(UIScreen.MainScreen.Bounds);

            global::Xamarin.Forms.Forms.Init();

			var setup = new Setup(this, Window);
			setup.Initialize();

			var startup = Mvx.Resolve<IMvxAppStart>();
			startup.Start();

			XFGloss.iOS.Library.Init();

			LoadApplication(setup.FormsApplication);

			Window.MakeKeyAndVisible();

            FirebasePushNotificationManager.Initialize(options, true);

			return true;
		}


		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
            #if DEBUG
            	FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken, FirebaseTokenType.Sandbox);
            #endif
            #if RELEASE
                                FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken,FirebaseTokenType.Production);
            #endif

		}

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);

		}

		// To receive notifications in foregroung on iOS 9 and below.
		// To receive notifications in background in any iOS version
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			// If you are receiving a notification message while your app is in the background,
			// this callback will not be fired 'till the user taps on the notification launching the application.

			// If you disable method swizzling, you'll need to call this method. 
			// This lets FCM track message delivery and analytics, which is performed
			// automatically with method swizzling enabled.
			FirebasePushNotificationManager.DidReceiveMessage(userInfo);
			// Do your magic to handle the notification data
			System.Console.WriteLine(userInfo);
		}

		public override void OnActivated(UIApplication uiApplication)
		{
			FirebasePushNotificationManager.Connect();
			base.OnActivated(uiApplication);

		}
		public override void DidEnterBackground(UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
			FirebasePushNotificationManager.Disconnect();
		}
    }
}
