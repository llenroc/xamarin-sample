using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Forms.Presenters;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using Acr.UserDialogs;
using Refractored.XamForms.PullToRefresh.Droid;
using UXDivers.Artina.Shared;
using Xamarin.Forms.Platform.Android;
using FFImageLoading.Forms.Droid;
using Xamarin.Forms;
using UXDivers.Artina.Shared.Droid;
using Plugin.FirebasePushNotification;

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
        protected override void OnCreate(Bundle bundle)
        {

			try
			{
				ToolbarResource = Resource.Layout.Toolbar;
				TabLayoutResource = Resource.Layout.Tabs;

				base.OnCreate(bundle);

				Forms.Init(this, bundle);
				PullToRefreshLayoutRenderer.Init();
				XFGloss.Droid.Library.Init(this, bundle);
				//Initializing FFImageLoading
				CachedImageRenderer.Init();
				UserDialogs.Init(this);

				GrialKit.Init(this, "Bullytect.Droid.GrialLicense");

                //FormsHelper.ForceLoadingAssemblyContainingType(typeof(UXDivers.Effects.Effects));


				var formsPresenter = (MvxFormsPagePresenter)Mvx.Resolve<IMvxAndroidViewPresenter>();
				LoadApplication(formsPresenter.FormsApplication);


                //FirebasePushNotificationManager.ProcessIntent(Intent);
				
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
    }
}
