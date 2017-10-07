
using System;
using System.Diagnostics;
using System.Reactive.Linq;
using Acr.UserDialogs;
using Bullytect.Core.I18N;
using Bullytect.Core.Messages;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using Bullytect.Core.Utils;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;
using Bullytect.Core.Rest.Models.Exceptions;
using Bullytect.Core.Config;
using Bullytect.Core.Exceptions;
using System.IO;
using Plugin.Media.Abstractions;
using System.Reactive;
using Bullytect.Core.Helpers;

namespace Bullytect.Core.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {

        readonly IParentService _parentService;


        public ProfileViewModel(IParentService parentService, IUserDialogs userDialogs,
                                IMvxMessenger mvxMessenger, AppHelper appHelper) : base(userDialogs, mvxMessenger, appHelper)
        {
            _parentService = parentService;

            SaveChangesCommand = ReactiveCommand.CreateFromObservable<Unit, ParentEntity>((param) =>
            {

                return NewProfileImage != null ?
                    _parentService.UploadProfileImage(NewProfileImage.GetStream())
                                  .SelectMany((_) => _parentService.Update(SelfParent.FirstName, SelfParent.LastName, SelfParent.Birthdate, SelfParent.Email, SelfParent.Telephone)) :

                    _parentService.Update(SelfParent.FirstName, SelfParent.LastName, SelfParent.Birthdate, SelfParent.Email, SelfParent.Telephone);
                    
                
            });


            SaveChangesCommand.Subscribe(AccountUpdated);

            SaveChangesCommand.IsExecuting.ToProperty(this, x => x.IsBusy, out _isBusy);

			SaveChangesCommand.ThrownExceptions.Subscribe(HandleExceptions);

            LoadProfileCommand = ReactiveCommand.CreateFromObservable<Unit, ParentEntity>((param) => _parentService.GetProfileInformation());

            LoadProfileCommand.ToProperty(this, x => x.SelfParent, out _selfParent);

            LoadProfileCommand.IsExecuting.ToProperty(this, x => x.IsBusy, out _isBusy);

			LoadProfileCommand.ThrownExceptions.Subscribe(HandleExceptions);

            DeleteAccountCommand = ReactiveCommand
                .CreateFromObservable<Unit, string>((param) =>
                {
                    return Observable.FromAsync<bool>((_) => _userDialogs.ConfirmAsync(new ConfirmConfig()
                    {
                        Title = AppResources.Profile_Confirm_Account_Deleting

                    })).Where((confirmed) => confirmed).Do((_) => _userDialogs.ShowLoading(AppResources.Profile_Account_Deleting))
						.SelectMany((_) => _parentService.DeleteAccount())
                                     .Do((_) => _userDialogs.HideLoading());
                });


            DeleteAccountCommand.Subscribe((_) => _appHelper.Toast(AppResources.Profile_Account_Deleted, System.Drawing.Color.FromArgb(12, 131, 193)));


			DeleteAccountCommand.ThrownExceptions.Subscribe(HandleExceptions);


            TakePhotoCommand = ReactiveCommand.CreateFromObservable<Unit, MediaFile>((_) => appHelper.PickPhotoStream());
			

            TakePhotoCommand.Subscribe((ImageStream) => {
                _userDialogs.HideLoading();
                NewProfileImage = ImageStream;
                OnNewSelectedImage(ImageStream);
			});

            TakePhotoCommand.ThrownExceptions.Subscribe(HandleExceptions);

   

		}

        #region Properties


        protected MediaFile _newProfileImage;

        public MediaFile NewProfileImage
        {
            get => _newProfileImage;
            set => SetProperty(ref _newProfileImage, value);
        }

        protected ObservableAsPropertyHelper<ParentEntity> _selfParent;
		public ParentEntity SelfParent
		{
			get { return _selfParent.Value; }
		}


		readonly string _prefix = "+34";

		public string Prefix
		{
			get => _prefix;
		}


        public delegate void NewSelectedImageEvent(object sender, MediaFile NewProfileImage);
		public event NewSelectedImageEvent NewSelectedImage;

		protected virtual void OnNewSelectedImage(MediaFile NewProfileImage)
		{
			NewSelectedImage?.Invoke(this, NewProfileImage);
		}

        #endregion

        #region commands


        public ReactiveCommand<Unit, ParentEntity> SaveChangesCommand { get; protected set; }

        public ReactiveCommand<Unit, string> DeleteAccountCommand { get; protected set; }

        public ReactiveCommand<Unit, ParentEntity> LoadProfileCommand { get; protected set; }

        public ReactiveCommand<Unit, MediaFile> TakePhotoCommand { get; set; }


		#endregion

		void AccountUpdated(ParentEntity parent)
		{
			Debug.WriteLine(String.Format("Parent: {0}", parent.ToString()));
            _appHelper.Toast(AppResources.Profile_Account_Updated, System.Drawing.Color.FromArgb(12, 131, 193));
		}

		protected override void HandleExceptions(Exception ex)
		{
            if (ex is UploadImageFailException)
            {
                _appHelper.Toast(AppResources.Profile_Updating_Profile_Image_Failed, System.Drawing.Color.FromArgb(255, 0, 0));

            }
            else if (ex is CanNotTakePhotoFromCameraException) {
                _appHelper.Toast(AppResources.Profile_Can_Not_Take_Photo_From_Camera, System.Drawing.Color.FromArgb(255, 0, 0));
            }
            else
            {
                base.HandleExceptions(ex);
            }
		}

    }
}
