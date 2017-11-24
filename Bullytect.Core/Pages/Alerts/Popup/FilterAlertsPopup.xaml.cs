
using Bullytect.Core.Pages.Common.Templates;
using Bullytect.Core.ViewModels;
using Rg.Plugins.Popup.Pages;
using Bullytect.Core.Pages.Home.Settings.Templates;

namespace Bullytect.Core.Pages.Alerts.Popup
{
    public partial class FilterAlertsPopup : PopupPage
    {
        public FilterAlertsPopup(AlertsViewModel ViewModel)
        {
            InitializeComponent();

            BindingContext = ViewModel;

            if (ViewModel != null)
            {

                TableSectionAlertLevels.Clear();

                TableSectionAlertLevels.Add(new CommonCategoryCell
                {
                    BindingContext = ViewModel.AllCategory
                });

                foreach (var item in ViewModel.AlertsLevelCategories)
                {
                    var AlertCategoryCell = new AlertCategoryCell
                    {
                        BindingContext = item
                    };

                    TableSectionAlertLevels.Add(AlertCategoryCell);

                }

            }
        }

    }
}
