using System;
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Common
{
    public abstract partial class BaseContentPage<T> : MvxContentPage<T> where T : BaseViewModel
    {

        public BaseContentPage(){
            NavigationPage.SetHasNavigationBar(this, false);
        }

		protected override bool OnBackButtonPressed()
		{
			ViewModel.BackPressedCommand.Execute(null);
			return true;
		}

        protected async void PushModalAsync(ContentPage page) 
            => await Navigation.PushModalAsync(NavigationPageHelper.Create(page));

    }
}
