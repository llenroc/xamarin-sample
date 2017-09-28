using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.OAuth.Models;
using Bullytect.Core.OAuth.Providers.Facebook;
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

        public EditSonViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger, IParentService parentService,
                                ISocialMediaService socialMediaService, IImagesService imagesService) : base(userDialogs, mvxMessenger)
        {

            _parentService = parentService;
            _socialMediaService = socialMediaService;
            _imagesService = imagesService;

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
						CurrentSocialMedia = SocialMediaList;
					}).Select((_) => true);
				});

			RefreshCommand = ReactiveCommand.CreateCombined(new[] { GetSonByIdCommand, GetAllSocialMediaCommand });

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
                                .Do((SocialMediaEntities) => CurrentSocialMedia = SocialMediaEntities)
                                .Select((_) => true);   
            });

            SaveChangesCommand.Subscribe((_) => {
                _userDialogs.ShowSuccess(AppResources.EditSon_Saved_Changes_Successfully);
            });

            SaveChangesCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.EditSon_Saving_Changes));

			SaveChangesCommand.ThrownExceptions.Subscribe(HandleExceptions);


        }

		SonEntity _currentSon = new SonEntity();

		public SonEntity CurrentSon
		{
			get => _currentSon;
			set => SetProperty(ref _currentSon, value);
		}

        IList<SocialMediaEntity> _currentSocialMedia = new List<SocialMediaEntity>();

		public IList<SocialMediaEntity> CurrentSocialMedia
		{
			get => _currentSocialMedia;
			set => SetProperty(ref _currentSocialMedia, value);
		}

        string _newSchoolName;

		public string NewSchoolName
		{
			get => _newSchoolName;
			set => SetProperty(ref _newSchoolName, value);
		}

        IList<string> _schools = new List<string>() { "Colegio 1", "Colegio 2", "Colegio 3"};

		public IList<string> Schools
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

        public void Init(string sontToEdit)
        {
            SonToEdit = sontToEdit;
        }

        #region commands

            public ReactiveCommand<string, bool> SaveChangesCommand { get; protected set; }

            public ReactiveCommand<string, ImageEntity> TakePhotoCommand { get; set; }

            public ReactiveCommand RefreshCommand { get; protected set; }

            public ICommand ToggleFacebookSocialMediaCommand => new MvxCommand<bool>((bool Enabled) => ToggleSocialMediaHandler(Enabled, new FacebookOAuth2(), AppConstants.FACEBOOK));

            public ICommand ToggleInstagramSocialMediaCommand => new MvxCommand<bool>((bool Enabled) => ToggleSocialMediaHandler(Enabled, new InstagramOAuth2(), AppConstants.INSTAGRAM));

            public ICommand ToggleYoutubeSocialMediaCommand => new MvxCommand<bool>((bool Enabled) => ToggleSocialMediaHandler(Enabled, new InstagramOAuth2(), AppConstants.YOUTUBE));

            public ICommand SaveSchoolCommand => new MvxCommand(() =>
            {
                _userDialogs.ShowSuccess(string.Format("School {0}", NewSchoolName));
                Schools.Add(NewSchoolName);
            });


        #endregion


        void ToggleSocialMediaHandler(bool Enabled, OAuth2 Provider, string Type ) {

            if(Enabled) {

                Debug.WriteLine(string.Format("Enable Social Media: {0}", Type));

				var oauthService = DependencyService.Get<IOAuth>();
                oauthService
                    .authenticate(Provider)
                    .Where(AccessToken => !string.IsNullOrWhiteSpace(AccessToken))
                    .Subscribe(AccessToken =>
                    {

                        var SocialMedia = CurrentSocialMedia.SingleOrDefault(Social => Social.Type.Equals(Type));

                        if(SocialMedia != null) {
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

                ((List<SocialMediaEntity>)CurrentSocialMedia).RemoveAll((Social) => Social.Type.Equals(Type));

                _userDialogs.ShowSuccess(AppResources.EditSon_Social_Media_Deleted);
            
            }

			
        }
    }
}
