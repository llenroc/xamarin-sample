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

    			// Required for proper Push notifications handling
    			var setupSingleton = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
    			setupSingleton.EnsureInitialized();

                global::Xamarin.Forms.Forms.Init(this, bundle);
                GrialKit.Init(this, "Bullytect.Droid.GrialLicense");
    			UserDialogs.Init(this);
    			PullToRefreshLayoutRenderer.Init();
    			XFGloss.Droid.Library.Init(this, bundle);
    			//Initializing FFImageLoading
    			CachedImageRenderer.Init();
    			FormsHelper.ForceLoadingAssemblyContainingType(typeof(UXDivers.Effects.Effects));

                LoadApplication(FormsApplication);

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
    }
}
