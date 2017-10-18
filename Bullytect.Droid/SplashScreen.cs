
using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Droid.Views;
using Plugin.PushNotification;

namespace Bullytect.Droid
{
	[Activity(
		Name = "com.usal.bisite.bulltect.SplashScreen",
		Label = "bulltect"
		, MainLauncher = true
		, Icon = "@mipmap/ic_launcher"
		, Theme = "@style/Theme.Splash"
		, NoHistory = true
		, ScreenOrientation = ScreenOrientation.Portrait)]
	public class SplashScreen : MvxSplashScreenActivity
	{

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            PushNotificationManager.ProcessIntent(Intent);
        }

		protected override void TriggerFirstNavigate()
		{
			StartActivity(typeof(MvxFormsApplicationActivity));
		}
	}
}
