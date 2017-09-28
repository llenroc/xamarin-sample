
using Bullytect.Core.Pages.AddSchool;
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.EditSon
{
    public partial class EditSonPage : MvxContentPage<EditSonViewModel>
    {
        public EditSonPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

		protected override void OnAppearing()
		{
            if(!string.IsNullOrWhiteSpace(ViewModel.SonToEdit)){

                if(ViewModel.CurrentSon == null)
                    RefreshLayout.RefreshCommand?.Execute(null);

            }

            Header.AddSchoolAction = async () =>  {
                var page = new AddSchoolPopup();
                page.BindingContext = ViewModel;
                await PopupNavigation.PushAsync(page);
            };



		}


    }
}
