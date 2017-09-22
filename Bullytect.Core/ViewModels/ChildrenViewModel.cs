
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Acr.UserDialogs;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using Bullytect.Rest.Models.Exceptions;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class ChildrenViewModel : BaseViewModel
    {

        readonly IParentService _parentsService;

        public ChildrenViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger, IParentService parentsService) : base(userDialogs, mvxMessenger)
        {

            _parentsService = parentsService;


            LoadChildrenCommand = ReactiveCommand.CreateFromObservable<string, IList<SonEntity>>((param) => _parentsService.GetChildren());

            LoadChildrenCommand.IsExecuting.ToProperty(this, x => x.IsBusy, out _isBusy);

            LoadChildrenCommand.ThrownExceptions.Subscribe(HandleExceptions);

		}


        public override void Start()
        {
            LoadChildrenCommand.Execute(null);
        }

        #region commands

        public ReactiveCommand<string, IList<SonEntity>> LoadChildrenCommand { get; protected set; }

		#endregion


        protected override void HandleExceptions(Exception ex)
        {
		    if (ex is NoChildrenFoundException)
			{
				Debug.WriteLine("No Chidlren Founds");
			}
			else
			{
				base.HandleExceptions(ex);
			}
		}



    }
}
