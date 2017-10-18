
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Helpers;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Rest.Models.Exceptions;
using Bullytect.Core.Services;
using Bullytect.Core.ViewModels.Core.Models;
using Microcharts;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using MvvmHelpers;
using ReactiveUI;
using SkiaSharp;

namespace Bullytect.Core.ViewModels
{
    public class ResultsViewModel : BaseViewModel
    {

        readonly IParentService _parentService;
        readonly IAlertService _alertsService;

        public ResultsViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger,
                                AppHelper appHelper, IParentService parentService, IAlertService alertsService) : base(userDialogs, mvxMessenger, appHelper)
        {

            _parentService = parentService;
            _alertsService = alertsService;


            RefreshCommand = ReactiveCommand.CreateFromObservable<Unit, PageModel>((param) => Observable.Zip(_parentService.GetLastIterations(), _alertsService.GetAlertsBySon(), (IterationEntities, AlertsBySon) => new PageModel()
            {
                IterationsEntities = IterationEntities,
                AlertsBySon = AlertsBySon
            }));

            RefreshCommand.Subscribe((PageModel) =>
            {
                LastIteration = PageModel.IterationsEntities?.FirstOrDefault();
                //AlertsBySon.ReplaceRange(PageModel.AlertsBySon);
                //LastIterations.ReplaceRange(IterationEntities);
                NoIterationsFound = false;
            });

            RefreshCommand.IsExecuting.ToProperty(this, x => x.IsBusy, out _isBusy);

            RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);
        }

        #region properties

        ChartModel _lastIterationsChart;
        public ChartModel LastIterationsChart {

            get => _lastIterationsChart ?? (_lastIterationsChart = new ChartModel(){
                Title = "Las Iterations",
                Entries = new List<Entry>() {
					new Entry(45)
					{
						Label = String.Format("{0:d/M/yyyy HH:mm:ss}", new DateTime()),
						ValueLabel = "45",
						Color = SKColor.Parse("#6BC7E0")
					},
					new Entry(36)
					{
						Label = String.Format("{0:d/M/yyyy HH:mm:ss}", new DateTime().AddMinutes(10)),
						ValueLabel = "36",
						Color = SKColor.Parse("#6BC7E0")
					},
					new Entry(67)
					{
						Label = String.Format("{0:d/M/yyyy HH:mm:ss}", new DateTime().AddMinutes(20)),
						ValueLabel = "67",
						Color = SKColor.Parse("#6BC7E0")
					},
					new Entry(45)
					{
						Label = String.Format("{0:d/M/yyyy HH:mm:ss}", new DateTime().AddMinutes(30)),
						ValueLabel = "45",
						Color = SKColor.Parse("#6BC7E0")
					},
					new Entry(45)
					{
						Label = String.Format("{0:d/M/yyyy HH:mm:ss}", new DateTime().AddMinutes(40)),
						ValueLabel = "45",
						Color = SKColor.Parse("#6BC7E0")
					}
                },
                Type = typeof(LineChart)

            });

            set => SetProperty(ref _lastIterationsChart, value);

        }



        public ObservableRangeCollection<ChartModel> AlertsBySonChartList { get; } = new ObservableRangeCollection<ChartModel>() {

            new ChartModel () {
                Title = "Sergio Martín",
                Entries = new List<Entry> () {
					new Entry(34)
                    {
                        Label = "INFO",
                        ValueLabel = "34",
                        Color = SKColor.Parse("#2C8DA9")
                    },
					new Entry(23)
					{
						Label = "WARNING",
						ValueLabel = "23",
						Color = SKColor.Parse("#FFA700")
					},
					new Entry(10)
					{
						Label = "DANGER",
						ValueLabel = "10",
						Color = SKColor.Parse("#D93028")
					}
				},
                Type = typeof(DonutChart)
            },
			new ChartModel () {
                Title = "Alberto Lopez",
				Entries = new List<Entry> () {
					new Entry(34)
					{
						Label = "INFO",
						ValueLabel = "34",
						Color = SKColor.Parse("#2C8DA9")
					},
					new Entry(23)
					{
						Label = "WARNING",
						ValueLabel = "23",
						Color = SKColor.Parse("#FFA700")
					},
					new Entry(10)
					{
						Label = "DANGER",
						ValueLabel = "10",
						Color = SKColor.Parse("#D93028")
					}
				},
                Type = typeof(DonutChart)
			},
			new ChartModel () {
				Title = "Alberto Lopez",
				Entries = new List<Entry> () {
					new Entry(34)
					{
						Label = "INFO",
						ValueLabel = "34",
						Color = SKColor.Parse("#2C8DA9")
					},
					new Entry(23)
					{
						Label = "WARNING",
						ValueLabel = "23",
						Color = SKColor.Parse("#FFA700")
					},
					new Entry(10)
					{
						Label = "DANGER",
						ValueLabel = "10",
						Color = SKColor.Parse("#D93028")
					}
				},
                Type = typeof(DonutChart)
			}

        };


        IterationEntity _lastIteration;

        public IterationEntity LastIteration
        {
            get => _lastIteration;
            set => SetProperty(ref _lastIteration, value);
        }

        bool _noIterationsFound = false;

        public bool NoIterationsFound
        {
            get => _noIterationsFound;
            set => SetProperty(ref _noIterationsFound, value);
        }

        #endregion


        #region delegates

        public delegate void AlertsBySonLoadedEvent(object sender, IList<AlertsBySon> AlertsBySon);
        public event AlertsBySonLoadedEvent AlertsBySonLoaded;

        protected virtual void OnAlertsBySonLoaded(IList<AlertsBySon> AlertsBySon)
        {
            AlertsBySonLoaded?.Invoke(this, AlertsBySon);
        }

        #endregion


        #region commands

        public ReactiveCommand<Unit, PageModel> RefreshCommand { get; protected set; }

        public ICommand GoToSettingsCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<ResultsSettingsViewModel>());
            }
        }

        #endregion

        protected override void HandleExceptions(Exception ex)
        {

            if (ex is NoIterationsFoundForSelfParentException)
            {
                NoIterationsFound = true;
            }
            else
            {
                base.HandleExceptions(ex);
            }
        }

        #region models

        public class PageModel
        {

            public IList<IterationEntity> IterationsEntities { get; set; }
            public IList<AlertsBySon> AlertsBySon { get; set; }
        }

        #endregion


        /*IObservable<DonutChart> CreateCommentsBySonChart() {

            return _parentService.GetCommentsBySonForLastIteration().Select((CommentsBySonDict) => CommentsBySonDict.ToList()).Select((CommentsBySonList) => CommentsBySonList.Select((CommentsBySon) => new Entry(float.Parse(CommentsBySon.Value))
			{
                Label = CommentsBySon.Key,
                ValueLabel = CommentsBySon.Value,
                Color = SKColor.Parse("#266489")
            })).Select((Entries) => {

                var chart = new DonutChart() { Entries = Entries };
                return chart;
            });
        }*/
    }
}
