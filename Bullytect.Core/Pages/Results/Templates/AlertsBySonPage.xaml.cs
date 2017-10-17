using System;
using System.Collections.Generic;
using Bullytect.Core.ViewModels;
using Bullytect.Core.ViewModels.Core.Models;
using Microcharts;
using Microcharts.Forms;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Results.Templates
{
    public partial class AlertsBySonPage : ContentView
    {
        const int CHART_HEIGHT = 250;
        const int CHART_WIDTH = 250;

		public static readonly BindableProperty AlertsBySonProperty = BindableProperty.Create(
			nameof(AlertsBySon),
			typeof(IList<AlertsBySon>),
			typeof(AlertsBySonPage),
			propertyChanging: OnAlertsBySonPropertyChanging );

        public AlertsBySonPage()
        {
            InitializeComponent();
        }

		public IList<AlertsBySon> AlertsBySon
		{
			get { return (IList<AlertsBySon>)GetValue(AlertsBySonProperty); }
			set { SetValue(AlertsBySonProperty, value); }

		}

        static void OnAlertsBySonPropertyChanging(BindableObject bindable, object oldValue, object newValue){
			var page = bindable as AlertsBySonPage;
			var charts = newValue as IList<AlertsBySon>;

			page.AlertsBySonChart.Children.Clear();

			page.AlertsBySonChart.HeightRequest = CHART_HEIGHT * charts.Count;

			foreach (var chart in charts)
			{

				ChartView MCChart = new ChartView()
				{
					HeightRequest = CHART_HEIGHT,
					WidthRequest = CHART_WIDTH

				};
				//sfChart.Title.Text = chart.FullName;

                var ChartContainer = new StackLayout();

                ChartContainer.Children.Add(new Label() {
                    Text = chart.FullName
                });


				MCChart.Chart = new DonutChart() { Entries = chart.Alerts };

				MCChart.Chart.LabelTextSize = 20.45f;

                ChartContainer.Children.Add(MCChart);

				page.AlertsBySonChart.Children.Add(ChartContainer);
			}
        }

    }
}
