﻿
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using Bullytect.Rest.Models.Exceptions;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        
        readonly IParentService _parentService;

        public HomeViewModel(IUserDialogs userDialogs, IParentService parentService, IMvxMessenger mvxMessenger): base(userDialogs, mvxMessenger)
        {
            _parentService = parentService;


            var loadProfileCommand = ReactiveCommand
                .CreateFromObservable<string, bool>((param) =>
                {
					return _parentService.GetProfileInformation().Do((parent) =>
					{
                        Debug.WriteLine("Parent Profile " + parent?.ToString());
						SelfParent = parent;
					}).Select((_) => true);
                });

            var loadChildrenCommand = ReactiveCommand.CreateFromObservable<string, bool>((param) => {
                return _parentService.GetChildren().Do((children) =>
                {
                    Debug.WriteLine("Children Count " + children?.Count);
                    Children = children;
                }).Select((_) => true);
            });


			RefreshCommand = ReactiveCommand.CreateCombined(new[] { loadProfileCommand, loadChildrenCommand });

            RefreshCommand.IsExecuting.ToProperty(this, x => x.IsBusy, out _isBusy);   

            RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);

		}



        ParentEntity _selfParent;

        public ParentEntity SelfParent
		{
			get => _selfParent;
			set => SetProperty(ref _selfParent, value);
		}

		IList<SonEntity> _children;

		public IList<SonEntity> Children
		{
			get => _children;
			set => SetProperty(ref _children, value);
		}

        public override void Start()
        {

            //RefreshCommand.Execute(null);
        }


        #region commands

        public ReactiveCommand RefreshCommand { get; protected set; }

        public ICommand GoToNotificationsCommand
		{
			get
			{
				return new MvxCommand(() => ShowViewModel<NotificationViewModel>());
			}
		}


		public ICommand GoToChildrenCommand
		{
			get
			{
				return new MvxCommand(() => ShowViewModel<ChildrenViewModel>());
			}
		}

        public ICommand ShowSonProfileCommand => new MvxCommand<SonEntity>((SonEntity SonEntity) => ShowViewModel<SonProfileViewModel>(new SonProfileViewModel.SonParameter(){
            FullName = SonEntity.FullName,
            Birthdate = SonEntity.Birthdate,
            School = SonEntity.School
        }));

        #endregion


        protected override void HandleExceptions(Exception ex){

			if (ex is LoadProfileFailedException)
			{
				_userDialogs.ShowError(AppResources.Profile_Loading_Failed);
            } else if (ex is NoChildrenFoundException) {
                Debug.WriteLine("No Chidlren Founds");
            }
			else
			{
                base.HandleExceptions(ex);
			}
        }
	}
}
