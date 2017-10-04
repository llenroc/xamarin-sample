
using MvvmCross.Forms.Core;
using Bullytect.Core.ViewModels;
using Xamarin.Forms;
using System;
using System.IO;
using Plugin.Media.Abstractions;

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

            ViewModel.NewSelectedImage += ViewModel_OnNewSelectedImage;

		}

        protected override void OnDisappearing() {
            ViewModel.NewSelectedImage -= ViewModel_OnNewSelectedImage;
        }


		void ViewModel_OnNewSelectedImage(Object sender, MediaFile NewProfileImage)
		{
            profileImage.Source = ImageSource.FromStream(() => NewProfileImage.GetStream());
		}
    }
}
