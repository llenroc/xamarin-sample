

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Pages.Common;
using Bullytect.Core.ViewModels;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Home
{
    public partial class HomePage : BaseContentPage<HomeViewModel>
    {
        public HomePage()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {

            ViewModel.ChildrenLoaded += ViewModel_OnChildrenLoaded;

        }

        protected override void OnDisappearing()
        {
            ViewModel.ChildrenLoaded -= ViewModel_OnChildrenLoaded;
        }


        void ViewModel_OnChildrenLoaded(Object sender, List<SonEntity> SonEntities)
        {
            ChildProfileContainer.Children.Clear();
            foreach(var SonEntity in SonEntities){
                ChildProfileContainer.Children.Add(new ChildProfileImage(){
                    ProfileId = SonEntity.Identity,
                    ProfileImage = SonEntity.ProfileImage,
                    ProfileName = SonEntity.FirstName,
                    ProfileWidth = 85,
                    ProfileHeight = 85,
                    BadgeText = "4",
                    BadgeColor = "#22c064",
                    BadgeCommand = ViewModel.GoToAlertsCommand,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    ClickedItemCommand = ViewModel.ShowSonProfileCommand
                });
            }
        }

		/// First item Appearing => animate MoveDown
		private void SearchPageViewCellWithId_OnFirstApper(object sender, EventArgs e) => MoveDown();

		
		/// First item Disappearing => animate MoveUp
		private void SearchPageViewCellWithId_OnFirstDisapp(object sender, EventArgs e) => MoveUp();

		private void MoveDown()
		{
            Debug.WriteLine("Move Down .... ");
            //AlertsListView.HeightRequest -= 500;
			//AlertsBody.TranslateTo(0, 0, 500, Easing.Linear);
			//Toolbar.TranslateTo(0, 0, 500, Easing.Linear);
		}

		private void MoveUp()
		{
            Debug.WriteLine("Move Up .... ");
            //AlertsListView.HeightRequest += 500;
			//AlertsBody.TranslateTo(0, -200, 500, Easing.Linear);
			//Toolbar.TranslateTo(0, -100, 500, Easing.Linear);
		}


		
    }
}
