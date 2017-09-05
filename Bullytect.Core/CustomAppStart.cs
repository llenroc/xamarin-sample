

using Bullytect.Core.Services;
using Bullytect.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace Bullytect.Core
{
	public class CustomAppStart: MvxNavigatingObject, IMvxAppStart
	{
		
		public void Start(object hint = null)
		{

            var auth = Mvx.Resolve<IAuthenticationService>();

			if (!auth.IsLoggedIn())
			{
				ShowViewModel<WelcomeViewModel>();
			}
			else
			{
				ShowViewModel<HomeViewModel>();
			}
		}
	}
}
