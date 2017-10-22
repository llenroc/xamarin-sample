
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

		void Handle_PositionSelected(object sender, int pos)
		{
			Debug.WriteLine("Chart pos -> " + pos);
			ViewModel.RefreshChartCommand.Execute(pos);
		}
    }
}
