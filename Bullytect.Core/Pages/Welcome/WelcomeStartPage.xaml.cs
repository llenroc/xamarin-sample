
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Welcome
{
    public partial class WelcomeStartPage : MvxContentPage<WelcomeViewModel>
	{
		public WelcomeStartPage()
		{
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
		}
	}
}
