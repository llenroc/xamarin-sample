
using MvvmCross.Forms.Core;
using Bullytect.Core.ViewModels;

namespace Bullytect.Core.Pages.Profile
{
    public partial class ProfilePage : MvxContentPage<ProfileViewModel>
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

		protected override void OnAppearing()
		{
            if(ViewModel.SelfParent == null)
			    RefreshLayout.RefreshCommand?.Execute(null);
		}
    }
}
