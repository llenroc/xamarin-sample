using System;
using System.Collections.Generic;
using Bullytect.Core.I18N;
using Bullytect.Core.ViewModels.Core.Models;
using Microcharts;
using Microcharts.Forms;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Common.Templates
{
    public partial class ChartPageTemplate : ContentView
    {

        static int CHART_HEIGHT_DEFAULT = 250;
        static int CHART_WIDTH_DEFAULT = 250;
        static Type DEFAULT_CHART_TYPE = typeof(BarChart);


		public static readonly BindableProperty IsLoadingProperty = BindableProperty.Create(
            nameof(IsLoading),
            typeof(bool),
            typeof(ChartPageTemplate),
            defaultValue: true,
			propertyChanging: (bindable, oldValue, newValue) =>
			{
				var chartPage = bindable as ChartPageTemplate;
				var isLoading = (bool)newValue;
                chartPage.LoadingData.IsVisible = isLoading;
                chartPage.ChartPageContainer.IsVisible = !isLoading;
			});

		public static readonly BindableProperty DataFoundProperty = BindableProperty.Create(
			nameof(DataFound),
			typeof(bool),
			typeof(ChartPageTemplate),
			defaultValue: true,
			propertyChanging: (bindable, oldValue, newValue) =>
			{
				var chartPage = bindable as ChartPageTemplate;
				var DataFound = (bool)newValue;
                chartPage.NoDataFound.IsVisible = !DataFound;
                chartPage.ChartPageContainer.IsVisible = DataFound;
			});

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(ChartPageTemplate),
            defaultValue: string.Empty,
            propertyChanging: (bindable, oldValue, newValue) =>
            {
                var chartPage = bindable as ChartPageTemplate;
                var newTitle = newValue as string;
                chartPage.ChartTitle.Text = newTitle;
            });

		public static readonly BindableProperty LoadingTextProperty = BindableProperty.Create(
		   nameof(LoadingText),
		   typeof(string),
		   typeof(ChartPageTemplate),
            defaultValue: AppResources.Common_Loading,
		   propertyChanging: (bindable, oldValue, newValue) =>
		   {
			   var chartPage = bindable as ChartPageTemplate;
			   var newLoadingText = newValue as string;
			   chartPage.LoadingLabel.Text = newLoadingText;
		   });


        public static readonly BindableProperty ChartWidthProperty = BindableProperty.Create(
            nameof(ChartWidth),
            typeof(int),
            typeof(ChartPageTemplate),
            defaultValue: CHART_WIDTH_DEFAULT);



        public static readonly BindableProperty ChartHeightProperty = BindableProperty.Create(
            nameof(ChartHeight),
            typeof(int),
            typeof(ChartPageTemplate),
            defaultValue: CHART_HEIGHT_DEFAULT);


        public static readonly BindableProperty ChartListProperty = BindableProperty.Create(
            nameof(ChartList),
            typeof(IList<ChartModel>),
            typeof(ChartPageTemplate),
            defaultValue: new List<ChartModel>(),
            propertyChanging: OnChartListPropertyChanging);


		public static readonly BindableProperty ChartProperty = BindableProperty.Create(
			nameof(Chart),
			typeof(ChartModel),
			typeof(ChartPageTemplate),
			defaultValue: new ChartModel(),
			propertyChanging: OnChartPropertyChanging);



        public ChartPageTemplate()
        {
            InitializeComponent();

        }


        #region properties

        public bool IsLoading
        {
			get { return (bool)GetValue(IsLoadingProperty); }
			set { SetValue(IsLoadingProperty, value); }
        }

        public bool DataFound
        {
			get { return (bool)GetValue(DataFoundProperty); }
			set { SetValue(DataFoundProperty, value); }

        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public string LoadingText
        {
			get { return (string)GetValue(LoadingTextProperty); }
			set { SetValue(LoadingTextProperty, value); }

        }

        public int ChartWidth
        {
            get { return (int)GetValue(ChartWidthProperty); }
            set { SetValue(ChartWidthProperty, value); }
        }


        public int ChartHeight
        {
            get { return (int)GetValue(ChartHeightProperty); }
            set { SetValue(ChartHeightProperty, value); }
        }

        public IList<ChartModel> ChartList
        {
            get { return (IList<ChartModel>)GetValue(ChartListProperty); }
            set { SetValue(ChartListProperty, value); }

        }

		public ChartModel Chart
		{
			get { return (ChartModel)GetValue(ChartProperty); }
			set { SetValue(ChartProperty, value); }

		}

        #endregion

        #region methods


        static StackLayout createChart(ChartPageTemplate page, ChartModel chart){

            var chartHeight = chart.Height != 0 ? chart.Height : page.ChartHeight;

			ChartView MCChart = new ChartView()
			{
				HeightRequest = chartHeight,
				WidthRequest = chart?.Width != 0 ? chart.Width : page.ChartWidth

			};

			var ChartContainer = new StackLayout();

			ChartContainer.Children.Add(new Label()
			{
				Text = chart.Title
			});

			var chartTypeObj = (Chart)Activator.CreateInstance(chart.Type ?? DEFAULT_CHART_TYPE);
			chartTypeObj.Entries = chart.Entries;

			MCChart.Chart = chartTypeObj;

			MCChart.Chart.LabelTextSize = 20.45f;

			ChartContainer.Children.Add(MCChart);

            return ChartContainer;

        }

        static void OnChartListPropertyChanging(BindableObject bindable, object oldValue, object newValue)
        {

			var page = bindable as ChartPageTemplate;
			var chartsList = newValue as IList<ChartModel>;

            page.ChartsContainer.HeightRequest = 0;
            page.ChartsContainer.Children.Clear();

            if(chartsList.Count > 0 ){

				foreach (var chart in chartsList)
				{
                    var chartHeight = chart.Height != 0 ? chart.Height : page.ChartHeight;
                    var chartContainer = createChart(page, chart);
					
                    page.ChartsContainer.HeightRequest += chartHeight;

					page.ChartsContainer.Children.Add(chartContainer);

				}


                page.ChartsContainer.HeightRequest += 10;

            }
		}

        static void OnChartPropertyChanging(BindableObject bindable, object oldValue, object newValue)
        {
            var page = bindable as ChartPageTemplate;
            var chart = newValue as ChartModel;
            if(chart != null) {

				page.ChartsContainer.HeightRequest = 0;
				page.ChartsContainer.Children.Clear();
				var chartContainer = createChart(page, chart);
				page.ChartsContainer.HeightRequest += chartContainer.Height;
				page.ChartsContainer.Children.Add(chartContainer);
            }

        }


        #endregion
    }
}
