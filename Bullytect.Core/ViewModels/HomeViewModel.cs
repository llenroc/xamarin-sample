
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

namespace Bullytect.Core.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        
        readonly IParentService _parentService;
        readonly IAlertService _alertService;

        public HomeViewModel(IUserDialogs userDialogs, IParentService parentService, 
            IMvxMessenger mvxMessenger, IImagesService imagesService, IAlertService alertService) : base(userDialogs, mvxMessenger, imagesService)
        {
            _parentService = parentService;
            _alertService = alertService;

			RefreshCommand = ReactiveCommand.CreateFromObservable(() => _parentService.GetProfileInformation().Do((parent) =>
			{
				Debug.WriteLine("Parent Profile " + parent?.ToString());
				SelfParent = parent;
			}).SelectMany((_) => _alertService.GetLast10AlertsForSelfParent().Do((AlertsPageEntity) =>
			{
				Debug.WriteLine("Total Alerts  " + AlertsPageEntity?.Alerts?.Count);
				AlertsPage = AlertsPageEntity;
				NoAlertsFound = false;
			})));

            RefreshCommand.IsExecuting.ToProperty(this, x => x.IsBusy, out _isBusy);

            RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);

			TakePhotoCommand =  ReactiveCommand.CreateFromObservable<string, ImageEntity>((param) =>
			{
                return PickPhotoStream()
                    .Select(MediaFile => MediaFile.GetStream())
                    .Do((_) => _userDialogs.ShowLoading(AppResources.Profile_Updating_Profile_Image))
                    .SelectMany((FileStream) => parentService.UploadProfileImage(FileStream))
                    .Do((_) => _userDialogs.HideLoading());


			});

            TakePhotoCommand.Subscribe((image) => {
                _userDialogs.ShowSuccess(AppResources.Profile_Updating_Profile_Image_Success);
            });

            TakePhotoCommand.ThrownExceptions.Subscribe(HandleExceptions);

        }

        #region properties

        ParentEntity _selfParent = null;

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

        bool _noAlertsFound = false;

        public bool NoAlertsFound
        {
            get => _noAlertsFound;
            set => SetProperty(ref _noAlertsFound, value);
        }

        #endregion


        #region commands

        public ReactiveCommand RefreshCommand { get; protected set; }

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

        public ICommand AddSonCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<EditSonViewModel>());
            }
        }
        

        public ICommand ShowAlertDetailCommand => new MvxCommand<AlertEntity>((AlertEntity AlertEntity) => ShowViewModel<AlertDetailViewModel>(new AlertDetailViewModel.AlertParameter(){
            Identity = AlertEntity.Identity,
            Level = AlertEntity.Level,
            Title = AlertEntity.Title,
            Payload = AlertEntity.Payload,
            CreateAt = AlertEntity.CreateAt,
            SonFullName = AlertEntity.Son.FullName,
            SonIdentity = AlertEntity.Son.Identity
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

            if (ex is LoadProfileFailedException)
            {
                _userDialogs.ShowError(AppResources.Home_Loading_Failed);
            }
            else if( ex is NoNewAlertsFoundException) {
                NoAlertsFound = true;
            }else if (ex is CanNotTakePhotoFromCameraException) {
                var toastConfig = new ToastConfig(AppResources.Profile_Can_Not_Take_Photo_From_Camera);
                toastConfig.SetDuration(3000);
                toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(12, 131, 193));
                _userDialogs.Toast(toastConfig);
            }
			else
			{
                base.HandleExceptions(ex);
			}
        }
	}
}
