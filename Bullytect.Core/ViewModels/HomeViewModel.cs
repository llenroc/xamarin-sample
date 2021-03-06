﻿
using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;
using Bullytect.Core.Rest.Models.Exceptions;
using Bullytect.Core.Exceptions;
using Bullytect.Core.Helpers;
using Plugin.Media.Abstractions;
using System.Reactive;
using Bullytect.Core.Config;
using System.Collections.Generic;
using MvvmHelpers;
using System.Linq;

namespace Bullytect.Core.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {

        readonly IParentService _parentService;
        readonly IAlertService _alertService;

        public HomeViewModel(IUserDialogs userDialogs, IParentService parentService,
            IMvxMessenger mvxMessenger, AppHelper appHelper, IAlertService alertService) : base(userDialogs, mvxMessenger, appHelper)
        {
            _parentService = parentService;
            _alertService = alertService;

            RefreshCommand = ReactiveCommand.CreateFromObservable<Unit, PageModel>((param) =>
            {
                RefreshBindings();

                return Observable.Zip(
                    _parentService.GetProfileInformation(),
                    _parentService.GetChildren().OnErrorResumeNext(Observable.Return(new List<SonEntity>())),
                    _alertService.GetLastAlertsForSelfParent().OnErrorResumeNext(Observable.Return(new AlertsPageEntity())),
                    (ParentEntity, SonEntities, AlertsPageEntity) => new PageModel()
                    {
                        SelfParent = ParentEntity,
                        SonEntities = SonEntities,
                        AlertsPage = AlertsPageEntity
                    });

            });

            RefreshCommand.Subscribe(OnPageModelLoadedHandler);

            RefreshCommand.IsExecuting.Subscribe((IsLoading) => IsBusy = IsLoading);

            RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);


        }

        #region properties

        ParentEntity _selfParent = new ParentEntity();

        public ParentEntity SelfParent
        {
            get => _selfParent;
            set => SetProperty(ref _selfParent, value);
        }

        public ObservableRangeCollection<SonEntity> SonEntities { get; } = new ObservableRangeCollection<SonEntity>();

        AlertsPageEntity _alertsPage = new AlertsPageEntity();

        public AlertsPageEntity AlertsPage
        {
            get => _alertsPage;
            set => SetProperty(ref _alertsPage, value);
        }


        public string ListAlertTitle
        {
            get => Settings.Current.AntiquityOfAlerts == 0 ?
                           AppResources.Home_Alerts_List_Title :
                           string.Format(AppResources.Home_Alerts_List_Title_Filter, Settings.Current.AntiquityOfAlerts);

        }

		public bool ShouldShowNoAlertsFound
		{
            get => SelfParent?.Children > 0 && AlertsPage?.Returned == 0 && !IsBusy && !ErrorOccurred;
		}

        public bool ShouldShowNoChildrenFound
        {
            get => SelfParent?.Children == 0 && !IsBusy && !ErrorOccurred;
        }

        #endregion

        #region delegates

        public delegate void ChildrenLoadedEvent(object sender, List<SonEntity> SonEntities);
        public event ChildrenLoadedEvent ChildrenLoaded;

        protected virtual void OnChildrenLoaded(List<SonEntity> SonEntities)
        {
            ChildrenLoaded?.Invoke(this, SonEntities);
        }

        #endregion 

        #region commands

        public ReactiveCommand<Unit, PageModel> RefreshCommand { get; protected set; }


        public ICommand GoToChildrenCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<ChildrenViewModel>());
            }
        }


        public ICommand GoToSettingsCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<HomeSettingsViewModel>());
            }
        }

        public ICommand AddSonCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<EditSonViewModel>());
            }
        }


        public ICommand ShowAlertDetailCommand => new MvxCommand<AlertEntity>((AlertEntity AlertEntity) => ShowViewModel<AlertDetailViewModel>(new AlertDetailViewModel.AlertParameter()
        {
            Identity = AlertEntity.Identity,
            Level = AlertEntity.Level,
            Title = AlertEntity.Title,
            Payload = AlertEntity.Payload,
            CreateAt = AlertEntity.CreateAt,
            SonFullName = AlertEntity.Son.FullName,
            SonIdentity = AlertEntity.Son.Identity,
            ProfileImage = AlertEntity.Son.ProfileImage,
            Category = AlertEntity.Category,
            Since = AlertEntity.Since
        }));

        public ICommand ShowSonProfileCommand
        {
            get
            {
                return new MvxCommand<string>((SonId) => {

                    var SonEntity = SonEntities.FirstOrDefault((Son) => Son.Identity.Equals(SonId));

                    if(SonEntity != null)

                        ShowViewModel<SonProfileViewModel>(new { SonEntity.Identity});

                });
            }
        }


        public ICommand GoToAlertsCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<AlertsViewModel>());
            }
        }

        public ICommand SeeMoreCommand
        {
            get
            {
                return new MvxCommand(() => {

                    if (SelfParent?.Children > 0)
                        ShowViewModel<ChildrenViewModel>();
                    else
                        ShowViewModel<EditSonViewModel>();
                });
            }

        }


        #endregion


        protected override void HandleExceptions(Exception ex)
        {

            _userDialogs.HideLoading();

            if (ex is LoadProfileFailedException)
            {
                _userDialogs.ShowError(AppResources.Home_Loading_Failed);
            }
            else
            {
                base.HandleExceptions(ex);
            }
        }


        void OnPageModelLoadedHandler(PageModel pageModel)
        {
            DataFound = true;
            ErrorOccurred = false;
            AlertsPage.HydrateWith(pageModel.AlertsPage);
            SelfParent.HydrateWith(pageModel.SelfParent);
            SonEntities.ReplaceRange(pageModel.SonEntities);
            OnChildrenLoaded(pageModel.SonEntities);
            RefreshBindings();
        }

        void RefreshBindings() {
            IsTimeout = false;
			RaisePropertyChanged(nameof(ListAlertTitle));
			RaisePropertyChanged(nameof(ShouldShowNoAlertsFound));
			RaisePropertyChanged(nameof(ShouldShowNoChildrenFound));
        }

        protected override void OnBackPressed()
        {


            _appHelper.RequestConfirmation(AppResources.Profile_Confirm_SignOut)
                      .Where((confirmed) => confirmed)
                      .Subscribe((_) =>
                      {

                          Settings.AccessToken = null;
                          ShowViewModel<AuthenticationViewModel>(new AuthenticationViewModel.AuthenticationParameter()
                          {
                              ReasonForAuthentication = AuthenticationViewModel.SIGN_OUT
                          });
                      });



        }

		#region models

		public class PageModel
		{

            public ParentEntity SelfParent { get; set; } = new ParentEntity();
            public List<SonEntity> SonEntities { get; set; } = new List<SonEntity>();
            public AlertsPageEntity AlertsPage { get; set; } = new AlertsPageEntity();

		}

        #endregion
    }
}
