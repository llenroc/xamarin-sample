using Bullytect.Core;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.iOS;
using MvvmCross.iOS.Platform;
using MvvmCross.Platform.Platform;
using UIKit;

namespace Bullytect.iOS
{
    public class Setup : MvxFormsIosSetup
    {
        public Setup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

		protected override IMvxApplication CreateApp()
		{
			return new CoreApp();
		}
		protected override MvvmCross.Forms.Core.MvxFormsApplication CreateFormsApplication()
		{
			return new App();
		}
        
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}
