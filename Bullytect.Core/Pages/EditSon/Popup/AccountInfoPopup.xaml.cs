
using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.EditSon.Popup
{
    public partial class AccountInfoPopup : PopupPage
    {
        public AccountInfoPopup(string Title, string Description, string Image = null)
        {
            InitializeComponent();

            TitleLabel.Text = Title;
            DescriptionLabel.Text = Description;

            Uri ImageUri;
            if (!string.IsNullOrWhiteSpace(Image))
            {
                if (Uri.TryCreate(Image, UriKind.Absolute, out ImageUri))
                {
                    UserImage.IsVisible = true;
                    UserImage.Source = ImageSource.FromUri(ImageUri);

                }
            }
                
            
        }
    }
}
