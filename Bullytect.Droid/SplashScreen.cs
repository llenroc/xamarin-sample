using System;
using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using FFImageLoading.Forms.Droid;
using MvvmCross.Droid.Views;
using MvvmCross.Forms.Droid;
using Refractored.XamForms.PullToRefresh.Droid;
using UXDivers.Artina.Shared;
using Xamarin.Forms;

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
		protected override void TriggerFirstNavigate()
		{
			StartActivity(typeof(MvxFormsApplicationActivity));
		}
	}
}
