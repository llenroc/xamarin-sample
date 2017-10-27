

namespace Bullytect.Core
{
    using System.Diagnostics;
    using Acr.UserDialogs;
    using Bullytect.Core.I18N;
    using Bullytect.Core.I18N.Services;
    using Bullytect.Core.Messages;
    using MvvmCross.Forms.Core;
    using MvvmCross.Platform;
    using MvvmCross.Plugins.Messenger;
    using Xamarin.Forms;
    using Bullytect.Utils.Helpers;
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



		protected override void OnStart()
		{

            Debug.WriteLine("Forms App OnStart ...");
            var messenger = Mvx.Resolve<IMvxMessenger>();
            // subscribe to Exception Ocurred Message
            messenger.Subscribe<ExceptionOcurredMessage>(OnExceptionOcurredMessage);
		}

	}
}
