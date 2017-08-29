using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using NControl.Controls.Droid;
using ImageCircle.Forms.Plugin.Droid;

namespace bullytect.Droid
{
	[Activity(Label = "BullyTect", Icon = "@drawable/icon", Theme = "@style/DefaultTheme", MainLauncher = false,
			  ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

		public static bool IsRunning
		{
			get;
			private set;
		}

		public void AdjustStatusBar(int size)
		{
			//Temp hack until the FormsAppCompatActivity works for full screen
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				var statusBarHeightInfo = typeof(FormsAppCompatActivity).GetField("_statusBarHeight",
											  System.Reflection.BindingFlags.Instance |
											  System.Reflection.BindingFlags.NonPublic);

				statusBarHeightInfo.SetValue(this, size);
			}
		}

        protected override void OnCreate(Bundle bundle)
        {


			try
			{
				AdjustStatusBar(0);

				base.OnCreate(bundle);

				TabLayoutResource = Resource.Layout.Tabbar;
				ToolbarResource = Resource.Layout.Toolbar;

				Window.SetSoftInputMode(SoftInput.AdjustResize);
				Window.DecorView.SystemUiVisibility = StatusBarVisibility.Visible;

				global::Xamarin.Forms.Forms.Init(this, bundle);
				NControls.Init();
				ImageCircleRenderer.Init();

				LoadApplication(new App(new Autofac.Core.IModule[] { }));
				XFGloss.Droid.Library.Init(this, bundle);
			}
			catch (Exception e)
			{
				Console.WriteLine("**BullyTect LAUNCH EXCEPTION**\n\n" + e);
			}


        }
    }
}
