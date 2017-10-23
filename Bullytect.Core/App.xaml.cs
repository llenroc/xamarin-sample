

namespace Bullytect.Core
{
    using System;
    using System.Diagnostics;
    using Acr.UserDialogs;
    using Bullytect.Core.Config;
    using Bullytect.Core.I18N;
    using Bullytect.Core.I18N.Services;
    using Bullytect.Core.Messages;
    using Bullytect.Core.Services;
    using MvvmCross.Forms.Core;
    using MvvmCross.Platform;
    using MvvmCross.Plugins.Messenger;
    using Plugin.DeviceInfo;
    using Xamarin.Forms;
    using Bullytect.Utils.Helpers;
    using MvvmCross.Core.Navigation;
    using Bullytect.Core.ViewModels;
    using Plugin.FirebasePushNotification;
    using Bullytect.Core.Rest.Utils;

    public partial class App : MvxFormsApplication
    {

		public static int ScreenWidth;
		public static int ScreenHeight;

		private void ConfigLocale()
		{

            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android )
			{
				Debug.WriteLine("Get Culture Info ...");
                var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
				Debug.WriteLine(ci.ToString());
                HttpClientFactory.getHttpClient()?.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", ci.Name);
				AppResources.Culture = ci; // set the RESX for resource localization
				DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
			}
		}

        public App()
        {
            ConfigLocale();
            InitializeComponent();

			if (ScreenHeight == 0)
			{
				ScreenHeight = 1000;
			}
			if (ScreenWidth == 0)
			{
				ScreenWidth = 600;
			}
        }


        void OnExceptionOcurredMessage(ExceptionOcurredMessage exceptionOcurredMessage) 
        {
            Debug.WriteLine("OnExceptionOcurredMessage ...");

            var userDialogs = Mvx.Resolve<IUserDialogs>();

            userDialogs.ShowError(AppResources.Global_ErrorOcurred);

            if (exceptionOcurredMessage.Ex != null)
			    exceptionOcurredMessage.Ex.Track();
            
        }

		void OnSessionExpiredMessage(SessionExpiredMessage sessionExpiredMessage)
		{
			Debug.WriteLine("OnSessionExpiredMessage ...");

            var userDialogs = Mvx.Resolve<IUserDialogs>();
            var navigationService = Mvx.Resolve<IMvxNavigationService>();

            userDialogs.ShowError(AppResources.Common_Invalid_Session);

            Settings.AccessToken = null;

            navigationService?.Navigate<AuthenticationViewModel>();

		}

		void OnSignOutMessage(SignOutMessage signOutMessage)
		{
			Debug.WriteLine("OnSignOutMessage ...");

			var navigationService = Mvx.Resolve<IMvxNavigationService>();

			Settings.AccessToken = null;

            navigationService?.Navigate<AuthenticationViewModel>();

		}

		protected override void OnStart()
		{

            Debug.WriteLine("Forms App OnStart ...");
            var messenger = Mvx.Resolve<IMvxMessenger>();
            // subscribe to Exception Ocurred Message
            messenger.Subscribe<ExceptionOcurredMessage>(OnExceptionOcurredMessage);
			// subscribe to SessionExpiredMessage
			messenger.Subscribe<SessionExpiredMessage>(OnSessionExpiredMessage);
			// subscribe to SessionExpiredMessage
			messenger.Subscribe<SignOutMessage>(OnSignOutMessage);


			// Handle when your app starts
			CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
			{
				Debug.WriteLine($"TOKEN REC: {p.Token}");
				Settings.FcmToken = p.Token;
			};

            Debug.WriteLine($"TOKEN: {CrossFirebasePushNotification.Current.Token}");
            Settings.FcmToken = CrossFirebasePushNotification.Current.Token;

			CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
			{

				System.Diagnostics.Debug.WriteLine("Received");

			};

			CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
			{
				System.Diagnostics.Debug.WriteLine("Opened");
				foreach (var data in p.Data)
				{
					System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
				}

				if (!string.IsNullOrEmpty(p.Identifier))
				{
					System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
				}

			};


		}

	}
}
