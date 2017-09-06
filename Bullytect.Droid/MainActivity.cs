using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Forms.Presenters;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using Acr.UserDialogs;
using Plugin.FirebasePushNotification;

namespace Bullytect.Droid
{


	[Activity(Name = "com.usal.bisite.bullytect.MainActivity",  Label = "BullyTect", Icon = "@drawable/icon", Theme = "@style/DefaultTheme", MainLauncher = false,
			  ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {

			try
			{
    			TabLayoutResource = Resource.Layout.Tabbar;
    			ToolbarResource = Resource.Layout.Toolbar;

    			base.OnCreate(bundle);

    			global::Xamarin.Forms.Forms.Init(this, bundle);

                XFGloss.Droid.Library.Init(this, bundle);

                UserDialogs.Init(this);

    			var formsPresenter = (MvxFormsPagePresenter)Mvx.Resolve<IMvxAndroidViewPresenter>();
    			LoadApplication(formsPresenter.FormsApplication);

                FirebasePushNotificationManager.ProcessIntent(Intent);
				
			}
			catch (Exception e)
			{
				Console.WriteLine("**BullyTect LAUNCH EXCEPTION**\n\n" + e);
			}


        }
    }
}
