
using Bullytect.Core.ViewModels;
using Xamarin.Forms;
using System;
using Plugin.Media.Abstractions;
using Bullytect.Core.Models.Domain;
using FFImageLoading.Forms;
using FFImageLoading.Cache;
using Bullytect.Core.Pages.Common;

namespace Bullytect.Core.Pages.Profile
{
    public partial class ProfilePage : BaseContentPage<ProfileViewModel>
    {
        public ProfilePage()
        {
            InitializeComponent();
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
