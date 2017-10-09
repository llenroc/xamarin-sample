

using System;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.ViewModels;
using FFImageLoading.Cache;
using FFImageLoading.Forms;
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

		protected override void OnAppearing()
		{

			ViewModel.NewSelectedImage += ViewModel_OnNewSelectedImageAsync;

		}

		protected override void OnDisappearing()
		{
			ViewModel.NewSelectedImage -= ViewModel_OnNewSelectedImageAsync;
		}

        async void ViewModel_OnNewSelectedImageAsync(Object sender, ImageEntity NewProfileImage)
        {
            await CachedImage.InvalidateCache(profileImage.Source, CacheType.All, true);
            profileImage.ReloadImage();
        }
    }
}
