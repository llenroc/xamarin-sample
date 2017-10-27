
using System.Diagnostics;
using Bullytect.Core.Pages.Common;
using Bullytect.Core.ViewModels;

namespace Bullytect.Core.Pages.Results
{
    public partial class ResultsPage : BaseContentPage<ResultsViewModel>
    {
        public ResultsPage()
        {
            InitializeComponent();
        }

		protected override void OnAppearing()
		{

			CarouseView.PositionSelected += Handle_PositionSelected;

		}

		protected override void OnDisappearing()
		{

			CarouseView.PositionSelected -= Handle_PositionSelected;

		}

		void Handle_PositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
		{
            Debug.WriteLine("Chart pos -> " + e.NewValue);
			ViewModel.RefreshChartCommand.Execute(e.NewValue);
		}
    }
}
