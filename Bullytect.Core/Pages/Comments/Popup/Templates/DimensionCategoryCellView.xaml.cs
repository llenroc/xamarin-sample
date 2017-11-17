using System;
using Bullytect.Core.Pages.Common.Templates;
using Bullytect.Core.ViewModels.Core.Models;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Comments.Popup.Templates
{

    public class DimensionCategoryCell : ViewCell
    {
        public DimensionCategoryCell()
        {

            View = new DimensionCategoryCellView();
        }
    }

    public partial class DimensionCategoryCellView : ContentView
    {
        public DimensionCategoryCellView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var category = BindingContext as DimensionCategoryModel;
            if (category == null)
                return;

        }


        async void OnCategoryInfo(object sender, EventArgs args)
        {

            var Category = BindingContext as DimensionCategoryModel;
            var page = new CommonInfoPopup(Category.Name, Category.Description);
            await PopupNavigation.PushAsync(page);

        }
    }
}
