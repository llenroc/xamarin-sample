using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using Bullytect.Core;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Droid;
using MvvmCross.Platform;
using MvvmCross.Plugins.Validation;
using MvvmCross.Plugins.Validation.Droid;

namespace Bullytect.Droid
{
    public class Setup : MvxFormsAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

		protected override void InitializePlatformServices()
		{
			base.InitializePlatformServices();

			Mvx.RegisterType<IMvxToastService>(() => new MvxAndroidToastService(ApplicationContext));
		}


		protected override IMvxApplication CreateApp()
		{
			return new Core.CoreApp();
		}
		protected override MvxFormsApplication CreateFormsApplication()
		{
			return new App();
		}

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}
