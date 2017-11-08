
using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Droid.Views;

namespace Bullytect.Droid
{
	[Activity(
		Name = "com.usal.bisite.bulltect.SplashScreen",
        Label = "BullTec"
		, MainLauncher = true
		, Icon = "@mipmap/ic_launcher"
		, Theme = "@style/Theme.Splash"
		, NoHistory = true
		, ScreenOrientation = ScreenOrientation.Portrait)]
	public class SplashScreen : MvxSplashScreenActivity
	{


		protected override void TriggerFirstNavigate()
		{
			StartActivity(typeof(MvxFormsApplicationActivity));
		}
	}
}
