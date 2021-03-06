﻿
using System;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Pages.Common;
using Bullytect.Core.Pages.EditSon.Popup;
using Bullytect.Core.ViewModels;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Pages;
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
            schoolEntry.Focused += OnSelectSchool;

		}

		protected override void OnDisappearing()
		{
			ViewModel.NewSelectedImage -= ViewModel_OnNewSelectedImage;
            ViewModel.SchoolAdded -= ViewModel_OnSchoolAddedAsync;
            ViewModel.SonUpdated -= ViewModel_OnOnSonUpdatedAsync;
            schoolEntry.Focused -= OnSelectSchool;
		}

		void ViewModel_OnNewSelectedImage(Object sender, MediaFile NewProfileImage)
		{

			profileImage.Source = ImageSource.FromStream(() => NewProfileImage.GetStream());
		}

        async void ViewModel_OnSchoolAddedAsync(Object sender, SchoolEntity SchoolEntity)
        {
            if(PopupNavigation.PopupStack.Count > 0)
                await PopupNavigation.PopAllAsync(animate: true);
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

   

        async void OnSelectSchool(object sender, EventArgs args)
        {
            ((Entry)sender).Unfocus();
            PopupPage page = null;
            if (ViewModel.TotalSchools > 0)
                page = new SearchSchoolPopup();
            else
                page = new AddSchoolPopup();
            if (PopupNavigation.PopupStack.Count > 0)
            {
                await PopupNavigation.PopAllAsync();
            }
            page.BindingContext = ViewModel;
            await PopupNavigation.PushAsync(page);
        }

        async void OnShowSchoolLocation(object sender, EventArgs args)
        {
            await PopupNavigation.PushAsync(new SchoolMapPopup(ViewModel.CurrentSon.School));

        }


    }
}
