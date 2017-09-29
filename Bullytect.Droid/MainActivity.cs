using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Forms.Presenters;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using Acr.UserDialogs;
using Plugin.FirebasePushNotification;
using Refractored.XamForms.PullToRefresh.Droid;
using UXDivers.Artina.Shared;
using Android.Views;
using Xamarin.Forms.Platform.Android;
using FFImageLoading.Forms.Droid;

namespace Bullytect.Droid
{


	[Activity(
        Name = "com.usal.bisite.bulltect.MainActivity",  
        Label = "BullTect", 
        Icon = "@mipmap/ic_launcher", 
        Theme = "@style/Theme.Splash",
		MainLauncher = true,
		LaunchMode = LaunchMode.SingleTask,
		ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize
    )]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {

			try
			{
				// Changing to App's theme since we are OnCreate and we are ready to 
				// "hide" the splash
				base.Window.RequestFeature(WindowFeatures.ActionBar);
				base.SetTheme(Resource.Style.AppTheme);


				ToolbarResource = Resource.Layout.Toolbar;
				TabLayoutResource = Resource.Layout.Tabs;

    			base.OnCreate(bundle);

				//Initializing FFImageLoading
				CachedImageRenderer.Init();

    			global::Xamarin.Forms.Forms.Init(this, bundle);
				PullToRefreshLayoutRenderer.Init();
                XFGloss.Droid.Library.Init(this, bundle);
                GrialKit.Init(this, "Bullytect.Droid.GrialLicense");

                UserDialogs.Init(this);

                FormsHelper.ForceLoadingAssemblyContainingType(typeof(UXDivers.Effects.Effects));

    			var formsPresenter = (MvxFormsPagePresenter)Mvx.Resolve<IMvxAndroidViewPresenter>();
    			LoadApplication(formsPresenter.FormsApplication);

                FirebasePushNotificationManager.ProcessIntent(Intent);
				
			}
			catch (Exception e)
			{
				Console.WriteLine("**BullTect LAUNCH EXCEPTION**\n\n" + e);
			}


        }
    }
}
