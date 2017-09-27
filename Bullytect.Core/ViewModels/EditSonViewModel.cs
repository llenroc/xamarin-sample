using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using Acr.UserDialogs;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using Bullytect.Core.Utils;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

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

			TakePhotoCommand.ThrownExceptions.Subscribe(HandleExceptions);

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

        string _sonToEdit = null;

		public string SonToEdit
		{
			get => _sonToEdit;
			set => SetProperty(ref _sonToEdit, value);
		}


        public void Init(string sontToEdit)
        {
            SonToEdit = sontToEdit;
        }

        #region commands

            public ReactiveCommand<string, ImageEntity> TakePhotoCommand { get; set; }

            public ReactiveCommand RefreshCommand { get; protected set; }

        #endregion
    }
}
