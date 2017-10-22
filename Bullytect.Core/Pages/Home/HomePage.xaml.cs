

using System;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Pages.Common;
using Bullytect.Core.Pages.Common.Controls;
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
            /*await CachedImage.InvalidateCache(profileImage.Source, CacheType.All, true);
            profileImage.ReloadImage();*/
        }

        private bool IsUpper = false;

       /* void LayoutTouchListner_OnTouchEvent(object sender, EventArgs eventArgs)
    		{
    			var a = eventArgs as EvArg;

    			LayoutTouchListnerCtrl.IsEnebleScroll = true;

    			// ignore the weak touch
    			if (a.Val > 10 || a.Val < -10)
    			{
    				if (a.Val > 0)
    				{
    					if (IsUpper)
    					{
    						MoveDown();
    					}
    				}
    				else
    				{
    					MoveUp();
    				}
    			}
    		}*/

		/// <summary>
		/// First item Appearing => animate MoveDown
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SearchPageViewCellWithId_OnFirstApper(object sender, EventArgs e)
		{
			IsUpper = true;
			MoveDown();
		}

		/// <summary>
		/// First item Disappearing => animate MoveUp
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SearchPageViewCellWithId_OnFirstDisapp(object sender, EventArgs e)
		{
			IsUpper = false;
			MoveUp();
		}

		private void MoveDown()
		{
			Body.TranslateTo(0, 0, 500, Easing.Linear);
			//MenuGrid.TranslateTo(0, 0, 500, Easing.Linear);
		}

		private void MoveUp()
		{
			Body.TranslateTo(0, -200, 500, Easing.Linear);
			//MenuGrid.TranslateTo(0, -100, 500, Easing.Linear);
		}


		
    }
}
