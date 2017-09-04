

namespace Bullytect.Core
{
    using System;
    using Bullytect.Core.I18N;
    using Bullytect.Core.PatformServices;
    using Bullytect.Utils.Helpers;
    using MvvmCross.Forms.Core;
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

		protected override void OnStart()
		{
			// Handling for App Exceptions.
			MessagingCenter.Subscribe<object, Exception>(this, EventTypeName.EXCEPTION_OCCURRED, (object sender, Exception exception) => {
				if (exception == null)
					return;
				exception.Track();
			});
		}
    }
}
