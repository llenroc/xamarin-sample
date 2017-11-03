
using System;
using System.Diagnostics;
using System.Reactive.Linq;
using Acr.UserDialogs;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;
using Bullytect.Core.Rest.Models.Exceptions;
using Bullytect.Core.Exceptions;
using Plugin.Media.Abstractions;
using System.Reactive;
using Bullytect.Core.Helpers;
using Bullytect.Core.Utils;

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
                                  .SelectMany((_) => _parentService.Update(SelfParent.FirstName, SelfParent.LastName, SelfParent.Birthdate, SelfParent.Email, string.Concat(SelfParent.PhonePrefix, SelfParent.PhoneNumber))) :

                    _parentService.Update(SelfParent.FirstName, SelfParent.LastName, SelfParent.Birthdate, SelfParent.Email, string.Concat(SelfParent.PhonePrefix, SelfParent.PhoneNumber));
                
            });


            SaveChangesCommand.Subscribe(AccountUpdatedHandler);

            SaveChangesCommand.IsExecuting.Subscribe((IsLoading) => HandleIsExecuting(IsLoading, AppResources.Profile_Save_Changes));

			SaveChangesCommand.ThrownExceptions.Subscribe(HandleExceptions);

            RefreshCommand = ReactiveCommand.CreateFromObservable<Unit, ParentEntity>((param) => _parentService.GetProfileInformation());

            RefreshCommand.Subscribe((ParentEntity) => {
                SelfParent.HydrateWith(ParentEntity);
                ResetCommonProps();
                IsDirtyMonitoring = true;
            });

            RefreshCommand.IsExecuting.Subscribe((IsLoading) => HandleIsExecuting(IsLoading, AppResources.Profile_Loading_Data));

			RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);

            DeleteAccountCommand = ReactiveCommand
                .CreateFromObservable<Unit, string>((param) =>
                {
                    return Observable.FromAsync<bool>((_) => _userDialogs.ConfirmAsync(new ConfirmConfig()
                    {
                        Title = AppResources.Profile_Confirm_Account_Deleting

                })).Where((confirmed) => confirmed).Do((_) => HandleIsExecuting(true, AppResources.Profile_Account_Deleting))
						.SelectMany((_) => _parentService.DeleteAccount())
                                     .Do((_) => HandleIsExecuting(false, AppResources.Profile_Account_Deleting));
                });


            DeleteAccountCommand.Subscribe((_) =>
            {
				Bullytect.Core.Config.Settings.AccessToken = null;
				ShowViewModel<AuthenticationViewModel>(new AuthenticationViewModel.AuthenticationParameter()
				{
                    ReasonForAuthentication = AuthenticationViewModel.ACCOUNT_DELETED
				});

             });


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

        [IsDirtyMonitoring]
        public MediaFile NewProfileImage
        {
            get => _newProfileImage;
            set => SetProperty(ref _newProfileImage, value);
        }

        protected ParentEntity _selfParent = new ParentEntity();

        [IsDirtyMonitoring]
		public ParentEntity SelfParent
		{
			get => _selfParent;
			set => SetProperty(ref _selfParent, value);
		}

		#endregion

		#region delegates


		public delegate void NewSelectedImageEvent(object sender, MediaFile NewProfileImage);
		public event NewSelectedImageEvent NewSelectedImage;

		protected virtual void OnNewSelectedImage(MediaFile NewProfileImage)
		{
			NewSelectedImage?.Invoke(this, NewProfileImage);
		}

		public delegate void AccountUpdatedEvent(object sender, ParentEntity ParentEntity);
		public event AccountUpdatedEvent AccountUpdated;

		protected virtual void OnAccountUpdated(ParentEntity ParentEntity)
		{
			AccountUpdated?.Invoke(this, ParentEntity);
		}

        #endregion



        #region commands


        public ReactiveCommand<Unit, ParentEntity> SaveChangesCommand { get; protected set; }

        public ReactiveCommand<Unit, string> DeleteAccountCommand { get; protected set; }

        public ReactiveCommand<Unit, ParentEntity> RefreshCommand { get; protected set; }

        public ReactiveCommand<Unit, MediaFile> TakePhotoCommand { get; set; }


		#endregion

		void AccountUpdatedHandler(ParentEntity Parent)
		{
			Debug.WriteLine(String.Format("Parent: {0}", Parent.ToString()));
            SelfParent.HydrateWith(Parent);
            _userDialogs.ShowSuccess(AppResources.Profile_Account_Updated);
            OnAccountUpdated(Parent);
            ResetCommonProps();
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
			else if (ex is CanNotTakePhotoFromCameraException) {
                _appHelper.Toast(AppResources.Profile_Can_Not_Take_Photo_From_Camera, System.Drawing.Color.FromArgb(255, 0, 0));
            }
            else
            {
                base.HandleExceptions(ex);
            }
		}

		protected override void OnBackPressed()
		{


			if (IsDirty)
			{

                _appHelper.RequestConfirmation(AppResources.Profile_Cancel_Changes)
					  .Subscribe((_) => base.OnBackPressed());
			}
			else
			{

				base.OnBackPressed();
			}


		}

    }
}
