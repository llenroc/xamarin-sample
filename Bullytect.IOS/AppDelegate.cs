
using System;
using Bullytect.Core.OAuth.Models;
using CarouselView.FormsPlugin.iOS;
using FFImageLoading.Forms.Touch;
using Foundation;
using Lottie.Forms.iOS.Renderers;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.iOS;
using MvvmCross.Platform;
using Plugin.PushNotification;
using Refractored.XamForms.PullToRefresh.iOS;
using UIKit;
using UXDivers.Artina.Shared;

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
			CachedImageRenderer.Init(); // Initializing FFImageLoading
			AnimationViewRenderer.Init(); // Initializing Lottie
			PullToRefreshLayoutRenderer.Init();
			XFGloss.iOS.Library.Init();
            CarouselViewRenderer.Init();
			// Presenters Initialization
			global::Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();

            GrialKit.Init(new ThemeColors(), "Bullytect.iOS.GrialLicense");
           
			FormsHelper.ForceLoadingAssemblyContainingType(typeof(UXDivers.Effects.Effects));
			FormsHelper.ForceLoadingAssemblyContainingType<UXDivers.Effects.iOS.CircleEffect>();

			var setup = new Setup(this, Window);
			setup.Initialize();

			var startup = Mvx.Resolve<IMvxAppStart>();
			startup.Start();

			LoadApplication(setup.FormsApplication);

			Window.MakeKeyAndVisible();

            PushNotificationManager.Initialize(options, true);

			return true;
		}


		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			PushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
		}

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			PushNotificationManager.RemoteNotificationRegistrationFailed(error);

		}
		// To receive notifications in foregroung on iOS 9 and below.
		// To receive notifications in background in any iOS version
		public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{

			PushNotificationManager.DidReceiveMessage(userInfo);
		}

		public override bool OpenUrl ( UIApplication application, NSUrl url, 
                                      string sourceApplication, NSObject annotation)
		{
            #if DEBUG
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.AppendLine("OpenURL Called");
			sb.Append("     url         = ").AppendLine(url.AbsoluteUrl.ToString());
			sb.Append("     application = ").AppendLine(sourceApplication);
			sb.Append("     annotation  = ").AppendLine(annotation?.ToString());
			System.Diagnostics.Debug.WriteLine(sb.ToString());
            #endif

			// Convert iOS NSUrl to C#/netxf/BCL System.Uri - common API
			Uri uri_netfx = new Uri(url.AbsoluteString);

			// load redirect_url Page
			AuthenticationState.Authenticator.OnPageLoading(uri_netfx);

			return true;
		}
    }
}
