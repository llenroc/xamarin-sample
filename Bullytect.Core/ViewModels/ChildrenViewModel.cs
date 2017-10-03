
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;
using Bullytect.Core.Rest.Models.Exceptions;

namespace Bullytect.Core.ViewModels
{
    public class ChildrenViewModel : BaseViewModel
    {

        readonly IParentService _parentsService;

        public ChildrenViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger, IParentService parentsService) : base(userDialogs, mvxMessenger)
        {

            _parentsService = parentsService;


            LoadChildrenCommand = ReactiveCommand.CreateFromObservable<string, IList<SonEntity>>((param) => _parentsService.GetChildren());

            LoadChildrenCommand.Subscribe((children) =>
            {
                Debug.WriteLine("Children Count " + children?.Count);
                Children = children;
            });

            LoadChildrenCommand.IsExecuting.ToProperty(this, x => x.IsBusy, out _isBusy);

            LoadChildrenCommand.ThrownExceptions.Subscribe(HandleExceptions);

        }


        #region properties

        IList<SonEntity> _children = new List<SonEntity>();

        public IList<SonEntity> Children
        {
            get => _children;
            set => SetProperty(ref _children, value);
        }

        #endregion

        #region commands

        public ReactiveCommand<string, IList<SonEntity>> LoadChildrenCommand { get; protected set; }


        public ICommand GoToAddSonCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<EditSonViewModel>());
            }
        }


		public ICommand ShowSonProfileCommand
		{
			get
			{
                return new MvxCommand<SonEntity>((SonEntity) => ShowViewModel<SonProfileViewModel>(new SonProfileViewModel.SonParameter(){
                    Identity = SonEntity.Identity,
                    FullName = SonEntity.FullName,
                    Birthdate = SonEntity.Birthdate,
                    School = SonEntity.School
                }));
			}
		}

        public ICommand EditSonCommand => new MvxCommand<string>((string Id) => ShowViewModel<EditSonViewModel>(new { Id }));

        #endregion


        protected override void HandleExceptions(Exception ex)
        {
		    if (ex is NoChildrenFoundException)
			{
                DataFound = false;
			}
			else
			{
				base.HandleExceptions(ex);
			}
		}



    }
}
