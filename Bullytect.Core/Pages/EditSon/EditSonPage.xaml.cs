
using Bullytect.Core.I18N;
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.EditSon
{
    public partial class EditSonPage : MvxContentPage<EditSonViewModel>
    {
        public EditSonPage()
        {
            InitializeComponent();
        }

		protected override void OnAppearing()
		{
            if (ViewModel.SonToEdit != null) {

                if(ViewModel.CurrentSon == null)
                    RefreshLayout.RefreshCommand?.Execute(null);

                Title = AppResources.Page_Edit_Son_Title;

            } else {
                Title = AppResources.Page_Add_Son_Title;
            }
				
		}


    }
}
