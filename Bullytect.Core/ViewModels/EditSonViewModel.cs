using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Exceptions;
using Bullytect.Core.Helpers;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.OAuth.Models;
using Bullytect.Core.OAuth.Providers.Facebook;
using Bullytect.Core.OAuth.Providers.Google;
using Bullytect.Core.OAuth.Providers.Instagram;
using Bullytect.Core.OAuth.Services;
using Bullytect.Core.Pages.EditSon.Popup;
using Bullytect.Core.Rest.Models.Exceptions;
using Bullytect.Core.Services;
using Bullytect.Core.Utils;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using MvvmHelpers;
using Plugin.Media.Abstractions;
using ReactiveUI;
using Rg.Plugins.Popup.Services;

namespace Bullytect.Core.ViewModels
{
    public class EditSonViewModel : BaseViewModel
    {

        readonly IParentService _parentService;
        readonly ISocialMediaService _socialMediaService;
        readonly ISchoolService _schoolService;
        readonly IOAuthService _oauthService;

        public EditSonViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger, IParentService parentService,
                                ISocialMediaService socialMediaService, AppHelper appHelper, IOAuthService oauthService, ISchoolService schoolService) : base(userDialogs, mvxMessenger, appHelper)
        {

            _parentService = parentService;
            _socialMediaService = socialMediaService;
            _schoolService = schoolService;
            _oauthService = oauthService;


            RefreshCommand = ReactiveCommand.CreateFromObservable<Unit, PageModel>((param) => !PageLoaded ? LoadPageModel(): Observable.Empty<PageModel>());

            RefreshCommand.Subscribe(OnPageModelLoaded);

            RefreshCommand.IsExecuting.Subscribe((isExecuting) => HandleIsExecuting(isExecuting, AppResources.Common_Loading));

			RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);

            ForceRefreshCommand = ReactiveCommand.CreateFromObservable<Unit, PageModel>((param) => LoadPageModel());

			ForceRefreshCommand.Subscribe(OnPageModelLoaded);

            ForceRefreshCommand.IsExecuting.Subscribe((isExecuting) => HandleIsExecuting(isExecuting, AppResources.Common_Loading));

			ForceRefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);

            TakePhotoCommand = ReactiveCommand.CreateFromObservable<Unit, MediaFile>((param) => _appHelper.PickPhotoStream());

            TakePhotoCommand.Subscribe((ImageFile) =>
            {
                _userDialogs.HideLoading();
                NewProfileImage = ImageFile;
                OnNewSelectedImage(ImageFile);
            });

            TakePhotoCommand.ThrownExceptions.Subscribe(HandleExceptions);


            SaveChangesCommand = ReactiveCommand
                .CreateFromObservable<Unit, bool>((param) =>
                {

                    return (CurrentSon.Identity == null ?
                            _parentService.AddSonToSelfParent(CurrentSon.FirstName, CurrentSon.LastName, CurrentSon.Birthdate, CurrentSon.School.Identity) :
                            _parentService.UpdateSonInformation(CurrentSon.Identity, CurrentSon.FirstName, CurrentSon.LastName, CurrentSon.Birthdate, CurrentSon.School.Identity))
                                .Do((SonEntity) => CurrentSon.HydrateWith(SonEntity))
                                .SelectMany((SonEntity) => _socialMediaService
                                            .SaveAllSocialMedia(
                                                CurrentSon.Identity,
                                                CurrentSocialMedia.Select(s => { s.Son = CurrentSon.Identity; return s; }).ToList()
                                               ).Catch<IList<SocialMediaEntity>, NoSocialMediaFoundException>(ex => Observable.Return(new List<SocialMediaEntity>()))
                                           )
                                .Do((SocialMediaEntities) => CurrentSocialMedia.ReplaceRange(SocialMediaEntities))

                                .SelectMany((_) =>
                                {
                                    return NewProfileImage != null ?
                                            _parentService.UploadSonProfileImage(CurrentSon.Identity, NewProfileImage.GetStream()) :
                                                          Observable.Empty<ImageEntity>();
                                })
                                .Select((_) => true)
                                .DefaultIfEmpty(true);
                });

            SaveChangesCommand.Subscribe((_) =>
            {
                NewProfileImage = null;
                OnSonUpdated(CurrentSon);
                _userDialogs.ShowSuccess(AppResources.EditSon_Saved_Changes_Successfully);
                IsDirtyMonitoring = true;
            });

            SaveChangesCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.EditSon_Saving_Changes));

            SaveChangesCommand.ThrownExceptions.Subscribe(HandleExceptions);

            SaveSchoolCommand = ReactiveCommand.CreateFromObservable<Unit, SchoolEntity>((param) => _schoolService.CreateSchool(NewSchool.Name, NewSchool.Residence, NewSchool.Latitude, NewSchool.Longitude, NewSchool.Province, NewSchool.Tfno, NewSchool.Email));

            SaveSchoolCommand.Subscribe((SchoolAdded) =>
            {
          
                NewSchool = new SchoolEntity();
                CurrentSon.School.HydrateWith(SchoolAdded);
                _appHelper.ShowAlert(AppResources.EditSon_School_Saved);
                OnSchoolAdded(SchoolAdded);
            
            });


            SaveSchoolCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.EditSon_Saving_School));

            SaveSchoolCommand.ThrownExceptions.Subscribe(HandleExceptions);


            FindSchoolsCommand = ReactiveCommand.CreateFromObservable<string, IList<SchoolEntity>>(
                (name) => _schoolService.FindSchools(name));
        
            FindSchoolsCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.EditSon_Find_School));

            FindSchoolsCommand.ThrownExceptions.Subscribe(HandleExceptions);

            FindSchoolsCommand.Subscribe((SchoolsFounded) => {
                SearchPerformed = true;
                Schools.ReplaceRange(SchoolsFounded);
            });
        }

        #region properties

        public ObservableRangeCollection<SocialMediaEntity> CurrentSocialMedia { get; } = new ObservableRangeCollection<SocialMediaEntity>();
        public ObservableRangeCollection<SchoolEntity> Schools { get; } = new ObservableRangeCollection<SchoolEntity>();


        protected MediaFile _newProfileImage;

        public MediaFile NewProfileImage
        {
            get => _newProfileImage;
            set => SetProperty(ref _newProfileImage, value);
        }

        SonEntity _currentSon = new SonEntity();

        [IsDirtyMonitoring]
        public SonEntity CurrentSon
        {
            get => _currentSon;
            set => SetProperty(ref _currentSon, value);
        }


        SchoolEntity _newSchool = new SchoolEntity();

        public SchoolEntity NewSchool
        {
            get => _newSchool;
            set => SetProperty(ref _newSchool, value);
        }

        bool _searchPerformed = false;

        public bool SearchPerformed
        {
            get => _searchPerformed;
            set => SetProperty(ref _searchPerformed, value);
        }

        string _sonToEdit = null;

        public string SonToEdit
        {
            get => _sonToEdit;
            set => SetProperty(ref _sonToEdit, value);
        }

        bool _addSchool;

        public bool AddSchool
        {
            get => _addSchool;
            set => SetProperty(ref _addSchool, value);
        }

        public string PageTitle
        {

            get => !string.IsNullOrEmpty(SonToEdit) ? AppResources.Page_Edit_Son_Title : AppResources.Page_Add_Son_Title;

        }

        long _totalSchools;

        public long TotalSchools
        {
            get => _totalSchools;
            set => SetProperty(ref _totalSchools, value);

        }

        bool _pageLoaded = false;

        public bool PageLoaded {
            get => _pageLoaded;
            set => SetProperty(ref _pageLoaded, value);
        }


        public DateTime MinimumDatetime { get; set; } = DateTime.Now.AddYears(-18);

        public DateTime MaximumDatetime { get; set; } = DateTime.Now.AddYears(-8);

        #endregion

        #region delegates

        public delegate void NewSelectedImageEvent(object sender, MediaFile NewProfileImage);
        public event NewSelectedImageEvent NewSelectedImage;

        protected virtual void OnNewSelectedImage(MediaFile NewProfileImage)
        {
            NewSelectedImage?.Invoke(this, NewProfileImage);
        }

        public delegate void SchoolAddedEvent(object sender, SchoolEntity SchoolEntity);
        public event SchoolAddedEvent SchoolAdded;

        protected virtual void OnSchoolAdded(SchoolEntity SchoolEntity)
        {
            SchoolAdded?.Invoke(this, SchoolEntity);
        }

        public delegate void SonUpdatedEvent(object sender, SonEntity SonEntity);
        public event SonUpdatedEvent SonUpdated;

        protected virtual void OnSonUpdated(SonEntity SonEntity)
        {
            SonUpdated?.Invoke(this, SonEntity);
        }

        #endregion

        public void Init(string SonIdentity)
        {
            if (!string.IsNullOrEmpty(SonIdentity))
                SonToEdit = SonIdentity;
        }

        public override void Start()
        {
            CurrentSocialMedia.CollectionChanged += CurrentSocialMediaCollectionChanged;
            Schools.CollectionChanged += SchoolsCollectionChanged;
        }

        void CurrentSocialMediaCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(CurrentSocialMedia));
        }

        void SchoolsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Schools));
        }

        #region commands

        public ReactiveCommand<Unit, bool> SaveChangesCommand { get; protected set; }

        public ReactiveCommand<Unit, MediaFile> TakePhotoCommand { get; set; }

        public ReactiveCommand<Unit, PageModel> RefreshCommand { get; protected set; }

        public ReactiveCommand<Unit, PageModel> ForceRefreshCommand { get; protected set;  }

        public ReactiveCommand<string, IList<SchoolEntity>> FindSchoolsCommand { get; protected set; }

        public ICommand ToggleFacebookSocialMediaCommand
                        => new MvxCommand(() => ToggleSocialMediaHandler(new SonFacebookOAuth2(), AppConstants.FACEBOOK));

        public ICommand ToggleInstagramSocialMediaCommand
                        => new MvxCommand(() => ToggleSocialMediaHandler(new InstagramOAuth2(), AppConstants.INSTAGRAM));

        public ICommand ToggleYoutubeSocialMediaCommand
                        => new MvxCommand(() => ToggleSocialMediaHandler(new SonGoogleOAuth2(), AppConstants.YOUTUBE));

        public ReactiveCommand<Unit, SchoolEntity> SaveSchoolCommand { get; set; }


        public ICommand ResetSocialMediaCommand
                        => new MvxCommand<string>((string Type) => ResetSocialMediaHandler(Type));

        public ICommand ShowAddSchoolPopupCommand
                        => new MvxCommand(async () =>
                        {
                            SearchPerformed = false;
                            if(PopupNavigation.PopupStack.Count > 0) {
                                await PopupNavigation.PopAllAsync();
                            }
                            var page = new AddSchoolPopup();
                            page.BindingContext = this;
                            await PopupNavigation.PushAsync(page);
                        });


        public ICommand SchoolSelectedCommand
            => new MvxCommand<SchoolEntity>(async (SchoolSelected) => {
                SearchPerformed = false;
                CurrentSon.School.HydrateWith(SchoolSelected);
                if (PopupNavigation.PopupStack.Count > 0)
                    await PopupNavigation.PopAllAsync(true);
                Schools.Clear();
            });

		#endregion

		#region methods

		protected override void OnBackPressed()
		{

			if (IsDirty)
			{

                _appHelper.RequestConfirmation(AppResources.EditSon_Cancel)
					  .Subscribe((_) => base.OnBackPressed());
			}
			else
			{

				base.OnBackPressed();
			}
		}

        IObservable<SonInformation> GetSonInformation()
        {

            return Observable.Zip(
                _parentService.GetSonById(SonToEdit),
                _socialMediaService.GetAllSocialMediaBySon(SonToEdit)
                    .Catch<IList<SocialMediaEntity>, NoSocialMediaFoundException>(ex => Observable.Return(new List<SocialMediaEntity>())),
                (SonEntity SonEntity, IList<SocialMediaEntity> SocialMedia) =>
                new SonInformation()
                {
                    Son = SonEntity,
                    SocialMedia = SocialMedia
                }).ObserveOn(Scheduler.Default);
        }


        IObservable<long> GetTotalSchools(){
            return _schoolService.CountSchools();
        }


        IObservable<PageModel> LoadPageModel() {
            

			if (!string.IsNullOrEmpty(SonToEdit))
			{

                return Observable.Zip(GetSonInformation(), GetTotalSchools(), (sonInformation, Total) => new PageModel()
				{
					SonInformation = sonInformation,
                    TotalSchools = Total

				}).ObserveOn(Scheduler.Default);
			}
			else
			{

                return GetTotalSchools().Select((Total) => new PageModel()
				{
                    TotalSchools = Total

				});
			}

        }

        void ResetSocialMediaHandler(string Type)
        {
			var index = CurrentSocialMedia.Select((SocialItem, SocialIndex)
												  => new { SocialItem, SocialIndex }).FirstOrDefault(i => i.SocialItem.Type.Equals(Type))?.SocialIndex;

            if(index.HasValue && index.Value >= 0) {

				Debug.WriteLine(string.Format("Disable Social Media: {0}", Type));

				CurrentSocialMedia.RemoveAt(index.Value);

				_userDialogs.ShowSuccess(AppResources.EditSon_Social_Media_Deleted);
            }
			
        }

        void ToggleSocialMediaHandler(OAuth2 Provider, string Type)
        {


            var index = CurrentSocialMedia.Select((SocialItem, SocialIndex) 
                                                  => new { SocialItem, SocialIndex }).FirstOrDefault(i => i.SocialItem.Type.Equals(Type))?.SocialIndex;
            

             if(!index.HasValue || (index.HasValue && CurrentSocialMedia.ElementAt(index.Value).InvalidToken)) {

				Debug.WriteLine(string.Format("Enable Social Media: {0}", Type));

				_oauthService.Authenticate(Provider)
                             .Where(AuthDict => AuthDict.ContainsKey("access_token") && !string.IsNullOrWhiteSpace("access_token"))
                             .Subscribe(AuthDict =>
					{

						if (index.HasValue)
						{

							var SocialMedia = CurrentSocialMedia.ElementAt(index.Value);
                            SocialMedia.AccessToken = AuthDict["access_token"];
                            if(AuthDict.ContainsKey("refresh_token"))
                                SocialMedia.RefreshToken = AuthDict["refresh_token"];
							SocialMedia.InvalidToken = false;
							RaisePropertyChanged(nameof(CurrentSocialMedia));
						}
						else
						{
							CurrentSocialMedia.Add(new SocialMediaEntity()
							{
                                AccessToken = AuthDict["access_token"],
                                RefreshToken = AuthDict.ContainsKey("refresh_token") 
                                                       ? AuthDict["refresh_token"] : string.Empty,
								InvalidToken = false,
								Type = Type
							});
						}

							//_userDialogs.ShowSuccess(AppResources.EditSon_Social_Media_Added);
					});


            } else {

				Debug.WriteLine(string.Format("Disable Social Media: {0}", Type));

				CurrentSocialMedia.RemoveAt(index.Value);

				_userDialogs.ShowSuccess(AppResources.EditSon_Social_Media_Deleted);
            }
        }


        void OnPageModelLoaded(PageModel Page) {

            if (Page.SonInformation != null)
            {
                CurrentSon.HydrateWith(Page.SonInformation.Son);
                CurrentSocialMedia.ReplaceRange(Page.SonInformation.SocialMedia);
            }

            TotalSchools = Page.TotalSchools;

            ResetCommonProps();

            PageLoaded = true;

			IsDirtyMonitoring = true;

        }

		protected override void HandleExceptions(Exception ex)
		{
			if (ex is UploadImageFailException)
			{
				_appHelper.ShowAlert(AppResources.Profile_Updating_Profile_Image_Failed);

			}
			else if (ex is UploadFileIsTooLargeException)
			{

				_appHelper.ShowAlert(((UploadFileIsTooLargeException)ex).Response.Data);
			}
			else if (ex is CanNotTakePhotoFromCameraException)
			{
				_appHelper.Toast(AppResources.Profile_Can_Not_Take_Photo_From_Camera, System.Drawing.Color.FromArgb(255, 0, 0));
			}
            else if (ex is NoSchoolsFoundException) 
            {
                SearchPerformed = true;
                Schools.Clear();

            }
			else
			{
				base.HandleExceptions(ex);
			}
		}

        #endregion


        #region models

        public class PageModel
        {

            public SonInformation SonInformation { get; set; }
            public long TotalSchools { get; set; }

        }


        public class SonInformation
        {

            public SonEntity Son { get; set; }
            public IList<SocialMediaEntity> SocialMedia { get; set; }

        }


        #endregion

    }
}
