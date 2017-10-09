
using System;
using Bullytect.Core.Pages.SonProfileFullScreen;
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.NotificationDetail
{
    public partial class NotificationDetailPage : MvxContentPage<AlertDetailViewModel>
    {
        public NotificationDetailPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

		async void OnImageTapped(Object sender, EventArgs e)
		{
			var imagePreview = new SonProfileFullScreenPage((sender as FFImageLoading.Forms.CachedImage).Source);

			await Navigation.PushModalAsync(NavigationPageHelper.Create(imagePreview));
		}
    }
}
