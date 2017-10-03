

using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Droid.Presenters;
using Xamarin.Forms;

namespace Bullytect.Droid.Presenters
{
    public class CustomAndroidPresenter: MvxFormsDroidPagePresenter
    {

        public override void Show(MvxViewModelRequest request)
        {



            if (request.PresentationValues?["NavigationCommand"] == "StackClear")
            {

                var navigation = FormsApplication.MainPage.Navigation;


                /*if (navigation.NavigationStack.Count > 1)
                {
                    var page = navigation.NavigationStack.First();
                    if (page.GetType() == typeof(NavigationPage))
                        page = ((NavigationPage)page).CurrentPage;

                    var typesMatch = page.GetType() == typeof(T);

                    while (!typesMatch)
                    {
                        NavigationProperty.RemovePage(page);

                        page = stack.FirstOrDefault();
                        if (page == null)
                            throw new InvalidOperationException("Could not find the requested page");

                        if (page is NavigationPage)
                            page = ((NavigationPage)page).CurrentPage;

                        typesMatch = page.GetType() == typeof(T);
                    }
                    this..PopToRootAsync(animated: true);
                    return;
                }*/



            }

            base.Show(request);
        }

			
        
    }
}
