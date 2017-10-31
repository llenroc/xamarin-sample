
using System;
using Bullytect.Core.Pages.Common;
using Bullytect.Core.Pages.SonProfileFullScreen;
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.SonProfile
{
    public partial class SonProfilePage : BaseContentPage<SonProfileViewModel>
    {
        public SonProfilePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing(){
            Title = ViewModel.FullName;
        }

	    async void OnImageTapped(Object sender, EventArgs e)
		{
			var imagePreview = new SonProfileFullScreenPage((sender as FFImageLoading.Forms.CachedImage).Source);

			await Navigation.PushModalAsync(NavigationPageHelper.Create(imagePreview));
		}
    }
}
