
using MvvmCross.Forms.Core;
using Bullytect.Core.ViewModels;
using Xamarin.Forms;
using System;
using System.IO;
using Plugin.Media.Abstractions;
using Bullytect.Core.Models.Domain;
using FFImageLoading.Forms;
using FFImageLoading.Cache;

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

            ViewModel.NewSelectedImage += ViewModel_OnNewSelectedImage;
            ViewModel.AccountUpdated += ViewModel_OnAccountUpdatedAsync;

		}

        protected override void OnDisappearing() {
            ViewModel.NewSelectedImage -= ViewModel_OnNewSelectedImage;
            ViewModel.AccountUpdated -= ViewModel_OnAccountUpdatedAsync;
        }


		void ViewModel_OnNewSelectedImage(Object sender, MediaFile NewProfileImage)
		{
            profileImage.Source = ImageSource.FromStream(() => NewProfileImage.GetStream());
		}

        async void ViewModel_OnAccountUpdatedAsync(Object sender, ParentEntity ParentEntity)
        {
            await CachedImage.InvalidateCache(profileImage.Source, CacheType.All, true);
            profileImage.ReloadImage();
        }

    }
}
