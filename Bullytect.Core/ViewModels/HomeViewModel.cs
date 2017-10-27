
using System;
using System.Diagnostics;
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
                    _alertService.GetLastAlertsForSelfParent().OnErrorResumeNext(Observable.Return(new AlertsPageEntity())),
                    (ParentEntity, AlertsPageEntity) => new PageModel()
                    {
                        SelfParent = ParentEntity,
                        AlertsPage = AlertsPageEntity
                    });

            });

            RefreshCommand.Subscribe(OnPageModelLoadedHandler);

            RefreshCommand.IsExecuting.Subscribe((IsLoading) => IsBusy = IsLoading);

            RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);

            TakePhotoCommand = ReactiveCommand.CreateFromObservable<string, ImageEntity>((param) =>
           {
               return _appHelper.PickPhotoStream()
                   .Select(MediaFile => MediaFile.GetStream())
                   .Do((_) => _userDialogs.ShowLoading(AppResources.Profile_Updating_Profile_Image))
                   .SelectMany((FileStream) => parentService.UploadProfileImage(FileStream))
                   .Do((_) => _userDialogs.HideLoading());
           });

            TakePhotoCommand.Subscribe((image) =>
            {
                SelfParent.ProfileImage = image.Identity;
                OnNewSelectedImage(image);
                _userDialogs.ShowSuccess(AppResources.Profile_Updating_Profile_Image_Success);
            });

            TakePhotoCommand.ThrownExceptions.Subscribe(HandleExceptions);

        }

        #region properties

        ParentEntity _selfParent = new ParentEntity();

        public ParentEntity SelfParent
        {
            get => _selfParent;
            set => SetProperty(ref _selfParent, value);
        }

        AlertsPageEntity _alertsPage = new AlertsPageEntity();

        public AlertsPageEntity AlertsPage
        {
            get => _alertsPage;
            set => SetProperty(ref _alertsPage, value);
        }


        public string ListAlertTitle
        {
            get => string.Format(AppResources.Home_Alerts_List_Title, Settings.Current.AntiquityOfAlerts);

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

        public delegate void NewSelectedImageEvent(object sender, ImageEntity NewProfileImage);
        public event NewSelectedImageEvent NewSelectedImage;

        protected virtual void OnNewSelectedImage(ImageEntity NewProfileImage)
        {
            NewSelectedImage?.Invoke(this, NewProfileImage);
        }

        #endregion

        #region commands

        public ReactiveCommand<Unit, PageModel> RefreshCommand { get; protected set; }

        public ICommand GoToProfileCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<ProfileViewModel>());
            }
        }


        public ICommand GoToChildrenCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<ChildrenViewModel>());
            }
        }


        public ICommand GoToResultsCommand
        {
            get
            {
                return new MvxCommand(() => {

                    if(SelfParent?.Children > 0){
                        ShowViewModel<ResultsViewModel>();
                    } else {
                        _appHelper.ShowAlert(AppResources.Home_Results_No_Children);
                    }

                });
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
            ProfileImage = AlertEntity.Son.ProfileImage
        }));

        public ReactiveCommand<string, ImageEntity> TakePhotoCommand { get; set; }

        public ICommand GoToAlertsCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<AlertsViewModel>());
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
            else if (ex is UploadImageFailException)
            {
                _appHelper.Toast(AppResources.Profile_Updating_Profile_Image_Failed, System.Drawing.Color.FromArgb(255, 0, 0));

            }
            else if (ex is CanNotTakePhotoFromCameraException)
            {
                _appHelper.Toast(AppResources.Profile_Can_Not_Take_Photo_From_Camera, System.Drawing.Color.FromArgb(12, 131, 193));
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
            RefreshBindings();
        }

        void RefreshBindings() {
			RaisePropertyChanged(nameof(ListAlertTitle));
			RaisePropertyChanged(nameof(ShouldShowNoAlertsFound));
			RaisePropertyChanged(nameof(ShouldShowNoChildrenFound));
        }

		#region models

		public class PageModel
		{

            public ParentEntity SelfParent { get; set; } = new ParentEntity();
            public AlertsPageEntity AlertsPage { get; set; } = new AlertsPageEntity();

		}

        #endregion
    }
}
