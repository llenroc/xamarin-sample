
using System;
using Bullytect.Core.Pages.Common;
using Bullytect.Core.Pages.SonProfileFullScreen;
using Bullytect.Core.ViewModels;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.NotificationDetail
{
    public partial class NotificationDetailPage : BaseContentPage<AlertDetailViewModel>
    {
        public NotificationDetailPage()
        {
            InitializeComponent();
        }

		void OnImageTapped(Object sender, EventArgs e)
		{
			var imagePreview = new SonProfileFullScreenPage((sender as FFImageLoading.Forms.CachedImage).Source);
            PushModalAsync(imagePreview);
		}
    }
}
