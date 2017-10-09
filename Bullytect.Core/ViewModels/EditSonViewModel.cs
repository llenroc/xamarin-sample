﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
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
                                ISocialMediaService socialMediaService, AppHelper appHelper, IOAuthService oauthService,  ISchoolService schoolService) : base(userDialogs, mvxMessenger, appHelper)
        {

            _parentService = parentService;
            _socialMediaService = socialMediaService;
            _schoolService = schoolService;
            _oauthService = oauthService;

            // Refresh Command (Get son Information And All School Names)
            RefreshCommand = ReactiveCommand.CreateFromObservable(() =>
            {

				var GetAllSchools = GetSchoolNames().Do((SchoolNames) =>
				{
					if (SchoolNames?.Count() > 0)
						Schools.ReplaceRange(SchoolNames);

				});

                return !string.IsNullOrEmpty(SonToEdit) ? 
                              GetSonInformation().Do((SonInformation) =>
                              {
                                  CurrentSon = SonInformation.Son;
                                  CurrentSocialMedia.ReplaceRange(SonInformation.SocialMedia);
                              })
                              .SelectMany((_) => GetAllSchools).Do((_) => {

								  var index = Schools.Select((SchoolItem, SchoolIndex) => new { SchoolItem, SchoolIndex })
																	   .First(i => i.SchoolItem.Identity.Equals(CurrentSon.School)).SchoolIndex;
								  SchoolSelectedIndex = index;
							  }) : GetAllSchools;


            }); 

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
                                 _parentService.AddSonToSelfParent(CurrentSon.FirstName, CurrentSon.LastName, CurrentSon.Birthdate, CurrentSon.School) :
                                 _parentService.UpdateSonInformation(CurrentSon.Identity, CurrentSon.FirstName, CurrentSon.LastName, CurrentSon.Birthdate, CurrentSon.School))
                                .Do((SonEntity) => CurrentSon = SonEntity)
                                .SelectMany((SonEntity) => _socialMediaService.SaveAllSocialMedia(CurrentSon.Identity, CurrentSocialMedia.Select(s => { s.Son = CurrentSon.Identity; return s; }).ToList()))
                                .Do((SocialMediaEntities) => CurrentSocialMedia.ReplaceRange(SocialMediaEntities))
                                .SelectMany((_) =>
                                {
                                    return NewProfileImage != null ?
                                            _parentService.UploadSonProfileImage(CurrentSon.Identity, NewProfileImage.GetStream()) :
                                            Observable.Empty<ImageEntity>();
                                })
                                .Select((_) => true)
                                .Finally(() => NewProfileImage = null);
                });

            SaveChangesCommand.Subscribe((_) =>
            {
                OnSonUpdated(CurrentSon);
                _userDialogs.ShowSuccess(AppResources.EditSon_Saved_Changes_Successfully);
            });

            SaveChangesCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.EditSon_Saving_Changes));

            SaveChangesCommand.ThrownExceptions.Subscribe(HandleExceptions);

            SaveSchoolCommand = ReactiveCommand.CreateFromObservable<Unit, SchoolEntity>((param) =>
            {
                return _schoolService.CreateSchool(NewSchool.Name, NewSchool.Residence, NewSchool.Location, NewSchool.Province, NewSchool.Tfno, NewSchool.Email);

            });

            SaveSchoolCommand.Subscribe((SchoolAdded) =>
            {
                NewSchool = new SchoolEntity();
                Schools.Add(new SchoolPickerModel() {
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

        public string PageTitle {

            get => !string.IsNullOrEmpty(SonToEdit) ? AppResources.Page_Edit_Son_Title : AppResources.Page_Add_Son_Title;

        }

        int _schoolSelectedIndex;
        public int SchoolSelectedIndex {
			get => _schoolSelectedIndex;
            set {

                CurrentSon.School = Schools[value]?.Identity;
                SetProperty(ref _schoolSelectedIndex, value);
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

        public ReactiveCommand RefreshCommand { get; protected set; }

        public ICommand ToggleFacebookSocialMediaCommand
                        => new MvxCommand<bool>((bool Enabled) => ToggleSocialMediaHandler(Enabled, new FacebookOAuth2(), AppConstants.FACEBOOK));

        public ICommand ToggleInstagramSocialMediaCommand
                        => new MvxCommand<bool>((bool Enabled) => ToggleSocialMediaHandler(Enabled, new InstagramOAuth2(), AppConstants.INSTAGRAM));

        public ICommand ToggleYoutubeSocialMediaCommand
                        => new MvxCommand<bool>((bool Enabled) => ToggleSocialMediaHandler(Enabled, new GoogleOAuth2(), AppConstants.YOUTUBE));

        public ReactiveCommand<Unit, SchoolEntity> SaveSchoolCommand { get; set; }



        #endregion

        #region methods

        private IObservable<SonInformation> GetSonInformation (){

            return Observable.Zip(
                _parentService.GetSonById(SonToEdit), 
                _socialMediaService.GetAllSocialMediaBySon(SonToEdit),
                (SonEntity SonEntity, IList<SocialMediaEntity> SocialMedia) => 
                new SonInformation () {
                    Son = SonEntity,
                    SocialMedia = SocialMedia
                });
        }

        private IObservable<IEnumerable<SchoolPickerModel>> GetSchoolNames()
        {
            return _schoolService.AllNames().Select(SchoolNamesDict => SchoolNamesDict.ToList()).Select((SchoolEntryList) => SchoolEntryList.Select((SchoolEntry) => new SchoolPickerModel()
            {
                Identity = SchoolEntry.Key,
                Name = SchoolEntry.Value
            })).OnErrorResumeNext(Observable.Return(new List<SchoolPickerModel>()));
		}

        private void ToggleSocialMediaHandler(bool Enabled, OAuth2 Provider, string Type)
        {

            var index = CurrentSocialMedia.Select((SocialItem, SocialIndex) => new { SocialItem, SocialIndex }).First(i => i.SocialItem.Type.Equals(Type)).SocialIndex;

            if (Enabled)
            {

                if(index < 0 || CurrentSocialMedia.ElementAt(index).InvalidToken) {

					Debug.WriteLine(string.Format("Enable Social Media: {0}", Type));

                    _oauthService.Authenticate(Provider).Where(AccessToken => !string.IsNullOrWhiteSpace(AccessToken))
						.Subscribe(AccessToken =>
						{
							if (index >= 0)
							{

								var SocialMedia = CurrentSocialMedia.ElementAt(index);
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

                }

            }
            else
            {

                Debug.WriteLine(string.Format("Disable Social Media: {0}", Type));

                CurrentSocialMedia.RemoveAt(index);

                _userDialogs.ShowSuccess(AppResources.EditSon_Social_Media_Deleted);

            }
        }

        #endregion


        protected class SonInformation {

            public SonEntity Son { get; set; }
            public IList<SocialMediaEntity> SocialMedia { get; set; }

        }

        public class SchoolPickerModel {

            public string Identity { get; set; }
            public string Name { get; set; }
        }

    }
}
