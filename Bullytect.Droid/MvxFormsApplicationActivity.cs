using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Acr.UserDialogs;
using Refractored.XamForms.PullToRefresh.Droid;
using UXDivers.Artina.Shared;
using FFImageLoading.Forms.Droid;
using UXDivers.Artina.Shared.Droid;
using MvvmCross.Forms.Droid;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Presenters;
using MvvmCross.Droid.Views;
using Xamarin.Forms.Platform.Android;
using MvvmCross.Droid.Platform;
using Plugin.Permissions;
using CarouselView.FormsPlugin.Android;
using Plugin.PushNotification;
using Bullytect.Core;

namespace Bullytect.Droid
{


	[Activity(
        Name = "com.usal.bisite.bulltect.MvxFormsApplicationActivity",
        Label = "bulltect",
        Icon = "@mipmap/ic_launcher",
        Theme = "@style/AppTheme",
        MainLauncher = false,
		LaunchMode = LaunchMode.SingleTask,
        ScreenOrientation = ScreenOrientation.Portrait
    )]
    public class MvxFormsApplicationActivity : FormsAppCompatActivity
    {

		MvxFormsApplication _formsApplication;
		protected MvxFormsApplication FormsApplication
		{
			get
			{
				if (_formsApplication == null)
				{
					var formsPresenter = (IMvxFormsPagePresenter)Mvx.Resolve<IMvxAndroidViewPresenter>();
					_formsApplication = formsPresenter.FormsApplication;
				}
				return _formsApplication;
			}
		}


        protected override void OnCreate(Bundle bundle)
        {
			try
			{
    			ToolbarResource = Resource.Layout.Toolbar;
    			TabLayoutResource = Resource.Layout.Tabs;

    			base.OnCreate(bundle);

				App.ScreenWidth = (int)((int)Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density); // real pixels
				App.ScreenHeight = (int)((int)Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density); // real pixels

				// Required for proper Push notifications handling
				var setupSingleton = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
    			setupSingleton.EnsureInitialized();

                global::Xamarin.Forms.Forms.Init(this, bundle);
                GrialKit.Init(this, "Bullytect.Droid.GrialLicense");

				// Presenters Initialization
				global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, bundle);

				// Xamarin.Auth CustomTabs Initialization/Customisation
				global::Xamarin.Auth.CustomTabsConfiguration.ActionLabel = null;
				global::Xamarin.Auth.CustomTabsConfiguration.MenuItemTitle = null;
				global::Xamarin.Auth.CustomTabsConfiguration.AreAnimationsUsed = true;
				global::Xamarin.Auth.CustomTabsConfiguration.IsShowTitleUsed = false;
				global::Xamarin.Auth.CustomTabsConfiguration.IsUrlBarHidingUsed = false;
				global::Xamarin.Auth.CustomTabsConfiguration.IsCloseButtonIconUsed = false;
				global::Xamarin.Auth.CustomTabsConfiguration.IsActionButtonUsed = false;
				global::Xamarin.Auth.CustomTabsConfiguration.IsActionBarToolbarIconUsed = false;
				global::Xamarin.Auth.CustomTabsConfiguration.IsDefaultShareMenuItemUsed = false;

				global::Android.Graphics.Color color_xamarin_blue;
				color_xamarin_blue = new global::Android.Graphics.Color(0x34, 0x98, 0xdb);
				global::Xamarin.Auth.CustomTabsConfiguration.ToolbarColor = color_xamarin_blue;


				// ActivityFlags for tweaking closing of CustomTabs
				// please report findings!
				global::Xamarin.Auth.CustomTabsConfiguration.
				   ActivityFlags =
						global::Android.Content.ActivityFlags.NoHistory
						|
						global::Android.Content.ActivityFlags.SingleTop
						|
						global::Android.Content.ActivityFlags.NewTask
						;

				global::Xamarin.Auth.CustomTabsConfiguration.IsWarmUpUsed = true;
				global::Xamarin.Auth.CustomTabsConfiguration.IsPrefetchUsed = true;

    			UserDialogs.Init(this);
    			PullToRefreshLayoutRenderer.Init();
    			XFGloss.Droid.Library.Init(this, bundle);
    			//Initializing FFImageLoading
    			CachedImageRenderer.Init();
                CarouselViewRenderer.Init();
    			FormsHelper.ForceLoadingAssemblyContainingType(typeof(UXDivers.Effects.Effects));

                LoadApplication(FormsApplication);
                PushNotificationManager.ProcessIntent(Intent);

    			var starter = Mvx.Resolve<IMvxAppStart>();
    			starter.Start();

            }
            catch (Exception e)
			{

				System.Diagnostics.Debug.WriteLine("**BullTect LAUNCH EXCEPTION**\n\n" + e);
			}


		}

		public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);

			DeviceOrientationLocator.NotifyOrientationChanged();
		}

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
