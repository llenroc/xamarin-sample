

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

    public partial class App : MvxFormsApplication
    {

		private void ConfigLocale()
		{

            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android )
			{
				Debug.WriteLine("Get Culture Info ...");
				var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
				Debug.WriteLine(ci.ToString());
				AppResources.Culture = ci; // set the RESX for resource localization
				DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
			}
		}

        public App()
        {
            ConfigLocale();
            InitializeComponent();
        }

		void OnAuthenticatedUserMessage(AuthenticatedUserMessage authenticatedUserMessage)
		{

            Debug.WriteLine("OnAuthenticatedUserMessage ...");

            /*var deviceGroupsService = Mvx.Resolve<IDeviceGroupsService>();
            // save token
            deviceGroupsService.saveDevice(CrossDeviceInfo.Current.Id, Settings.FcmToken).Subscribe(device => {
                Debug.WriteLine(String.Format("Device Saved: {0}", device.ToString()));
            });*/

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
            // subscribe to Authenticated User Message
            messenger.Subscribe<AuthenticatedUserMessage>(OnAuthenticatedUserMessage);
            // subscribe to Exception Ocurred Message
            messenger.Subscribe<ExceptionOcurredMessage>(OnExceptionOcurredMessage);
			// subscribe to SessionExpiredMessage
			messenger.Subscribe<SessionExpiredMessage>(OnSessionExpiredMessage);
			// subscribe to SessionExpiredMessage
			messenger.Subscribe<SignOutMessage>(OnSignOutMessage);





            //Handling FCM Token
			/*CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
			{
				Debug.WriteLine($"TOKEN REC: {p.Token}");
				Settings.FcmToken = p.Token;
			};
			Debug.WriteLine($"TOKEN: {CrossFirebasePushNotification.Current.Token}");
			Settings.FcmToken = CrossFirebasePushNotification.Current.Token;*/

		}

	}
}
