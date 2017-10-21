

using System;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Pages.Common;
using Bullytect.Core.ViewModels;
using FFImageLoading.Cache;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Home
{
    public partial class HomePage : BaseContentPage<HomeViewModel>
    {
        public HomePage()
        {
            InitializeComponent();
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
