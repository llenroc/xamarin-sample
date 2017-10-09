
using System;
using System.Linq;
using System.Reactive.Linq;
using Acr.UserDialogs;
using Bullytect.Core.Helpers;
using Bullytect.Core.Services;
using Microcharts;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;
using SkiaSharp;

namespace Bullytect.Core.ViewModels
{
    public class ResultsViewModel : BaseViewModel
    {

        readonly IParentService _parentService;


        public ResultsViewModel(IUserDialogs userDialogs, 
                                IMvxMessenger mvxMessenger, AppHelper appHelper, IParentService parentService) : base(userDialogs, mvxMessenger, appHelper)
        {

            _parentService = parentService;


            RefreshCommand = ReactiveCommand.CreateFromObservable(() => CreateCommentsBySonChart().Do((DonutChart Chart) =>
            {
                CommentsBySonChart = Chart;
                OnGenerateCommentsBySonChart(Chart);
            }));

			RefreshCommand.IsExecuting.ToProperty(this, x => x.IsBusy, out _isBusy);

			RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);
        }

        #region properties


        DonutChart _commentsBySonChart;
        public DonutChart CommentsBySonChart {
            get => _commentsBySonChart;
            set => SetProperty(ref _commentsBySonChart, value);
        }


        #endregion


        #region delegates

        public delegate void GenerateCommentsBySonChartEvent(object sender, DonutChart Chart);
		public event GenerateCommentsBySonChartEvent GenerateCommentsBySonChart;

		protected virtual void OnGenerateCommentsBySonChart(DonutChart Chart)
		{
			GenerateCommentsBySonChart?.Invoke(this, Chart);
		}

        #endregion


        #region commands

            public ReactiveCommand RefreshCommand { get; protected set; }

        #endregion


        IObservable<DonutChart> CreateCommentsBySonChart() {

            return _parentService.GetCommentsBySonForLastIteration().Select((CommentsBySonDict) => CommentsBySonDict.ToList()).Select((CommentsBySonList) => CommentsBySonList.Select((CommentsBySon) => new Entry(float.Parse(CommentsBySon.Value))
			{
                Label = CommentsBySon.Key,
                ValueLabel = CommentsBySon.Value,
                Color = SKColor.Parse("#266489")
            })).Select((Entries) => {

                var chart = new DonutChart() { Entries = Entries };
                return chart;
            });
        }
    }
}
