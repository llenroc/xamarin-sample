
using System;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Pages.AddSchool;
using Bullytect.Core.Pages.EditSon.Popup;
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.EditSon
{
    public partial class EditSonPage : MvxContentPage<EditSonViewModel>
    {
        public EditSonPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

		protected override void OnAppearing()
		{

            Header.AddSchoolAction = async () =>  {
                var page = new AddSchoolPopup();
                page.BindingContext = ViewModel;
                await PopupNavigation.PushAsync(page);
            };

            ViewModel.NewSelectedImage += ViewModel_OnNewSelectedImage;

            ViewModel.SchoolAdded += ViewModel_OnSchoolAddedAsync;

		}

		protected override void OnDisappearing()
		{
			ViewModel.NewSelectedImage -= ViewModel_OnNewSelectedImage;
            ViewModel.SchoolAdded -= ViewModel_OnSchoolAddedAsync;
		}

		void ViewModel_OnNewSelectedImage(Object sender, MediaFile NewProfileImage)
		{
			profileImage.Source = ImageSource.FromStream(() => NewProfileImage.GetStream());
		}

        async void ViewModel_OnSchoolAddedAsync(Object sender, SchoolEntity SchoolEntity)
        {
            await PopupNavigation.PopAsync(animate: true);
        }

        async void OnSocialMediaInfoAsync(object sender, EventArgs args)
        {
            var page = new SocialMediaInfoPopup();
            page.BindingContext = ViewModel;
            await PopupNavigation.PushAsync(page);

        }

    }
}
