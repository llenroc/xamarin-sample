

using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Home
{
    public partial class HomePage : MvxContentPage<HomeViewModel>
    {
        public HomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing() {
            if(ViewModel.SelfParent == null && ViewModel.Children?.Count == 0)
                RefreshLayout.RefreshCommand?.Execute(null);
        }
    }
}
