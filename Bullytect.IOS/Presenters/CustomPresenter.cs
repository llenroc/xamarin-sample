
using System.Diagnostics;
using System.Linq;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.iOS.Presenters;
using UIKit;

namespace Bullytect.iOS.Presenters
{
    public class CustomPresenter: MvxFormsIosPagePresenter
    {
		public CustomPresenter(UIWindow window, MvxFormsApplication mvxFormsApp)
		 : base(window, mvxFormsApp)
		{
		}

		public override void Show(MvxViewModelRequest request)
		{
 
			if (request.PresentationValues?["NavigationCommand"] == "StackClear")
			{


                var navigation = FormsApplication.MainPage.Navigation;
                Debug.WriteLine("Navigation Back Stack Count -> " + navigation.NavigationStack.Count());
                navigation.PopToRootAsync();
                Debug.WriteLine("Navigation Back Stack Count After PopToRootAsync -> " + navigation.NavigationStack.Count());
                return;
            }

            base.Show(request);
		}
    }
}
