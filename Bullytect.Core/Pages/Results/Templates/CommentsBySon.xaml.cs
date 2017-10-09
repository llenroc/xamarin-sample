
using Bullytect.Core.ViewModels;
using Microcharts;
using Microcharts.Forms;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Results.Templates
{
    public partial class CommentsBySon : ContentPage
    {
        public CommentsBySon()
        {
            InitializeComponent();

			NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {

            var ViewModel = BindingContext as ResultsViewModel;
            if(ViewModel != null) {
                ViewModel.GenerateCommentsBySonChart += ViewModel_OnGenerateCommentsBySonChartEvent;


            }



        }


        protected override void OnDisappearing()
        {

			var ViewModel = BindingContext as ResultsViewModel;
			if (ViewModel != null)
			{
				ViewModel.GenerateCommentsBySonChart -= ViewModel_OnGenerateCommentsBySonChartEvent;

			}
        }


		void ViewModel_OnGenerateCommentsBySonChartEvent(object sender, DonutChart Chart)
		{

           /* var chartView = new ChartView()
            {
                Chart = Chart
            };

            Container.Children.Add(chartView);*/
           
            //CommentsBySonChart.Chart = Chart;
			
		}
    }
}
