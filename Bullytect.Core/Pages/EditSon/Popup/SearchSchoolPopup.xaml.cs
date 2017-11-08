

using System.Diagnostics;
using Bullytect.Core.ViewModels;
using Rg.Plugins.Popup.Pages;

namespace Bullytect.Core.Pages.EditSon.Popup
{
    public partial class SearchSchoolPopup : PopupPage
    {
        public SearchSchoolPopup()
        {
            InitializeComponent();

            searchBar.TextChanged += (object sender, Xamarin.Forms.TextChangedEventArgs e) =>
            {

                if (e.NewTextValue == string.Empty && e.OldTextValue.Length > 1)
                {

                    var ViewModel = (EditSonViewModel)BindingContext;
                    ViewModel?.Schools.Clear();
                }

            };



        }
    }
}
