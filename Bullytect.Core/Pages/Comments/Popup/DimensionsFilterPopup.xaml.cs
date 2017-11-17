
using System;
using Bullytect.Core.I18N;
using Bullytect.Core.Pages.Comments.Popup.Templates;
using Bullytect.Core.Pages.Common.Templates;
using Bullytect.Core.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace Bullytect.Core.Pages.Comments.Popup
{
    public partial class DimensionsFilterPopup : PopupPage
    {
        public DimensionsFilterPopup(CommentsViewModel ViewModel)
        {
            InitializeComponent();

            if(ViewModel != null) {

                BindingContext = ViewModel;

                TableSectionCategories.Add(new CommonCategoryCell
                {
                    BindingContext = ViewModel.AllCategory
                });

                foreach (var item in ViewModel.Categories)
                {
                    var DimensionCategoryCell = new DimensionCategoryCell
                    {
                        BindingContext = item
                    };

                    TableSectionCategories.Add(DimensionCategoryCell);
                }

            }

        }

        async void OnTimeIntervalInfo(object sender, EventArgs args)
        {

            var page = new CommonInfoPopup(AppResources.Settings_Statistics_General_Interval, AppResources.Settings_Statistics_General_Interval_Description);
            await PopupNavigation.PushAsync(page);

        }
    }
}
