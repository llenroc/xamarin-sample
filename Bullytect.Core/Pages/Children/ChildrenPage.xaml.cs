﻿
using System;
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Children
{
    public partial class ChildrenPage : MvxContentPage<ChildrenViewModel>
    {
        public ChildrenPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            //ChildrenListView.GestureRecognizers.Clear();
            //ChildrenListView.GestureRecognizers.Add(new TapGestureRecognizer());
        }


		protected override void OnAppearing()
		{
            if(ViewModel.Children?.Count == 0)
			    ChildrenListView.RefreshCommand?.Execute(null);

            ChildrenListView.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                // don't do anything if we just de-selected the row
                if (e.Item == null) return;
                // do something with e.SelectedItem
                ((ListView)sender).SelectedItem = null; // de-select the row
            };
        }
    }
}
