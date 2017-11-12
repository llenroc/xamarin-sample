
using System;
using Bullytect.Core.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.EditSon.Popup
{
    public partial class AddSchoolPopup : PopupPage
    {
        public AddSchoolPopup()
        {
            InitializeComponent();

            LocationEntry.Focused += OnSelectSchoolFromMap;
        }

        async void OnSelectSchoolFromMap(object sender, EventArgs args)
        {
            var ViewModel = BindingContext as EditSonViewModel;
            if(ViewModel != null) {
                ((Entry)sender).Unfocus();
                SchoolMapPopup popup = new SchoolMapPopup(ViewModel.NewSchool, true);
                await PopupNavigation.PushAsync(popup);

            }

        }
    }
}
