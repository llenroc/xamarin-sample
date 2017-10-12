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
using Bullytect.Core.Helpers;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.OAuth.Models;
using Bullytect.Core.OAuth.Providers.Facebook;
using Bullytect.Core.OAuth.Providers.Google;
using Bullytect.Core.OAuth.Providers.Instagram;
using Bullytect.Core.OAuth.Services;
using Bullytect.Core.Rest.Models.Exceptions;
using Bullytect.Core.Rest.Utils;
using Bullytect.Core.Services;
using Bullytect.Core.Utils;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using MvvmHelpers;
using Plugin.Media.Abstractions;
using ReactiveUI;

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


            RefreshCommand = ReactiveCommand.CreateFromObservable<Unit, PageModel>((param) => LoadPageModel());

            RefreshCommand.Subscribe(OnPageModelLoaded);

			RefreshCommand.IsExecuting.ToProperty(this, x => x.IsBusy, out _isBusy);

			RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);


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
                                _parentService.AddSonToSelfParent(CurrentSon.FirstName, CurrentSon.LastName, CurrentSon.Birthdate, CurrentSon.SchoolIdentity) :
                                _parentService.UpdateSonInformation(CurrentSon.Identity, CurrentSon.FirstName, CurrentSon.LastName, CurrentSon.Birthdate, CurrentSon.SchoolIdentity))
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
            });

            SaveChangesCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.EditSon_Saving_Changes));

            SaveChangesCommand.ThrownExceptions.Subscribe(HandleExceptions);

            SaveSchoolCommand = ReactiveCommand.CreateFromObservable<Unit, SchoolEntity>((param) => _schoolService.CreateSchool(NewSchool.Name, NewSchool.Residence, NewSchool.Location, NewSchool.Province, NewSchool.Tfno, NewSchool.Email));

            SaveSchoolCommand.Subscribe((SchoolAdded) =>
            {
                NewSchool = new SchoolEntity();
                Schools.Add(new SchoolPickerModel()
                {
                    Identity = SchoolAdded.Identity,
                    Name = SchoolAdded.Name
                });
                OnSchoolAdded(SchoolAdded);
                _appHelper.Toast(AppResources.EditSon_School_Saved, System.Drawing.Color.FromArgb(12, 131, 193));
            });


            SaveSchoolCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.EditSon_Saving_School));

            SaveSchoolCommand.ThrownExceptions.Subscribe(HandleExceptions);
        }

        #region properties

        public ObservableRangeCollection<SocialMediaEntity> CurrentSocialMedia { get; } = new ObservableRangeCollection<SocialMediaEntity>();
        public ObservableRangeCollection<SchoolPickerModel> Schools { get; } = new ObservableRangeCollection<SchoolPickerModel>();


        protected MediaFile _newProfileImage;

        public MediaFile NewProfileImage
        {
            get => _newProfileImage;
            set => SetProperty(ref _newProfileImage, value);
        }

        SonEntity _currentSon = new SonEntity();

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

        int _schoolSelectedIndex;
        public int SchoolSelectedIndex
        {
            get => _schoolSelectedIndex;
            set
            {
                if(Schools != null && value >= 0 && value < Schools.Count()) {
					CurrentSon.SchoolIdentity = Schools[value]?.Identity;
					CurrentSon.SchoolName = Schools[value]?.Name;
                    SetProperty(ref _schoolSelectedIndex, value);
                }

            }
        }

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

            LoadPageModel().ObserveOn(Scheduler.Default).Subscribe(OnPageModelLoaded);
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

        public ICommand ToggleFacebookSocialMediaCommand
                        => new MvxCommand(() => ToggleSocialMediaHandler(new FacebookOAuth2(), AppConstants.FACEBOOK));

        public ICommand ToggleInstagramSocialMediaCommand
                        => new MvxCommand(() => ToggleSocialMediaHandler(new InstagramOAuth2(), AppConstants.INSTAGRAM));

        public ICommand ToggleYoutubeSocialMediaCommand
                        => new MvxCommand(() => ToggleSocialMediaHandler(new GoogleOAuth2(), AppConstants.YOUTUBE));

        public ReactiveCommand<Unit, SchoolEntity> SaveSchoolCommand { get; set; }


        public ICommand ResetSocialMediaCommand
                        => new MvxCommand<string>((string Type) => ResetSocialMediaHandler(Type));


        #endregion

        #region methods

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
                });
        }

        IObservable<IEnumerable<SchoolPickerModel>> GetSchoolNames()
        {
            return _schoolService.AllNames().Select(SchoolNamesDict => SchoolNamesDict.ToList()).Select((SchoolEntryList) => SchoolEntryList.Select((SchoolEntry) => new SchoolPickerModel()
            {
                Identity = SchoolEntry.Key,
                Name = SchoolEntry.Value
            })).OnErrorResumeNext(Observable.Return(new List<SchoolPickerModel>()));
        }

        IObservable<PageModel> LoadPageModel() {
            

			if (!string.IsNullOrEmpty(SonToEdit))
			{

			    return Observable.Zip(GetSonInformation(), GetSchoolNames(), (sonInformation, schools) => new PageModel()
				{
					SonInformation = sonInformation,
					Schools = schools

				});
			}
			else
			{

				return GetSchoolNames().Select((schools) => new PageModel()
				{
					Schools = schools

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
							 .Where(AccessToken => !string.IsNullOrWhiteSpace(AccessToken))
					.Subscribe(AccessToken =>
					{

						if (index.HasValue)
						{

							var SocialMedia = CurrentSocialMedia.ElementAt(index.Value);
							SocialMedia.AccessToken = AccessToken;
							SocialMedia.InvalidToken = false;
							RaisePropertyChanged(nameof(CurrentSocialMedia));
						}
						else
						{
							CurrentSocialMedia.Add(new SocialMediaEntity()
							{
								AccessToken = AccessToken,
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

			if (Page?.Schools?.Count() > 0)
			{
				Schools.ReplaceRange(Page.Schools);
			}


			if (Page.SonInformation != null)
			{
                CurrentSon.HydrateWith(Page.SonInformation.Son);
				CurrentSocialMedia.ReplaceRange(Page.SonInformation.SocialMedia);
			}

			if (Page?.Schools?.Count() > 0 && Page.SonInformation != null)
			{

				var index = Schools.Select((SchoolItem, SchoolIndex) => new { SchoolItem, SchoolIndex })
								   .FirstOrDefault(i => i.SchoolItem.Identity.Equals(CurrentSon.SchoolIdentity))?.SchoolIndex;

                if (index.HasValue && index.Value >= 0)
                    SchoolSelectedIndex = index.Value;
			}

        }

        #endregion


        #region models

        public class PageModel
        {

            public SonInformation SonInformation { get; set; }
            public IEnumerable<SchoolPickerModel> Schools { get; set; }

        }


        public class SonInformation
        {

            public SonEntity Son { get; set; }
            public IList<SocialMediaEntity> SocialMedia { get; set; }

        }

        public class SchoolPickerModel
        {

            public string Identity { get; set; }
            public string Name { get; set; }
        }


        #endregion

    }
}
