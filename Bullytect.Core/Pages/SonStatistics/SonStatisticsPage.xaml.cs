﻿
using System.Diagnostics;
using Bullytect.Core.Pages.Common;
using Bullytect.Core.ViewModels;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.SonStatistics
{
    public partial class SonStatisticsPage : BaseContentPage<SonStatisticsViewModel>
    {
        public SonStatisticsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

		protected override void OnAppearing()
		{

            CarouseView.PositionSelected += Handle_PositionSelected;

		}

		protected override void OnDisappearing()
		{

            CarouseView.PositionSelected -= Handle_PositionSelected;
			
		}

        void Handle_PositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e) {
            
            Debug.WriteLine("Chart pos -> " + e.NewValue);
            ViewModel.RefreshChartCommand.Execute(e.NewValue);
        }
    }
}
