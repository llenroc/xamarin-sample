using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using Bullytect.Core;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Droid;
using MvvmCross.Platform;
using MvvmCross.Droid.Views;
using Bullytect.Droid.Presenters;
using MvvmCross.Core.Views;

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

        /*protected override IMvxAndroidViewPresenter CreateViewPresenter(){
			var presenter = new CustomAndroidPresenter();
			Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);
            presenter.FormsApplication = FormsApplication;
			return presenter;
        }*/

    }
}
