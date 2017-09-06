

namespace Bullytect.Core
{
    using System;
    using System.Diagnostics;
    using Bullytect.Core.Config;
    using Bullytect.Core.I18N;
    using Bullytect.Core.Messages;
    using Bullytect.Core.PatformServices;
    using Bullytect.Core.Services;
    using Bullytect.Utils.Helpers;
    using MvvmCross.Forms.Core;
    using MvvmCross.Platform;
    using MvvmCross.Plugins.Messenger;
    using Plugin.FirebasePushNotification;
    using Xamarin.Forms;

    public partial class App : MvxFormsApplication
    {

		private void ConfigLocale()
		{
			if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
			{
				var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
				AppResources.Culture = ci; // set the RESX for resource localization
				DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
			}
		}

        public App()
        {
            ConfigLocale();
            InitializeComponent();
        }

		async void OnAuthenticatedUserMessage(AuthenticatedUserMessage authenticatedUserMessage)
		{

            Debug.WriteLine("OnAuthenticatedUserMessage ...");

            var deviceGroupsService = Mvx.Resolve<IDeviceGroupsService>();
            // save token
            var device = await deviceGroupsService.saveToken(Settings.FcmToken);

            Debug.WriteLine(String.Format("Device Saved: {0}", device.ToString()));
			
		}


        void OnExceptionOcurredMessage(ExceptionOcurredMessage exceptionOcurredMessage) 
        {
            Debug.WriteLine("OnExceptionOcurredMessage ...");

            if (exceptionOcurredMessage.Ex != null)
			    exceptionOcurredMessage.Ex.Track();
            
        }

		protected override void OnStart()
		{


            var messenger = Mvx.Resolve<IMvxMessenger>();
            // subscribe to Authenticated User Message
            messenger.Subscribe<AuthenticatedUserMessage>(OnAuthenticatedUserMessage);
            // subscribe to Exception Ocurred Message
            messenger.Subscribe<ExceptionOcurredMessage>(OnExceptionOcurredMessage);

            //Handling FCM Token
			CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
			{
				Debug.WriteLine($"TOKEN REC: {p.Token}");
				Settings.FcmToken = p.Token;
			};
			Debug.WriteLine($"TOKEN: {CrossFirebasePushNotification.Current.Token}");
			Settings.FcmToken = CrossFirebasePushNotification.Current.Token;


			// Handling for App Exceptions.
			MessagingCenter.Subscribe<object, Exception>(this, EventTypeName.EXCEPTION_OCCURRED, (object sender, Exception exception) => {
				if (exception == null)
					return;
				exception.Track();
			});

			
		}
    }
}
