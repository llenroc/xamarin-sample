using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Common.Templates
{
    public partial class CommonInfoPopup : PopupPage
    {
        public CommonInfoPopup(string Title, string Description)
        {
            InitializeComponent();

            TitleLabel.Text = Title;

            DescriptionLabel.Text = Description;
        }
    }
}
