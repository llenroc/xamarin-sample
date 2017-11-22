
using System;
using Bullytect.Core.Pages.Common.Templates;
using Bullytect.Core.ViewModels.Core.Models;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Comments.Popup.Templates
{

    public class SocialMediaCategoryCell : ViewCell
    {
        public SocialMediaCategoryCell()
        {

            View = new SocialMediaCategoryCellView();
        }
    }

    public partial class SocialMediaCategoryCellView : ContentView
    {
        public SocialMediaCategoryCellView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var category = BindingContext as SocialMediaCategoryModel;
            if (category == null)
                return;

        }

        async void OnSocialMediaInfo(object sender, EventArgs args)
        {

            var Category = BindingContext as SocialMediaCategoryModel;
            var page = new CommonInfoPopup(Category.Name, Category.Description);
            await PopupNavigation.PushAsync(page);

        }
    }
}
