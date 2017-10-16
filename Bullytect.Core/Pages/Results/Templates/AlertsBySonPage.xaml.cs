using System;
using System.Collections.Generic;
using Bullytect.Core.ViewModels;
using Bullytect.Core.ViewModels.Core.Models;
using Syncfusion.SfChart.XForms;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Results.Templates
{
    public partial class AlertsBySonPage : ContentView
    {
        const int CHART_HEIGHT = 400;
        const int CHART_WIDTH = 400;

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

				SfChart sfChart = new SfChart()
				{
					HeightRequest = CHART_HEIGHT,
					WidthRequest = CHART_WIDTH,
					Legend = new ChartLegend(),

				};

				sfChart.Title.Text = chart.FullName;

				sfChart.Series.Add(new DoughnutSeries()
				{
					ItemsSource = chart.Alerts,
					DataMarker = new ChartDataMarker()
				});

				page.AlertsBySonChart.Children.Add(sfChart);
			}
        }

    }
}
