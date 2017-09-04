using Android.Content;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using Bullytect.Core;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Droid;

namespace Bullytect.Droid
{
    public class Setup : MvxFormsAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
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
