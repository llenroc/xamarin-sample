
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

namespace Bullytect.Core.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {

        readonly IParentService _parentService;
        readonly IImagesService _imagesService;


        public ProfileViewModel(IParentService parentService, IUserDialogs userDialogs, 
                                IMvxMessenger mvxMessenger, IImagesService imagesService): base(userDialogs, mvxMessenger)
        {
            _parentService = parentService;
            _imagesService = imagesService;

            SaveChangesCommand = ReactiveCommand.CreateFromObservable<string, ParentEntity>((_) => _parentService.Update(SelfParent.FirstName, SelfParent.LastName, SelfParent.Birthdate, SelfParent.Email, SelfParent.Telephone));
 
            SaveChangesCommand.Subscribe(AccountUpdated);

            SaveChangesCommand.IsExecuting.ToProperty(this, x => x.IsBusy, out _isBusy);

			SaveChangesCommand.ThrownExceptions.Subscribe(HandleExceptions);

            LoadProfileCommand = ReactiveCommand.CreateFromObservable<string, ParentEntity>((param) => _parentService.GetProfileInformation());

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
                _userDialogs.ShowSuccess(AppResources.Profile_Account_Deleted);
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

            TakePhotoCommand = CommandFactory.CreateTakePhotoCommand(_parentService, _imagesService, _userDialogs);

            TakePhotoCommand.Subscribe((image) => {
                _userDialogs.ShowSuccess(AppResources.Profile_Updating_Profile_Image_Success);
            });
            TakePhotoCommand.ThrownExceptions.Subscribe(HandleExceptions);

		}

        #region Properties

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

        #endregion

        #region commands


        public ReactiveCommand<string, ParentEntity> SaveChangesCommand { get; protected set; }

        public ReactiveCommand<string, string> DeleteAccountCommand { get; protected set; }

        public ReactiveCommand<string, ParentEntity> LoadProfileCommand { get; protected set; }

        public ReactiveCommand<string, bool> SignOutCommand { get; protected set; }

        public ReactiveCommand<string, ImageEntity> TakePhotoCommand { get; set; }


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
                toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(12, 131, 193));
                _userDialogs.Toast(toastConfig);
            }
            else if (ex is CanNotTakePhotoFromCameraException) {
                var toastConfig = new ToastConfig(AppResources.Profile_Can_Not_Take_Photo_From_Camera);
                toastConfig.SetDuration(3000);
                toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(12, 131, 193));
                _userDialogs.Toast(toastConfig);
            }
            else
            {
                base.HandleExceptions(ex);
            }
		}

    }
}
