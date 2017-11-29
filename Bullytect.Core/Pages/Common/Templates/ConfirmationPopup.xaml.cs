using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Common.Templates
{
    public partial class ConfirmationPopup : PopupPage
    {
        public ConfirmationPopup(string Description)
        {
            InitializeComponent();

            DescriptionLabel.Text = Description;


        }

        async void OnClose(object sender, EventArgs e){
            if (PopupNavigation.PopupStack.Count > 0)
                await PopupNavigation.PopAllAsync(true);

        }

    }
}
