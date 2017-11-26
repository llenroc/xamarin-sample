

using System;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Utils;
using Bullytect.Core.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.EditSon.Popup
{
    public partial class SearchSchoolPopup : PopupPage
    {

        Timer tmr;

        public SearchSchoolPopup()
        {
            InitializeComponent();

        }

        async void OnShowSchoolLocation(object sender, EventArgs args)
        {

            var page = new SchoolMapPopup(BindingContext as SchoolEntity);
            await PopupNavigation.PushAsync(page);

        }

        void SearchBarOnTextChangedAsync(object sender, TextChangedEventArgs e)
        {

            var ViewModel = (EditSonViewModel)BindingContext;

            if (ViewModel != null)
            {

                if (tmr?.IsCancellationRequested == false)
                    tmr?.Dispose();

                if (e.NewTextValue == string.Empty && e.OldTextValue.Length > 1)
                {
                    ViewModel.Schools.Clear();
                }
                else if (e.NewTextValue.Length > 0)
                {
                    tmr = new Timer((_) => {
                        ViewModel.FindSchoolsCommand.Execute(e.NewTextValue).Subscribe();
                        tmr?.Dispose();
                    }, this, 2000, 4000);
                }

            }
        }
    }
}
