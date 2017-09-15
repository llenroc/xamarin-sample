

using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Droid.Presenters;

namespace Bullytect.Droid.Presenters
{
    public class CustomAndroidPresenter: MvxFormsDroidPagePresenter
    {

		public override void Show(MvxViewModelRequest request)
		{
			if (request.PresentationValues?["NavigationCommand"] == "StackClear")
            {
                this.FormsApplication.MainPage.Navigation.PopToRootAsync(animated: true);
				return;
			}

			base.Show(request);
		}
        
    }
}
