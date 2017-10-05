
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

namespace Bullytect.Core.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {

        readonly IParentService _parentService;


        public ProfileViewModel(IParentService parentService, IUserDialogs userDialogs,
                                IMvxMessenger mvxMessenger, IImagesService imagesService) : base(userDialogs, mvxMessenger, imagesService)
        {
            _parentService = parentService;

            SaveChangesCommand = ReactiveCommand.CreateFromObservable<string, ParentEntity>((param) =>
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
                .CreateFromObservable<string, string>((param) =>
                {
                    return Observable.FromAsync<bool>((_) => _userDialogs.ConfirmAsync(new ConfirmConfig()
                    {
                        Title = AppResources.Profile_Confirm_Account_Deleting

                    })).Where((confirmed) => confirmed).Do((_) => _userDialogs.ShowLoading(AppResources.Profile_Account_Deleting))
						.SelectMany((_) => _parentService.DeleteAccount())
                                     .Do((_) => _userDialogs.HideLoading());
                });


            DeleteAccountCommand.Subscribe((_) => {

				var toastConfig = new ToastConfig(AppResources.Profile_Account_Deleted);
				toastConfig.SetDuration(3000);
				toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(12, 131, 193));
				_userDialogs.Toast(toastConfig);

            });

			DeleteAccountCommand.ThrownExceptions.Subscribe(HandleExceptions);


			SignOutCommand = ReactiveCommand
                .CreateFromObservable<string, bool>((param) =>
				{
                    return Observable.FromAsync<bool>((_) => _userDialogs.ConfirmAsync(new ConfirmConfig()
                    {
                        Title = AppResources.Profile_Confirm_SignOut

                    })).Where((confirmed) => confirmed);
				});

            SignOutCommand.Subscribe((_) =>
            {
                Settings.AccessToken = null;
                //var mvxBundle = new MvxBundle(new Dictionary<string, string> { { "NavigationCommand", "StackClear" } });
                ShowViewModel<AuthenticationViewModel>(new AuthenticationViewModel.AuthenticationParameter()
                {
                    ReasonForAuthentication = AuthenticationViewModel.SIGN_OUT
                });
            });

            TakePhotoCommand = ReactiveCommand.CreateFromObservable<string, MediaFile>((param) => PickPhotoStream());
			

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


        public ReactiveCommand<string, ParentEntity> SaveChangesCommand { get; protected set; }

        public ReactiveCommand<string, string> DeleteAccountCommand { get; protected set; }

        public ReactiveCommand<Unit, ParentEntity> LoadProfileCommand { get; protected set; }

        public ReactiveCommand<string, bool> SignOutCommand { get; protected set; }

        public ReactiveCommand<string, MediaFile> TakePhotoCommand { get; set; }


		#endregion

		void AccountUpdated(ParentEntity parent)
		{
			Debug.WriteLine(String.Format("Parent: {0}", parent.ToString()));
            var toastConfig = new ToastConfig(AppResources.Profile_Account_Updated);
			toastConfig.SetDuration(3000);
			toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(12, 131, 193));
			_userDialogs.Toast(toastConfig);
		}

		protected override void HandleExceptions(Exception ex)
		{
            if (ex is UploadImageFailException)
            {
                var toastConfig = new ToastConfig(AppResources.Profile_Updating_Profile_Image_Failed);
                toastConfig.SetDuration(3000);
                toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(255, 0, 0));
                _userDialogs.Toast(toastConfig);
            }
            else if (ex is CanNotTakePhotoFromCameraException) {
                var toastConfig = new ToastConfig(AppResources.Profile_Can_Not_Take_Photo_From_Camera);
                toastConfig.SetDuration(3000);
                toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(255, 0, 0));
                _userDialogs.Toast(toastConfig);
            }
            else
            {
                base.HandleExceptions(ex);
            }
		}

    }
}
