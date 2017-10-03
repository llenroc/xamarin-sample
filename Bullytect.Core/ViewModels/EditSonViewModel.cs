using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
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
using ReactiveUI;
using Xamarin.Forms;

namespace Bullytect.Core.ViewModels
{
    public class EditSonViewModel : BaseViewModel
    {

        readonly IParentService _parentService;
        readonly ISocialMediaService _socialMediaService;
        readonly IImagesService _imagesService;
        readonly ISchoolService _schoolService;

        public EditSonViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger, IParentService parentService,
                                ISocialMediaService socialMediaService, IImagesService imagesService, ISchoolService schoolService) : base(userDialogs, mvxMessenger)
        {

            _parentService = parentService;
            _socialMediaService = socialMediaService;
            _imagesService = imagesService;
            _schoolService = schoolService;

			var GetSonByIdCommand  = ReactiveCommand
				.CreateFromObservable<string, bool>((param) =>
				{
                return _parentService.GetSonById(SonToEdit).Do((SonEntity) =>
					{
						Debug.WriteLine("Son Entity " + SonEntity?.ToString());
						CurrentSon = SonEntity;
					}).Select((_) => true);
				});

			var GetAllSocialMediaCommand = ReactiveCommand
				.CreateFromObservable<string, bool>((param) =>
				{
                return _socialMediaService.GetAllSocialMediaBySon(SonToEdit).Do((SocialMediaList) =>
					{
                        Debug.WriteLine("Social Medias Count " + SocialMediaList?.Count);
                        CurrentSocialMedia = new ObservableCollection<SocialMediaEntity>(SocialMediaList);
					}).Select((_) => true);
				});


			var GetAllSchoolNamesCommand = ReactiveCommand.CreateFromObservable<string, bool>((param) =>
			{
                return _schoolService.AllNames().OnErrorResumeNext(Observable.Return(new List<string>())).Do((Schools) =>
				{
					Debug.WriteLine("Schools Names Count " + Schools?.Count);
                    Schools = new ObservableCollection<string>(Schools);
				}).Select((_) => true);
			});

			RefreshCommand = ReactiveCommand.CreateCombined(new[] { GetSonByIdCommand, GetAllSocialMediaCommand, GetAllSchoolNamesCommand  });

			RefreshCommand.IsExecuting.ToProperty(this, x => x.IsBusy, out _isBusy);

			RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);

			TakePhotoCommand = CommandFactory.CreateTakePhotoCommand(_parentService, _imagesService, _userDialogs);

			TakePhotoCommand.Subscribe((image) => {
				_userDialogs.ShowSuccess(AppResources.Profile_Updating_Profile_Image_Success);
			});

			TakePhotoCommand.ThrownExceptions.Subscribe(HandleExceptions);


            SaveChangesCommand = ReactiveCommand
                .CreateFromObservable<string, bool>((param) => {

					return (CurrentSon.Identity == null ?
								 _parentService.AddSonToSelfParent(CurrentSon.FirstName, CurrentSon.LastName, CurrentSon.Birthdate, CurrentSon.School) :
								 _parentService.UpdateSonInformation(CurrentSon.Identity, CurrentSon.FirstName, CurrentSon.LastName, CurrentSon.Birthdate, CurrentSon.School))
                                .Do((SonEntity) => CurrentSon = SonEntity)
                                .SelectMany((SonEntity) => _socialMediaService.SaveAllSocialMedia(CurrentSon.Identity, CurrentSocialMedia.Select(s => { s.Son = CurrentSon.Identity; return s; }).ToList()))
                                .Do((SocialMediaEntities) => CurrentSocialMedia = new ObservableCollection<SocialMediaEntity>(SocialMediaEntities))
                                .Select((_) => true);   
            });

            SaveChangesCommand.Subscribe((_) => {
                _userDialogs.ShowSuccess(AppResources.EditSon_Saved_Changes_Successfully);
            });

            SaveChangesCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.EditSon_Saving_Changes));

			SaveChangesCommand.ThrownExceptions.Subscribe(HandleExceptions);

            SaveSchoolCommand = ReactiveCommand.CreateFromObservable<string, SchoolEntity>((param) =>
            {
                return _schoolService.CreateSchool(NewSchool.Name, NewSchool.Residence, NewSchool.Location, NewSchool.Province, NewSchool.Tfno, NewSchool.Email);

            });

            SaveSchoolCommand.Subscribe((school) => {
                var toastConfig = new ToastConfig(AppResources.EditSon_School_Saved);
				toastConfig.SetDuration(3000);
				toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(12, 131, 193));
				_userDialogs.Toast(toastConfig);
                Schools.Add(school.Name);
            });

            SaveSchoolCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.EditSon_Saving_School));

            SaveSchoolCommand.ThrownExceptions.Subscribe(HandleExceptions);
        }

        #region properties

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

        public ObservableCollection<string> Schools {
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

		#endregion


		public void Init(string SontIdentity)
		{
            if(!string.IsNullOrEmpty(SontIdentity))
			    SonToEdit = SontIdentity;
		}


        #region commands

        public ReactiveCommand<string, bool> SaveChangesCommand { get; protected set; }

        public ReactiveCommand<string, ImageEntity> TakePhotoCommand { get; set; }

        public ReactiveCommand RefreshCommand { get; protected set; }

        public ICommand ToggleFacebookSocialMediaCommand 
                        => new MvxCommand<bool>((bool Enabled) => ToggleSocialMediaHandler(Enabled, new FacebookOAuth2(), AppConstants.FACEBOOK));

        public ICommand ToggleInstagramSocialMediaCommand 
                        => new MvxCommand<bool>((bool Enabled) => ToggleSocialMediaHandler(Enabled, new InstagramOAuth2(), AppConstants.INSTAGRAM));

        public ICommand ToggleYoutubeSocialMediaCommand 
                        => new MvxCommand<bool>((bool Enabled) => ToggleSocialMediaHandler(Enabled, new GoogleOAuth2(), AppConstants.YOUTUBE));

        public ReactiveCommand<string, SchoolEntity> SaveSchoolCommand { get; set; }


        #endregion


        void ToggleSocialMediaHandler(bool Enabled, OAuth2 Provider, string Type ) {


            var index = CurrentSocialMedia.Select((SocialItem, SocialIndex) => new { SocialItem, SocialIndex }).First(i => i.SocialItem.Type.Equals(Type)).SocialIndex;

            if(Enabled) {

                Debug.WriteLine(string.Format("Enable Social Media: {0}", Type));

				var oauthService = DependencyService.Get<IOAuth>();
                oauthService
                    .authenticate(Provider)
                    .Where(AccessToken => !string.IsNullOrWhiteSpace(AccessToken))
                    .Subscribe(AccessToken =>
                    {
                        if(index >= 0) {

                            var SocialMedia = CurrentSocialMedia.ElementAt(index);
                            SocialMedia.AccessToken = AccessToken;
                            SocialMedia.InvalidToken = false;
                        } else {
                            CurrentSocialMedia.Add(new SocialMediaEntity() {
                                AccessToken = AccessToken,
                                InvalidToken = false,
                                Type = Type
                            });
                        }

                        _userDialogs.ShowSuccess(AppResources.EditSon_Social_Media_Added);
                    });

            } else {

                Debug.WriteLine(string.Format("Disable Social Media: {0}", Type));

                CurrentSocialMedia.RemoveAt(index);

                _userDialogs.ShowSuccess(AppResources.EditSon_Social_Media_Deleted);
            
            }
        }
    }
}
