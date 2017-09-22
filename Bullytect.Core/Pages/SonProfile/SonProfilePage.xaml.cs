
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;

namespace Bullytect.Core.Pages.SonProfile
{
    public partial class SonProfilePage : MvxContentPage<SonProfileViewModel>
    {
        public SonProfilePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing(){
            Title = ViewModel.FullName;
        }
    }
}
