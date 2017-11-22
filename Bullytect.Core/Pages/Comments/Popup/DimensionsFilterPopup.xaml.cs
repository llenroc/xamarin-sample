
using System;
using Bullytect.Core.I18N;
using Bullytect.Core.Pages.Comments.Popup.Templates;
using Bullytect.Core.Pages.Common.Templates;
using Bullytect.Core.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Comments.Popup
{
    public partial class DimensionsFilterPopup : PopupPage
    {


        TableSection DimensionCategories { get; set; }


        public DimensionsFilterPopup(CommentsViewModel ViewModel)
        {
            InitializeComponent();

            if(ViewModel != null) {

                BindingContext = ViewModel;

                TableSectionSocialMediaCategories.Add(new CommonCategoryCell
                {
                    BindingContext = ViewModel.AllSocialMediaCategory
                });

                foreach (var item in ViewModel.SocialMediaCategories)
                {
                    var SocialMediaCategoryCell = new SocialMediaCategoryCell
                    {
                        BindingContext = item
                    };

                    TableSectionSocialMediaCategories.Add(SocialMediaCategoryCell);
                }

                DimensionCategories = TableFilterComments.Root[TableFilterComments.Root.IndexOf(TableSectionDimensionCategories)];

                DimensionCategories.Add(new CommonCategoryCell
                {
                    BindingContext = ViewModel.AllDimensionCategory
                });

                foreach (var item in ViewModel.DimensionCategories)
                {
                    var DimensionCategoryCell = new DimensionCategoryCell
                    {
                        BindingContext = item
                    };

                    DimensionCategories.Add(DimensionCategoryCell);
                }

                ViewModel.UpdateDimensionFilter();

                if(!ViewModel.EnableDimensionFilter)
                    TableFilterComments.Root.Remove(DimensionCategories);


                SwitchDimension.Toggled += ToggledEventHandler;

            }
        }


        void ToggledEventHandler(object sender, ToggledEventArgs e){
            var ViewModel = BindingContext as CommentsViewModel;
            if(e.Value){
                if(DimensionCategories != null)
                    TableFilterComments.Root.Add(DimensionCategories);
                ViewModel?.UpdateDimensionFilter();
            } else {
                ViewModel?.ClearDimensionFilter();
                TableFilterComments.Root.Remove(DimensionCategories);
            }

        }

        async void OnTimeIntervalInfo(object sender, EventArgs args)
        {

            var page = new CommonInfoPopup(AppResources.Settings_Statistics_General_Interval, AppResources.Settings_Statistics_General_Interval_Description);
            await PopupNavigation.PushAsync(page);

        }
    }
}
