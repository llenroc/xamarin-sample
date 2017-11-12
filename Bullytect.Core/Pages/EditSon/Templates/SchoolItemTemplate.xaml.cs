using System;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Pages.EditSon.Popup;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.EditSon.Templates
{
    public partial class SchoolItemTemplate : ContentView
    {
        public SchoolItemTemplate()
        {
            InitializeComponent();
        }

        async void OnShowSchoolLocation(object sender, EventArgs args)
        {

            var page = new SchoolMapPopup(BindingContext as SchoolEntity);
            await PopupNavigation.PushAsync(page);

        }
    }
}
