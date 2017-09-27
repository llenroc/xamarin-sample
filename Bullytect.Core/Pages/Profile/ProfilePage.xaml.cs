
using MvvmCross.Forms.Core;
using Bullytect.Core.ViewModels;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Profile
{
    public partial class ProfilePage : MvxContentPage<ProfileViewModel>
    {
        public ProfilePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

		protected override void OnAppearing()
		{
            if(ViewModel.SelfParent == null)
			    RefreshLayout.RefreshCommand?.Execute(null);
		}
    }
}
