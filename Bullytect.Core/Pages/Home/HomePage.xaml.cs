

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
            ViewModel.RefreshPageStart += ViewModel_OnRefreshPageStart;
            ViewModel.RefreshPageFinished += ViewModel_OnRefreshPageFinished;

		}

		protected override void OnDisappearing()
		{
			ViewModel.NewSelectedImage -= ViewModel_OnNewSelectedImageAsync;
			ViewModel.RefreshPageStart -= ViewModel_OnRefreshPageStart;
			ViewModel.RefreshPageFinished -= ViewModel_OnRefreshPageFinished;
		}

        void ViewModel_OnRefreshPageStart(Object sender) {
            LoadingIndicator.Start();

        }

		void ViewModel_OnRefreshPageFinished(Object sender)
		{
            LoadingIndicator.Stop();
		}

        async void ViewModel_OnNewSelectedImageAsync(Object sender, ImageEntity NewProfileImage)
        {
            await CachedImage.InvalidateCache(profileImage.Source, CacheType.All, true);
            profileImage.ReloadImage();
        }
    }
}
