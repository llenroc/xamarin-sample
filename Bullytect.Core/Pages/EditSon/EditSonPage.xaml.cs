
using System;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Pages.Common;
using Bullytect.Core.Pages.EditSon.Popup;
using Bullytect.Core.ViewModels;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.EditSon
{
    public partial class EditSonPage : BaseContentPage<EditSonViewModel>
    {
        public EditSonPage()
        {
            InitializeComponent();
        }

		protected override void OnAppearing()
		{

            ViewModel.NewSelectedImage += ViewModel_OnNewSelectedImage;

            ViewModel.SchoolAdded += ViewModel_OnSchoolAddedAsync;
            ViewModel.SonUpdated += ViewModel_OnOnSonUpdatedAsync;

		}

		protected override void OnDisappearing()
		{
			ViewModel.NewSelectedImage -= ViewModel_OnNewSelectedImage;
            ViewModel.SchoolAdded -= ViewModel_OnSchoolAddedAsync;
            ViewModel.SonUpdated -= ViewModel_OnOnSonUpdatedAsync;
		}

		void ViewModel_OnNewSelectedImage(Object sender, MediaFile NewProfileImage)
		{

			profileImage.Source = ImageSource.FromStream(() => NewProfileImage.GetStream());
		}

        async void ViewModel_OnSchoolAddedAsync(Object sender, SchoolEntity SchoolEntity)
        {
            await PopupNavigation.PopAsync(animate: true);
        }


        void ViewModel_OnOnSonUpdatedAsync(Object sender, SonEntity SonEntity)
		{
			profileImage.ReloadImage();
		}


        async void OnSocialMediaInfoAsync(object sender, EventArgs args)
        {
            var page = new SocialMediaInfoPopup();
            page.BindingContext = ViewModel;
            await PopupNavigation.PushAsync(page);
        }


    }
}
