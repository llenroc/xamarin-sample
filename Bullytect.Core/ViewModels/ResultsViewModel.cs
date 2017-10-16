
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
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using MvvmHelpers;
using ReactiveUI;
using Syncfusion.SfChart.XForms;

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

        public ObservableRangeCollection<IterationEntity> LastIterations { get; } = new ObservableRangeCollection<IterationEntity>(){
            new IterationEntity() {
                FinishDate = new DateTime(),
                TotalComments = 45.0
            },
            new IterationEntity() {
                FinishDate = new DateTime().AddMinutes(10),
                TotalComments = 36.0
            },
            new IterationEntity() {
                FinishDate = new DateTime().AddMinutes(20),
                TotalComments = 67.0
            },
            new IterationEntity() {
                FinishDate = new DateTime().AddMinutes(30),
                TotalComments = 45.0
            },
            new IterationEntity() {
                FinishDate = new DateTime().AddMinutes(40),
                TotalComments = 45.0
            }

        };

        public ObservableRangeCollection<AlertsBySon> AlertsBySon { get; } = new ObservableRangeCollection<AlertsBySon>() {
                new AlertsBySon() {
                    FullName = "Sergio Martín",
                    Alerts = new List<ChartDataPoint>(){
                        new ChartDataPoint("INFO", 23),
                        new ChartDataPoint("WARNING", 12),
                        new ChartDataPoint("ERROR", 21)
                    }
                },
                new AlertsBySon() {
                    FullName = "Pedro Martín",
                    Alerts = new List<ChartDataPoint>(){
                        new ChartDataPoint("INFO", 23),
                        new ChartDataPoint("WARNING", 12),
                        new ChartDataPoint("ERROR", 21)
                    }
                },
                new AlertsBySon() {
                    FullName = "María Martín",
                    Alerts = new List<ChartDataPoint>(){
                        new ChartDataPoint("INFO", 23),
                        new ChartDataPoint("WARNING", 12),
                        new ChartDataPoint("ERROR", 21)
                    }
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
