
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
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using MvvmHelpers;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class ResultsViewModel : BaseViewModel
    {

        readonly IParentService _parentService;


        public ResultsViewModel(IUserDialogs userDialogs, 
                                IMvxMessenger mvxMessenger, AppHelper appHelper, IParentService parentService) : base(userDialogs, mvxMessenger, appHelper)
        {

            _parentService = parentService;


            RefreshCommand = ReactiveCommand.CreateFromObservable<Unit, IList<IterationEntity>>((param) => _parentService.GetLastIterations());

            RefreshCommand.Subscribe((IterationEntities) => {
                LastIteration = IterationEntities.FirstOrDefault();
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

            IterationEntity _lastIteration;

            public IterationEntity LastIteration {
                get => _lastIteration;
                set => SetProperty(ref _lastIteration, value);
            }

        bool _noIterationsFound = false;

            public bool NoIterationsFound {
                get => _noIterationsFound;
                set => SetProperty(ref _noIterationsFound, value);
            }

        #endregion


        #region delegates

        /*public delegate void GenerateCommentsBySonChartEvent(object sender, DonutChart Chart);
		public event GenerateCommentsBySonChartEvent GenerateCommentsBySonChart;

		protected virtual void OnGenerateCommentsBySonChart(DonutChart Chart)
		{
			GenerateCommentsBySonChart?.Invoke(this, Chart);
		}*/

        #endregion


        #region commands

            public ReactiveCommand<Unit, IList<IterationEntity>> RefreshCommand { get; protected set; }

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
