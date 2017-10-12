
using System;
using Bullytect.Core.Pages.SonProfileFullScreen;
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.SonProfile
{
    public partial class SonProfilePage : MvxContentPage<SonProfileViewModel>
    {
        public SonProfilePage()
        {
            InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
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
