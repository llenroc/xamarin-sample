using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Plugin.Media.Abstractions;
using ReactiveUI;
using Xamarin.Forms;

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
            RefreshCommand = ReactiveCommand.CreateFromObservable(() => !string.IsNullOrEmpty(SonToEdit) ?
                              GetSonInformation().Do((SonInformation) =>
                              {
                                  CurrentSon = SonInformation.Son;
                                  CurrentSocialMedia = new ObservableCollection<SocialMediaEntity>(SonInformation.SocialMedia);
                              }).SelectMany((_) => GetSchoolNames())
                              .Do((SchoolNames) => Schools = new ObservableCollection<string>(SchoolNames)) :
                              GetSchoolNames().Do((SchoolNames) => Schools = new ObservableCollection<string>(SchoolNames)));

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
                                .Do((SocialMediaEntities) => CurrentSocialMedia = new ObservableCollection<SocialMediaEntity>(SocialMediaEntities))
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
                _userDialogs.ShowSuccess(AppResources.EditSon_Saved_Changes_Successfully);
            });

            SaveChangesCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.EditSon_Saving_Changes));

            SaveChangesCommand.ThrownExceptions.Subscribe(HandleExceptions);

            SaveSchoolCommand = ReactiveCommand.CreateFromObservable<Unit, SchoolEntity>((param) =>
            {
                return _schoolService.CreateSchool(NewSchool.Name, NewSchool.Residence, NewSchool.Location, NewSchool.Province, NewSchool.Tfno, NewSchool.Email);

            });

            SaveSchoolCommand.Subscribe((school) => _appHelper.Toast(AppResources.EditSon_School_Saved, System.Drawing.Color.FromArgb(12, 131, 193)));


            SaveSchoolCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.EditSon_Saving_School));

            SaveSchoolCommand.ThrownExceptions.Subscribe(HandleExceptions);
        }

        #region properties


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

        ObservableCollection<SocialMediaEntity> _currentSocialMedia = new ObservableCollection<SocialMediaEntity>();

        public ObservableCollection<SocialMediaEntity> CurrentSocialMedia
        {
            get => _currentSocialMedia;
            set => SetProperty(ref _currentSocialMedia, value);
        }


        SchoolEntity _newSchool;

        public SchoolEntity NewSchool
        {
            get => _newSchool;
            set => SetProperty(ref _newSchool, value);
        }

        ObservableCollection<string> _schools = new ObservableCollection<string>();

        public ObservableCollection<string> Schools
        {
            get => _schools;
            set => SetProperty(ref _schools, value);
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

        #endregion

        #region delegates

        public delegate void NewSelectedImageEvent(object sender, MediaFile NewProfileImage);
        public event NewSelectedImageEvent NewSelectedImage;

        protected virtual void OnNewSelectedImage(MediaFile NewProfileImage)
        {
            NewSelectedImage?.Invoke(this, NewProfileImage);
        }

        #endregion



        public void Init(string SonIdentity)
        {
            if (!string.IsNullOrEmpty(SonIdentity))
                SonToEdit = SonIdentity;
        }

		public override void Start()
		{

			if (!string.IsNullOrEmpty(SonToEdit))
				GetSonInformation().Subscribe((SonInformation) =>
				{
					CurrentSon = SonInformation.Son;
					CurrentSocialMedia = new ObservableCollection<SocialMediaEntity>(SonInformation.SocialMedia);

				});

			GetSchoolNames().Subscribe((SchoolNames) =>
			{
				Debug.WriteLine("Schools Names Count " + SchoolNames?.Count);
				Schools = new ObservableCollection<string>(SchoolNames);

			});
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

        private IObservable<IList<string>> GetSchoolNames() => _schoolService.AllNames().OnErrorResumeNext(Observable.Return(new List<string>()));



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

							_userDialogs.ShowSuccess(AppResources.EditSon_Social_Media_Added);
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

    }
}
