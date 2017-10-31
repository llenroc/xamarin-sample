

using System;
using System.Diagnostics;
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

            
		/// First item Appearing => animate MoveDown
		private void SearchPageViewCellWithId_OnFirstApper(object sender, EventArgs e) => MoveDown();

		
		/// First item Disappearing => animate MoveUp
		private void SearchPageViewCellWithId_OnFirstDisapp(object sender, EventArgs e) => MoveUp();

		private void MoveDown()
		{
            Debug.WriteLine("Move Down .... ");
            //AlertsListView.HeightRequest -= 500;
			//AlertsBody.TranslateTo(0, 0, 500, Easing.Linear);
			//Toolbar.TranslateTo(0, 0, 500, Easing.Linear);
		}

		private void MoveUp()
		{
            Debug.WriteLine("Move Up .... ");
            //AlertsListView.HeightRequest += 500;
			//AlertsBody.TranslateTo(0, -200, 500, Easing.Linear);
			//Toolbar.TranslateTo(0, -100, 500, Easing.Linear);
		}


		
    }
}
