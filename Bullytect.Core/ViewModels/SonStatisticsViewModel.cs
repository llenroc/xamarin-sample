using System;
using System.Collections.Generic;
using System.Reactive;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Helpers;
using Bullytect.Core.ViewModels.Core.Models;
using Microcharts;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using MvvmHelpers;
using ReactiveUI;
using SkiaSharp;

namespace Bullytect.Core.ViewModels
{
    public class SonStatisticsViewModel : BaseViewModel
    {
        public SonStatisticsViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger, AppHelper appHelper) : base(userDialogs, mvxMessenger, appHelper)
        {
        }


        public class SonStatisticsParameter
        {
            public string Identity { get; set; }
            public string FullName { get; set; }
        }


        public void Init(SonStatisticsParameter sonStatisticsParameter)
        {
            Identity = sonStatisticsParameter.Identity;
            FullName = sonStatisticsParameter.FullName;
        }

        #region properties


        string _identity;

        public string Identity
        {
            get => _identity;
            set => SetProperty(ref _identity, value);
        }

        string _fullName;

        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }


        #endregion


        #region commands

        public ReactiveCommand<Unit, PageModel> RefreshCommand { get; protected set; }

        public ICommand GoToSettingsCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<SonStatisticsSettingsViewModel>());
            }
        }

        #endregion

        #region properties

        ChartModel _socialMediaActivitiesChart;

        public ChartModel SocialMediaActivitiesChart
        {
            get => _socialMediaActivitiesChart ?? (_socialMediaActivitiesChart = new ChartModel()
            {
                Title = "Social Data",
                Entries = new List<Entry>(){
                    new Entry(34)
                    {
                        Label = "Facebook",
                        ValueLabel = "34%",
                        Color = SKColor.Parse("#3b5998")
                    },
                    new Entry(23)
                    {
                        Label = "Instagram",
                        ValueLabel = "23",
                        Color = SKColor.Parse("#E03867")
                    },
                    new Entry(10)
                    {
                        Label = "Youtube",
                        ValueLabel = "10",
                        Color = SKColor.Parse("#cc181e")
                    }

                },
                Type = typeof(DonutChart),

            });

            set => SetProperty(ref _socialMediaActivitiesChart, value);
        }


        ChartModel _fourDimensionsChart;

        public ChartModel FourDimensionsChart {
			get => _fourDimensionsChart ?? (_fourDimensionsChart = new ChartModel()
			{
				Title = "Four Dimensions",
				Entries = new List<Entry>(){
					new Entry(6)
					{
						Label = "Sexo",
						ValueLabel = "6",
						Color = SKColor.Parse("#6BC7E0")
					},
                    new Entry(2)
					{
						Label = "Droga",
						ValueLabel = "2",
						Color = SKColor.Parse("#6BC7E0")
					},
					new Entry(6)
					{
						Label = "Violencia",
						ValueLabel = "6",
						Color = SKColor.Parse("#6BC7E0")
					},
					new Entry(4)
					{
						Label = "Bulling",
						ValueLabel = "4",
						Color = SKColor.Parse("#6BC7E0")
					}

				},
                Type = typeof(BarChart),

			});
            set => SetProperty(ref _fourDimensionsChart, value);

        }

		ChartModel _sentimentAnalysisChart;

		public ChartModel SentimentAnalysisChart
        {
			get => _sentimentAnalysisChart ?? (_sentimentAnalysisChart = new ChartModel()
			{
				Title = "Sentiment Analysis",
				Entries = new List<Entry>(){
					new Entry(50)
					{
						Label = "Positive",
						ValueLabel = "50%",
						Color = SKColor.Parse("#00FF00")
					},
					new Entry(45)
					{
						Label = "Negative",
						ValueLabel = "45%",
						Color = SKColor.Parse("#FF0000")
					},
					new Entry(5)
					{
						Label = "Neutro",
						ValueLabel = "5%",
						Color = SKColor.Parse("#9c9c9c")
					}

				},
                Type = typeof(DonutChart),

			});
            set => SetProperty(ref _sentimentAnalysisChart, value);
        }


		ChartModel _communitiesCharts;

		public ChartModel CommunitiesCharts
        {
			get => _communitiesCharts ?? (_communitiesCharts = new ChartModel()
			{
				Title = "Communities",
				Entries = new List<Entry>(){
					new Entry(50)
					{
						Label = "Sex",
						ValueLabel = "50",
						Color = SKColor.Parse("#6BC7E0")
					},
					new Entry(4)
					{
						Label = "Violence",
						ValueLabel = "4",
						Color = SKColor.Parse("#6BC7E0")
					},
					new Entry(23)
					{
						Label = "Drugs",
						ValueLabel = "23",
						Color = SKColor.Parse("#6BC7E0")
					}

				},
                Type = typeof(LineChart),

			});
            set => SetProperty(ref _communitiesCharts, value);
        }


        #endregion


        #region models

        public class PageModel
        {
        }

        #endregion
    }
}
